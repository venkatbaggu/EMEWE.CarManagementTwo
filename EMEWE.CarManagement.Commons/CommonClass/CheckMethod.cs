using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using System.IO;
using System.Data;
using System.Collections;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Threading;
using WindowsFormsApplication3;
using System.Linq.Expressions;
using JLMDSAP.sappwd;
namespace EMEWE.CarManagement.Commons.CommonClass
{

    /// <summary>
    /// 该类用于管理策略的验证，方法都是无参数的，每个方法必须填写方法说明，
    /// 如果需要必须的参数到类CommonProperties存储，在验证过程中异常将异常记录SQL语句记录CheckMethod.listSql,
    /// 异常信息提示记录CheckMethod.listMessage
    /// </summary>
    public class CheckMethod
    {
        /// <summary>
        /// 异常SQL记录
        /// </summary>
        public static List<string> listSql = new List<string>();
        /// <summary>
        /// 异常信息提示记录
        /// </summary>
        public static List<string> listMessage = new List<string>();
        /// <summary>
        /// 方法使用默认值
        /// </summary>
        public static object DefaultObj;
        /// <summary>
        /// 存放异常信息
        /// </summary>
        public static string AbnormalInformation = "";
        /// <summary>
        ///存放异常记录
        /// </summary>
        public static List<UnusualRecord> listUnusualRecord = new List<UnusualRecord>();
        /// <summary>
        /// 是否显示车辆信息标识
        /// </summary>
        public static bool rboolISCarInformation = false;
        /// <summary>
        /// 是否刷卡放行
        /// </summary>
        public static bool rboolIc = false;
        #region  登记校验
        /// <summary>
        /// 验证车辆凭证有效性(是否过期)
        /// </summary>
        public static void ChkSmallTicket()
        {
            try
            {

                //验证前请先给CheckProperTies.ce.carinfo_name(车牌号)赋值,当前为登记保存时赋值，执行完成后，返回checkproperties.ce.isState(凭证是否有效),有效则提示车辆凭证有效，不需要再次登记返回True，反之则返回false，有效不能进行登记，无效则可以再次登记
                //根据车牌号验证该车辆凭证是否有效
                if (CheckProperties.ce.carInfo_Name != "")
                {
                    string sql = "Select * from SmallTicket where SmallTicket_CarInfo_ID=(Select top 1 CarInfo_ID from CarInfo where CarInfo_Name='" + CheckProperties.ce.carInfo_Name + "' order by CarInfo_ID desc)";
                    DataSet dataset = LinQBaseDao.Query(sql);

                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in dataset.Tables[0].Rows)
                        {
                            if (item["SmallTicket_State"].ToString() == "有效")
                            {
                                if (CommonalityEntity.ChkRepeat(CheckProperties.ce.carInfo_Name))
                                {
                                    sql = "select top(1) SortNumberInfo_TongXing,CarType_OtherProperty from View_CarState where CarInfo_Name='" + CheckProperties.ce.carInfo_Name + "' order by CarInfo_ID desc";
                                    dataset = LinQBaseDao.Query(sql);
                                    if (dataset.Tables[0].Rows.Count > 0)
                                    {
                                        string dds = dataset.Tables[0].Rows[0][0].ToString();
                                        if (dds != "已注销" && dds != "已出厂")
                                        {
                                            if (dataset.Tables[0].Rows[0][1].ToString() != "内部车辆")
                                            {
                                                listMessage.Add("不能重复登记该车辆");
                                                return;
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                CheckProperties.ce.IsState = false;
                            }
                        }
                    }
                }
                else
                {
                    listMessage.Add("请输入车牌号");
                    return;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 是否打印小票
        /// </summary>
        public static void IsSmallTicket()
        {
            CheckProperties.ce.isSerialNumber = true;
        }
        /// <summary>
        /// 验证车辆进出凭证(小票)
        /// </summary>
        public static void ChkSerialNumber()
        {
            try
            {

                CheckProperties.ce.isSerialNumber = true;

                //根据取得最大小票序号
                string smallSql = "select top(1) SmallTicket_Serialnumber from SmallTicket where SmallTicket_Position_ID=" + CommonalityEntity.Position_ID + " and SmallTicket_Serialnumber!='' order by SmallTicket_ID desc";
                string css = "000000000000";
                object obj = LinQBaseDao.GetSingle(smallSql);
                if (obj != null)
                {
                    css = obj.ToString();
                }
                int cs = 0;
                //获取当前小票的序号
                if (css.Length >= 12)
                {
                    cs = int.Parse(css.Substring(8, css.Length - 8).ToString());
                }
                if (cs >= 9999)
                {
                    cs = 0;
                }
                //根据当前小票的序号，生成序号
                string number = CommonalityEntity.SortNumber(cs);
                //设置小票号
                //获取服务器时间
                string date = LinQBaseDao.GetSingle("select CONVERT(nvarchar(20),GETDATE(),120)").ToString();
                string year = date.Substring(2, 2);
                string moth = date.Substring(5, 2);
                string day = date.Substring(8, 2);
                string position = CommonalityEntity.Position_Value;
                if (year != "" && moth != "" && position != "" && number != "" && day != "")
                {

                    //组合小票号(年+月+门岗值+自动序号)
                    CheckProperties.ce.serialNumber = year + moth + day + position + number;
                }
                else
                {
                    listMessage.Add("小票生成失败,请重试！");
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 验证凭证有效性(时间)
        /// </summary>
        public static void GetSerialNumber1()
        {
            try
            {
                //验证凭证的有效性，使用前需要为CheckProperties.ce.carType_ID(车辆类型编号)赋值，得到该车辆类型的管控，根据管控确定该车辆类型使用时间或者次数限制，时间则CheckProperties.ce.serTime不为-1，次数则CheckProperties.ce.serCount不为-1，该方法得到凭证的有效性，次数or时间。


                //获取管控详细信息
                //string conSql = "Select * from ControlInfo where ControlInfo_ID in (select ManagementStrategy_ControlInfo_Id from ManagementStrategy where ManagementStrategy_CarType_Id=" + CheckProperties.ce.carType_ID + " and ManagementStrategy_State='启动' and ManagementStrategy_Type='登记') and controlInfo_heightID in(select ControlInfo_ID from ControlInfo where ControlInfo_HeightID=(select ControlInfo_ID from ControlInfo where ControlInfo_IDValue='1202'))";
                string conSql = "select * from ControlInfo where ControlInfo_ID in(select ManagementStrategy_ControlInfo_Id from ManagementStrategy where ManagementStrategy_DriSName  in(select CarType_DriSName from CarType where CarType_ID=" + CheckProperties.ce.carType_ID + " ) and ManagementStrategy_State='启动') and controlInfo_Rule='GetSerialNumber1' and controlInfo_IDValue='060101'";
                List<ControlInfo> list = LinQBaseDao.GetItemsForListing<ControlInfo>(conSql).ToList();//查询执行车辆类型的管控详细信息
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (item.ControlInfo_IDValue == "060101")//时间
                        {
                            CheckProperties.ce.serTime = item.ControlInfo_Value.ToString();
                        }
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod GetSerialNumber():");
            }
        }

        /// <summary>
        /// 是否优先校验
        /// </summary>
        public static void YXCheck()
        {
            CommonalityEntity.ISYX = true;
        }
        /// <summary>
        /// 优先车
        /// </summary>
        public static void YXCar()
        {
            CommonalityEntity.yxcar = true;
        }
        /// <summary>
        /// 优先驾驶员
        /// </summary>
        public static void YXStaffInfo()
        {
            CommonalityEntity.yxstaffinfo = true;
        }
        /// <summary>
        /// 优先公司
        /// </summary>
        public static void YXCustomerInfo()
        {
            CommonalityEntity.yxcustomerinfo = true;
        }

        /// <summary>
        /// 优先车是否自动进门授权
        /// </summary>
        public static void YXINCheck()
        {
            CommonalityEntity.yxincheck = true;
        }


        /// <summary>
        /// 验证凭证有效性(次数)
        /// </summary>
        public static void GetSerialNumber2()
        {
            try
            {
                //验证凭证的有效性，使用前需要为CheckProperties.ce.carType_ID(车辆类型编号)赋值，得到该车辆类型的管控，根据管控确定该车辆类型使用时间或者次数限制，时间则CheckProperties.ce.serTime不为-1，次数则CheckProperties.ce.serCount不为-1，该方法得到凭证的有效性，次数or时间。


                //获取管控详细信息
                string conSql = "select * from ControlInfo where ControlInfo_ID in(select ManagementStrategy_ControlInfo_Id from ManagementStrategy where ManagementStrategy_DriSName  in(select CarType_DriSName from CarType where CarType_ID=" + CheckProperties.ce.carType_ID + " ) and ManagementStrategy_State='启动') and controlInfo_Rule='GetSerialNumber2' and controlInfo_IDValue='060102'";
                List<ControlInfo> list = LinQBaseDao.GetItemsForListing<ControlInfo>(conSql).ToList();//查询执行车辆类型的管控详细信息
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (item.ControlInfo_IDValue == "060102")//次数
                        {
                            CheckProperties.ce.serCount = item.ControlInfo_Value.ToString();
                        }
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod GetSerialNumber():");
            }
        }
        /// <summary>
        /// 是否排队
        /// </summary>
        public static void ISSortNumber()
        {
            CheckProperties.ce.isSort = true;
        }
        /// <summary>
        /// 排队号and sortnumberInfo_PositionValue
        /// </summary>
        public static void GetSortNumber()
        {
            try
            {

                //使用该方法则使用该方法的车辆类型需要排队，必须进行排队校验，使用前需要为CheckProperties.ce.carType_Value(车辆类型值)，SystemClass.PosistionValue(门岗值),判断是否执行了该方法为CheckProperties.ce.isSort 为true执行了该方法，需要排队，反之false 不需要排队，返回CheckProperties.ce.sort_Value为当前排队序号

                if (CheckProperties.ce.isSort)
                {
                    //取得最大排队序号
                    //string sortSql = "Select  top(1)  SortNumberInfo_sortValue from SortNumberInfo where sortNumberInfo_CarTypeValue='" + CheckProperties.ce.carType_Value + "' and  charindex('"+SystemClass.PosistionValue+"',sortnumberinfo_Positionvalue)>0   and SortNumberInfo_sortValue<>0 and SortNumberInfo_sortValue<>''   order by sortNumberInfo_ID desc";
                    string sortSql = "Select  top(1)  SortNumberInfo_sortValue from SortNumberInfo where sortNumberInfo_CarTypeValue='" + CheckProperties.ce.carType_Value + "' and  charindex('" + CommonalityEntity.Position_Value + "',sortnumberinfo_Positionvalue)>0   and SortNumberInfo_sortValue<>0 and SortNumberInfo_sortValue<>''   order by sortNumberInfo_ID desc";
                    if (LinQBaseDao.GetSingle(sortSql) != null)
                    {
                        CheckProperties.ce.sort_Value = int.Parse(LinQBaseDao.GetSingle(sortSql).ToString());
                    }
                    else
                    {
                        CheckProperties.ce.sort_Value = 0;
                    }
                    //排队号
                    if (CheckProperties.ce.sort_Value >= 9999)
                    {
                        CheckProperties.ce.sort_Value = 0;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 校验排队号
        /// </summary>
        public static void ChkSortNumber()
        {
            try
            {

                //校验当前排队号是否为通行排队号，使用前需要给CheckProperties.ce.CarTypeKey(排队校验车辆类型的键值)赋值,得到当前最小排队号，返回CheckProperties.ce.sortValue(排队序号)
                if (!string.IsNullOrEmpty(CheckProperties.ce.CarType_Name))
                {
                    //判断排队号是否为当前最小的排队号
                    // string mixSql = "select Min(SortNumberInfo_SortValue) from SortNumberINfo where  sortNumberInfo_Tongxing='排队中' and SortNumberInfo_SmallTicket_ID in (select SmallTicket_ID from SmallTicket where smallTicket_CarInfo_ID in (select CarInfo_ID from CarInfo where CarInfo_CarType_ID in (select CarType_Id from CarType where CarType_Name='" + CheckProperties.ce.CarType_Name + "')))";
                    string mixSql = "select top(1)  SortNumberInfo_SortValue from View_CarState where  sortNumberInfo_Tongxing='排队中'  and CarType_Name='" + CheckProperties.ce.CarType_Name + "' order by SortNumberInfo_ID ";
                    if (LinQBaseDao.GetSingle(mixSql) != null)
                    {
                        CheckProperties.ce.sortValue = int.Parse(LinQBaseDao.GetSingle(mixSql).ToString());
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 进门排队号校验
        /// </summary>
        public static void ChkInSortNumber()
        {
            try
            {
                if (!string.IsNullOrEmpty(CheckProperties.ce.CarType_Name))
                {
                    //判断排队号是否为当前最小的排队号
                    string mixSql = "select top(1) SortNumberInfo_ID from SortNumberINfo where  sortNumberInfo_Tongxing='排队中' and SortNumberInfo_SmallTicket_ID in (select SmallTicket_ID from SmallTicket where smallTicket_CarInfo_ID in (select CarInfo_ID from CarInfo where CarInfo_CarType_ID in (select CarType_Id from CarType where CarType_Name='" + CheckProperties.ce.CarType_Name + "'))) and SortNumberInfo_DrivewayValue ='" + SystemClass.DrivewayValue + "'and SortNumberInfo_PositionValue='" + SystemClass.PosistionValue + "' order by  SortNumberInfo_ID ";
                    if (LinQBaseDao.GetSingle(mixSql) != null)
                    {
                        CheckProperties.ce.sortValue = int.Parse(LinQBaseDao.GetSingle(mixSql).ToString());
                        string sortv = CheckProperties.dt.Rows[0]["SortNumberInfo_ID"].ToString();
                        if (!string.IsNullOrEmpty(CheckProperties.dt.Rows[0]["SmallTicket_SortNumber"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(sortv))
                            {
                                if (CheckProperties.ce.sortValue < Convert.ToInt32(sortv))
                                {
                                    AbnormalInformation += "进门排队号校验异常,车辆没有按排队顺序进厂" + "\r\n";
                                    listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "进门排队号校验", "车辆没有按排队顺序进厂", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                                }
                            }
                        }

                    }
                }

            }
            catch
            {

            }

        }
        /// <summary>
        /// 是否拍照
        /// </summary>
        public static void ChkPhoto()
        {
            //调用此方法，判断是否需要拍照，如该执行了该方法，则CheckProperties.ce.isImageList为true 反之false
            CheckProperties.ce.isImageList = true;
        }
        /// <summary>
        /// 验证照片有效性
        /// </summary>
        public static void GetPhoto()
        {
            //使用该方法，得到有效的照片信息，使用前需要为CheckProperties.ce.carType_ID(车辆类型编号),SystemClass.PositionID(门岗编号),SystemClass.BaseFile(图片临时路径),SystemClass.PosistionValue(门岗值)赋值，在为该方法配置管控信息时，其默认值为图片的有效时间，返回CheckProperties.ce.Imagelist图片名称集合,无图片则为null
            try
            {
                CommonalityEntity.listCarPic = new List<string>();//存放无效图片
                CommonalityEntity.listCarPicEffective = new List<string>();//存放有效图片
                string[] image = null;

                //根据门岗获取当前门岗的拍照通道值
                string path = "";
                //组合文件夹名称
                path = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + CommonalityEntity.Driveway_Value + "\\";

                //获取指定文件夹下的图片
                image = Directory.GetFiles(path);
                var images = from m in image orderby m descending select m;
                //根据通道编号得到该通道下的摄像机值
                DataSet dataset = LinQBaseDao.Query("select * from Camera where Camera_Driveway_id=" + CommonalityEntity.Driveway_ID + "");
                string stc = "";
                string stc1 = "";

                if (dataset.Tables[0].Rows.Count > 0)
                {
                    if (dataset.Tables[0].Rows.Count == 1)
                    {
                        stc = dataset.Tables[0].Rows[0]["Camera_AddCard"].ToString();
                    }
                    else
                    {
                        stc = dataset.Tables[0].Rows[0]["Camera_AddCard"].ToString();
                        stc1 = dataset.Tables[0].Rows[1]["Camera_AddCard"].ToString();
                    }
                }
                else
                {
                    return;
                }


                //照片有效时间 分钟
                object objDouble;
                if (CommonalityEntity.ISInOut)
                {
                    objDouble = LinQBaseDao.GetSingle("select ControlInfo_Value from ControlInfo where ControlInfo_IDValue='19'");
                }
                else
                {
                    objDouble = LinQBaseDao.GetSingle("select ControlInfo_Value from ControlInfo where ControlInfo_IDValue='26'");
                }
                double doubleMin = 0;
                if (objDouble != null && objDouble != "")
                {
                    doubleMin = double.Parse(objDouble.ToString());
                }
                //验证图片是否有效
                foreach (string pathStr in images)
                {
                    string fileName = pathStr.Substring(pathStr.LastIndexOf('\\') + 1);//图片名称
                    string fileKind = fileName.Substring(fileName.LastIndexOf('.') + 1);//图片后缀
                    //验证是否为图片
                    if (fileKind.ToLower() != "jpg" && fileKind.ToLower() != "jpeg" && fileKind.ToLower() != "png" && fileKind.ToLower() != "gif" && fileKind.ToLower() != "bmp")
                    {
                        continue;
                    }
                    //保存所有的图片名称
                    CheckProperties.ce.AllImageList.Add(fileName);
                    string mi = ImageFile.GetTime(fileName.ToString());//获取时间差

                    if (double.Parse(mi) <= doubleMin)//判断是否过期
                    {
                        if (fileName.Length == 21)//判断图片名称字符长度
                        {

                            string stf = fileName.Substring(16, 1);
                            if (stc == stf || stc1 == stf)
                            {
                                CommonalityEntity.listCarPicEffective.Add(fileName);
                            }
                            else
                            {
                                CommonalityEntity.listCarPic.Add(fileName);//添加过期图片
                            }
                        }
                        else if (fileName.Length == 22)
                        {
                            string stf = fileName.Substring(16, 2);
                            if (stc == stf || stc1 == stf)
                            {
                                CommonalityEntity.listCarPicEffective.Add(fileName);
                            }
                            else
                            {
                                CommonalityEntity.listCarPic.Add(fileName);//添加过期图片
                            }
                        }
                    }
                    else
                    {
                        CommonalityEntity.listCarPic.Add(fileName);//添加过期图片
                    }
                }
                if (CommonalityEntity.listCarPicEffective.Count() <= 0)
                {
                    if (CommonalityEntity.listCarPic.Count <= 0)
                    {
                        CommonalityEntity.strUnusualRecordTable = "CarPic";
                        AbnormalInformation += "车辆照片校验异常,车辆还没有拍照" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆照片校验", "车辆还没有拍照", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                    else
                    {
                        CommonalityEntity.strUnusualRecordTable = "CarPic";
                        AbnormalInformation += "车辆照片校验异常,车辆照片已过期" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆照片校验", "车辆照片已过期", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    //判断照片是否少了 
                    if (CommonalityEntity.listCarPicEffective.Count() < dataset.Tables[0].Rows.Count)
                    {
                        CommonalityEntity.strUnusualRecordTable = "CarPic";
                        AbnormalInformation += "车辆照片校验异常,车辆照片不全" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆照片校验", "车辆照片不全", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog("CheckMethod GetPhoto():" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 修改临时凭证有效性
        /// </summary>
        public static void ChkUpdateCredentials()
        {
            //可以修改凭证有效性
            CheckProperties.ce.ISUpdateCredentials = true;
        }
        /// <summary>
        /// 验证车辆是否黑名单
        /// </summary>
        public static void ChkCarBlack()
        {
            try
            {

                //校验登记车辆是否为黑名单车辆，使用需要为CheckProperties.ce.carInfo_Name(车牌号)赋值,返回CheckProperties.ce.carIsBlack如为true则该车辆为黑名单状态，不能进行登记，提示内容为该车辆处于黑名单，

                //得到车牌号,根据车牌号，得到车辆编号，根据车辆编号，查询黑名单表，判断是否已经存在黑名单记录
                string carSql = "Select * from BlackList where BlackList_CarInfo_ID in (Select Car_ID from Car where Car_Name='" + CheckProperties.ce.carInfo_Name + "')";
                DataSet dataset = LinQBaseDao.Query(carSql);
                foreach (DataRow item in dataset.Tables[0].Rows)
                {
                    if (item["BlackList_State"].ToString() == "拒绝入厂")
                    {
                        listMessage.Add("该车辆处于黑名单，状态:" + item["BlackList_State"].ToString());
                        CheckProperties.ce.carIsBlack = true;
                    }
                    if (item["BlackList_State"].ToString() == "警告")
                    {
                        // listMessage.Add("该车辆处于黑名单，状态:" + item["BlackList_State"].ToString());
                    }
                    if (item["BlackList_State"].ToString() == "通知")
                    {
                        // listMessage.Add("该车辆处于黑名单，状态:" + item["BlackList_State"].ToString());
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 验证人员是否黑名单
        /// </summary>
        public static void ChkStaffBlack()
        {
            try
            {

                //校验登记车辆是否为黑名单车辆，使用需要为CheckProperties.ce.staffInfo_ID(人员编号)赋值,返回CheckProperties.ce.stfIsBlack如为true则该车辆为黑名单状态，不能进行登记，提示内容为该车辆处于黑名单，
                if (CheckProperties.ce.staffInfo_ID != "")
                {
                    //CheckProperties.ce.staffInfo_ID = CheckProperties.ce.staffInfo_ID.Substring(0, CheckProperties.ce.staffInfo_ID.Length - 1);
                    //根据选择的驾驶员，得到驾驶员的编号，根据编号，查询该驾驶员是否黑名单
                    string stfSql = "Select * from BlackList where BlackList_StaffInfo_ID in (" + CheckProperties.ce.staffInfo_ID + ")";
                    DataSet dataset = LinQBaseDao.Query(stfSql);
                    foreach (DataRow item in dataset.Tables[0].Rows)
                    {
                        if (item["BlackList_State"].ToString() == "拒绝入厂")
                        {
                            listMessage.Add("该驾驶员处于黑名单，状态:" + item["BlackList_State"].ToString());
                            CheckProperties.ce.stfIsBlack = true;
                        }
                        if (item["BlackList_State"].ToString() == "警告")
                        {
                            //  listMessage.Add("该驾驶员处于黑名单，状态:" + item["BlackList_State"].ToString());
                        }
                        if (item["BlackList_State"].ToString() == "通知")
                        {
                            // listMessage.Add("该驾驶员处于黑名单，状态:" + item["BlackList_State"].ToString());
                        }
                    }

                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 验证公司是否黑名单
        /// </summary>
        public static void ChkCuBlack()
        {
            try
            {

                //校验登记车辆是否为黑名单车辆，使用需要为CheckProperties.ce.customerInfo_ID(公司编号)赋值,返回CheckProperties.ce.cusIsBlack如为true则该车辆为黑名单状态，不能进行登记，提示内容为该车辆处于黑名单，

                if (CheckProperties.ce.customerInfo_ID != -1)
                {
                    //根据选择的公司，得到公司编号，根据编号，查询该公司是否黑名单
                    string cusSql = "Select * from BlackList where BlackList_CustomerInfo_ID=" + CheckProperties.ce.customerInfo_ID + "";
                    DataSet dataset = LinQBaseDao.Query(cusSql);
                    foreach (DataRow item in dataset.Tables[0].Rows)
                    {
                        if (item["BlackList_State"].ToString() == "拒绝入厂")
                        {
                            listMessage.Add("该公司处于黑名单，状态:" + item["BlackList_State"].ToString());
                            CheckProperties.ce.cusIsBlack = true;
                        }
                        if (item["BlackList_State"].ToString() == "警告")
                        {
                            // listMessage.Add("该公司处于黑名单，状态:" + item["BlackList_State"].ToString());
                        }
                        if (item["BlackList_State"].ToString() == "通知")
                        {
                            // listMessage.Add("该公司处于黑名单，状态:" + item["BlackList_State"].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 验证是否登记国废等级
        /// </summary>
        public static void ChkLevelWaste()
        {
            //是否输入了国废等级
            if (CheckProperties.ce.carInfo_LevelWaste == "")
            {
                listMessage.Add("请选择国废等级!");
                CheckProperties.ce.levelIsWaste = true;
            }
        }


        /// <summary>
        /// 验证人卡有效性
        /// </summary>
        public static void ChkStaffICNumber()
        {
            string str = "";
            try
            {
                #region 人卡
                DataTable StaffInfoDT = LinQBaseDao.Query("select StaffInfo_ID,StaffInfo_Name, StaffInfo_Identity,StaffInfo_Phone, ICCard_ID, ICCard_EffectiveType, ICCard_BeginTime,ICCard_EndTime,ICCard_count,ICCard_HasCount,ICCard_State,StaffInfo_State   from View_StaffInfo_ICCard where  ICCard_Value='" + CommonalityEntity.ICC2 + "'").Tables[0];

                //如果驾驶员信息不为空
                if (StaffInfoDT.Rows.Count > 0)
                {
                    string StaffInfo_ID = StaffInfoDT.Rows[0]["StaffInfo_ID"].ToString();
                    string StaffInfo_Name = StaffInfoDT.Rows[0]["StaffInfo_Name"].ToString();
                    string StaffInfo_Identity = StaffInfoDT.Rows[0]["StaffInfo_Identity"].ToString();
                    string StaffInfo_Phone = StaffInfoDT.Rows[0]["StaffInfo_Phone"].ToString();

                    string ICCard_EffectiveType = StaffInfoDT.Rows[0]["ICCard_EffectiveType"].ToString();
                    string ICCard_count = StaffInfoDT.Rows[0]["ICCard_count"].ToString();
                    string ICCard_HasCount = StaffInfoDT.Rows[0]["ICCard_HasCount"].ToString();
                    string ICCard_State = StaffInfoDT.Rows[0]["ICCard_State"].ToString();
                    string StaffInfo_State = StaffInfoDT.Rows[0]["StaffInfo_State"].ToString();

                    if (ICCard_State != "启动")
                    {
                        str = "IC卡状态为：" + ICCard_State;
                    }
                    if (StaffInfo_State != "启动")
                    {
                        str = "驾驶员状态为：" + StaffInfo_State;
                    }

                    if (ICCard_EffectiveType == "次数")
                    {
                        if (string.IsNullOrEmpty(ICCard_HasCount))
                        {
                            if (ICCard_count == "0" || string.IsNullOrEmpty(ICCard_count))
                            {
                                str = "人卡已过期";
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ICCard_count))
                            {
                                int ct = Convert.ToInt32(ICCard_HasCount);
                                int ht = Convert.ToInt32(ICCard_HasCount);
                                if (ct - ht <= 0)
                                {
                                    str = "人卡已过期";
                                }
                            }
                            else
                            {
                                str = "人卡已过期";
                            }
                        }
                    }
                    if (ICCard_EffectiveType == "有效期")
                    {
                        DateTime ICCard_BeginTime = Convert.ToDateTime(StaffInfoDT.Rows[0]["ICCard_BeginTime"].ToString());
                        DateTime ICCard_EndTime = Convert.ToDateTime(StaffInfoDT.Rows[0]["ICCard_EndTime"].ToString());
                        if (CommonalityEntity.GetServersTime() > ICCard_BeginTime && CommonalityEntity.GetServersTime() < ICCard_EndTime)
                        {
                            TimeSpan th = ICCard_EndTime - CommonalityEntity.GetServersTime();
                            int s = Convert.ToInt32(th.TotalHours);
                        }
                        else
                        {
                            str = "人卡已过期";
                        }
                    }
                }
                #endregion
            }
            catch
            {
                CommonalityEntity.WriteTextLog("验证人卡 ChkStaffICNumber()");
            }
            AbnormalInformation += str + "\r\n";
        }

        /// <summary>
        /// 验证车卡有效性
        /// </summary>
        public static void ChkCarICNumber()
        {
            string str = "";
            try
            {
                #region 车卡
                DataTable carDT = LinQBaseDao.Query("select ICCard_ID, ICCard_EffectiveType, ICCard_BeginTime,ICCard_EndTime,ICCard_count,ICCard_HasCount ,ICCard_State,Car_Name,Car_State  from View_Car_ICard_CarType   where ICCard_Value='" + CommonalityEntity.ICC1 + "' ").Tables[0];
                //如果车辆基础信息不为空
                if (carDT.Rows.Count > 0)
                {
                    string Car_Name = carDT.Rows[0]["Car_Name"].ToString();
                    string ICCard_EffectiveType = carDT.Rows[0]["ICCard_EffectiveType"].ToString();

                    string ICCard_count = carDT.Rows[0]["ICCard_count"].ToString();
                    string ICCard_HasCount = carDT.Rows[0]["ICCard_HasCount"].ToString();
                    string ICCard_State = carDT.Rows[0]["ICCard_State"].ToString();
                    string Car_State = carDT.Rows[0]["Car_State"].ToString();

                    if (ICCard_State != "启动")
                    {
                        str = "IC卡状态为：" + ICCard_State;
                    }
                    if (Car_State != "启动")
                    {
                        str = "车辆状态为：" + Car_State;
                    }
                    if (ICCard_EffectiveType == "次数")
                    {
                        if (string.IsNullOrEmpty(ICCard_HasCount))
                        {
                            if (ICCard_count == "0" || string.IsNullOrEmpty(ICCard_count))
                            {
                                str = "车卡已过期";
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ICCard_count))
                            {
                                int ct = Convert.ToInt32(ICCard_HasCount);
                                int ht = Convert.ToInt32(ICCard_HasCount);
                                if (ct - ht <= 0)
                                {
                                    str = "车卡已过期";
                                }
                            }
                            else
                            {
                                str = "车卡已过期";
                            }
                        }
                    }
                    if (ICCard_EffectiveType == "有效期")
                    {
                        DateTime ICCard_BeginTime = Convert.ToDateTime(carDT.Rows[0]["ICCard_BeginTime"].ToString());
                        DateTime ICCard_EndTime = Convert.ToDateTime(carDT.Rows[0]["ICCard_EndTime"].ToString());
                        if (CommonalityEntity.GetServersTime() > ICCard_BeginTime && CommonalityEntity.GetServersTime() < ICCard_EndTime)
                        {
                            TimeSpan th = ICCard_EndTime - CommonalityEntity.GetServersTime();
                            int s = Convert.ToInt32(th.TotalHours);
                        }
                        else
                        {
                            str = "车卡已过期";
                        }
                    }
                }
                #endregion
            }
            catch
            {
                CommonalityEntity.WriteTextLog("验证车卡 ChkCarICNumber()");
            }
            AbnormalInformation += str + "\r\n";

        }
        /// <summary>
        /// 车辆进出凭证(车卡)
        /// </summary>
        public static void ChkCarIcNumber()
        {
            CommonalityEntity.carICHave = true;
        }
        /// <summary>
        /// 车辆进出凭证(人卡)
        /// </summary>
        public static void ChkUserIcNumber()
        {
            CommonalityEntity.UserICHave = true;
        }

        /// <summary>
        /// 车辆进出凭证(保安卡)
        /// </summary>
        public static void ChkEnsureSafetyIcNumber()
        {
            CommonalityEntity.EnsureSafetyICHave = true;
            ICMethod();
        }


        /// <summary>
        /// 验证该登记车辆通行策略是否完成
        /// </summary>
        public static void ChkRepeat()
        {
            try
            {

                string sql = "select top 1 (CarInfo_ID) from CarInfo where CarInfo_name='" + CheckProperties.ce.carInfo_Name + "' order by carInfo_time desc";
                object obj = LinQBaseDao.GetSingle(sql);
                if (obj != null)
                {
                    string inOutSql = "Select * from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + obj.ToString() + "";
                    DataSet dataset = LinQBaseDao.Query(inOutSql);
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in dataset.Tables[0].Rows)
                        {
                            try
                            {
                                object name = item["CarInOutRecord_IsFulfIll"];
                                if (item["CarInOutRecord_IsFulfIll"].ToString() == "false")
                                {
                                    CheckProperties.ce.IsManagerFl = false;
                                    listMessage.Add("通行策略未完成，不能重复登记");
                                }
                                else
                                {
                                    CheckProperties.ce.IsManagerFl = true;
                                }

                            }
                            catch
                            {
                                CommonalityEntity.WriteTextLog("ChkRepeat()");
                            }
                        }
                    }
                    else
                    {
                        CheckProperties.ce.IsManagerFl = true;
                    }
                }
                else
                {
                    //CheckProperties.ce.IsManagerFl = false;
                    //listMessage.Add("通行策略未完成，不能重复登记");
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 验证送货车辆是否输入PO号
        /// </summary>
        public static void ChkSongHuo()
        {
            ChkSapSongHuo();
        }
        /// <summary>
        /// 验证成品车辆是否输入车牌号
        /// </summary>
        public static void ChkChengPin()
        {
            ChkSapChengPin();
        }
        /// <summary>
        /// 验证三废车辆是否输入交货单号
        /// </summary>
        public static void ChkSanFei()
        {
            ChkSapSanFei();
        }
        /// <summary>
        /// 校验装货通知单
        /// </summary>
        public static void ChkSapOflag()
        {
            try
            {

                if (CheckProperties.ce.carInfo_Name == "")
                {
                    listMessage.Add("成品车辆装货通知单校验失败!" + "\r\n");
                    CheckProperties.ce.IsOflag = false;
                }
                else
                {
                    SAPClass de = new SAPClass();
                    Thread thread = new Thread(new ThreadStart(de.ChengPin));
                    de.LV_CARNO = CheckProperties.ce.carInfo_Name;
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();

                    if (de.Table2 != null)
                    {
                        if (de.Table2.Rows[0]["type"].ToString().ToUpper() == "S")
                        {
                            if (de.Table1.Rows[0]["O_FLAG"].ToString().ToUpper() == "X")//"X"已开装货通知单
                            {
                                CheckProperties.ce.IsOflag = true;
                            }
                            else
                            {
                                CheckProperties.ce.IsOflag = false;
                                listMessage.Add("请开装货通知单" + "\r\n");
                            }
                        }
                        else
                        {
                            listMessage.Add("成品车辆装货通知单校验失败," + de.Table2.Rows[0]["type"].ToString().ToUpper() + de.Table2.Rows[0]["message"].ToString() + "\r\n");
                        }
                    }
                    else
                    {
                        listMessage.Add("连接失败!");
                        CheckProperties.ce.IsOflag = false;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 送货车辆SAP效验
        /// </summary>
        public static void ChkSapSongHuo()
        {
            try
            {

                CheckProperties.ce.IsSongHuo = true;
                string str = "";
                if (CommonalityEntity.ISSapCheck(CheckProperties.ce.CarType_Name, CheckProperties.ce.SongHuoNumber, out str))
                {
                    CheckProperties.ce.SapISCheck = true;
                }
                else
                {
                    listMessage.Add(str + "\r\n");
                    return;
                }


                SAPClass de = new SAPClass();
                Thread thread = new Thread(new ThreadStart(de.SendGoods));
                de.LV_EBELN = CheckProperties.ce.SongHuoNumber;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                if (de.Table2 != null)
                {
                    CheckProperties.ce.SapSongHuoTable2 = de.Table2;
                    if (de.Table1 != null)
                    {
                        if (de.Table1.Rows.Count >= 1)//登记人选择项
                        {
                            CheckProperties.ce.SapSongHuoTable = de.Table1;
                        }
                        else
                        {
                            listMessage.Add("SAP无返回值");
                            return;
                        }
                    }
                    else
                    {
                        listMessage.Add("SAP无返回值");
                        return;
                    }
                }
                else
                {
                    listMessage.Add("连接失败");
                    return;
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// SAP成品车辆效验
        /// </summary>
        public static void ChkSapChengPin()
        {
            try
            {
                CheckProperties.ce.IsChengPin = true;
                if (CommonalityEntity.ChkRepeat(CheckProperties.ce.ChengPinNumber))
                {
                    listMessage.Add("不能重复登记该车辆");
                    return;
                }
                SAPClass de = new SAPClass();
                Thread thread = new Thread(new ThreadStart(de.ChengPin));
                de.LV_CARNO = CheckProperties.ce.ChengPinNumber;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                if (de.Table2 != null)
                {
                    CheckProperties.ce.SapChengPinTable2 = de.Table2;
                    if (de.Table1 != null)
                    {
                        if (de.Table1.Rows.Count >= 1)//登记人选择项
                        {
                            CheckProperties.ce.SapChengPinTable = de.Table1;
                        }
                        else
                        {
                            listMessage.Add("SAP无返回值");
                            return;
                        }
                    }
                    else
                    {
                        listMessage.Add("SAP无返回值");
                        return;
                    }
                }
                else
                {
                    listMessage.Add("连接失败");
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// SAP三废车辆效验
        /// </summary>
        public static void ChkSapSanFei()
        {
            try
            {
                CheckProperties.ce.IsSangFei = true;
                SAPClass de = new SAPClass();
                Thread thread = new Thread(new ThreadStart(de.SanFei));
                de.LV_VBELN = CheckProperties.ce.SangFeiNumber;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                if (de.Table2 != null)
                {
                    CheckProperties.ce.SapSangFeiTable2 = de.Table2;
                    if (de.Table1 != null)
                    {
                        if (de.Table1.Rows.Count >= 1)//登记人选择项
                        {
                            CheckProperties.ce.SapSangFeiTable = de.Table1;
                        }
                        else
                        {
                            listMessage.Add("SAP无返回值");
                            return;
                        }
                    }
                    else
                    {
                        listMessage.Add("SAP无返回值");
                        return;
                    }
                }
                else
                {
                    listMessage.Add("连接失败");
                    return;
                }
            }
            catch (Exception)
            {

            }
        }
        #region  车辆进出厂
        /// <summary>
        /// 车辆登记Sap校验(成品车辆进厂校验)
        /// </summary>
        public static void ChkSapFinished()
        {
            try
            {

                if (CommonalityEntity.GetSAP(CommonalityEntity.SapNumber, "I", CheckProperties.ce.carInfo_Name, CheckProperties.ce.serialNumber, "1"))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    listMessage.Add("SAP验证未通过!" + "\r\n");
                    CheckProperties.ce.IsSap = false;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 车辆登记Sap校验(送货车辆进厂校验)
        /// </summary>
        public static void ChkSapDelivery()
        {
            try
            {

                if (CommonalityEntity.GetSAP(CommonalityEntity.SapNumber, "I", CheckProperties.ce.carInfo_Name, CheckProperties.ce.serialNumber, "1"))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    listMessage.Add("SAP验证未通过!" + "\r\n");
                    CheckProperties.ce.IsSap = false;
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 车辆登记Sap校验(三废车辆进厂校验)
        /// </summary>
        public static void ChkSapWastes()
        {
            try
            {

                if (CommonalityEntity.GetSAP(CommonalityEntity.SapNumber, "I", CheckProperties.ce.carInfo_Name, CheckProperties.ce.serialNumber, "1"))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    listMessage.Add("SAP验证未通过!" + "\r\n");
                    CheckProperties.ce.IsSap = false;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
        /// <summary>
        /// SAP送货车辆校验43 45
        /// </summary>
        public static void ChkSongHuoFour()
        {
            //if (CheckProperties.ce.SongHuoNumber.Length >= 2)
            //{
            //    string number = CheckProperties.ce.SongHuoNumber.Substring(0, 2);
            //    if (number == "45")
            //    {
            //        string sql = "select * from ManagementStrategy where ManagementStrategy_ControlInfo_ID in(Select ControlInfo_ID from ControlInfo where ControlInfo_Name='SAP送货车辆校验') and ManagementStrategy_Value='45'";
            //        CheckProperties.ce.FourList = LinQBaseDao.GetItemsForListing<ManagementStrategy>(sql).ToList();
            //    }
            //    if (number == "43")
            //    {
            //        string sql = "select * from ManagementStrategy where ManagementStrategy_ControlInfo_ID in(Select ControlInfo_ID from ControlInfo where ControlInfo_Name='SAP送货车辆校验') and ManagementStrategy_Value='43'";
            //        CheckProperties.ce.ThrList = LinQBaseDao.GetItemsForListing<ManagementStrategy>(sql).ToList();
            //    }
            //}
            //else//生产物料车
            //{
            //    string sql = "select * from ManagementStrategy where ManagementStrategy_ControlInfo_ID in(Select ControlInfo_ID from ControlInfo where ControlInfo_Name='SAP送货车辆校验') and ManagementStrategy_Value='45'";
            //    CheckProperties.ce.FourList = LinQBaseDao.GetItemsForListing<ManagementStrategy>(sql).ToList();
            //}
        }
        #endregion

        //SAP登记保存成品车辆单号校验
        public static void ISInCheckSapSave()
        {
            try
            {
                string str = "";
                if (CommonalityEntity.ISSapCheck(CheckProperties.ce.CarType_Name, CheckProperties.ce.ChengPinNumber, out str))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    CheckProperties.ce.IsSap = false;
                    listMessage.Add(str + "\r\n");
                    return;
                }
            }
            catch (Exception)
            {

            }
        }
        # region   成品车进门Sap校验
        //SAP成品车辆进门单号校验
        public static void ISInCheckNumber()
        {
            try
            {
                string str = "";
                if (CommonalityEntity.ISSapCheck(CommonalityEntity.CarType_Name, CheckProperties.ce.ChengPinNumber, out str))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    CheckProperties.ce.IsSap = false;
                    listMessage.Add(str + "\r\n");
                    return;
                }
            }
            catch (Exception)
            {

            }
        }

        // SAP成品车辆进门放行校验
        public static void ISInChengPin()
        {
            ChkSapFinished();
        }
        #endregion

        #region 成品车出门Sap校验
        //SAP成品车辆进门单号校验
        public static void ISOutCheckNumber()
        {
            try
            {
                string str = "";
                if (CommonalityEntity.ISSapCheck(CommonalityEntity.CarType_Name, CheckProperties.ce.ChengPinNumber, out str))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    CheckProperties.ce.IsSap = false;
                    listMessage.Add(str + "\r\n");
                    return;
                }
            }
            catch (Exception)
            {

            }
        }

        // SAP成品车辆出门门放行校验
        public static void ISOutChengPin()
        {
            try
            {
                if (CommonalityEntity.GetSAP(CommonalityEntity.SapNumber, "C", CheckProperties.ce.carInfo_Name, CheckProperties.ce.serialNumber, "1"))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    listMessage.Add("SAP验证未通过!" + "\r\n");
                    CheckProperties.ce.IsSap = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 送货车辆进门Sap校验
        // SAP送货车辆进门单号校验
        public static void ISInSHCheckNumber()
        {
            try
            {

                string str = "";
                if (CommonalityEntity.ISSapCheck(CommonalityEntity.CarType_Name, CheckProperties.ce.SongHuoNumber, out str))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    CheckProperties.ce.IsSap = false;
                    listMessage.Add(str + "\r\n");
                    return;
                }
            }
            catch (Exception)
            {

            }
        }

        // SAP送货车辆进门放行校验
        public static void ISInSonghuo()
        {
            ChkSapDelivery();
        }
        #endregion

        #region 三废车辆进门Sap校验
        // SAP三废车辆进门单号校验
        public static void ISInSFCheckNumber()
        {
            try
            {

                string str = "";
                if (CommonalityEntity.ISSapCheck(CheckProperties.ce.CarType_Name, CheckProperties.ce.SangFeiNumber, out str))
                {
                    CheckProperties.ce.IsSap = true;
                }
                else
                {
                    CheckProperties.ce.IsSap = false;
                    listMessage.Add(str + "\r\n");
                    return;
                }
            }
            catch (Exception)
            {

            }
        }

        // SAP三废车辆进门放行校验
        public static void ISInSanFei()
        {
            ChkSapWastes();
        }
        #endregion

        #region 校验信息
        /// <summary>
        /// 呼叫校验
        /// </summary>
        public static void CallCheckMethod()
        {
            try
            {
                string sortxing = CheckProperties.dt.Rows[0]["SortNumberInfo_TongXing"].ToString();
                if (sortxing != "待通行")
                {
                    if (sortxing == "排队中")
                    {
                        CheckProperties.ce.IsHuJiao = false;
                        AbnormalInformation += "该车辆没有呼叫！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "呼叫校验", "该车辆没有呼叫！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                    else
                    {
                        if (Convert.ToInt32(CheckProperties.dt.Rows[0]["SmallTicket_Allowcounted"].ToString()) >= Convert.ToInt32(CheckProperties.dt.Rows[0]["SmallTicket_Allowcount"].ToString()))
                        {
                            CheckProperties.ce.IsHuJiao = false;
                            AbnormalInformation += "该车辆" + sortxing + "！" + "\r\n";
                            listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "呼叫校验", "该车辆" + sortxing + "！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                        }
                    }
                }
                else
                {
                    CheckProperties.ce.IsHuJiao = true;
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("CheckMethod.CallCheckMethod()");
            }
        }
        /// <summary>
        /// 呼叫后到进厂时间效验
        /// </summary>
        /// <returns></returns>
        public static void ValTime()
        {
            try
            {
                //系统当前时间
                DateTime newTime = CommonalityEntity.GetServersTime();
                //获取呼叫计时时间
                string ControlInfo_Value = LinQBaseDao.GetSingle("select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='ValTime'").ToString();
                string oldTimeStr = CheckProperties.dt.Rows[0]["SortNumberInfo_CallTime"].ToString();
                if (!string.IsNullOrEmpty(CheckProperties.dt.Rows[0]["SmallTicket_SortNumber"].ToString()))
                {
                    string caltime = CheckProperties.dt.Rows[0]["SortNumberInfo_CallTime"].ToString();
                    if (caltime == "")
                    {
                        if (DateTime.Parse(caltime) > DateTime.Parse(CheckProperties.dt.Rows[0]["SortNumberInfo_Time"].ToString()))
                        {
                            //获取两个时间间隔的分钟数
                            TimeSpan ts = Convert.ToDateTime(oldTimeStr).AddMinutes(Convert.ToInt32(ControlInfo_Value)) - newTime;
                            bool istrue = ts.ToString().Contains('-');
                            //获取系统
                            if (istrue)
                            {
                                AbnormalInformation += "该车辆呼叫后到进厂时间超时" + "\r\n";
                                listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, " 呼叫后到进厂时间效验", "该车辆呼叫后到进厂时间超时！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                            }
                        }
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoForm.ValTime()");
            }

        }
        /// <summary>
        /// 地感校验(调用前，刷卡的时候，
        /// 需要得到设备管理表中的刷卡通道赋值给
        /// CommonalityEntity.Driveway_Value)
        /// 刷小票的时候，根据小票获取小票对应
        /// 通行策略的通道值赋值
        /// </summary>
        public static void CameraCheck()
        {
            string strbox = "无地感，请检查！！！！" + "\r\n";
            try
            {
                string strsql = String.Format("select * from DeviceControl where DeviceControl_positionValue='" + SystemClass.PosistionValue + "' and DeviceControl_DrivewayValue='" + CommonalityEntity.Driveway_Value + "'");
                List<DeviceControl> list = LinQBaseDao.GetItemsForListing<DeviceControl>(strsql).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (item.DeviceControl_FanSate != "1")
                        {
                            AbnormalInformation += strbox;
                            listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "无地感", strbox, CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                        }
                    }
                }
                else
                {
                    AbnormalInformation += strbox;
                    listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "无地感", strbox, CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.CameraCheck()");
            }
        }
        /// <summary>
        /// 授权校验
        /// </summary>
        public static void RightCheckMethod()
        {
            string strInOut = "";
            try
            {
                if (CommonalityEntity.ISInOut)
                {
                    if (CheckProperties.dt.Rows[0]["CarInOutRecord_InCheck"].ToString() != "是")
                    {
                        AbnormalInformation += "该车辆没有进门授权！！！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, strInOut, strInOut, CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (CheckProperties.dt.Rows[0]["CarInOutRecord_OutCheck"].ToString() != "是")
                    {
                        AbnormalInformation += "该车辆没有出门授权！！！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, strInOut, strInOut, CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.RightCheckMethod()");
            }
        }
        /// <summary>
        /// 读车辆信息
        /// </summary>
        public static void SetCarInfoMethod()
        {
            try
            {
                CheckMethod.rboolISCarInformation = true;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.AddCarInOutRecordMethod()");
            }
        }

        /// <summary>
        /// 车辆类型有效性校验
        /// </summary>
        public static void CarTypeValidityCheckMethod()
        {
            try
            {
                int intSmallTicket_Allowcounted = 0;
                string strsql = String.Format("select * from CarType where CarType_ID={0}", CommonalityEntity.GetInt(CheckProperties.dt.Rows[0]["CarType_ID"].ToString()));
                var p = LinQBaseDao.Query(strsql).Tables[0].AsEnumerable();
                if (p.Where(n => n.Field<string>("CarType_Validity") == " 临时").Count() > 0)
                {
                    foreach (var tem in p)
                    {
                        if (tem.Field<int>("CarType_ValidityValue").ToString() != "0")
                        {
                            int intCarType_ValidityValue = CommonalityEntity.GetInt(tem.Field<int>("CarType_ValidityValue").ToString());
                            if (tem.Field<string>("CarType_ValidityTemporary").Contains("小时"))
                            {
                                if (CheckProperties.dt.Rows[0]["CarInfo_Time"] != null)//登记时间
                                {
                                    if (!TimeMethod(CheckProperties.dt.Rows[0]["CarInfo_Time"].ToString(), intCarType_ValidityValue.ToString(), "", true))
                                    {
                                        AbnormalInformation += "该车辆类型允许通行时间已超过设置值,需要重新登记！" + "\r\n";
                                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆类型有效性校验", "该车辆允许通行时间已超过设置值,需要重新登记！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                                    }
                                }
                            }
                            if (tem.Field<string>("CarType_ValidityTemporary").Contains("次数"))
                            {
                                if (CheckProperties.dt.Rows[0]["SmallTicket_Allowcounted"] != null)//已进厂次数
                                {
                                    intSmallTicket_Allowcounted = CommonalityEntity.GetInt(CheckProperties.dt.Rows[0]["SmallTicket_Allowcounted"].ToString());
                                }


                                if (intSmallTicket_Allowcounted >= intCarType_ValidityValue && intCarType_ValidityValue > 0)
                                {
                                    CommonalityEntity.rboolRelease = false;
                                    AbnormalInformation += "该车辆类型允许通行次数已超过设置值,需要重新登记！" + "\r\n";
                                    listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆类型有效性校验", "该车辆允许通行次数已超过设置值,需要重新登记！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                                }
                            }
                        }

                    }

                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("CheckMethod.CarTypeValidityCheckMethod()");
            }

        }
        /// <summary>
        ///刷卡放行校验
        /// </summary>
        public static void ICMethod()
        {
            try
            {
                rboolIc = true;
            }
            catch
            {

                CommonalityEntity.WriteTextLog("CheckMethod.ICMethod()");
            }
        }
        /// <summary>
        ///车辆状态校验
        /// </summary>
        public void CarRegistrationCheckMethod()
        {
            try
            {
                if (CheckProperties.dt.Rows[0]["CarInfo_State"] != null)
                {
                    if (CheckProperties.dt.Rows[0]["CarInfo_State"].ToString() != "启动")
                    {
                        CommonalityEntity.rboolRelease = false;
                        AbnormalInformation += "该车辆登记状态未启动！请启动！！不能放行！！！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆登记状态校验", "该车辆登记状态未启动！请启动", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                if (CheckProperties.dt.Rows[0]["SortNumberInfo_TongXing"].ToString() == "已注销")
                {
                    CommonalityEntity.rboolRelease = false;
                    AbnormalInformation += "该车辆通行状态已注销！不能放行！！！" + "\r\n";
                    listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆通行状态校验", "该车辆通行状态已注销！不能放行", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                }
                if (CommonalityEntity.ISInOut)
                {
                    if (CheckProperties.dt.Rows[0]["SortNumberInfo_TongXing"].ToString() == "已进厂")
                    {
                        AbnormalInformation += "该车辆通行状态已进厂！不能放行！！！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆通行状态校验", "该车辆通行状态已进厂！不能放行", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (CheckProperties.dt.Rows[0]["SortNumberInfo_TongXing"].ToString() == "已出厂")
                    {
                        AbnormalInformation += "该车辆通行状态已出厂！不能放行！！！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆通行状态校验", "该车辆通行状态已出厂！不能放行", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.CarRegistrationCheckMethod()");
            }

        }


        /// <summary>
        /// 凭证校验
        /// </summary>
        /// <returns></returns>
        public void SmallTicketCheckMethod()
        {

            try
            {
                int intSmallTicket_Allowcounted = 0;
                int intSmallTicket_Allowcount = 0;
                int intSmallTicket_Allowhour = 0;
                if (CheckProperties.dt.Rows[0]["SmallTicket_Allowcounted"] != null)//已进厂次数
                {
                    intSmallTicket_Allowcounted = CommonalityEntity.GetInt(CheckProperties.dt.Rows[0]["SmallTicket_Allowcounted"].ToString());
                }
                if (CheckProperties.dt.Rows[0]["SmallTicket_Allowcount"] != null)//允许进厂次数
                {
                    intSmallTicket_Allowcount = CommonalityEntity.GetInt(CheckProperties.dt.Rows[0]["SmallTicket_Allowcount"].ToString());
                    if (intSmallTicket_Allowcount > 0)
                    {

                        if (intSmallTicket_Allowcounted >= intSmallTicket_Allowcount && intSmallTicket_Allowcount > 0)
                        {
                            CommonalityEntity.rboolRelease = false;
                            AbnormalInformation += "该车辆允许通行次数已超过设置值,需要重新登记！" + "\r\n";
                            listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "凭证校验异常", "该车辆允许通行次数已超过设置值,需要重新登记！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                        }
                    }
                }
                if (CheckProperties.dt.Rows[0]["SmallTicket_Allowhour"] != null)//允许进厂时间
                {
                    if (CheckProperties.dt.Rows[0]["SmallTicket_Allowhour"].ToString() != "0")//允许进厂时间
                    {
                        if (CheckProperties.dt.Rows[0]["CarInOutRecord_Time"] != null)//登记时间
                        {
                            DateTime oldtime = Convert.ToDateTime(CheckProperties.dt.Rows[0]["CarInOutRecord_Time"].ToString());
                            int i = Convert.ToInt32(CheckProperties.dt.Rows[0]["SmallTicket_Allowhour"].ToString());
                            DateTime nowtime = CommonalityEntity.GetServersTime();

                            TimeSpan t = oldtime.AddHours(i) - nowtime;
                            bool istrue = t.ToString().Contains('-');
                            if (istrue)
                            {
                                AbnormalInformation += "该车辆允许通行时间已超过设置值,需要重新登记！" + "\r\n";
                                listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "凭证校验异常", "该车辆允许通行时间已超过设置值,需要重新登记！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                            }
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.SmallTicketCheckMethod()");
            }

        }

        /// <summary>
        /// 排队校验
        /// </summary>
        public static void QueueCheck()
        {
            try
            {
                if (CheckProperties.dt.Rows[0]["SortNumberInfo_Remark"] != null)
                {
                    if (CheckProperties.dt.Rows[0]["SortNumberInfo_Remark"].ToString() == "异常呼叫")
                    {
                        AbnormalInformation += "该车辆异常呼叫,请检查车辆排队序号!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "排队校验异常", "该车辆异常呼叫,请检查车辆排队序号!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.QueueCheck()");
            }
        }

        /// <summary>
        /// 出门当前通行策略校验
        /// </summary>
        public static void DrivewayStrategyCheckMethod()
        {

        }

        /// <summary>
        /// 开始时间与当前时间差是否大于设置时间 或开始时间与结束时间差是否大于设置时间
        /// </summary>
        /// <param name="Begin_Time">开始时间</param>
        /// <param name="Setup_Time">有效时间值</param>
        /// <param name="End_Time">结束时间</param>
        /// <param name="rboolTime">TRUE :开始时间与当前时间差是否大于设置时间 FALSE: 开始时间与结束时间差是否大于设置时间</param>
        /// <returns></returns>
        private static bool TimeMethod(string Begin_Time, string Setup_Time, string End_Time, bool rboolTime)
        {
            bool rbool = true;
            try
            {

                string passtime1 = null;
                if (rboolTime)
                {
                    passtime1 = CommonalityEntity.GetServersTime().ToString();
                }
                else
                {
                    passtime1 = End_Time;
                }
                DateTime d1 = DateTime.Parse(Begin_Time);
                DateTime d2 = DateTime.Parse(passtime1);

                TimeSpan t = d2 - d1;

                long minute = (long)t.TotalMinutes;

                if (minute > long.Parse(Setup_Time))
                {
                    rbool = false;
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.TimeMethod()");
            }
            return rbool;
        }

        /// <summary>
        /// 车辆打印发货单到最后出门时间校验、
        /// </summary>
        public static void CarInvoice_EndTimeMethod()
        {
            try
            {
                string strBusinessRecord_PrintinvoiceTime = "";
                string strControlInfo_Value = "";

                DataTable dt = LinQBaseDao.Query(" select BusinessRecord_PrintinvoiceTime from BusinessRecord where BusinessRecord_CarInOutRecord_ID=" + CommonalityEntity.CarInoutid + " and BusinessRecord_Type='" + CommonalityEntity.loadSecondWeight + "' ").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    strBusinessRecord_PrintinvoiceTime = dt.Rows[0][0].ToString();
                    if (!string.IsNullOrEmpty(strBusinessRecord_PrintinvoiceTime))
                    {
                        DataTable dtct = LinQBaseDao.Query("select ControlInfo_Value from ControlInfo where ControlInfo_Rule='CarInvoice_EndTimeMethod'").Tables[0];
                        if (dtct.Rows.Count > 0)
                        {
                            strControlInfo_Value = dtct.Rows[0][0].ToString();
                            if (!TimeMethod(strBusinessRecord_PrintinvoiceTime, strControlInfo_Value, "", true))
                            {
                                AbnormalInformation += "车辆从完成打印发货单到出门的时间出现异常,已超过设定值!" + "\r\n";
                                listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆打印发货单到最后出门时间校验", "车辆从完成打印发货单到出门的时间出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                            }
                        }
                    }
                    else
                    {
                        AbnormalInformation += "车辆打印发货单到最后出门时间校验,没有打印发货单!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆打印发货单到最后出门时间校验", "车辆从完成打印发货单到出门的时间出现异常,没有打印发货单", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    AbnormalInformation += "车辆打印发货单到最后出门时间校验,没有打印发货单!" + "\r\n";
                    listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆打印发货单到最后出门时间校验", "车辆从完成打印发货单到出门的时间出现异常,没有打印发货单!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.CarInvoice_EndTimeMethod()");
            }
        }

        /// <summary>
        /// 车辆进门到最后出门时间校验、
        /// </summary>
        public static void CarIn_EndTimeMethod()
        {
            string strCarInOutRecord_Time = "";
            string strControlInfo_Value = "";
            try
            {
                strCarInOutRecord_Time = CheckProperties.dt.Rows[0]["CarInOutRecord_InTime"].ToString();

                string strsql = "select CarType_InOutTime from CarType where CarType_Name='" + CheckProperties.dt.Rows[0]["CarType_Name"].ToString() + "'";
                strControlInfo_Value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!TimeMethod(strCarInOutRecord_Time, strControlInfo_Value, "", true))
                {
                    AbnormalInformation += "车进门到出门时间出现异常,已超过设定值!" + "\r\n";
                    listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆进门到最后出门时间校验", "车进门到出门时间出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.CarIn_EndTimeMethod()");
            }
        }
        /// <summary>
        /// 车辆从打印发货单到地磅时间校验
        /// </summary>
        public static void CarInvoice_LoadometerTimeMethod()
        {
            try
            {
                string strBusinessRecord_PrintinvoiceTime = "";
                string strBusinessRecord_WeightTime = "";
                string strControlInfo_Value = "";

                string strsql = String.Format("select * from BusinessRecord where BusinessRecord_CarInOutRecord_ID={0}", CommonalityEntity.CarInoutid);
                var p = LinQBaseDao.Query(strsql).Tables[0].AsEnumerable();
                var p1 = p.Where(n => n.Field<string>("BusinessRecord_Type") == CommonalityEntity.loadSecondWeight);
                foreach (var m in p1)
                {
                    if (m.Field<DateTime>("BusinessRecord_PrintinvoiceTime") != null)
                    {
                        strBusinessRecord_PrintinvoiceTime = m.Field<DateTime>("BusinessRecord_PrintinvoiceTime").ToString();
                        break;
                    }
                }
                var p2 = p.Where(n => n.Field<string>("BusinessRecord_Type") == CommonalityEntity.outWeight);
                foreach (var m in p2)
                {
                    if (m.Field<DateTime>("BusinessRecord_WeightTime") != null)//出门地磅时间
                    {
                        strBusinessRecord_WeightTime = m.Field<DateTime>("BusinessRecord_WeightTime").ToString();
                        break;
                    }
                }
                if (string.IsNullOrEmpty(strBusinessRecord_PrintinvoiceTime) || string.IsNullOrEmpty(strBusinessRecord_WeightTime))
                {
                    if (string.IsNullOrEmpty(strBusinessRecord_PrintinvoiceTime))
                    {
                        AbnormalInformation += "车辆从打印发货单到地磅时间出现异常,没有打印发货单！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从打印发货单到地磅时间校验", "车辆从打印发货单到地磅时间校验出现异常,没有打印发货单！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                    else
                    {
                        AbnormalInformation += "车辆从打印发货单到地磅时间出现异常,没有出门地磅信息！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从打印发货单到地磅时间校验", "车辆从打印发货单到地磅时间校验出现异常,没有出门地磅信息！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='CarInvoice_LoadometerTimeMethod'";
                    strControlInfo_Value = LinQBaseDao.GetSingle(strsql).ToString();
                    if (!TimeMethod(strBusinessRecord_PrintinvoiceTime, strControlInfo_Value, strBusinessRecord_WeightTime, false))
                    {
                        AbnormalInformation += "车辆从打印发货单到地磅时间出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从打印发货单到地磅时间校验", "车辆从打印发货单到地磅时间出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.CarInvoice_LoadometerTimeMethod()");
            }
        }
        /// <summary>
        /// 车辆出门地磅到出门时间校验
        /// </summary>
        public static void CarLoadometer_OutTimeMethod()
        {
            try
            {
                string strBusinessRecord_WeightTime = ""; ;
                string strControlInfo_Value = "";
                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.outWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_WeightTime != null)//出门地磅时间
                    {
                        strBusinessRecord_WeightTime = bus.BusinessRecord_WeightTime.ToString();
                        break;
                    }
                }
                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='CarInvoice_LoadometerTimeMethod'";
                strControlInfo_Value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!string.IsNullOrEmpty(strBusinessRecord_WeightTime))
                {
                    if (!TimeMethod(strBusinessRecord_WeightTime, strControlInfo_Value, "", true))
                    {
                        AbnormalInformation += "车辆出门地磅到出门时间校验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆出门地磅到出门时间校验", "车辆出门地磅到出门时间校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));

                    }
                }
                else
                {
                    AbnormalInformation += "车辆出门地磅到出门时间校验出现异常,没有出门地磅信息！" + "\r\n";
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.CarLoadometer_OutTimeMethod()");
            }
        }
        /// <summary>
        /// 车辆从卸货点到地磅时间校验
        /// </summary>
        public static void CarschargeCargo_LoadometerTimeMethod()
        {
            try
            {
                string strBusinessRecord_WeightTime1 = "";
                string strBusinessRecord_WeightTime2 = "";
                string strControlInfo_Value = "";
                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.unloadSecondWeight;
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.outWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strBusinessRecord_WeightTime1 = bus.BusinessRecord_WeightTime.ToString();
                        break;
                    }
                }
                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        strBusinessRecord_WeightTime2 = bus.BusinessRecord_WeightTime.ToString();
                        break;
                    }
                }
                if (string.IsNullOrEmpty(strBusinessRecord_WeightTime1) || string.IsNullOrEmpty(strBusinessRecord_WeightTime2))
                {
                    if (string.IsNullOrEmpty(strBusinessRecord_WeightTime1))
                    {
                        AbnormalInformation += "车辆从卸货点到地磅时间校验出现异常,没有卸货点第二次过磅信息" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从卸货点到地磅时间校验", "车辆从卸货点到地磅时间校验出现异常,没有卸货点第二次过磅信息!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                    if (string.IsNullOrEmpty(strBusinessRecord_WeightTime2))
                    {
                        AbnormalInformation += "车辆从卸货点到地磅时间校验出现异常，没有出门过磅信息" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从卸货点到地磅时间校验", "车辆从卸货点到地磅时间校验出现异常,没有出门过磅信息", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_IDValue='38' ";
                    strControlInfo_Value = LinQBaseDao.GetSingle(strsql).ToString();
                    if (!TimeMethod(strBusinessRecord_WeightTime1, strControlInfo_Value, strBusinessRecord_WeightTime2, false))
                    {
                        AbnormalInformation += "车辆从卸货点到地磅时间出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从卸货点到地磅时间校验", "车辆从卸货点到地磅时间出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.CarschargeCargo_LoadometerTimeMethod()");
            }
        }

        /// <summary>
        /// 复磅时间到出门时间判断
        /// </summary>
        public static void CarGoOutMethodTime()
        {
            try
            {
                string strBusinessRecord_WeightTime1 = "";
                string strBusinessRecord_WeightTime2 = "";
                string strControlInfo_Value = "";
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.outWeight;
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        strBusinessRecord_WeightTime1 = bus.BusinessRecord_WeightTime.ToString();
                        break;
                    }
                }
                strBusinessRecord_WeightTime2 = LinQBaseDao.GetSingle("select getdate()").ToString();

                if (string.IsNullOrEmpty(strBusinessRecord_WeightTime1) || string.IsNullOrEmpty(strBusinessRecord_WeightTime2))
                {
                    if (string.IsNullOrEmpty(strBusinessRecord_WeightTime1))
                    {
                        AbnormalInformation += "车辆没有复磅信息" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从复磅到出门时间校验", "车辆从复磅到出门时间校验异常,没有复磅信息", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                    if (string.IsNullOrEmpty(strBusinessRecord_WeightTime2))
                    {
                        strBusinessRecord_WeightTime2 = DateTime.Now.ToString();
                    }
                }
                else
                {
                    string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_IDValue='56' ";
                    strControlInfo_Value = LinQBaseDao.GetSingle(strsql).ToString();
                    if (!TimeMethod(strBusinessRecord_WeightTime1, strControlInfo_Value, strBusinessRecord_WeightTime2, false))
                    {
                        AbnormalInformation += "车辆从复磅到出门时间校验异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从复磅到出门时间校验", "车辆从复磅到出门时间校验异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 车辆进门到地磅时间校验
        /// </summary>
        public static void CarInMethod()
        {
            try
            {


                string strBusinessRecord_WeightTime = "";//进门地磅时间
                string strCarInOutInfoRecord_Time = "";//进厂时间
                string strControlInfo_Value = "";
                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.upWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_WeightTime != null)//出门地磅时间
                    {
                        strBusinessRecord_WeightTime = bus.BusinessRecord_WeightTime.ToString();
                        break;
                    }
                }
                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='CarInMethod'";
                strControlInfo_Value = LinQBaseDao.GetSingle(strsql).ToString();
                strsql = "select CarInOutInfoRecord_Time from CarInOutInfoRecord where CarInOutInfoRecord_CarInOutRecord_ID in (select CarInOutRecord_ID from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + CommonalityEntity.CarInfo_ID + ") and CarInOutInfoRecord_Remark like '%进%' order by CarInOutInfoRecord_ID desc";
                DataTable dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    strCarInOutInfoRecord_Time = dt.Rows[0][0].ToString();
                    if (string.IsNullOrEmpty(strBusinessRecord_WeightTime))
                    {
                        AbnormalInformation += "车辆进门到地磅时间校验出现异常,没有进门地磅信息！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆进门到地磅时间校验", "车辆进门到地磅时间校验出现异常,没有进门地磅信息！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                    else
                    {
                        if (!TimeMethod(strCarInOutInfoRecord_Time, strControlInfo_Value, strBusinessRecord_WeightTime, true))
                        {
                            AbnormalInformation += "车辆进门到地磅时间校验出现异常,已超过设定值!" + "\r\n";
                            listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆进门到地磅时间校验", "车辆进门到地磅时间校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));

                        }
                    }
                }
                else
                {
                    AbnormalInformation += "车辆进门到地磅时间校验出现异常,车辆没有进厂信息!" + "\r\n";
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 车辆地磅到装货时间校验
        /// </summary>
        public static void CarInCPMethod()
        {
            try
            {

                string strupWeightTime = "";//进门地磅时间
                string strloadFirstWeight = "";//装货点时间
                string strControlInfo_Value = "";
                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.upWeight;
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.loadFirstWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_WeightTime != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strupWeightTime = bus.BusinessRecord_WeightTime.ToString();
                    }
                }
                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_WeightTime != null)
                    {
                        strloadFirstWeight = bus.BusinessRecord_WeightTime.ToString();
                    }
                }

                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='CarInCPMethod' ";
                string controlinfo_value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!string.IsNullOrEmpty(controlinfo_value))
                {
                    strControlInfo_Value = controlinfo_value;
                }
                if (!string.IsNullOrEmpty(strloadFirstWeight) && !string.IsNullOrEmpty(strupWeightTime))
                {
                    if (!TimeMethod(strupWeightTime, strControlInfo_Value, strloadFirstWeight, true))
                    {
                        CommonalityEntity.strUnusualRecordTable = "BusinessRecord";
                        AbnormalInformation += "车辆地磅到装货时间校验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆地磅到装货时间校验", "车辆地磅到装货时间校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(strloadFirstWeight))
                    {
                        AbnormalInformation += "车辆地磅到装货时间校验出现异常,没有装货点第一次过磅信息!" + "\r\n";
                    }
                    if (string.IsNullOrEmpty(strupWeightTime))
                    {
                        AbnormalInformation += "车辆地磅到装货时间校验出现异常,没有进门过磅信息!" + "\r\n";
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 车辆地磅到卸货点时间校验
        /// </summary>
        public static void CarInXHMethod()
        {
            try
            {

                string strupWeightTime = "";//进门地磅时间
                string strunloadFirstWeightTime = "";//卸货点时间
                string strControlInfo_Value = "";
                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.upWeight;
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.unloadFirstWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_WeightTime != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strupWeightTime = bus.BusinessRecord_WeightTime.ToString();
                    }
                }
                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_WeightTime != null)
                    {
                        strunloadFirstWeightTime = bus.BusinessRecord_WeightTime.ToString();
                    }
                }

                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='CarInXHMethod' ";
                string controlinfo_value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!string.IsNullOrEmpty(controlinfo_value))
                {
                    strControlInfo_Value = controlinfo_value;
                }
                if (!string.IsNullOrEmpty(strunloadFirstWeightTime) && !string.IsNullOrEmpty(strupWeightTime))
                {
                    if (!TimeMethod(strupWeightTime, strControlInfo_Value, strunloadFirstWeightTime, true))
                    {
                        CommonalityEntity.strUnusualRecordTable = "BusinessRecord";
                        AbnormalInformation += "车辆地磅到卸货点时间校验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆地磅到卸货点时间校验", "车辆地磅到卸货点时间校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(strunloadFirstWeightTime))
                    {
                        AbnormalInformation += "车辆地磅到卸货点时间校验出现异常,没有卸货点第一次过磅信息!" + "\r\n";
                    }
                    if (string.IsNullOrEmpty(strupWeightTime))
                    {
                        AbnormalInformation += "车辆地磅到卸货点时间校验出现异常,没有进门过磅信息!" + "\r\n";
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 车辆从装货点到地磅时间校验
        /// </summary>
        public static void ZHTimeMethod()
        {
            try
            {
                string strBusinessRecord_PrintinvoiceTime = "";
                string strBusinessRecord_WeightTime = "";
                string strControlInfo_Value = "";

                string strsql = String.Format("select * from BusinessRecord where BusinessRecord_CarInOutRecord_ID={0}", CommonalityEntity.CarInoutid);
                var p = LinQBaseDao.Query(strsql).Tables[0].AsEnumerable();
                var p1 = p.Where(n => n.Field<string>("BusinessRecord_Type") == CommonalityEntity.loadSecondWeight);
                foreach (var m in p1)
                {
                    if (m.Field<DateTime>("BusinessRecord_WeightTime") != null)
                    {
                        strBusinessRecord_PrintinvoiceTime = m.Field<DateTime>("BusinessRecord_WeightTime").ToString();
                        break;
                    }
                }
                var p2 = p.Where(n => n.Field<string>("BusinessRecord_Type") == CommonalityEntity.outWeight);
                foreach (var m in p2)
                {
                    if (m.Field<DateTime>("BusinessRecord_WeightTime") != null)//出门地磅时间
                    {
                        strBusinessRecord_WeightTime = m.Field<DateTime>("BusinessRecord_WeightTime").ToString();
                        break;
                    }
                }
                if (string.IsNullOrEmpty(strBusinessRecord_PrintinvoiceTime) || string.IsNullOrEmpty(strBusinessRecord_WeightTime))
                {
                    if (string.IsNullOrEmpty(strBusinessRecord_PrintinvoiceTime))
                    {
                        AbnormalInformation += "车辆从装货点到地磅时间校验出现异常,没有装货点第二次过磅信息！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从装货点到地磅时间校验", "车辆从装货点到地磅时间校验出现异常,没有装货点第二次过磅信息！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                    else
                    {
                        AbnormalInformation += "车辆从装货点到地磅时间校验出现异常,没有出门地磅信息！" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从装货点到地磅时间校验", "车辆从装货点到地磅时间校验出现异常,没有出门地磅信息！", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='ZHTimeMethod'";
                    strControlInfo_Value = LinQBaseDao.GetSingle(strsql).ToString();
                    if (!TimeMethod(strBusinessRecord_PrintinvoiceTime, strControlInfo_Value, strBusinessRecord_WeightTime, false))
                    {
                        AbnormalInformation += "车辆从装货点到地磅时间校验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "车辆从装货点到地磅时间校验", "车辆从装货点到地磅时间校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.ZHTimeMethod()");
            }
        }

        /// <summary>
        /// 过磅重量比较，是否超出设定值
        /// </summary>
        /// <param name="First_Weight">第一次过磅重量</param>
        /// <param name="Restrict_Weight">设置值</param>
        /// <param name="Second_Weight">第二次过磅重量</param>
        /// <param name="Second_Weight">10国废20成品30三废</param>
        /// <returns></returns>

        private static bool WeightMethod(double First_Weight, double Restrict_Weight, double Second_Weight, int type)
        {
            bool rbool = true;
            try
            {
                double d1 = First_Weight;
                double d2 = Second_Weight;

                if (type == 20)
                {
                    double bl = Math.Round((d2 - d1) / d1,3);
                    if (Math.Abs(bl * 1000) > Restrict_Weight)
                    {
                        return false;
                    }
                }
                else
                {
                    double t = d2 * 1000 - d1 * 1000;
                    if (t > Restrict_Weight)
                    {
                        rbool = false;
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.WeightMethod()");
            }
            return rbool;
        }

        /// <summary>
        /// 国废卸货车辆卸货点第一次过磅与进门地磅检验
        /// </summary>
        public static void GoOutEmptyeMethod()
        {
            try
            {
                double strBusinessRecord_Weight1 = 0;
                double strBusinessRecord_Weight2 = 0;
                double strControlInfo_Value = 0;

                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.upWeight;
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.unloadFirstWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strBusinessRecord_Weight1 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }
                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        strBusinessRecord_Weight2 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }

                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_IDValue='450201' ";
                string controlinfo_value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!string.IsNullOrEmpty(controlinfo_value))
                {
                    strControlInfo_Value = Convert.ToDouble(controlinfo_value);
                }
                if (strBusinessRecord_Weight1 > 0 && strBusinessRecord_Weight2 > 0)
                {
                    if (!WeightMethod(strBusinessRecord_Weight1, strControlInfo_Value, strBusinessRecord_Weight2, 10))
                    {
                        CommonalityEntity.strUnusualRecordTable = "BusinessRecord";
                        AbnormalInformation += "进门地磅重量与卸货第一次过磅重量校验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "进门地磅重量与卸货第一次过磅重量校验", "进门地磅重量与卸货第一次过磅重量校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (strBusinessRecord_Weight1 <= 0)
                    {
                        AbnormalInformation += "进门地磅重量与卸货第一次过磅重量校验出现异常,没有进门地磅信息!" + "\r\n";
                    }
                    if (strBusinessRecord_Weight2 <= 0)
                    {
                        AbnormalInformation += "进门地磅重量与卸货第一次过磅重量校验出现异常,没有卸货点第一次过磅信息!" + "\r\n";
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.GoOutEmptyeMethod()");
            }
        }

        /// <summary>
        /// 国废卸货车辆出门地磅与卸货点第二次过磅检验
        /// </summary>
        public static void GoOutHeavyMethod()
        {
            try
            {
                double strBusinessRecord_Weight1 = 0;
                double strBusinessRecord_Weight2 = 0;
                double strControlInfo_Value = 0;

                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.unloadSecondWeight;
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.outWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strBusinessRecord_Weight1 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }
                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        strBusinessRecord_Weight2 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }
                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_IDValue='450202' ";
                string controlinfo_value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!string.IsNullOrEmpty(controlinfo_value))
                {
                    strControlInfo_Value = Convert.ToDouble(controlinfo_value);
                }
                if (strBusinessRecord_Weight1 > 0 && strBusinessRecord_Weight2 > 0)
                {
                    if (!WeightMethod(strBusinessRecord_Weight1, strControlInfo_Value, strBusinessRecord_Weight2, 10))
                    {
                        CommonalityEntity.strUnusualRecordTable = "BusinessRecord";
                        AbnormalInformation += "出门地磅重量与卸货第二次过磅重量校验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "出门地磅重量与卸货第二次过磅重量校验", "出门地磅重量与卸货第二次过磅重量校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (strBusinessRecord_Weight1 <= 0)
                    {
                        AbnormalInformation += "出门地磅重量与卸货第二次过磅重量校验出现异常,没有卸货点第二次过磅信息!" + "\r\n";
                    }
                    if (strBusinessRecord_Weight2 <= 0)
                    {
                        AbnormalInformation += "出门地磅重量与卸货第二次过磅重量校验出现异常,没有出门地磅信息!" + "\r\n";
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.GoOutEmptyeMethod()");
            }
        }

        /// <summary>
        /// 成品车辆装货点第一次过磅与进门地磅检验
        /// </summary>
        public static void GoLoadEmptyeMethod()
        {
            try
            {
                double strBusinessRecord_Weight1 = 0;
                double strBusinessRecord_Weight2 = 0;
                double strControlInfo_Value = 0;

                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.upWeight;
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.loadFirstWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strBusinessRecord_Weight1 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }
                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strBusinessRecord_Weight2 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }
                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_IDValue='450101' ";
                string controlinfo_value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!string.IsNullOrEmpty(controlinfo_value))
                {
                    strControlInfo_Value = Convert.ToDouble(controlinfo_value);
                }
                if (strBusinessRecord_Weight1 > 0 && strBusinessRecord_Weight2 > 0)
                {
                    if (!WeightMethod(strBusinessRecord_Weight1, strControlInfo_Value, strBusinessRecord_Weight2, 10))
                    {
                        CommonalityEntity.strUnusualRecordTable = "BusinessRecord";
                        AbnormalInformation = "进门地磅重量与装货第一次过磅重量校验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "进门地磅重量与装货第一次过磅重量校验", "进门地磅重量与装货第一次过磅重量校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (strBusinessRecord_Weight1 <= 0)
                    {
                        AbnormalInformation += "进门地磅重量与装货第一次过磅重量校验出现异常,没有进门地磅信息!" + "\r\n";
                    }
                    if (strBusinessRecord_Weight2 <= 0)
                    {
                        AbnormalInformation += "进门地磅重量与装货第一次过磅重量校验出现异常,没有装货第一次过磅信息!" + "\r\n";
                    }
                }


            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.GoOutEmptyeMethod()");
            }
        }


        /// <summary>
        /// 成品车辆出门地磅与装货点第二次过磅校验
        /// </summary>
        public static void GoLoadHeavyMethod()
        {
            try
            {
                double strBusinessRecord_Weight1 = 0;
                double strBusinessRecord_Weight2 = 0;
                double strControlInfo_Value = 0;
                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.loadSecondWeight;
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.outWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strBusinessRecord_Weight1 = Convert.ToDouble(bus.BusinessRecord_Weight);
                    }
                }
                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strBusinessRecord_Weight2 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }
                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_IDValue='450201' ";
                string controlinfo_value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!string.IsNullOrEmpty(controlinfo_value))
                {
                    strControlInfo_Value = Convert.ToDouble(controlinfo_value);
                }
                if (strBusinessRecord_Weight1 > 0 && strBusinessRecord_Weight2 > 0)
                {
                    if (!WeightMethod(strBusinessRecord_Weight1, strControlInfo_Value, strBusinessRecord_Weight2, 10))
                    {
                        CommonalityEntity.strUnusualRecordTable = "BusinessRecord";
                        AbnormalInformation += "出门地磅重量与装货第二次过磅重量校验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "出门地磅重量与装货第二次过磅重量校验", "出门地磅重量与装货第二次过磅重量校验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (strBusinessRecord_Weight1 <= 0)
                    {
                        AbnormalInformation += "出门地磅重量与装货第二次过磅重量校验出现异常,没有装货第二次过磅信息!" + "\r\n";
                    }
                    if (strBusinessRecord_Weight2 <= 0)
                    {
                        AbnormalInformation += "出门地磅重量与装货第二次过磅重量校验出现异常,没有出门地磅信息!" + "\r\n";
                    }
                }


            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.GoOutEmptyeMethod()");
            }
        }

        /// <summary>
        /// 成品国废和内部成品国废的卸货点第二次过磅与装货点第一次过磅效验
        /// </summary>
        public static void GoLoadSmallMethod()
        {
            try
            {
                double strBusinessRecord_Weight1 = 0;
                double strBusinessRecord_Weight2 = 0;
                double strControlInfo_Value = 0;

                Expression<Func<BusinessRecord, bool>> funcbus = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.unloadSecondWeight;
                Expression<Func<BusinessRecord, bool>> funcbusr = n => n.BusinessRecord_CarInOutRecord_ID == Convert.ToInt32(CommonalityEntity.CarInoutid) && n.BusinessRecord_Type == CommonalityEntity.loadFirstWeight;
                IEnumerable<BusinessRecord> businessrecord = BusinessRecordDAL.Query(funcbus);
                IEnumerable<BusinessRecord> businessrecordr = BusinessRecordDAL.Query(funcbusr);

                foreach (var bus in businessrecord)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        CommonalityEntity.intCarInOutInfoRecord_ID = bus.BusinessRecord_ID;
                        strBusinessRecord_Weight1 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }
                foreach (var bus in businessrecordr)
                {
                    if (bus.BusinessRecord_Weight != null)
                    {
                        strBusinessRecord_Weight2 = Convert.ToDouble(bus.BusinessRecord_Weight);
                        break;
                    }
                }
                string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_IDValue='450202' ";
                string controlinfo_value = LinQBaseDao.GetSingle(strsql).ToString();
                if (!string.IsNullOrEmpty(controlinfo_value))
                {
                    strControlInfo_Value = Convert.ToDouble(controlinfo_value);
                }
                if (strBusinessRecord_Weight1 > 0 && strBusinessRecord_Weight2 > 0)
                {
                    if (!WeightMethod(strBusinessRecord_Weight1, strControlInfo_Value, strBusinessRecord_Weight2, 10))
                    {
                        CommonalityEntity.strUnusualRecordTable = "BusinessRecord";
                        AbnormalInformation += "卸货点第二次过磅与装货点第一次过磅效验出现异常,已超过设定值!" + "\r\n";
                        listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "卸货点第二次过磅与装货点第一次过磅效验", "卸货点第二次过磅与装货点第一次过磅效验出现异常,已超过设定值!", CommonalityEntity.NAME, Convert.ToInt32(CommonalityEntity.CarInfo_ID)));
                    }
                }
                else
                {
                    if (strBusinessRecord_Weight1 <= 0)
                    {
                        AbnormalInformation += "卸货点第二次过磅与装货点第一次过磅效验出现异常,没有卸货点第二次过磅信息!" + "\r\n";
                    }
                    if (strBusinessRecord_Weight2 <= 0)
                    {
                        AbnormalInformation += "卸货点第二次过磅与装货点第一次过磅效验出现异常,没有装货点第一次过磅信息!" + "\r\n";
                    }
                }


            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckMethod.GoOutEmptyeMethod()");
            }
        }

        /// <summary>
        /// 读取Sap过磅数据
        /// </summary>
        public static void ISSAPLoadData(string CarNo, string IvehicleType)
        {
            try
            {
                DT_pd_info_upload_res dtc = null;

                DT_pd_info_upload_reqRow dtr = new DT_pd_info_upload_reqRow();
                dtr.I_CARNO = CarNo;
                dtr.I_VEHICLE_TYPE = IvehicleType;
                DT_pd_info_upload_reqRow[] listdt = new DT_pd_info_upload_reqRow[] { dtr };
                JLMDSAP.SAPQM sp = new JLMDSAP.SAPQM();
                dtc = sp.SIbdinfouploadreq(listdt);
                if (dtc != null)
                {
                    //if (dtc.I_FLAG == "Y")
                    //{
                    string strsql = "";
                    DT_pd_info_upload_resZMMT_CPC_DAT[] dtzmmt = dtc.ZMMT_CPC_DAT;
                    DT_pd_info_upload_resZLET_BD_DATE[] dtzlet = dtc.ZLET_BD_DATE;


                    if (IvehicleType == "20")
                    {
                        if (dtzmmt != null)
                        {
                            //var cst = (from c in dtzmmt
                            //           orderby c.KC_DATE descending
                            //           select c).FirstOrDefault();
                            foreach (var item in dtzmmt)
                            {

                                //if (item.PZ != "")//状态
                                //{
                                //    if (item.PZ.ToUpper() != "C")
                                //    {
                                //        AbnormalInformation += "未完成过磅,状态为：" + item.PZ + "\r\n";
                                //    }
                                //}
                                if (item.PZ != "")//皮重
                                {
                                    if (item.PZ == "0")
                                    {
                                        AbnormalInformation += "没有皮重过磅信息!" + "\r\n";
                                    }
                                    else
                                    {
                                        strsql += AddUpdateBusinessRecord(CommonalityEntity.upWeight, item.BD_NO, item.PZ, item.KC_DATE + " " + item.KC_TIME, "", item.KC_LOCATION);
                                    }
                                }
                                if (item.MZ != "")//毛重
                                {
                                    if (item.MZ == "0")
                                    {
                                        AbnormalInformation += "没有毛重过磅信息!" + "\r\n";
                                    }
                                    else
                                    {
                                        strsql += AddUpdateBusinessRecord(CommonalityEntity.loadSecondWeight, item.BD_NO, item.MZ, item.MC_DATE + " " + item.MC_TIME, "", item.MC_LOCATION);
                                    }
                                }
                                if (item.FBZL != "")//出门复磅重量
                                {
                                    if (item.FBZL == "0")
                                    {
                                        AbnormalInformation += "没有出门复磅信息!" + "\r\n";
                                    }
                                    else
                                    {
                                        strsql += AddUpdateBusinessRecord(CommonalityEntity.outWeight, item.BD_NO, item.FBZL, item.FB_DATE + " " + item.FB_TIME, "", item.FB_LOCATION);
                                    }
                                }
                                if (item.FHZL != "")
                                {
                                    LinQBaseDao.ExecuteSql("update CarInfo set CarInfo_Weight='" + item.FHZL + "' where CarInfo_ID in (select CarInOutRecord_CarInfo_ID from CarInOutRecord where CarInOutRecord_ID='" + CommonalityEntity.CarInoutid + "')");
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(strsql))
                        {
                            if (LinQBaseDao.ExecuteSql(strsql) <= 0)
                            {
                                AbnormalInformation += "获取SAP过磅数据失败!" + "\r\n";
                            }
                        }
                        else
                        {
                            AbnormalInformation += "获取SAP过磅数据失败!" + "\r\n";
                        }
                    }
                    else
                    {
                        if (dtzlet != null)
                        {
                            foreach (var item in dtzlet)
                            {
                                if (item.F_W != "")
                                {
                                    if (item.FBZL == "0")
                                    {
                                        AbnormalInformation += "没有毛重过磅信息!" + "\r\n";
                                    }
                                    else
                                    {
                                        strsql += AddUpdateBusinessRecord(CommonalityEntity.upWeight, item.BD_NO, item.F_W, item.F_DATE + " " + item.F_TIME, item.F_USER, item.F_BT);
                                    }
                                }
                                if (item.S_W != "")
                                {
                                    if (item.S_W == "0")
                                    {
                                        AbnormalInformation += "没有皮重过磅信息!" + "\r\n";
                                    }
                                    else
                                    {
                                        strsql += AddUpdateBusinessRecord(CommonalityEntity.unloadSecondWeight, item.BD_NO, item.S_W, item.S_DATE + " " + item.S_TIME, item.S_USER, item.S_BT);
                                    }
                                }
                                if (item.FBZL != "")
                                {
                                    if (item.FBZL == "0")
                                    {
                                        AbnormalInformation += "没有出门复磅信息!" + "\r\n";
                                    }
                                    else
                                    {
                                        strsql += AddUpdateBusinessRecord(CommonalityEntity.outWeight, item.BD_NO, item.FBZL, item.FB_DATE + " " + item.FB_TIME, item.FB_USER, item.FB_BT);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(strsql))
                            {
                                if (LinQBaseDao.ExecuteSql(strsql) <= 0)
                                {
                                    AbnormalInformation += "获取SAP过磅数据失败!" + "\r\n";
                                }
                            }
                            else
                            {
                                AbnormalInformation += "获取SAP过磅数据失败!" + "\r\n";
                            }
                        }
                    }
                    //}
                    //else
                    //{
                    //    AbnormalInformation += "获取SAP过磅数据失败,原因是：" + dtc.I_MSG + "\r\n";
                    //}
                }
                else
                {
                    AbnormalInformation += "获取SAP过磅数据失败,返回值为空!" + "\r\n";
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 添加修改BusinessRecord
        /// </summary>
        /// <param name="type">过磅类型</param>
        /// <param name="BD_NO">磅单号</param>
        /// <param name="weight">重量</param>
        /// <param name="DATE">过磅时间</param>
        /// /// <param name="wuser">过磅人</param>
        /// <param name="LOCATION">过磅地点</param>
        /// <returns></returns>
        private static string AddUpdateBusinessRecord(string type, string BD_NO, string weight, string DATE, string wuser, string LOCATION)
        {
            string strsql = "";
            try
            {
                object obj = LinQBaseDao.GetSingle("select BusinessRecord_ID from BusinessRecord where BusinessRecord_CarInOutRecord_ID ='" + CommonalityEntity.CarInoutid + "' and BusinessRecord_Type='" + type + "' and BusinessRecord_State='启动'");
                if (obj != null)
                {
                    strsql += "update BusinessRecord set BusinessRecord_Content='" + BD_NO + "',BusinessRecord_Weight='" + weight + "',BusinessRecord_WeightTime='" + DATE + "',BusinessRecord_WeightPerson='" + wuser + "',BusinessRecord_Remark='" + LOCATION + "'  where BusinessRecord_ID ='" + obj + "'";
                }
                else
                {
                    strsql += " insert into BusinessRecord(BusinessRecord_CarInOutRecord_ID,BusinessRecord_Content,BusinessRecord_State,BusinessRecord_Type,BusinessRecord_Weight,BusinessRecord_WeightTime,BusinessRecord_WeightPerson,BusinessRecord_Remark) values('" + CommonalityEntity.CarInoutid + "','" + BD_NO + "','启动','" + type + "','" + weight + "','" + DATE + "','" + wuser + "','" + LOCATION + "')";
                }
                return strsql;
            }
            catch (Exception)
            {
                return " ";
            }
        }


        /// <summary>
        /// 净重与订单重量效验
        /// </summary>
        public static void OrderLoadMethod()
        {
            try
            {
                object obj = LinQBaseDao.GetSingle("select CarInfo_Weight from CarInfo where CarInfo_ID='" + CommonalityEntity.CarInfo_ID + "'");
                if (obj != null)
                {
                    if (Convert.ToDouble(obj) > 0)
                    {
                        object objbusweight = LinQBaseDao.GetSingle("select top(1) ((select BusinessRecord_Weight from BusinessRecord where BusinessRecord_CarInOutRecord_ID='" + CommonalityEntity.CarInoutid + "' and BusinessRecord_Type='" + CommonalityEntity.loadSecondWeight + "') -(select BusinessRecord_Weight from BusinessRecord where BusinessRecord_CarInOutRecord_ID='" + CommonalityEntity.CarInoutid + "' and BusinessRecord_Type='" + CommonalityEntity.upWeight + "')) as BusinessRecord_weightmonth from BusinessRecord where BusinessRecord_CarInOutRecord_ID='" + CommonalityEntity.CarInoutid + "'");
                        double strControlInfo_Value = 0;
                        if (objbusweight != null)
                        {
                            string strsql = "select ControlInfo_Value from ControlInfo where  ControlInfo_IDValue='450103' ";
                            string controlinfo_value = LinQBaseDao.GetSingle(strsql).ToString();
                            if (!string.IsNullOrEmpty(controlinfo_value))
                            {
                                strControlInfo_Value = Convert.ToDouble(controlinfo_value);
                            }
                            if (!WeightMethod(Convert.ToDouble(obj), strControlInfo_Value, Convert.ToDouble(objbusweight), 20))
                            {
                                AbnormalInformation += "净重与订单重量效验超过设定值!" + "\r\n";
                            }
                        }
                        else
                        {
                            AbnormalInformation += "没有获取到车辆过磅数据!" + "\r\n";
                        }
                    }
                }
                else
                {
                    AbnormalInformation += "没有获取到车辆过磅数据!" + "\r\n";
                }
            }
            catch (Exception)
            {

            }
        }


        #endregion

        #region  成品二次排队
        /// <summary>
        /// 车辆出门到二次排队时间校验
        /// </summary>
        public static void OutTimeMethod()
        {
            CommonalityEntity.ISOutTime = true;
        }

        /// <summary>
        /// 二次排队车辆是否优先
        /// </summary>
        public static void SecondXYMethod()
        {
            CommonalityEntity.ISSecondXY = true;
        }

        #endregion

        ///emewe 103 20180915 增加无序进厂管控方法
        /// <summary>
        /// 排队无序
        /// </summary>
        public static void ISQueuingDisorder()
        {
            CheckProperties.ce.QueuingDisorder = true;
        }
  
    }
}