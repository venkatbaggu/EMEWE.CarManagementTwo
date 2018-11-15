using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using EMEWE.CarManagement.DAL;
using System.Data;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.CommonClass
{
    public class ControlPLC
    {
        /// <summary>
        /// 特权卡、万能卡集合，逗号隔开
        /// </summary>
        public static string ICCARD = "";

        #region 读卡地址码
        /// <summary>
        /// 通道一读卡地址码
        /// </summary>
        public static Int16 CardAddressOne = 1;
        /// <summary>
        /// 通道二读卡地址码
        /// </summary>
        public static Int16 CardAddressTwo = 2;
        /// <summary>
        /// 通道三读卡地址码
        /// </summary>
        public static Int16 CardAddressThree = 3;
        /// <summary>
        /// 通道四读卡地址码
        /// </summary>
        public static Int16 CardAddressFour = 4;
        /// <summary>
        /// 通道五读卡地址码
        /// </summary>
        public static Int16 CardAddressFive = 5;
        /// <summary>
        /// 通道六读卡地址码
        /// </summary>
        public static Int16 CardAddressSix = 6;
        #endregion

        #region PLC开闸地址码
        /// <summary>
        /// 通道一开闸地址码
        /// </summary>
        public static Int16 OpenAddressOne = 0;
        /// <summary>
        /// 通道二开闸地址码
        /// </summary>
        public static Int16 OpenAddressTwo = 1;
        /// <summary>
        /// 通道三开闸地址码
        /// </summary>
        public static Int16 OpenAddressThree = 2;
        /// <summary>
        /// 通道四开闸地址码
        /// </summary>
        public static Int16 OpenAddressFour = 3;
        /// <summary>
        /// 通道五开闸地址码
        /// </summary>
        public static Int16 OpenAddressFive = 4;
        /// <summary>
        /// 通道六开闸地址码
        /// </summary>
        public static Int16 OpenAddressSix = 5;
        #endregion

        #region PLC关闸地址码
        /// <summary>
        /// 通道一关闸地址码
        /// </summary>
        public static Int16 CloseAddressOne = 0;
        /// <summary>
        /// 通道二关闸地址码
        /// </summary>
        public static Int16 CloseAddressTwo = 1;
        /// <summary>
        /// 通道三关闸地址码
        /// </summary>
        public static Int16 CloseAddressThree = 2;
        /// <summary>
        /// 通道四关闸地址码
        /// </summary>
        public static Int16 CloseAddressFour = 3;
        /// <summary>
        /// 通道五关闸地址码
        /// </summary>
        public static Int16 CloseAddressFive = 4;
        /// <summary>
        /// 通道六关闸地址码
        /// </summary>
        public static Int16 CloseAddressSix = 5;
        #endregion
        /// <summary>
        /// 获取XML配置信息
        /// </summary>
        public static void GetSystem()
        {
            XmlDocument doc = new XmlDocument();
            string s = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "ControlSet.xml";
            doc.Load(s);
            XmlNode root = doc.DocumentElement;
            foreach (XmlNode item in root.ChildNodes)
            {
                if (item.Name == "ICCARD")
                {
                    ICCARD = item.InnerText;
                }
                //读卡地址
                if (item.Name == "CardAddressOne")
                {
                    CardAddressOne = ToInt16(item.InnerText);
                }
                if (item.Name == "CardAddressTwo")
                {
                    CardAddressTwo = ToInt16(item.InnerText);
                }
                if (item.Name == "CardAddressThree")
                {
                    CardAddressThree = ToInt16(item.InnerText);
                }
                if (item.Name == "CardAddressFour")
                {
                    CardAddressFour = ToInt16(item.InnerText);
                }
                if (item.Name == "CardAddressFive")
                {
                    CardAddressFive = ToInt16(item.InnerText);
                }
                if (item.Name == "CardAddressSix")
                {
                    CardAddressSix = ToInt16(item.InnerText);
                }
                //开闸地址
                if (item.Name == "OpenAddressOne")
                {
                    OpenAddressOne = ToInt16(item.InnerText);
                }
                if (item.Name == "OpenAddressTwo")
                {
                    OpenAddressTwo = ToInt16(item.InnerText);
                }
                if (item.Name == "OpenAddressThree")
                {
                    OpenAddressThree = ToInt16(item.InnerText);
                }
                if (item.Name == "OpenAddressFour")
                {
                    OpenAddressFour = ToInt16(item.InnerText);
                }
                if (item.Name == "OpenAddressFive")
                {
                    OpenAddressFive = ToInt16(item.InnerText);
                }
                if (item.Name == "OpenAddressSix")
                {
                    OpenAddressSix = ToInt16(item.InnerText);
                }
                //关闸地址
                if (item.Name == "CloseAddressOne")
                {
                    CloseAddressOne = ToInt16(item.InnerText);
                }
                if (item.Name == "CloseAddressTwo")
                {
                    CloseAddressTwo = ToInt16(item.InnerText);
                }
                if (item.Name == "CloseAddressThree")
                {
                    CloseAddressThree = ToInt16(item.InnerText);
                }
                if (item.Name == "CloseAddressFour")
                {
                    CloseAddressFour = ToInt16(item.InnerText);
                }
                if (item.Name == "CloseAddressFive")
                {
                    CloseAddressFive = ToInt16(item.InnerText);
                }
                if (item.Name == "CloseAddressSix")
                {
                    CloseAddressSix = ToInt16(item.InnerText);
                }
            }
        }

        /// <summary>
        /// 返回整型，返回零
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// 返回整型，返回零
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int16 ToInt16(object obj)
        {
            try
            {
                return Convert.ToInt16(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 特权卡
        /// </summary>
        public static void DownloadIC()
        {
            try
            {

                string strsql = "select ICCard_Value from ICCard where ICCard_ICCardType_ID in (select ICCardType_ID from ICCardType where ICCardType_Value in ('1010','1011')) and ICCard_State='启动'";
                DataTable dt = LinQBaseDao.Query(strsql).Tables[0];
                string dvalue = "";
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dvalue += dt.Rows[i][0].ToString() + ",";
                    }
                    ICCARD = dvalue.TrimEnd(',');
                    XmlDocument doc = new XmlDocument();
                    string s = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "ControlSet.xml";
                    doc.Load(s);
                    XmlNode root = doc.DocumentElement;
                    foreach (XmlNode item in root.ChildNodes)
                    {
                        if (item.Name == "ICCARD")
                        {
                            item.InnerText = ICCARD.ToString();
                        }
                    }
                    doc.Save(s);
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 下载通道地址
        /// </summary>
        public static void DownloadDri()
        {
            try
            {

                string strsql = "select Driveway_Value, Driveway_ReadCardPort,Driveway_Address,Driveway_CloseAddress from Driveway where  Driveway_Position_ID ='" + SystemClass.PositionID + "'";
                DataTable dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string dvalue = dt.Rows[i][0].ToString();
                        if (dvalue == "01")
                        {
                            CardAddressOne = ToInt16(dt.Rows[i][1].ToString());
                            OpenAddressOne = ToInt16(dt.Rows[i][2].ToString());
                            CloseAddressOne = ToInt16(dt.Rows[i][3].ToString());
                        }
                        if (dvalue == "02")
                        {
                            CardAddressTwo = ToInt16(dt.Rows[i][1].ToString());
                            OpenAddressTwo = ToInt16(dt.Rows[i][2].ToString());
                            CloseAddressTwo = ToInt16(dt.Rows[i][3].ToString());
                        }
                        if (dvalue == "03")
                        {
                            CardAddressThree = ToInt16(dt.Rows[i][1].ToString());
                            OpenAddressThree = ToInt16(dt.Rows[i][2].ToString());
                            CloseAddressThree = ToInt16(dt.Rows[i][3].ToString());
                        }
                        if (dvalue == "04")
                        {
                            CardAddressFour = ToInt16(dt.Rows[i][1].ToString());
                            OpenAddressFour = ToInt16(dt.Rows[i][2].ToString());
                            CloseAddressFour = ToInt16(dt.Rows[i][3].ToString());
                        }
                        if (dvalue == "05")
                        {
                            CardAddressFive = ToInt16(dt.Rows[i][1].ToString());
                            OpenAddressFive = ToInt16(dt.Rows[i][2].ToString());
                            CloseAddressFive = ToInt16(dt.Rows[i][3].ToString());
                        }
                        if (dvalue == "06")
                        {
                            CardAddressSix = ToInt16(dt.Rows[i][1].ToString());
                            OpenAddressSix = ToInt16(dt.Rows[i][2].ToString());
                            CloseAddressSix = ToInt16(dt.Rows[i][3].ToString());
                        }
                    }

                    SaveXML();
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 保存信息至XML
        /// </summary>
        private static void SaveXML()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string s = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "ControlSet.xml";
                doc.Load(s);
                XmlNode root = doc.DocumentElement;
                foreach (XmlNode item in root.ChildNodes)
                {
                    if (item.Name == "CardAddressOne")
                    {
                        item.InnerText = CardAddressOne.ToString();
                    }
                    if (item.Name == "CardAddressTwo")
                    {
                        item.InnerText = CardAddressTwo.ToString();
                    }
                    if (item.Name == "CardAddressThree")
                    {
                        item.InnerText = CardAddressThree.ToString();
                    }
                    if (item.Name == "CardAddressFour")
                    {
                        item.InnerText = CardAddressFour.ToString();
                    }
                    if (item.Name == "CardAddressFive")
                    {
                        item.InnerText = CardAddressFive.ToString();
                    }
                    if (item.Name == "CardAddressSix")
                    {
                        item.InnerText = CardAddressSix.ToString();
                    }

                    if (item.Name == "OpenAddressOne")
                    {
                        item.InnerText = OpenAddressOne.ToString();
                    }
                    if (item.Name == "OpenAddressTwo")
                    {
                        item.InnerText = OpenAddressTwo.ToString();
                    }
                    if (item.Name == "OpenAddressThree")
                    {
                        item.InnerText = OpenAddressThree.ToString();
                    }
                    if (item.Name == "OpenAddressFour")
                    {
                        item.InnerText = OpenAddressFour.ToString();
                    }
                    if (item.Name == "OpenAddressFive")
                    {
                        item.InnerText = OpenAddressFive.ToString();
                    }
                    if (item.Name == "OpenAddressSix")
                    {
                        item.InnerText = OpenAddressSix.ToString();
                    }

                    if (item.Name == "CloseAddressOne")
                    {
                        item.InnerText = CloseAddressOne.ToString();
                    }
                    if (item.Name == "CloseAddressTwo")
                    {
                        item.InnerText = CloseAddressTwo.ToString();
                    }
                    if (item.Name == "CloseAddressThree")
                    {
                        item.InnerText = CloseAddressThree.ToString();
                    }
                    if (item.Name == "CloseAddressFour")
                    {
                        item.InnerText = CloseAddressFour.ToString();
                    }
                    if (item.Name == "CloseAddressFive")
                    {
                        item.InnerText = CloseAddressFive.ToString();
                    }
                    if (item.Name == "CloseAddressSix")
                    {
                        item.InnerText = CloseAddressSix.ToString();
                    }
                }
                doc.Save(s);
            }
            catch (Exception)
            {
            }
        }
    }
}
