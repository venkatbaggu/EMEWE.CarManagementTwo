using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class CommonMethod
    {
        /// <summary>
        /// 生成小票号
        /// </summary>
        /// <returns></returns>
        public static string GetSerialnumber()
        {
            //取得最大小票序号
            string smallSql = "select top 1 SmallTicket_Serialnumber from SmallTicket order by SmallTicket_ID desc";
            string css = "000000000000";
            if (LinQBaseDao.GetSingle(smallSql) != null)
            {
                css = LinQBaseDao.GetSingle(smallSql).ToString();
            }
            int cs = int.Parse(css.Substring(6, css.Length - 6).ToString());
            if (cs > 999999)
            {
                cs = 0;
            }
            //获取门岗当前最大排队序号
            string number = Number(cs);//得到排队序号
            //设置小票号
            string year = CommonalityEntity.GetServersTime().Year.ToString().Substring(2, 2);
            string moth = "";
            if (int.Parse(CommonalityEntity.GetServersTime().Month.ToString()) < 10)
            {
                moth = "0" + CommonalityEntity.GetServersTime().Month.ToString();
            }
            else
            {
                moth = CommonalityEntity.GetServersTime().Month.ToString();
            }
            return year + moth +SystemClass.PosistionValue+ number;
        }
        /// <summary>
        /// 自动增长 生成排队号
        /// </summary>
        /// <param name="number">初始值</param>
        /// <returns>返回生成的排队号</returns>
        public static string Number(int nb)
        {
            string str = "";
            if (nb + 1 < 10)
            {
                str = "000000" + (nb + 1).ToString();
            }
            else if (nb + 1 < 100)
            {
                str = "00000" + (nb + 1).ToString();
            }
            else if (nb + 1 < 1000)
            {
                str = "0000" + (nb + 1).ToString();
            }
            else if (nb + 1 < 10000)
            {
                str = "000" + (nb + 1).ToString();
            }
            else if (nb + 1 < 100000)
            {
                str = "00" + (nb + 1).ToString();
            }
            else if (nb + 1 < 1000000)
            {
                str = "0" + (nb + 1).ToString();
            }
            else if (nb + 1 < 10000000)
            {
                str = (nb + 1).ToString();
            }
            else
            {
                str = (nb + 1).ToString();
            }
            return str;
        }
        /// <summary>
        /// 生成排队号
        /// </summary>
        /// <returns></returns>
        public static string GetSortValue(string CarType_Value)
        {
            //取得最大排队序号
            string sortSql = "Select max(SortNumberInfo_sortValue) from SortNumberInfo";
            int sort_Value = 0;
            object sortObj=LinQBaseDao.GetSingle(sortSql);
            if (sortObj != null)
            {
                sort_Value = int.Parse(sortObj.ToString());
            }
            else
            {
                sort_Value = 0;
            }
            //排队号
            if (sort_Value > 9999)
            {
                sort_Value = 0;
            }
            if (SystemClass.PosistionValue != "" && CarType_Value != "")
            {
                return SystemClass.PosistionValue + CarType_Value + (sort_Value + 1).ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
