using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;
using System.Collections;
using System.Data.Linq.SqlClient;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagementDAL;
using EMEWE.Common;
using EMEWE.CarManagement;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class LogInfoForm : Form
    {
        public LogInfoForm()
        {
            InitializeComponent();
        }

        Expression<Func<View_LogInfo_Dictionary, bool>> expr = null;
        PageControl page = new PageControl();
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;
        /// <summary>
        /// Load 的加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogInfoForm_Load(object sender, EventArgs e)
        {
            btnSearch_Click(btnSearch, null);
            InitUser();
            txtbeginTime.Value = CommonalityEntity.GetServersTime();
            txtendTime.Value = CommonalityEntity.GetServersTime();
        }
        /// <summary>
        /// 加载用户
        /// </summary>
        private void InitUser()
        {
            this.dgvLogInfo.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
            //expr = n => n.Log_ID != null;
            tscbxPageSize.SelectedIndex = 2;
           // LoadData();
            TypeBind();
        }
        private void TypeBind()
        {
            try
            {
                this.comboxType.DataSource = DictionaryDAL.GetValueDictionary("19");
                
                if (this.comboxType.DataSource != null)
                {
                    this.comboxType.DisplayMember = "Dictionary_Name";
                    this.comboxType.ValueMember = "Dictionary_ID";
                    this.comboxType.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("日志“操作类型”有误，请查询字典中的操作类型！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvLogInfo.DataSource = null;
                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()" );
            }

        }

        /// <summary>
        /// “搜 索” 的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            #region  注释
            try
            {
                int i = 0;
                string str = "";
                //得到查询的条件
                string name = this.txtLog_Name.Text.Trim();
                string beginTime = this.txtbeginTime.Text.Trim();
                string endTime = this.txtendTime.Text.Trim();
                string begin = beginTime + " 00:00:00";
                string end = endTime + "23:59:59 ";

                expr = PredicateExtensionses.True<View_LogInfo_Dictionary>();

                if (name != "")//操作人
                {
                    expr = expr.And(n => SqlMethods.Like(n.Log_Name, "%" + txtLog_Name.Text.Trim() + "%"));


                    i++;
                }
                if (beginTime != "")//开始时间
                {
                    expr = expr.And(n => n.Log_Time >= CommonalityEntity.GetDateTime(begin));

                    i++;
                }
                if (endTime != "")//结束时间
                {
                    expr = expr.And(n => n.Log_Time <= CommonalityEntity.GetDateTime(end));

                    i++;
                }
                if (beginTime != "" && endTime != "")
                {
                    if (CommonalityEntity.GetDateTime(beginTime) > CommonalityEntity.GetDateTime(endTime))
                    {
                        MessageBox.Show("查询开始时间不能大于结束时间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtbeginTime.Text = "";
                        txtendTime.Text = "";
                        return;
                    }

                }
                if (this.comboxType.SelectedValue != null)
                {
                    int stateID = Converter.ToInt(comboxType.SelectedValue.ToString());
                    if (stateID > 0)
                    {
                        Dictionary dicEntity = comboxType.SelectedItem as Dictionary;
                        if (dicEntity.Dictionary_Value == "全部")
                        {
                            expr = n => n.Log_ID != null;
                        }
                        else
                        {
                            expr = expr.And(n => n.Log_Type == CommonalityEntity.GetInt(comboxType.SelectedValue.ToString()));
                        }
                    }


                    i++;
                }

                if (i == 0)
                {
                    expr = n => n.Log_ID != null;
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("CommonalityEntity.btnSearch()" );
            }
            finally
            {
                page = new PageControl();
                LogInfoLoad("");
                LoadData();
            }
            #endregion
            if (btnSearch.Enabled)
            {
                btnSearch.Enabled = false;
                GetLogInfoSeach();
                LogInfoDAL.AddLoginfo("查询", "检测项目查询", CommonalityEntity.USERNAME);//操作日志
                btnSearch.Enabled = true;
            }
        }
        /// <summary>
        /// 搜索的方法
        /// </summary>
        private void GetLogInfoSeach()
        {
            try
            {
                sqlwhere = "  1=1";
                string beginTime = this.txtbeginTime.Value.ToString();
                string endTime = this.txtendTime.Value.ToString();
                string name = this.txtLog_Name.Text.Trim();
                string type = this.comboxType.Text.Trim();

                
                if (!string.IsNullOrEmpty(type))//日志操作类型
                {
                    if (type == "全部")
                    {
                        sqlwhere = "  1=1";
                    }
                    else
                    {  
                        sqlwhere += String.Format(" and Dictionary_Name like '%{0}%'", type);
                    }
                }
                if (!string.IsNullOrEmpty(name))//日志操作人名称
                {
                    sqlwhere += String.Format(" and Log_Name like '%{0}%'", name);
                }
                if (beginTime != "")//开始时间
                {
                    sqlwhere += String.Format(" and Log_Time >= '{0}'", beginTime);
                }
                if (endTime != "")//结束时间
                {
                    sqlwhere += String.Format(" and Log_Time <= '{0}'", endTime);
                }
                if (beginTime != "" && endTime != "")
                {
                    if (CommonalityEntity.GetDateTime(beginTime) > CommonalityEntity.GetDateTime(endTime))
                    {
                        MessageBox.Show("查询开始时间不能大于结束时间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtbeginTime.Text = "";
                        txtendTime.Text = "";
                        return;
                    }
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("LogInfoForm.GetLogInfoSeach()异常:");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelect_Click()
        {
            for (int i = 0; i < this.dgvLogInfo.Rows.Count; i++)
            {
                dgvLogInfo.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll_Click()
        {
            for (int i = 0; i < dgvLogInfo.Rows.Count; i++)
            {
                this.dgvLogInfo.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvLogInfo, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_LogInfo_Dictionary", "*", "Log_ID", "Log_ID", 0, sqlwhere, true);
        }
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslSelectAll")//全选
            {
                tslSelectAll_Click();
                return;
            }
            if (e.ClickedItem.Name == "tslNotSelect")//取消全选
            {
                tslNotSelect_Click();
                return;
            }
            LogInfoLoad(e.ClickedItem.Name);
        }
        #endregion

        #region  DataTimePicker控件的自定义显示  和 KeyPress事件
        /// <summary>
        /// DataTimePicker控件的文本操作方法
        /// </summary>
        private void DateTimeBeginISNull()
        {
            this.txtbeginTime.Format = DateTimePickerFormat.Custom;
            this.txtbeginTime.CustomFormat = " ";
        }
        private void DateTimeEndIsNull()
        {
            this.txtendTime.Format = DateTimePickerFormat.Custom;
            this.txtendTime.CustomFormat = " ";
        }
        /// <summary>
        /// 将“DataTiemPicker”控件赋值为null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbeginTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtbeginTime.Format = DateTimePickerFormat.Custom;
            this.txtbeginTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }
        private void txtendTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtendTime.Format = DateTimePickerFormat.Custom;
            this.txtendTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }
        /// <summary>
        /// “DataTiemPicker”控件的键盘事件 KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbeginTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
            {
                DateTimeBeginISNull();
            }
        }
        private void txtendTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            if (e.KeyChar == '\b')
            {
                DateTimeEndIsNull();
            }
        }
        #endregion

        private void dgvLogInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void dgvLogInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgvLogInfo.Rows.Count != 0)
            {
                for (int i = 0; i < this.dgvLogInfo.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        this.dgvLogInfo.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                    }
                    else
                    {
                        this.dgvLogInfo.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Gold;
                    }
                }
            }
        }

        private void gbSelect_Enter(object sender, EventArgs e)
        {

        }
    }
}
