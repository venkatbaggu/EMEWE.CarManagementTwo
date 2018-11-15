using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Data.Linq;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
  public  class DictionaryDAL
    {
        //DataContext dc = BaseDao.conStr();
        /// <summary>
        /// 增加字典信息
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static bool Insert(Dictionary dictionary)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    DataContext dc = BaseDao.conStr();
                    Table<Dictionary> tq = dc.GetTable<Dictionary>();
                    tq.InsertOnSubmit(dictionary);
                    dc.SubmitChanges();
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
        /// 新增一条质检记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneDictionary(Dictionary qcRecord)
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
        /// 单项更改
        /// 检索到的一个Dictionary对象做出的更新保持回数据库
        /// </summary>
        /// <param name="dictionary">Dictionary实体</param>
        /// <returns></returns>
        public static bool Update(Dictionary dictionary)
        {
            bool rbool = true;
            try
            {
                using (DCCarManagementDataContext db = new DCCarManagementDataContext())
                {
                    Dictionary dic = db.Dictionary.First(c => c.Dictionary_ID == dictionary.Dictionary_ID);
                    dic.Dictionary_Name = dictionary.Dictionary_Name;
                    dic.Dictionary_Value = dictionary.Dictionary_Value;
                    dic.Dictionary_OtherID = dictionary.Dictionary_OtherID;
                    dic.Dictionary_State = dictionary.Dictionary_State;
                    dic.Dictionary_Remark = dictionary.Dictionary_Remark;
                    db.SubmitChanges();
                }
            }
            catch 
            {
                rbool = false;
            }
            return rbool;
        }


        public static bool Update(Expression<Func<Dictionary, bool>> fun, Action<Dictionary> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<Dictionary>(new DCCarManagementDataContext(), fun, action);
            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }
        /// <summary>
        /// 多项更改
        /// 使用SubmitChanges将对检索到的进行的更新保持回数据库
        /// </summary>
        /// <param name="dictionary">Dictionary实体对象</param>
        /// <returns></returns>
        public static bool UpdateMore(Dictionary dictionary)
        {
            bool rbool = true;

            try
            {
                using (DCCarManagementDataContext db = new DCCarManagementDataContext())
                {

                    var q = from p in db.Dictionary
                            where p.Dictionary_ID == dictionary.Dictionary_ID
                            select p;
                    foreach (var p in q)
                    {
                        p.Dictionary_Name = dictionary.Dictionary_Name;
                        p.Dictionary_OtherID = dictionary.Dictionary_OtherID;
                        p.Dictionary_Remark = dictionary.Dictionary_Remark;
                        p.Dictionary_State = dictionary.Dictionary_State;
                        p.Dictionary_Value = dictionary.Dictionary_Value;
                    }
                    db.SubmitChanges();
                }
            }
            catch 
            {
                
                rbool = false;
            }
            return rbool;

        }



        public static List<Dictionary> GetListDictionary()
        {

            List<Dictionary> list = new List<Dictionary>();
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                Table<Dictionary> ti = db.GetTable<Dictionary>();
                var objTable = from a in ti select a;
                list = objTable.ToList<Dictionary>();
            }
            return list;
        }

        /// <summary>
        /// 根据启动状态和字典值获取字典信息
        /// </summary>
        /// <param name="strValue">字典值</param>
        /// <returns></returns>
        public static List<Dictionary> GetValueStateDictionary(string strValue)
        {
            List<Dictionary> list = new List<Dictionary>();
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    int itemid = db.Dictionary.First(d => (d.Dictionary_Value == strValue && d.Dictionary_State == true)).Dictionary_ID;
                    var v = (from c in db.Dictionary
                             where (c.Dictionary_ID == itemid || c.Dictionary_OtherID.Value == itemid) && c.Dictionary_State == true
                             select c.Dictionary_ID).ToArray();
                    list = (from c in db.Dictionary
                            where c.Dictionary_State == true && v.Contains(c.Dictionary_OtherID.Value)
                            select c).ToList();
                    Dictionary dic = new Dictionary();
                    dic.Dictionary_ID = 0;
                    dic.Dictionary_Name = "全部";
                    list.Add(dic);
                }
                catch 
                {
                    
                }
                finally { db.Connection.Close(); }
            }
            return list;
        }
        public static List<Dictionary> GetValueDictionary(string strValue)
        {
            List<Dictionary> list = new List<Dictionary>();
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    int itemid = db.Dictionary.First(d => (d.Dictionary_Value == strValue && d.Dictionary_State == true)).Dictionary_ID;

                    var v = (from c in db.Dictionary
                             where (c.Dictionary_ID == itemid || c.Dictionary_OtherID.Value == itemid) && c.Dictionary_State == true
                             select c.Dictionary_ID).ToArray();
                    list = (from c in db.Dictionary
                            where c.Dictionary_State == true && v.Contains(c.Dictionary_OtherID.Value)
                            select c).ToList();
                    Dictionary dic = new Dictionary();
                    dic.Dictionary_ID = -1;
                    dic.Dictionary_Name = "全部";
                    list.Add(dic);
                }
                catch 
                {
                    
                }
                finally { db.Connection.Close(); }
            }
            return list;
        }

        /// <summary>
        /// 所属列表信息
        /// 根据启动状态和有下级标识获取字典信息
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary> GetStateDictionary()
        {
            List<Dictionary> list = new List<Dictionary>();
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                Table<Dictionary> ti = db.GetTable<Dictionary>();
                var objTable = from c in ti
                               where c.Dictionary_State == true && c.Dictionary_ISLower == true
                               select c;
                //select new { c.Dictionary_ID, c.Dictionary_Value };
                list = objTable.ToList<Dictionary>();
            }
            return list;

        }
       /// <summary>
       /// 字典状态名称
       /// </summary>
       /// <returns></returns>
        public static List<Dictionary> GetStateDictionaryName()
        {
            List<Dictionary> list = new List<Dictionary>();
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                Table<Dictionary> ti = db.GetTable<Dictionary>();
                var objTable = from c in ti
                               //where c.Dictionary_State == true && c.Dictionary_ISLower == true
                               select c;
                //select new { c.Dictionary_ID, c.Dictionary_Value };
                list = objTable.ToList<Dictionary>();
            }
            return list;

        }
        /// <summary>
        /// 是否所属项(是否有下级)
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary> GetStateDictionaryOther()
        {
            List<Dictionary> list = new List<Dictionary>();
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                Table<Dictionary> ti = db.GetTable<Dictionary>();
                var objTable = from c in ti
                               where c.Dictionary_OtherID == 0 && c.Dictionary_ISLower == true
                               select c;
                list = objTable.ToList<Dictionary>();
            }
            return list;

        }
        /// <summary>
        /// 字典没有下级的字典名称
        /// </summary>
        /// <param name="OtherID"></param>
        /// <returns></returns>
        public static string GetDictionaryOtherID(string OtherID)
        {
            string other = "";
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                other = db.Dictionary.First(d => (d.Dictionary_ID == int.Parse(OtherID) && d.Dictionary_ISLower == false)).Dictionary_Name;
            }
            return other;

        }
        /// <summary>
        /// 字典有下级的字典名称
        /// </summary>
        /// <param name="OtherID"></param>
        /// <returns></returns>
        public static string GetDictionaryYesOtherID(string OtherID)
        {
            string other = "";
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                other = db.Dictionary.First(d => (d.Dictionary_ID == int.Parse(OtherID) && d.Dictionary_ISLower == true)).Dictionary_Name;
            }
            return other;

        }
        public static string GetOtherID(string OtherID)
        {
            string other = "";
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                other = db.Dictionary.First(d => (d.Dictionary_ID == int.Parse(OtherID))).Dictionary_Name;
            }
            return other;

        }
        /// <summary>
        /// 根据字典值获取字典编号
        /// </summary>
        /// <param name="strName">字典值</param>
        /// <returns></returns>
        public static int GetDictionaryID(string strName)
        {
            int rint = 0;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                rint = db.Dictionary.First(d => (d.Dictionary_Value == strName && d.Dictionary_State == true)).Dictionary_ID;
            }
            return rint;

        }
        /// <summary>
        /// 根据字典名称获取字典编号
        /// </summary>
        /// <param name="strName">字典值</param>
        /// <returns></returns>
        public static int GetDictionaryID1(string strName)
        {
            int rint = 0;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                rint = db.Dictionary.First(d => (d.Dictionary_Name == strName && d.Dictionary_State == true)).Dictionary_ID;
            }
            return rint;

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息Dictionary
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Dictionary> Query(Expression<Func<Dictionary, bool>> fun)
        {

            return LinQBaseDao.Query<Dictionary>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// 根据传入的sql在字典表中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> GetDictionary(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Dictionary>(sql);
        }
        /// <summary>
        /// summary>
        /// 按条件查询信息Dictionary
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Dictionary> Query()
        {

            return LinQBaseDao.Query<Dictionary>(new DCCarManagementDataContext());

        }

        /// <summary>
        /// LINQ更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <param name="tentity"></param>
        /// <param name="action"></param>
        public static bool UpdateDictionary(Expression<Func<Dictionary, bool>> fun, Action<Dictionary> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<Dictionary>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<Dictionary, bool>> fun)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {

                    LinQBaseDao.DeleteToMany<Dictionary>(new DCCarManagementDataContext(), fun);

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
        /// 根据传入的sql在Position中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> GetViewDictionary(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Dictionary>(sql);
        }
    }
}