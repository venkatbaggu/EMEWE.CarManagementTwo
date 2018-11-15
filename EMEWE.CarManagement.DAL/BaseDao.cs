using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data;

namespace EMEWE.CarManagement.DAL
{
  public  class BaseDao
    {
   
       public static DataContext dc = new DataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
        public BaseDao()
        {
           // dc = BaseDao.conStr();
        }
        /// <summary>
        /// 连接数据库字符串
        /// </summary>
        /// <returns></returns>
        public static DataContext conStr()
        {
            string str = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            DataContext dc1 = new DataContext(str);
            return dc1;
        }
        #region 新增信息
        /// <summary>
        /// 增加信息
        /// 说明：new一个对象，使用InsertOnSubmit方法将其加入到对应的集合中，使用SubmitChanges()提交到数据库。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int Insert(Object e)
        {
            int rnum = -1;
            try
            {
                Table<Object> tq = dc.GetTable<Object>();
                tq.InsertOnSubmit(e);
                dc.SubmitChanges();
                rnum = 1;
                //NorthwindDataContext db = new NorthwindDataContext();
                //var newCustomer = new Customer
                //{
                //    CustomerID = "MCSFT",
                //    CompanyName = "Microsoft",
                //    ContactName = "John Doe",
                //    ContactTitle = "Sales Manager",
                //    Address = "1 Microsoft Way",
                //    City = "Redmond",
                //    Region = "WA",
                //    PostalCode = "98052",
                //    Country = "USA",
                //    Phone = "(425) 555-1234",
                //    Fax = null
                //};
                //db.Customers.InsertOnSubmit(newCustomer);
                //db.SubmitChanges();
            }
            catch 
            {
                
                rnum = 0;
            }

            return rnum;
        }
        /// <summary>
        /// 一对多关系
        ///  说明：Category与Product是一对多的关系，提交Category(一端)的数据时，LINQ to SQL会自动将Product(多端)的数据一起提交。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int InsertList(object obj)
        {

            int rnum = -1;
            try
            {
                //var newCategory = new Category
                //{
                //    CategoryName = "Widgets",
                //    Description = "Widgets are the ……"
                //};
                //var newProduct = new Product
                //{
                //    ProductName = "Blue Widget",
                //    UnitPrice = 34.56M,
                //    Category = newCategory
                //};
                //db.Categories.InsertOnSubmit(newCategory);
                //db.SubmitChanges();
                //DCQUALITYDataContext db = new DCQUALITYDataContext();


            }
            catch 
            {
                rnum = 0;
            }

            return rnum;
        }
        /// <summary>
        /// 多对多
        /// 说明：在多对多关系中，我们需要依次提交
        /// </summary>
        /// <returns></returns>
        public static int InsertList2()
        {
            int rnum = -1;
            try
            {
                //var newEmployee = new Employee
                //{
                //    FirstName = "Kira",
                //    LastName = "Smith"
                //};
                //var newTerritory = new Territory
                //{
                //    TerritoryID = "12345",
                //    TerritoryDescription = "Anytown",
                //    Region = db.Regions.First()
                //};
                //var newEmployeeTerritory = new EmployeeTerritory
                //{
                //    Employee = newEmployee,
                //    Territory = newTerritory
                //};
                //db.Employees.InsertOnSubmit(newEmployee);
                //db.Territories.InsertOnSubmit(newTerritory);
                //db.EmployeeTerritories.InsertOnSubmit(newEmployeeTerritory);
                //db.SubmitChanges();
            }
            catch {
                rnum = 0;
            }

            return rnum;
        }
        /// <summary>
        /// 使用动态CUD重写(Override using Dynamic CUD)
        /// 说明：CUD就是Create、Update、Delete的缩写。下面的例子就是新建一个ID(主键)为32的Region，不考虑数据库中有没有ID为32的数据，如果有则替换原来的数据，没有则插入。
        /// </summary>
        /// <returns></returns>
        public static int InsertID()
        {

            int rnum = -1;
            try
            {
                //Region nwRegion = new Region()
                //{
                //    RegionID = 32,
                //    RegionDescription = "Rainy"
                //};
                //db.Regions.InsertOnSubmit(nwRegion);
                //db.SubmitChanges();
            }
            catch 
            {
                rnum = 0;
            }

            return rnum;
        }
        #endregion


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int Update(object obj)
        {
            int irnt = 0;
            #region 简单修改
            //Customer cust = db.Customers.First(c => c.CustomerID == "ALFKI");
            //cust.ContactTitle = "Vice President";
            //db.SubmitChanges();
            #endregion
            #region 多项更改
            //
            //var q = from p in db.Products
            //        where p.CategoryID == 1
            //        select p;
            //foreach (var p in q)
            //{
            //    p.UnitPrice += 1.00M;
            //}
            //db.SubmitChanges();
            #endregion
            return irnt;
        }


        /// <summary>
        /// 查询IQueryable<object> 
        /// </summary>
        /// <returns></returns>
        public static IQueryable<object> GetIQueryableObject()
        {

            Table<object> temp = dc.GetTable<object>();
            var emps = from a in temp select a;

            return emps;
        }
        /// <summary>
        /// 查询List<object> 
        /// </summary>
        /// <returns></returns>
        public static List<object> GetListObject()
        {
            List<object> list = null;
            Table<object> ti = dc.GetTable<object>();
            var objTable = from a in ti select a;
            list = objTable.ToList<object>();
            return list;
        }
   
        private static readonly string CONNSTRING = System.Configuration.ConfigurationManager.ConnectionStrings["EMEWEQCConnectionString"].ToString();

        /// <summary>
        /// 返回DataSet类型的方法,使用存储过程,有参数
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            using (SqlConnection objSqlConnectionB = new SqlConnection(CONNSTRING))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = objSqlConnectionB;
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;
                cmd.CommandTimeout = 1000000000;
                if (cmdParameters != null)
                {
                    foreach (SqlParameter parm in cmdParameters)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                da.Fill(ds);

                cmd.Parameters.Clear();
                return ds;
            }
        }
    }
}