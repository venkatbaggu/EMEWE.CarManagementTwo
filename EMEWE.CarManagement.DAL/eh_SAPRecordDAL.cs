using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;
using System.Data;

namespace EMEWE.CarManagement.DAL
{
    public static class eh_SAPRecordDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<eh_SAPRecord> Query()
        {

            return LinQBaseDao.Query<eh_SAPRecord>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// <summary>
        /// 按条件查询信息eh_SAPRecord
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<eh_SAPRecord> Query(Expression<Func<eh_SAPRecord, bool>> fun)
        {

            return LinQBaseDao.Query<eh_SAPRecord>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static eh_SAPRecord Single(Expression<Func<eh_SAPRecord, bool>> fun)
        {

            return LinQBaseDao.Single<eh_SAPRecord>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(eh_SAPRecord qcRecord)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                }
                catch
                {
                    rbool = false;
                }
                finally { db.Connection.Close(); }

            }
            return rbool;
        }
        /// <summary>
        /// LINQ更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <param name="tentity"></param>
        /// <param name="action"></param>
        public static bool Update(Expression<Func<eh_SAPRecord, bool>> fun, Action<eh_SAPRecord> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<eh_SAPRecord>(new DCCarManagementDataContext(), fun, action);
            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }
        /// <summary>
        /// 按条件删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        /// <param name="fun"></param>
        public static bool DeleteToMany(Expression<Func<eh_SAPRecord, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<eh_SAPRecord>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
        /// <summary>
        /// SAP数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static eh_SAPRecord GetSAP(string sql)
        {
            DataSet dataset = LinQBaseDao.Query(sql);
            eh_SAPRecord gSap = new eh_SAPRecord();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                gSap.Sap_ID = int.Parse(dr["Sap_ID"].ToString());
                gSap.Sap_Identify = dr["Sap_Identify"].ToString();
                gSap.Sap_InCarNumber = dr["Sap_InCarNumber"].ToString();
                gSap.Sap_InCarOperate = dr["Sap_InCarOperate"].ToString();
                gSap.Sap_InCRFLG = dr["Sap_InCRFLG"].ToString();
                gSap.Sap_InNO = dr["Sap_InNO"].ToString();
                gSap.Sap_InTime = Convert.ToDateTime(dr["Sap_InTime"].ToString());
                gSap.Sap_OutEMSG = dr["Sap_OutEMSG"].ToString();
                gSap.Sap_OutETYPE = dr["Sap_OutETYPE"].ToString();
                gSap.Sap_OutHG = dr["Sap_OutHG"].ToString();
                gSap.Sap_OutKDATB = Convert.ToDateTime(dr["Sap_OutKDATB"].ToString());
                gSap.Sap_OutKDATE = Convert.ToDateTime(dr["Sap_OutKDATE"].ToString());
                gSap.Sap_OutMAKTX = dr["Sap_OutMAKTX"].ToString();
                gSap.Sap_OutNAME1C = dr["Sap_OutNAME1C"].ToString();
                gSap.Sap_OutNAME1P = dr["Sap_OutNAME1P"].ToString();
                gSap.Sap_OutOFLAG = dr["Sap_OutOFLAG"].ToString();
                gSap.Sap_OutTELNUMBER = dr["Sap_OutTELNUMBER"].ToString();
                gSap.Sap_OutXZ = dr["Sap_OutXZ"].ToString();
                gSap.Sap_Remark = dr["Sap_Remark"].ToString();
                gSap.Sap_Serialnumber = dr["Sap_Serialnumber"].ToString();
                //if (dr["Sap_State"]==null)
                //{

                //}
                //else
                //{
                //    gSap.Sap_State = int.Parse(dr["Sap_State"].ToString());
                //}
                gSap.Sap_Type = dr["Sap_Type"].ToString();
            }
            return gSap;
        }
    }
}
