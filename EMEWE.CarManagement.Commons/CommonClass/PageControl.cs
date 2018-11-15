using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using System.Data.SqlClient;
using System.Drawing;
namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class PageControl
    {
        /// <summary>
        /// 无数据依然显示表头
        /// </summary>
        /// <param name="gridView">所要绑定的GridView</param>
        /// <param name="ds">所要绑定的数据集</param>
        public static void BindNoRecords(DataGridView gridView, DataSet ds)
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gridView.DataSource = ds;
                int columnCount = gridView.Rows[0].Cells.Count;
                gridView.Rows[0].Cells.Clear();

                gridView.Rows[0].Cells[0].Value = "没有任何记录！";
                //gridView.Rows[0].Cells.Add(new TableCell());
                //gridView.Rows[0].Cells[0].ColumnSpan = columnCount;
                //gridView.Rows[0].Cells[0].Text = "没有任何记录！";

                //  gridView.RowStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            }
        }

        /// <summary>
        /// 查询view_CarState时用
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="fldName"></param>
        /// <param name="strWhere"></param>
        /// <param name="fldSort"></param>
        /// <returns></returns>
        public static DataSet GetList(int PageSize, int PageIndex, string fldName, string strWhere, string fldSort)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@pageSize", SqlDbType.Int),
                    new SqlParameter("@page", SqlDbType.Int),
                    new SqlParameter("@tdname", SqlDbType.VarChar, 5000),
                    new SqlParameter("@strwhere", SqlDbType.VarChar, 5000),
                    new SqlParameter("@ord", SqlDbType.VarChar,500),
                    new SqlParameter("@sum", SqlDbType.Int),
                    };
            parameters[0].Value = PageSize;
            parameters[1].Value = PageIndex;
            parameters[2].Value = fldName;
            parameters[3].Value = strWhere;
            parameters[4].Value = fldSort;
            parameters[5].Value = 1;
            return ExecuteDataset(CommandType.StoredProcedure, "proc_carstate", parameters);
        }


        /// <summary>
        /// 存储过程分页方法
        /// </summary>
        /// <param name="PageSize">当前页显示的数据条数</param>
        /// <param name="PageIndex">第几页</param>
        /// <param name="fldName">字段</param>
        /// <param name="tablename">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="columnkey">主键</param>
        /// <param name="fldSort">排序字段</param>
        /// <param name="sort">排序规则：0升序1降序</param>
        /// <returns></returns>
        public static DataSet GetList(int PageSize, int PageIndex, string fldName, string tablename, string strWhere, string columnkey, string fldSort,int sort)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@tableColumn", SqlDbType.VarChar, 2000),
                    new SqlParameter("@tablename", SqlDbType.VarChar, 2000),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,2000),
                    new SqlParameter("@strPk", SqlDbType.VarChar,100),
                    new SqlParameter("@strOrder", SqlDbType.VarChar,100),
                    new SqlParameter("@strDirection", SqlDbType.Int)
                    };
            parameters[0].Value = PageSize;
            parameters[1].Value = PageIndex;
            parameters[2].Value = fldName;
            parameters[3].Value = tablename;
            parameters[4].Value = strWhere;
            parameters[5].Value = columnkey;
            parameters[6].Value = fldSort;
            parameters[7].Value = sort;
            return ExecuteDataset(CommandType.StoredProcedure, "Proc_GetPageListByNoTotal", parameters);
        }


        /// <summary>
        /// 调用存储过程获取分页数据
        /// </summary>
        /// <param name="tblName">要显示的表或多个表的连接 </param>
        /// <param name="fldName">要显示的字段列表</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="fldSort">排序字段列表或条件</param>
        /// <param name="strID">主键</param>
        /// <param name="OrderType">排序方法，0为升序，1为降序(如果是多字段排列Sort指代最后一个排序字段的排列顺序(最后一个排序字段不加排序标记)--程序传参如：' SortA Asc,SortB Desc,SortC ') </param>
        /// <param name="strWhere">查询条件,不需where</param>
        /// <param name="Dist">是否添加查询字段的 DISTINCT 默认0不添加/1添加 </param>
        /// <returns></returns>
        public static DataSet GetList(string tblName, string fldName, int PageSize, int PageIndex, string fldSort, string strID, int OrderType, string strWhere, bool boolDist)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@pageSize", SqlDbType.Int),
                    new SqlParameter("@page", SqlDbType.Int),
                    new SqlParameter("@fldSort", SqlDbType.VarChar,255),
                    new SqlParameter("@Sort", SqlDbType.Int),
                    new SqlParameter("@strCondition", SqlDbType.VarChar,4000),
                    new SqlParameter("@id", SqlDbType.VarChar,255),
                     new SqlParameter("@Dist", SqlDbType.Bit),
                    };
            parameters[0].Value = tblName;
            parameters[1].Value = fldName;
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = fldSort;
            parameters[5].Value = OrderType;
            parameters[6].Value = strWhere;
            parameters[7].Value = strID;
            parameters[8].Value = boolDist;
            return ExecuteDataset(CommandType.StoredProcedure, "GetRecordByPage", parameters);
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

        #region 分页参数
        /// <summary>
        /// 每页显示行数
        /// 初始化10条
        /// </summary>
        public int pageSize = 10;
        /// <summary>
        /// 总记录数
        /// </summary>
        int nMax = 0;
        /// <summary>
        /// 总页数＝总记录数/每页显示行数
        /// </summary>
        int pageCount = 0;
        /// <summary>
        /// 当前页号
        /// 初始化第一页
        /// </summary>
        int pageCurrent = 1;

        #endregion

        /// <summary>
        /// 每页条数
        /// </summary>
        // public string PageMaxCount = "10";
        DCCarManagementDataContext dc = new DCCarManagementDataContext();
        /// <summary>
        /// 分页处理
        /// </summary>
        /// <param name="ClickedItemName">当前操作行名，第一次加载时设置为“”</param>
        /// <param name="tstbPageCurrent">当前页数</param>
        /// <param name="tslPageCount">总页数</param>
        /// <param name="tslNMax">总条数</param>
        ///<param name="tscbxPageSize">每页显示条数</param>
        /// <param name="tblName">要显示的表或多个表的连接 </param>
        /// <param name="fldName">要显示的字段列表</param>
        /// <param name="fldSort">排序字段列表或条件</param>
        /// <param name="strID">主键</param>
        /// <param name="OrderType">排序方法，0为升序，1为降序(如果是多字段排列Sort指代最后一个排序字段的排列顺序(最后一个排序字段不加排序标记)--程序传参如：' SortA Asc,SortB Desc,SortC ') </param>
        /// <param name="strWhere">查询条件,不需where</param>
        /// <param name="Dist">是否添加查询字段的 DISTINCT 默认0不添加/1添加 </param>
        /// <returns>ds</returns>
        public DataSet BindBoundControl(DataGridView dgv, string ClickedItemName, ToolStripTextBox tstbPageCurrent, ToolStripLabel tslPageCount, ToolStripLabel tslNMax, ToolStripComboBox tscbxPageSize, string tblName, string fldName, string fldSort, string strID, int OrderType, string strWhere, bool boolDist)
        {
            DataSet ds = null;
            string tablename = tblName;
            if (tablename == "View_CarState1")
            {
                tblName = "View_CarState";
            }
            try
            {
                dgv.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                dgv.DataSource = null;
                if (string.IsNullOrEmpty(tslNMax.Text))
                {
                    nMax = 0;
                }
                else
                {
                    nMax = EMEWE.Common.Converter.ToInt(tslNMax.Text.Trim(), 0);
                }
                if (string.IsNullOrEmpty(tslPageCount.Text))
                {
                    pageCount = 0;
                }
                else
                {
                    pageCount = EMEWE.Common.Converter.ToInt(tslPageCount.Text.Trim(), 0);
                }
                if (string.IsNullOrEmpty(tscbxPageSize.Text.ToString()))
                {
                    pageSize = 10;//默认为每页10条
                }
                else
                {
                    pageSize = EMEWE.Common.Converter.ToInt(tscbxPageSize.Text.ToString(), 10);
                }

                if (nMax == 0)//第一次加载获取数据
                {
                    pageCurrent = 1;//当前页数从1开始  

                    ds = bindDATA(tblName, fldName, strWhere, fldSort, OrderType, pageCurrent, strID, tablename);
                    if (tablename == "View_CarState1")
                    {
                        if (string.IsNullOrEmpty(strWhere))
                        {
                            nMax = EMEWE.Common.Converter.ToInt(LinQBaseDao.GetSingle("select count(0) from carstateK").ToString(), 0);
                        }
                        else
                        {
                            nMax = EMEWE.Common.Converter.ToInt(LinQBaseDao.GetSingle("select count(0) from carstateK where " + strWhere).ToString(), 0);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strWhere))
                        {
                            nMax = EMEWE.Common.Converter.ToInt(LinQBaseDao.GetSingle("select count(0) from " + tblName).ToString(), 0);
                        }
                        else
                        {
                            nMax = EMEWE.Common.Converter.ToInt(LinQBaseDao.GetSingle("select count(0) from " + tblName + " where " + strWhere).ToString(), 0);
                        }
                    }

                    //获取绑定数据
                    //ds = GetList(tblName, fldName, pageSize, pageCurrent, fldSort, strID, OrderType, strWhere, boolDist);
                    //nMax = Common.Converter.ToInt(ds.Tables[1].Rows[0]["total"].ToString(), 0);
                    //if (nMax > 0)//获取数据并计算总页数，分页控件设置总页数和总条数
                    //{
                    //    pageCount = (nMax / pageSize);    //计算出总页数
                    //    if ((nMax % pageSize) > 0) pageCount++;
                    //    tslPageCount.Text = pageCount.ToString();
                    //    tslNMax.Text = nMax.ToString();
                    //    dgv.DataSource = ds.Tables[0].DefaultView;
                    //}
                    //else
                    //{ //未取到数据

                    //}
                }
                else
                {
                    if (ClickedItemName != "")//翻页判断并获取当前页
                    {
                        itemClicked(ClickedItemName);
                    }
                    //获取绑定数据

                    ds = bindDATA(tblName, fldName, strWhere, fldSort, OrderType, pageCurrent, strID, tablename);
                    if (tablename == "View_CarState1")
                    {
                        if (string.IsNullOrEmpty(strWhere))
                        {
                            nMax = EMEWE.Common.Converter.ToInt(LinQBaseDao.GetSingle("select count(0) from carstateK").ToString(), 0);
                        }
                        else
                        {
                            nMax = EMEWE.Common.Converter.ToInt(LinQBaseDao.GetSingle("select count(0) from carstateK where " + strWhere).ToString(), 0);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strWhere))
                        {
                            nMax = EMEWE.Common.Converter.ToInt(LinQBaseDao.GetSingle("select count(0) from " + tblName).ToString(), 0);
                        }
                        else
                        {
                            nMax = EMEWE.Common.Converter.ToInt(LinQBaseDao.GetSingle("select count(0) from " + tblName + " where " + strWhere).ToString(), 0);
                        }
                    }
                    //ds = GetList(tblName, fldName, pageSize, pageCurrent, fldSort, strID, OrderType, strWhere, boolDist);
                    //nMax = Common.Converter.ToInt(ds.Tables[1].Rows[0]["total"].ToString(), 0);

                }
                if (nMax > 0)//获取数据并计算总页数，分页控件设置总页数和总条数
                {
                    pageCount = (nMax / pageSize);    //计算出总页数
                    if ((nMax % pageSize) > 0) pageCount++;
                    tslPageCount.Text = pageCount.ToString();
                    tslNMax.Text = nMax.ToString();
                    dgv.DataSource = ds.Tables[0].DefaultView;

                    #region 调整列宽
                    // 设定包括Header和所有单元格的列宽自动调整
                    //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    //设定包括Header和所有单元格的行高自动调整
                    dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


                    //int widths = 0;
                    //for (int i = 0; i < dgv.Columns.Count; i++)
                    //{
                    //    dgv.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);  // 自动调整列宽   
                    //    widths += dgv.Columns[i].Width;   // 计算调整列后单元列的宽度和   

                    //}
                    //if (widths >= dgv.Size.Width)  // 如果调整列的宽度大于设定列宽   
                    //    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;  // 调整列的模式 自动   
                    //else
                    //    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // 如果小于 则填充   

                    //Cursor.Current = Cursors.Default;  

                    #endregion
                }
                else
                { //未取到数据

                }
                tstbPageCurrent.Text = pageCurrent.ToString();
                tscbxPageSize.Text = pageSize.ToString();
            }
            catch
            {
                //common.WriteTextLog("分页PageControl.BindBoundControl异常：" + "");
            }
            return ds;
        }


        private DataSet bindDATA(string tblName, string fldName, string strWhere, string fldSort, int OrderType, int pageCurrent, string strID, string tablename)
        {
            DataSet dt;
            int i = (pageCurrent - 1) * pageSize;
            string sort = "";


            string strsql = "";

            if (OrderType == 0)
            {
                if (!string.IsNullOrEmpty(fldSort))
                {
                    sort = " order by " + fldSort + " asc";
                }
                else
                {
                    sort = " order by " + strID + " asc";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(fldSort))
                {
                    sort = " order by " + fldSort + " desc";
                }
                else
                {
                    sort = " order by " + strID + " desc";
                }
            }
            //if (string.IsNullOrEmpty(strWhere))
            //{
            //    strsql = "select top(" + pageSize + ") " + fldName + " from " + tblName + " where  " + sort;
            //}
            //else
            //{
            //    strsql = "select top(" + pageSize + ") " + fldName + " from " + tblName + " where  " + strWhere + " " + sort;
            //}

            if (string.IsNullOrEmpty(strWhere))
            {
                strsql = "select top(" + pageSize + ") " + fldName + " from " + tblName + " where " + strID + " not in (select top (" + i + ") " + strID + " from " + tblName + " " + sort + " ) " + sort;
            }
            else
            {
                strsql = "select top(" + pageSize + ") " + fldName + " from " + tblName + " where " + strID + " not in (select top (" + i + ") " + strID + " from " + tblName + " where " + strWhere + "  " + sort + " ) and " + strWhere + " " + sort;
            }

            if (tablename == "View_CarState1")
            {
                strsql = strsql.Replace("'", "''");
                strWhere = strWhere.Replace("'", "''");
                strsql = "exec Proc_carstateK " + CommonalityEntity.isLoad + ",'" + strsql + "'";
                strsql = strsql.Replace("View_CarState", "carstateK");
                return dt = LinQBaseDao.Query(strsql);
            }
            else
            {
                return dt = LinQBaseDao.Query(strsql);
            }
        }
        /// <summary>
        /// 分页菜单事件响应处理
        /// </summary>
        /// <param name="ClickedItemName">操作行为名</param>
        public void itemClicked(string ClickedItemName)
        {
            string stritemName = ClickedItemName;
            if (stritemName.Length > 2)
            {
                ClickedItemName = stritemName.Remove(stritemName.Length - 1);
            }
            if (ClickedItemName == "tslPreviousPage" || ClickedItemName == "bindingNavigatorMovePreviousItem")//上一页
            {

                if (pageCurrent <= 1)
                {
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    pageCurrent--;
                    // nCurrent = pageSize * (pageCurrent - 1);
                }


            }
            if (ClickedItemName == "tslNextPage" || ClickedItemName == "bindingNavigatorMoveNextItem")//下一页
            {

                if (pageCurrent >= pageCount)
                {
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    pageCurrent++;
                    // nCurrent = pageSize * (pageCurrent - 1);
                }

            }
            if (ClickedItemName == "tslHomPage" || ClickedItemName == "bindingNavigatorMoveFirstItem")//首页
            {
                pageCurrent = 1;
                //nCurrent = pageSize * (pageCurrent - 1);

            }
            if (ClickedItemName == "tslLastPage" || ClickedItemName == "bindingNavigatorMoveLastItem")//尾页
            {
                pageCurrent = pageCount;
                // nCurrent = pageSize * (pageCurrent - 1);
            }


        }


        /// <summary>
        /// 更改每页显示数:如果总条数大于0，则重新计算总页数，当前页为第一页；如果总条数小于或等于0，则设置每页显示条数和当前页为第一页
        /// </summary>
        /// <param name="tstbPageCurrent">当前页数</param>
        /// <param name="tslPageCount">总页数</param>
        /// <param name="tslNMax">总条数</param>
        ///<param name="tscbxPageSize">每页显示条数</param>
        public void CalculatePageCount(ToolStripTextBox tstbPageCurrent, ToolStripLabel tslPageCount, ToolStripLabel tslNMax, ToolStripComboBox tscbxPageSize)
        {
            if (string.IsNullOrEmpty(tslNMax.Text))
            {
                nMax = 0;
            }
            else
            {
                nMax = EMEWE.Common.Converter.ToInt(tslNMax.Text.Trim(), 0);
            }

            if (string.IsNullOrEmpty(tscbxPageSize.Text.ToString()))
            {
                pageSize = 10;//默认为每页10条
            }
            else
            {
                pageSize = EMEWE.Common.Converter.ToInt(tscbxPageSize.Text.ToString(), 10);
            }

            if (nMax > 0 && pageSize > 0)
            {

                pageCount = (nMax / pageSize);    //计算出总页数
                if ((nMax % pageSize) > 0) pageCount++;
                tslPageCount.Text = pageCount.ToString();

            }
            pageCurrent = 1;
            tstbPageCurrent.Text = Convert.ToString(pageCurrent);
            tscbxPageSize.Text = pageSize.ToString();

        }



        public DataSet BindBoundControl(DataGridView dgv, string ClickedItemName, ToolStripTextBox tstbPageCurrent, ToolStripLabel tslPageCount, ToolStripLabel tslNMax, ToolStripComboBox tscbxPageSize, string fldName, string strWhere, string fldSort)
        {
            DataSet ds = null;
            try
            {
                dgv.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                dgv.DataSource = null;
                if (string.IsNullOrEmpty(tslNMax.Text))
                {
                    nMax = 0;
                }
                else
                {
                    nMax = EMEWE.Common.Converter.ToInt(tslNMax.Text.Trim(), 0);
                }
                if (string.IsNullOrEmpty(tslPageCount.Text))
                {
                    pageCount = 0;
                }
                else
                {
                    pageCount = EMEWE.Common.Converter.ToInt(tslPageCount.Text.Trim(), 0);
                }
                if (string.IsNullOrEmpty(tscbxPageSize.Text.ToString()))
                {
                    pageSize = 10;//默认为每页10条
                }
                else
                {
                    pageSize = EMEWE.Common.Converter.ToInt(tscbxPageSize.Text.ToString(), 10);
                }

                if (nMax == 0)//第一次加载获取数据
                {
                    pageCurrent = 1;//当前页数从1开始  

                    //获取绑定数据
                    ds = GetList(pageSize, pageCurrent, fldName, strWhere, fldSort);
                    nMax = Common.Converter.ToInt(ds.Tables[1].Rows[0][0].ToString(), 0);
                }
                else
                {
                    if (ClickedItemName != "")//翻页判断并获取当前页
                    {
                        itemClicked(ClickedItemName);
                    }
                    //获取绑定数据

                    ds = GetList(pageSize, pageCurrent, fldName, strWhere, fldSort);
                    nMax = Common.Converter.ToInt(ds.Tables[1].Rows[0][0].ToString(), 0);

                }

                if (nMax > 0)//获取数据并计算总页数，分页控件设置总页数和总条数
                {
                    pageCount = (nMax / pageSize);    //计算出总页数
                    if ((nMax % pageSize) > 0) pageCount++;
                    tslPageCount.Text = pageCount.ToString();
                    tslNMax.Text = nMax.ToString();
                    dgv.DataSource = ds.Tables[0].DefaultView;

                    #region 调整列宽
                    // 设定包括Header和所有单元格的列宽自动调整
                    //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    //设定包括Header和所有单元格的行高自动调整
                    //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    #endregion
                }
                else
                { //未取到数据

                }
                tstbPageCurrent.Text = pageCurrent.ToString();
                tscbxPageSize.Text = pageSize.ToString();
            }
            catch
            {
                //common.WriteTextLog("分页PageControl.BindBoundControl异常：" + "");
            }
            return ds;
        }

        /// <summary>
        /// 绑定gridview控件
        /// </summary>
        private void BindGridView()
        {

        }
        string tabn = "", tabc = "";
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="dgv">gridview控件</param>
        /// <param name="ClickedItemName">当前操作行名，第一次加载时设置为“”</param>
        /// <param name="tstbPageCurrent">当前页数</param>
        /// <param name="tslPageCount">总页数</param>
        /// <param name="tslNMax">总条数</param>
        ///<param name="tscbxPageSize">每页显示条数</param>
        /// <param name="fldName">字段</param>
        /// <param name="tabname">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="columnkey">主键</param>
        /// <param name="fldSort">排序字段</param>
        /// <param name="sort">排序规则：0升序1降序</param>
        /// <returns></returns>
        public DataSet BindBoundControl(DataGridView dgv, string ClickedItemName, ToolStripTextBox tstbPageCurrent, ToolStripLabel tslPageCount, ToolStripLabel tslNMax, ToolStripComboBox tscbxPageSize, string fldName,string tabname, string strWhere,string columnkey, string fldSort ,int sort)
        {
            DataSet ds = null;
            try
            {
                dgv.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                dgv.DataSource = null;
                if (string.IsNullOrEmpty(tslNMax.Text))
                {
                    nMax = 0;
                }
                else
                {
                    nMax = EMEWE.Common.Converter.ToInt(tslNMax.Text.Trim(), 0);
                }
                if (string.IsNullOrEmpty(tslPageCount.Text))
                {
                    pageCount = 0;
                }
                else
                {
                    pageCount = EMEWE.Common.Converter.ToInt(tslPageCount.Text.Trim(), 0);
                }
                if (string.IsNullOrEmpty(tscbxPageSize.Text.ToString()))
                {
                    pageSize = 10;//默认为每页10条
                }
                else
                {
                    pageSize = EMEWE.Common.Converter.ToInt(tscbxPageSize.Text.ToString(), 10);
                }

                if (nMax == 0)//第一次加载获取数据
                {
                    pageCurrent = 1;//当前页数从1开始  
                    //获取绑定数据
                    ds = GetList(pageSize, pageCurrent, fldName, tabname, strWhere, columnkey, fldSort, sort);
                }
                else
                {
                    if (ClickedItemName != "")//翻页判断并获取当前页
                    {
                        itemClicked(ClickedItemName);
                    }
                    //获取绑定数据

                    ds = GetList(pageSize, pageCurrent, fldName, tabname, strWhere, columnkey, fldSort, sort);
                }
                if (tabname != tabn || strWhere != tabc|| nMax==0)
                {
                    tabn = tabname;
                    tabc = strWhere;
                    if (string.IsNullOrEmpty(strWhere))
                    {
                        nMax = Convert.ToInt32(LinQBaseDao.GetSingle("select count(0) from " + tabname).ToString());
                    }
                    else
                    {
                        nMax = Convert.ToInt32(LinQBaseDao.GetSingle("select count(0) from " + tabname + " where " + strWhere).ToString());
                    }
                }

                if (nMax > 0)//获取数据并计算总页数，分页控件设置总页数和总条数
                {
                    pageCount = (nMax / pageSize);    //计算出总页数
                    if ((nMax % pageSize) > 0) pageCount++;
                    tslPageCount.Text = pageCount.ToString();
                    tslNMax.Text = nMax.ToString();
                    dgv.DataSource = ds.Tables[0].DefaultView;

                    #region 调整列宽
                    // 设定包括Header和所有单元格的列宽自动调整
                    //dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    //设定包括Header和所有单元格的行高自动调整
                    dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    #endregion
                }
                else
                { //未取到数据

                }
                tstbPageCurrent.Text = pageCurrent.ToString();
                tscbxPageSize.Text = pageSize.ToString();
            }
            catch
            {
            }
            return ds;
        }

        public void ShowToolTip(ToolTipIcon tti, string strTitle, string strMessage, Control controlName, Form form)
        {
            ToolTip toolTip1 = new ToolTip();
            if (!form.IsDisposed)
            {
                toolTip1.ToolTipIcon = tti;//ToolTipIcon.Error;
                toolTip1.ToolTipTitle = strTitle;
                Point showLocation = new Point(
                    controlName.Location.X + controlName.Width,
                    controlName.Location.Y);
                toolTip1.Show(strMessage, form, showLocation, 5000);
                //controlName.SelectAll();
                controlName.Focus();
            }
        }
    }
}
