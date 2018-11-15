using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    /// <summary>
    /// 使用该类是实例化，不能用静态方法，有可能要使用多线程进行处理
    /// </summary>
    public class CheckProperties
    {
        /// <summary>
        /// 管控方案
        /// </summary>
        public static CommonEntity ce = new CommonEntity();

        /// <summary>
        /// 命名空间和类
        /// </summary>
        public string NamespaceAndClass = "EMEWE.CarManagement.Commons.CommonClass.CheckMethod";

        /// <summary>
        /// 获得反射的方法
        /// </summary>
        public List<string> GetMethodsReflect()
        {

            List<string> list = new List<string>();
            MemberTypes Mymembertypes;

            // Get the type of a chosen class.
            Type Mytype = Type.GetType(NamespaceAndClass);
            // Get the MemberInfo array.
            MemberInfo[] Mymembersinfoarray = Mytype.GetMethods();
            // Get and display the name and the MemberType for each member.
            foreach (MemberInfo Mymemberinfo in Mymembersinfoarray)
            {
                Mymembertypes = Mymemberinfo.MemberType;
                if (Mymembertypes.ToString() == "Method")
                {
                    list.Add(Mymemberinfo.Name);
                    if (Mymemberinfo.Name == "SetCarInfoMethod")
                    {

                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 执行管理策略方法
        /// 查询语句一般string strSQL = "select * from ManagementStrategy where  ManagementStrategy_CarType_ID= ManagementStrategy_DrivewayStrategy_ID=ManagementStrategy_Type="
        /// 查询条件：车辆类型编号，通行策略编号，管控策略类型(登记、排队、进厂、出厂)
        /// </summary>
        ///<param name="strSql">查询SQl语句</param>
        public void ExecutionMethod(string strSql)
        {
            string s = "";

            try
            {
                Assembly assem = Assembly.GetExecutingAssembly();
                //// Create an object from the assembly, passing in the correct number
                //// and type of arguments for the constructor.
                Object o = assem.CreateInstance(NamespaceAndClass);
                #region 每次执行方法前需置空这些变量
                CheckMethod.listSql.Clear();
                CheckMethod.listMessage.Clear();
                #endregion

                List<string> listStr = GetMethodsReflect();
                #region 循环遍历执行的方法  数据库去获取管控策略
                string strMethodName = "";//方法名称
                DataSet ds = LinQBaseDao.Query(strSql);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (CommonalityEntity.IsUpdatedri)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                strMethodName = dr["ManagementStrategyRecord_Rule"].ToString();
                                if (!string.IsNullOrEmpty(strMethodName))
                                {
                                    if (strMethodName == "ISSAPLoadData")
                                    {
                                        if (CheckProperties.ce.CarType_Name.IndexOf("成品") >= 0)
                                        {
                                            CheckMethod.ISSAPLoadData(CheckProperties.ce.carInfo_Name, "20");
                                        }
                                        else if (CheckProperties.ce.CarType_Name.IndexOf("废纸") >= 0)
                                        {
                                            CheckMethod.ISSAPLoadData(CheckProperties.ce.carInfo_Name, "10");
                                        }
                                        else
                                        {
                                            CheckMethod.ISSAPLoadData(CheckProperties.ce.carInfo_Name, "30");
                                        }
                                        break;
                                    }
                                }
                            }
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {

                                strMethodName = dr["ManagementStrategyRecord_Rule"].ToString();
                                if (!string.IsNullOrEmpty(strMethodName))
                                {
                                    if (strMethodName == "ISSAPLoadData")
                                    {
                                        continue;
                                    }
                                    try
                                    {
                                        //验证执行的方法是否存在，存在执行方法，不存在做记事本日志记录 
                                        if (ISMethod(strMethodName, listStr))
                                        {
                                            //如果存在默认值将默认值赋给CheckMethod.DefaultObj
                                            if (!string.IsNullOrEmpty(dr["ManagementStrategyRecord_Value"].ToString()))
                                            {
                                                CheckMethod.DefaultObj = dr["ManagementStrategyRecord_Value"];
                                            }
                                            //  CommonalityEntity.intCarInOutInfoRecord_ID =CommonalityEntity.GetInt(dr["ManagementStrategy_ID"].ToString());
                                            // 执行指定命名空间类的指定方法    
                                            s = strMethodName;
                                            MethodInfo m = assem.GetType(NamespaceAndClass).GetMethod(strMethodName);
                                            Object ret = m.Invoke(o, new Object[] { });//如果有参数将参数按方法的顺序存入new Object[] { }里面使用逗号隔开如“new Object[] { 1,2}”
                                            if (ret != null)
                                            {//判断执行方法的返回值是否为空，不为空将结果放到指定的变量存储

                                            }
                                        }
                                        else
                                        {
                                            CommonClass.CommonalityEntity.WriteTextLog("执行管理策略方法未找到" + strMethodName + "方法？");
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                strMethodName = dr["ManagementStrategy_Rule"].ToString();
                                if (!string.IsNullOrEmpty(strMethodName))
                                {
                                    if (strMethodName == "ISSAPLoadData")
                                    {
                                        if (CheckProperties.ce.CarType_Name.IndexOf("成品") >= 0)
                                        {
                                            CheckMethod.ISSAPLoadData(CheckProperties.ce.carInfo_Name, "20");
                                        }
                                        else if (CheckProperties.ce.CarType_Name.IndexOf("废纸") >= 0)
                                        {
                                            CheckMethod.ISSAPLoadData(CheckProperties.ce.carInfo_Name, "10");
                                        }
                                        else
                                        {
                                            CheckMethod.ISSAPLoadData(CheckProperties.ce.carInfo_Name, "30");
                                        }
                                        break;
                                    }
                                }
                            }
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                strMethodName = dr["ManagementStrategy_Rule"].ToString();
                                if (!string.IsNullOrEmpty(strMethodName))
                                {
                                    if (strMethodName == "ISSAPLoadData")
                                    {
                                        continue;
                                    }
                                    try
                                    {
                                        //验证执行的方法是否存在，存在执行方法，不存在做记事本日志记录 
                                        if (ISMethod(strMethodName, listStr))
                                        {
                                            //如果存在默认值将默认值赋给CheckMethod.DefaultObj
                                            if (!string.IsNullOrEmpty(dr["ManagementStrategy_Value"].ToString()))
                                            {
                                                CheckMethod.DefaultObj = dr["ManagementStrategy_Value"];
                                            }
                                            //  CommonalityEntity.intCarInOutInfoRecord_ID =CommonalityEntity.GetInt(dr["ManagementStrategy_ID"].ToString());
                                            // 执行指定命名空间类的指定方法    
                                            s = strMethodName;

                                            MethodInfo m = assem.GetType(NamespaceAndClass).GetMethod(strMethodName);
                                            Object ret = m.Invoke(o, new Object[] { });//如果有参数将参数按方法的顺序存入new Object[] { }里面使用逗号隔开如“new Object[] { 1,2}”
                                            if (ret != null)
                                            {//判断执行方法的返回值是否为空，不为空将结果放到指定的变量存储

                                            }
                                        }
                                        else
                                        {
                                            CommonClass.CommonalityEntity.WriteTextLog("执行管理策略方法未找到" + strMethodName + "方法？");
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch
            {
                string a = s;

            }
        }

        public bool ISMethod(string MethodeName, List<string> list)
        {
            bool rbool = false;
            foreach (string str in list)
            {
                if (str.Trim() == MethodeName)
                {
                    rbool = true;
                    break;
                }
            }

            return rbool;
        }

        public static string ic = "";
        public static string strInOut = "";
        public static DataTable dt;
        /// <summary>
        /// 根据小票或IC卡号 查询该车辆是否完成整个业务流程 完成刚添加数据
        /// </summary>
        public static void CheckCarInOutRecordISCompleteMethod()
        {

            string strsql = "";
            try
            {
                if (!CommonalityEntity.SerialnumberICbool)//小票识别
                {
                    dt = new DataTable();
                    strsql = string.Format("select top(1)* from View_CarState where carinfo_name='{0}' and CarInfo_State='启动' order by CarInOutRecord_ID desc", CommonalityEntity.CarNO);
                    dt = LinQBaseDao.Query(strsql).Tables[0];
                    if (dt.Rows.Count <= 0)
                    {
                        //关闭当前窗体
                        CommonalityEntity.rboolRelease = false;
                        if (CommonalityEntity.SerialnumberICbool)//小票识别
                        {
                            CheckMethod.AbnormalInformation += "该小票可能已过期！！！不能放行！！！请检查小票" + "\r\n";
                        }
                        else
                        {
                            CheckMethod.AbnormalInformation += "该车辆没有登记或者IC卡没有与车辆关联！！！不能放行！！！" + "\r\n";
                        }
                        CommonalityEntity.strUnusualRecordTable = "CarInfo";
                        CheckMethod.listUnusualRecord.Add(CommonalityEntity.ADDUnusualRecord(2, "读取车辆信息异常", "该车辆没有被呼叫或者小票已过期！！！请检查小票", CommonalityEntity.NAME, CheckProperties.ce.CarInfo_ID));
                    }
                    else
                    {
                        CheckProperties.ce.CarInfo_ID = Convert.ToInt32(dt.Rows[0]["CarInfo_ID"].ToString());
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CheckProperties.CheckCarInOutRecordISCompleteMethod()");
            }

        }
        /// <summary>
        /// 使用该类保存生成的信息
        /// </summary>
        public class CommonEntity
        {

            public bool stfIsICCard = false;
            /// <summary>
            /// 是否可以修改临时凭证的有效性
            /// true 可以修改 false不可以修改
            /// </summary>
            public bool ISUpdateCredentials = false;
            /// <summary>
            /// 小票号
            /// (选择车辆类型时，根据管控执行小票生成
            /// 如该车辆类型管控中不需要小票，则不生成 为空“”)
            /// </summary>
            public string serialNumber = "";
            /// <summary>
            /// 时间(生成小票或者IC卡验证时赋值)
            /// </summary>
            public string serTime = "";
            /// <summary>
            /// 次数(生成小票或者IC验证时赋值)
            /// </summary>
            public string serCount = "";
            /// <summary>
            /// 时间(生成小票或者IC卡验证时赋值)
            /// </summary>
            public string icTime = "";
            /// <summary>
            /// 次数(生成小票或者IC验证时赋值)
            /// </summary>
            public string icCount = "";
            /// <summary>
            /// 排队号(初始值为0)
            /// </summary>
            public int sort_Value = 0;
            /// <summary>
            /// 是否排队true 排队 false 不排队
            /// </summary>
            public bool isSort = false;
            /// <summary>
            /// 是否需要小票验证
            /// </summary>
            public bool isSerialNumber = false;
            /// <summary>
            /// 车辆编号(选择车辆类型时，赋值车辆类型编号)
            /// </summary>
            public int carType_ID = -1;
            /// <summary>
            /// 车辆类型值(保存车辆信息时赋值，初始值为“”)
            /// </summary>
            public string carType_Value = "";
            /// <summary>
            /// 保存获取到的所有有效的图片名称
            /// </summary>
            public List<string> Imagelist = new List<string>();
            /// <summary>
            /// 保存获取到的所有‘的图片名称
            /// </summary>
            public List<string> AllImageList = new List<string>();
            /// <summary>
            /// 是否需要拍照
            /// </summary>
            public bool isImageList = false;
            /// <summary>
            /// 车牌号(保存登记车辆信息时赋值)
            /// </summary>
            public string carInfo_Name = "";
            /// <summary>
            /// 车辆是否黑名单(false不是黑名单 true黑名单)
            /// </summary>
            public bool carIsBlack = false;
            /// <summary>
            /// 驾驶员编号(保存登记车辆信息时赋值，初始值为-1)
            /// </summary>
            public string staffInfo_ID = "";
            /// <summary>
            /// 驾驶员是否黑名单(false不是黑名单 true黑名单)
            /// </summary>
            public bool stfIsBlack = false;
            /// <summary>
            /// 公司编号(保存登记车辆信息时赋值，初始值为-1)
            /// </summary>
            public int customerInfo_ID = -1;
            /// <summary>
            /// 公司是否黑名单(false不是黑名单 true黑名单)
            /// </summary>
            public bool cusIsBlack = false;
            /// <summary>
            /// 验证登记国废等级(国废车辆,保存登记车辆信息时赋值,初始值为"")
            /// </summary>
            public string carInfo_LevelWaste = "";
            /// <summary>
            /// 是否登记国废等级(false没有登记 true登记)
            /// </summary>
            public bool levelIsWaste = true;
            /// <summary>
            /// 是否填写登记国废等级(false不能填写国废等级选项true能填写)
            /// </summary>
            //public bool ISlevelIsWaste = true;






            /// <summary>
            /// SAP记录编号
            /// </summary>
            public string SAPIID = "";
            /// <summary>
            /// 是否通过SAP验证
            /// </summary>
            public bool IsSap = true;
            /// <summary>
            /// 照片验证 true 通行照片验证 false登记照片验证
            /// </summary>
            public bool IsPhoto = false;
            /// <summary>
            /// 车辆凭证是否有效 false 无效  true 有效
            /// </summary>
            public bool IsState = false;
            /// <summary>
            /// SAP成品车辆
            /// </summary>
            public DataTable SapChengPinTable = null;
            /// <summary>
            /// SAP成品车辆
            /// </summary>
            public DataTable SapChengPinTable2 = null;
            /// <summary>
            /// SAP成品车辆车牌号
            /// </summary>
            public string ChengPinNumber = "";
            /// <summary>
            /// 是否执行成品车辆校验
            /// </summary>
            public bool IsChengPin = false;
            /// <summary>
            /// SAP送货车辆
            /// </summary>
            public DataTable SapSongHuoTable = null;
            /// <summary>
            /// SAP送货车辆
            /// </summary>
            public DataTable SapSongHuoTable2 = null;
            /// <summary>
            /// SAP送货车辆送货单号
            /// </summary>
            public string SongHuoNumber = "";
            /// <summary>
            /// 是否执行送货车辆校验
            /// </summary>
            public bool IsSongHuo = false;
            /// <summary>
            /// SAP三废车辆
            /// </summary>
            public DataTable SapSangFeiTable = null;
            /// <summary>
            /// SAP三废车辆
            /// </summary>
            public DataTable SapSangFeiTable2 = null;
            /// <summary>
            /// SAP三废车辆PO号
            /// </summary>
            public string SangFeiNumber = "";
            /// <summary>
            /// 是否执行三废车辆校验
            /// </summary>
            public bool IsSangFei = false;
            /// <summary>
            /// 车辆类型名称(SAP校验)
            /// </summary>
            public string CarType_Name = "";
            /// <summary>
            /// 是否有效
            /// </summary>
            public bool SapISCheck = false;
            /// <summary>
            /// 生产物料车的车辆类型
            /// </summary>
            public List<ManagementStrategy> FourList = new List<ManagementStrategy>();
            /// <summary>
            /// 国废车辆的车辆类型
            /// </summary>
            public List<ManagementStrategy> ThrList = new List<ManagementStrategy>();
            /// <summary>
            /// 排队校验的键值
            /// </summary>
            public string CarTypeKey = "";
            /// <summary>
            /// 当前最小排队号
            /// </summary>
            public int sortValue = -1;
            /// <summary>
            /// 通行策略是否完成
            /// </summary>
            public bool IsManagerFl = true;
            /// <summary>
            /// 是否开装货通知单 (false 未开  true 已开)
            /// </summary>
            public bool IsOflag = false;
            /// <summary>
            /// 呼叫校验是否通过(false 未通过  true 通过)
            /// </summary>
            public bool IsHuJiao = false;
            /// <summary>
            /// 车辆信息编号(通行校验呼叫时间时请先赋值)
            /// </summary>
            public int CarInfo_ID = -1;

            ///emewe 103 20180915,增加排队车无序进厂变量

            /// <summary>
            /// 排队无序
            /// </summary>
            public bool QueuingDisorder = false;
        }
    }
}