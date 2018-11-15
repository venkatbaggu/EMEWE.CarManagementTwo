using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.DAL;
using System.Data;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class ControlAttributes
    {
        /// <summary>
        /// 控件判断
        /// </summary>admin
        /// <param name="ControlName">控件名称</param>
        /// <param name="FromName">窗体名称</param>
        /// <param name="ControlAttribute">控件属性，Visible，Enabled</param>
        /// <returns>bool</returns>
        public static bool BoolControl(string ControlName, string FromName, string ControlAttribute)
        {
            if (CommonalityEntity.USERNAME.ToString().Trim().ToLower() == "emewe")
            {
                return true;
            }
            try
            {

                DataTable tables = LinQBaseDao.Query("select  Permissions_" + ControlAttribute + "  from PermissionsInfo where Permissions_Menu_ID in (select Menu_ID from MenuInfo where Menu_ControlName='" + ControlName + "' and Menu_OtherID in (select Menu_ID from dbo.MenuInfo where Menu_FromName='" + FromName + "')) and (Permissions_UserId=" + CommonalityEntity.USERID + " or Permissions_Role_Id=" + CommonalityEntity.USERRoleID + ") ").Tables[0];
                if (tables.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(tables.Rows[0]["Permissions_" + ControlAttribute].ToString()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        public static int IsInt(object p)
        {
            int i;
            try
            {
                i = Int32.Parse(p.ToString());
                return i;
            }
            catch
            {

                return i = 0;
            }
        }

        /// <summary>
        /// 黑名单升级与降级
        /// </summary>
        /// <param name="Id">车辆ID、驾驶员ID、公司ID</param>
        /// <param name="type">黑名单类型：1代表车，2代表驾驶员，3代表公司</param>
        /// <param name="isbool">true为升级，false为降级</param>
        /// <returns></returns>
        public static bool ISBacklist(int Id, int type, bool isbool)
        {
            bool retust = true;
            try
            {
                string strsql = "";
                if (type == 1)
                {
                    strsql = "select * from Blacklist where Blacklist_CarInfo_ID=" + Id;
                }
                else if (type == 2)
                {
                    strsql = "select * from Blacklist where Blacklist_StaffInfo_ID=" + Id;
                }
                else
                {
                    strsql = "select * from Blacklist where Blacklist_CustomerInfo_ID=" + Id;
                }
                DataTable dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count < 0)
                {
                    return retust = false;
                }
                int blacklistid = Convert.ToInt32(dt.Rows[0]["Blacklist_ID"].ToString());
                string strDicid = dt.Rows[0]["Blacklist_Dictionary_ID"].ToString();
                int upcount = Convert.ToInt32(dt.Rows[0]["Blacklist_UpgradeCount"].ToString());//升级次数
                int downcount = Convert.ToInt32(dt.Rows[0]["Blacklist_DowngradeCount"].ToString());//降级次数

                DataTable dtdic = LinQBaseDao.Query("select * from Dictionary where Dictionary_ID=" + strDicid).Tables[0];
                int DicSpareint1 = Convert.ToInt32(dtdic.Rows[0]["Dictionary_Spare_int1"].ToString());//升级上限次数
                int DicSpareint2 = Convert.ToInt32(dtdic.Rows[0]["Dictionary_Spare_int2"].ToString());//降级上限次数
                int dicsort = Convert.ToInt32(dtdic.Rows[0]["Dictionary_Sort"].ToString());
                string otherid = dtdic.Rows[0]["Dictionary_OtherID"].ToString();


                //升级
                if (isbool)
                {
                    upcount += 1;
                    if (upcount >= DicSpareint1)
                    {
                        int strsort = Convert.ToInt32(LinQBaseDao.GetSingle("select Dictionary_Sort from Dictionary where Dictionary_OtherID = " + otherid + "order by Dictionary_Sort desc").ToString());
                        if (dicsort > strsort)
                        {
                            string strstate = LinQBaseDao.GetSingle("select Dictionary_Sort from Dictionary where Dictionary_OtherID =" + otherid + " and Dictionary_Sort=" + strsort + 1).ToString();
                            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == blacklistid;
                            Action<Blacklist> ap = s =>
                            {
                                s.Blacklist_State = strstate;
                                s.Blacklist_Dictionary_ID = 5;
                                s.Blacklist_UpgradeCount = upcount - DicSpareint1;
                            };
                            if (BlacklistDAL.Update(p, ap))
                            {
                                return retust = true;
                            }
                            return retust = false;
                        }
                        return retust = false;
                    }
                    else
                    {
                        Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == blacklistid;
                        Action<Blacklist> ap = s =>
                        {
                            s.Blacklist_UpgradeCount = upcount;
                        };
                        BlacklistDAL.Update(p, ap);
                        return retust = false;
                    }
                }
                else
                {
                    downcount += 1;
                    //降级
                    if (downcount >= DicSpareint2)
                    {
                        int strsort = Convert.ToInt32(LinQBaseDao.GetSingle("select Dictionary_Sort from Dictionary where Dictionary_OtherID =" + otherid + " order by Dictionary_Sort desc").ToString());
                        if (dicsort > strsort)
                        {
                            string strstate = LinQBaseDao.GetSingle("select Dictionary_Sort from Dictionary where Dictionary_OtherID =" + otherid + " and Dictionary_Sort=" + strsort + -1).ToString();
                            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == blacklistid;
                            Action<Blacklist> ap = s =>
                            {
                                s.Blacklist_State = strstate;
                                s.Blacklist_Dictionary_ID = downcount - DicSpareint2;

                            };
                            if (BlacklistDAL.Update(p, ap))
                            {
                                return retust = true;
                            }
                            return retust = false;
                        }
                        return retust = false;
                    }
                    else
                    {
                        Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == blacklistid;
                        Action<Blacklist> ap = s =>
                        {
                            s.Blacklist_UpgradeCount = upcount;
                        };
                        BlacklistDAL.Update(p, ap);
                        return retust = false;
                    }
                }
            }
            catch
            {
                return retust = false;
            }
        }

    }
}
