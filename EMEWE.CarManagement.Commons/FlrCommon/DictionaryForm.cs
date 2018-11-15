using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using System.Linq.Expressions;
using EMEWE.CarManagementDAL;
using EMEWE.Common;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class DictionaryForm : Form
    {
        public DictionaryForm()
        {
            InitializeComponent();
        }
        //public MainForm mf;//主窗体

        /// <summary>
        /// 记录字典编号
        ///</summary>
        int dictionaryID = 0;
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<Dictionary, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        //public MainFrom mf;//主窗体
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;
        /// <summary>
        /// Load 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictionaryForm_Load(object sender, EventArgs e)
        {
            userContext();
            btnUpdate.Enabled = false;
            this.txtDictionary_Name.Focus();
            BindDictionaryOther();
            this.dgvDictioanry.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
            this.comboxDictionary_OtherID.Text = "状态";
            this.comboxDictionary_State.Text = "启动";
            this.comboxDictionary_IsLower.Text = "否";  
            btnSelect_Click(btnSelect, null);  // 调用查询条件执行查询
            tscbxPageSize.SelectedIndex = 1;
            LoadData();
            //mf = new MainForm();
        }


        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnSave.Enabled = true;
                btnSave.Visible = true;
                btnDel.Enabled = true;
                btnDel.Visible = true;
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
            }
            else
            {
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "DictionaryForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "DictionaryForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "DictionaryForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "DictionaryForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "DictionaryForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "DictionaryForm", "Enabled");
            }
        }

        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvDictioanry.DataSource = null;
                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("DictionaryForm.LoadData():" );
            }
        }
        /// <summary>
        /// 绑定所属字典
        /// </summary>
        private void BindDictionaryOther()
        {
            try
            {
                List<Dictionary> list = DictionaryDAL.GetStateDictionaryOther();
                if (list.Count > 0)
                {
                    this.comboxDictionary_OtherID.DataSource = list;
                    this.comboxDictionary_OtherID.DisplayMember = "Dictionary_Name";
                    comboxDictionary_OtherID.ValueMember = "Dictionary_ID";
                    if (comboxDictionary_OtherID.SelectedValue == null)
                    {
                        comboxDictionary_OtherID.SelectedValue = -1;
                    }

                }
                List<Dictionary> list1 = DictionaryDAL.GetStateDictionaryOther();
                this.cbxDicname.DataSource = list1;
                this.cbxDicname.DisplayMember = "Dictionary_Name";
                if (list1.Count > 0)
                {

                    this.cbxDicname.ValueMember = "Dictionary_ID";
                    if (cbxDicname.SelectedValue == null)
                    {
                        cbxDicname.SelectedValue = -1;
                    }
                    cbxDicname.SelectedIndex=-1;
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("DictionaryForm.BindDictionaryOther():" );
                return;
            }
        }

        /// <summary>
        /// 查重方法
        /// </summary>
        /// <returns></returns>
        private bool btnCheck()
        {
            bool rbool = true;
            bool isdic = false;
            try
            {
                string Name = this.txtDictionary_Name.Text.ToString();
                string Value = this.txtDictionary_Value.Text;
                //判断名称是否已存在
                Expression<Func<Dictionary, bool>> funDictionary = n => n.Dictionary_Name == Name;
                IEnumerable<Dictionary> dict = DictionaryDAL.Query(funDictionary);
                if (dict.Count() > 0)
                {
                    isdic = false;
                    foreach (var item in dict)
                    {
                        if (item.Dictionary_ID == dictionaryID)
                        {
                            isdic = true;
                            break;
                        }
                    }
                    if (!isdic)
                    {
                        MessageBox.Show("该字典名称已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDictionary_Name.Focus();
                        return rbool = false;
                    }
                }
                Expression<Func<Dictionary, bool>> funDictionary1 = n => n.Dictionary_Value == Value;
                IEnumerable<Dictionary> diction = DictionaryDAL.Query(funDictionary1);
                if (diction.Count() > 0)
                {
                    isdic = false;
                    foreach (var item in diction)
                    {
                        if (item.Dictionary_ID == dictionaryID)
                        {
                            isdic = true;
                            break;
                        }
                    }
                    if (!isdic)
                    {
                        MessageBox.Show("该字典值已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDictionary_Value.Focus();
                        return rbool = false;
                    }
                }
                return rbool;

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("字典管理 btnCheck()" );
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// “保存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 定义变量保存
                string strDictionaryName = this.txtDictionary_Name.Text.Trim();
                string strDictionaryValue = this.txtDictionary_Value.Text.Trim();

                // 开始验证
                if (string.IsNullOrEmpty(strDictionaryName))
                {
                    MessageBox.Show("请您输入字典名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(strDictionaryValue))
                {
                    MessageBox.Show("请您输入字典值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                bool flag = true;
                if (this.comboxDictionary_State.Text.Trim() == "启动")
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                bool falg = true;
                if (this.comboxDictionary_IsLower.Text.Trim() == "是")
                {
                    falg = true;
                }
                else
                {
                    falg = false;
                }
                string other = "0";
                if (this.comboxDictionary_OtherID.SelectedValue != null)
                {
                    other = this.comboxDictionary_OtherID.SelectedValue.ToString();
                }
                else
                {
                    other = "0";
                }
                if (!btnCheck()) return; // 去重复
                var Dictionaryadd = new Dictionary
                {
                    Dictionary_Name = this.txtDictionary_Name.Text.Trim(),
                    Dictionary_Value = this.txtDictionary_Value.Text.Trim(),
                    Dictionary_State = flag,
                    Dictionary_OtherID = int.Parse(other),
                    Dictionary_ISLower = falg,
                    Dictionary_Remark = this.txtDictionary_Remark.Text.Trim()
                };

                if (DictionaryDAL.InsertOneDictionary(Dictionaryadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string strContent1 = "字典名称为：" + this.txtDictionary_Name.Text.Trim(); ;
                    CommonalityEntity.WriteLogData("新增", "新增 " + strContent1 + " 的信息", CommonalityEntity.USERNAME);//添加日志
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("字典管理 btnSave_Click()" );
            }
            finally
            {
                LogInfoLoad("");
                BindDictionaryOther();
            }
        }

        /// <summary>
        /// “修改” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvDictioanry.SelectedRows.Count <= 0)//选中行
                {
                    MessageBox.Show("请选择要修改的项！");
                    return;
                }
                else
                {
                    if (dgvDictioanry.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        bool issystem = Convert.ToBoolean(LinQBaseDao.GetSingle("select Dictionary_IsSystem from Dictionary where Dictionary_ID=" + this.dgvDictioanry.SelectedRows[0].Cells["Dictionary_ID"].Value.ToString()).ToString());
                        if (issystem)
                        {
                            MessageBox.Show("不能修改系统数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // 定义变量保存
                        string strDictionaryName = this.txtDictionary_Name.Text.Trim();
                        string strDictionaryValue = this.txtDictionary_Value.Text.Trim();

                        // 开始验证
                        if (string.IsNullOrEmpty(strDictionaryName))
                        {
                            MessageBox.Show("请您输入字典名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (string.IsNullOrEmpty(strDictionaryValue))
                        {
                            MessageBox.Show("请您输入字典值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        bool flag = true;
                        if (this.comboxDictionary_State.Text.Trim() == "启动")
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                        bool falg = true;
                        if (this.comboxDictionary_IsLower.Text.Trim() == "是")
                        {
                            falg = true;
                        }
                        else
                        {
                            falg = false;
                        }
                        string other = "0";
                        if (this.comboxDictionary_OtherID.SelectedValue != null)
                        {
                            other = this.comboxDictionary_OtherID.SelectedValue.ToString();
                        }
                        else
                        {
                            other = "0";
                        }
                        if (!btnCheck()) return; // 去重复
                        Expression<Func<Dictionary, bool>> p = n => n.Dictionary_ID == int.Parse(this.dgvDictioanry.SelectedRows[0].Cells["Dictionary_ID"].Value.ToString());
                        string strfront = "";
                        string strContent = "";
                        Action<Dictionary> ap = s =>
                        {
                            strfront = s.Dictionary_Name + "," + s.Dictionary_Value + "," + s.Dictionary_State + "," + s.Dictionary_OtherID + "," + s.Dictionary_ISLower + "," + s.Dictionary_Remark;
                            s.Dictionary_Name = this.txtDictionary_Name.Text.Trim();
                            s.Dictionary_Value = this.txtDictionary_Value.Text.Trim();
                            s.Dictionary_State = flag;
                            s.Dictionary_OtherID = int.Parse(other);
                            s.Dictionary_ISLower = falg;
                            s.Dictionary_Remark = this.txtDictionary_Remark.Text.Trim();
                            strContent = s.Dictionary_Name + "," + s.Dictionary_Value + "," + s.Dictionary_State + "," + s.Dictionary_OtherID + "," + s.Dictionary_ISLower + "," + s.Dictionary_Remark;
                        };

                        if (DictionaryDAL.UpdateDictionary(p, ap))
                        {
                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CommonalityEntity.WriteLogData("修改", "修改前：" + strfront + "；修改后：" + strContent, CommonalityEntity.USERNAME);//添加日志
                        }
                        else
                        {
                            MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("字典管理 btnUpdate_Click()" );
            }
            finally
            {
                LogInfoLoad("");
                BindDictionaryOther();
                this.btnUpdate.Enabled = false;
                this.btnSave.Enabled = true;
                Empty(); // 调用清空的方法
            }
        }
        /// <summary>
        /// “清空” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            Empty();
            this.btnUpdate.Enabled = false;
            this.btnSave.Enabled = true;
        }
        /// <summary>
        /// 清空的方法
        /// </summary>
        private void Empty()
        {
            this.txtDictionary_Name.Text = "";
            this.txtDictionary_Value.Text = "";
            this.comboxDictionary_State.Text = "启动";
            this.comboxDictionary_IsLower.Text = "否";
            this.txtDictionary_Remark.Text = "";
            this.cbxDicname.SelectedIndex = -1;
            this.cbxName.SelectedIndex = -1;

            // emewe 103 20180918 清空字典值的数据
            txtDictionaryValue.Text = "";
        }

        /// <summary>
        /// “删除” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            tbtnDelDictionary();
        }
        /// <summary>
        /// 删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnDelDictionary()
        {
            try
            {
                string str = "";
                string strdel = "";
                if (this.dgvDictioanry.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvDictioanry.SelectedRows.Count;
                        //string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int dictionary_id = int.Parse(this.dgvDictioanry.SelectedRows[i].Cells["Dictionary_ID"].Value.ToString());
                            Expression<Func<Dictionary, bool>> funuserinfo = n => n.Dictionary_ID == dictionary_id;
                            DataTable dt = LinQBaseDao.Query("select Dictionary_Name,Dictionary_IsSystem from Dictionary where Dictionary_ID=" + dictionary_id).Tables[0];
                            string strname = dt.Rows[0]["Dictionary_Name"].ToString();
                            if (Convert.ToBoolean(dt.Rows[0]["Dictionary_IsSystem"].ToString()))
                            {
                                str += strname + ",";
                                continue;
                            }
                            if (DictionaryDAL.DeleteToMany(funuserinfo))
                            {
                                strdel += strname + ",";
                                CommonalityEntity.WriteLogData("删除", "删除字典名称为：" + strname + " 的信息", CommonalityEntity.USERNAME);//添加日志
                            }
                        }
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = "字典名称:" + str + "为系统数据，不能删除\r\n";
                        }
                        if (!string.IsNullOrEmpty(strdel))
                        {
                            strdel = "字典名称：" + strdel + "删除成功";
                        }
                        ShowToolTip(ToolTipIcon.Info, "提示", str + strdel, txtDictionary_Remark, this);
                    }
                }
                else//没有选中
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch 
            {

                CommonalityEntity.WriteTextLog("字典管理 tbtnDelDictionary() 异常！+" );
            }
            finally
            {
                LogInfoLoad("");
            }
        }
        /// <summary>
        /// 显示工具提示  徐东冬
        /// </summary>
        /// <param name="tti">ToolTipIcon.Info、None、Warning、Error</param>
        /// <param name="str">工具提示标题（ToolTipTitle）</param>
        /// <param name="strMessage">提示信息</param>
        /// <param name="controlName">提示框显示的控件</param>
        /// <param name="form">提示框显示的窗体</param>
        public void ShowToolTip(ToolTipIcon tti, string strTitle, string strMessage, Control controlName, Form form)
        {
            if (!form.IsDisposed)
            {
                toolTip1.ToolTipIcon = tti; //ToolTipIcon.Error;
                toolTip1.ToolTipTitle = strTitle;
                Point showLocation = new Point(
                    controlName.Location.X + controlName.Width,
                    controlName.Location.Y);
                toolTip1.Show(strMessage, form, showLocation, 5000);
                //controlName.SelectAll();
                controlName.Focus();
            }
        }
        /// <summary>
        /// “搜 索” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (btnSelect.Enabled)
            {
                btnSelect.Enabled = false;
                GetDictionarySeach();
                //LogInfoDAL.AddLoginfo("查询", "检测项目查询", CommonalityEntity.USERNAME);//操作日志
                btnSelect.Enabled = true;
            }
        }
        /// <summary>
        /// 搜索的方法
        /// </summary>
        private void GetDictionarySeach()
        {
            try
            {
                sqlwhere = "  1=1";             
                string name = this.cbxName.Text.Trim();
                string value = this.txtDictionaryValue.Text.Trim();
                string dicID = this.cbxDicname.SelectedValue.ToString();

                if (!string.IsNullOrEmpty(name))//字典名称
                {
                    sqlwhere += String.Format(" and Dictionary_Name like  '%{0}%'", name);
                }
                if (!string.IsNullOrEmpty(value))//字典名称
                {
                    sqlwhere += String.Format(" and Dictionary_Value like  '%{0}%'", value);
                }
                if (!string.IsNullOrEmpty(dicID))//字典名称
                {
                    string strvalue = LinQBaseDao.GetSingle("select Dictionary_ID from Dictionary where Dictionary_ID=" + dicID).ToString();
                    sqlwhere += String.Format(" and Dictionary_ID=" + dicID + " or Dictionary_OtherID = {0}", strvalue);
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("DictionaryForm.GetDictionarySeach异常:" );
            }
            finally
            {
                LogInfoLoad("");
            }
        }
        private void BindDictionary(string itemName)
        {
            try
            {
                GetDictionarySeach();
                this.dgvDictioanry.AutoGenerateColumns = false;
                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("FormDictionary.BindDgvDictionary异常:" );
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
            for (int i = 0; i < this.dgvDictioanry.Rows.Count; i++)
            {
                dgvDictioanry.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll_Click()
        {
            for (int i = 0; i < dgvDictioanry.Rows.Count; i++)
            {
                this.dgvDictioanry.Rows[i].Selected = true;
            }
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
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvDictioanry, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "Dictionary", "case Dictionary_State when 'true' then '启动' else '暂停' end as Dictstate,case Dictionary_ISLower when 'true' then '是' else '否' end as DictISLower,case Dictionary_IsSystem when 'true' then '是' else '否' end as DictIsSystem,*", "Dictionary_ID", "Dictionary_ID", 0, sqlwhere, true);

            DataBindingComplete();
        }
        #endregion

        /// <summary>
        /// 用户双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDictioanry_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvDictioanry.SelectedRows.Count > 0)//选中行
            {
                if (dgvDictioanry.SelectedRows.Count > 1)
                {
                    MessageBox.Show("查看修改行信息一次只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvDictioanry.SelectedRows[0].Cells["Dictionary_ID"].Value.ToString());
                    Expression<Func<Dictionary, bool>> funviewinto = n => n.Dictionary_ID == ID;
                    foreach (var n in DictionaryDAL.Query(funviewinto))
                    {
                        dictionaryID = n.Dictionary_ID;
                        if (n.Dictionary_Name != null)
                        {
                            //字典名称
                            this.txtDictionary_Name.Text = n.Dictionary_Name;
                        }
                        if (n.Dictionary_Value != null)
                        {
                            // 字典值
                            this.txtDictionary_Value.Text = n.Dictionary_Value;
                        }
                        if (n.Dictionary_State != null)
                        {
                            // 字典状态
                            if ("True" == n.Dictionary_State.ToString())
                            {
                                this.comboxDictionary_State.Text = "启动";
                            }
                            else
                            {
                                this.comboxDictionary_State.Text = "暂停";
                            }
                        }
                        if (n.Dictionary_OtherID != null)
                        {
                            // 字典所属                            
                            if (n.Dictionary_OtherID != 0)
                            {
                                string id = n.Dictionary_OtherID.ToString();
                                string other = DictionaryDAL.GetOtherID(id); // 有下级
                                this.comboxDictionary_OtherID.Text = other;
                            }
                            else
                            {
                                this.comboxDictionary_OtherID.Text = n.Dictionary_Name.ToString();
                                //string id = n.Dictionary_OtherID.ToString();
                                //string other = DictionaryDAL.GetDictionaryYesOtherID(id); // 有下级
                                //this.comboxDictionary_OtherID.Text = other;
                            }

                        }
                        if (n.Dictionary_OtherID == null)
                        {
                            this.comboxDictionary_OtherID.Text = "";
                        }
                        if (n.Dictionary_ISLower != null)
                        {
                            // 是否有下级
                            if ("True" == n.Dictionary_ISLower.ToString())
                            {
                                this.comboxDictionary_IsLower.Text = "是";
                            }
                            else
                            {
                                this.comboxDictionary_IsLower.Text = "否";
                            }
                        }
                        if (n.Dictionary_Remark != null)
                        {
                            // 备注
                            this.txtDictionary_Remark.Text = n.Dictionary_Remark;
                        }
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要修改行的信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDictionary_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            //阻止从键盘输入键
            e.Handled = true;

            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {

                if ((e.KeyChar == (char)8)) { e.Handled = false; return; }
                else
                {
                    int len = this.txtDictionary_Value.Text.Length;
                    if (len < 7)
                    {
                        if (len == 0 && e.KeyChar != '0')
                        {
                            e.Handled = false; return;
                        }
                        //else if (len == 0)
                        //{
                        //    MessageBox.Show("编号不能以0开头！"); return;
                        //}
                        e.Handled = false; return;
                    }
                    else
                    {
                        MessageBox.Show("编号最多只能输入2位数字！");
                    }
                }
            }
            else
            {
                MessageBox.Show("编号只能输入数字！");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDicname_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cbxDicname.SelectedValue.ToString()))
                {
                    string strvalue = LinQBaseDao.GetSingle("select Dictionary_ID from Dictionary where Dictionary_ID='" + cbxDicname.SelectedValue.ToString() + "'").ToString();
                    DataTable dt = LinQBaseDao.Query("select Dictionary_Name,Dictionary_ID from Dictionary where Dictionary_OtherID='" + strvalue + "'").Tables[0];
                    this.cbxName.DataSource = dt;
                    this.cbxName.DisplayMember = "Dictionary_Name";
                    this.cbxName.ValueMember = "Dictionary_ID";
                    this.cbxName.SelectedValue = -1;
                }
            }
            catch 
            {

            }
        }
        /// <summary>
        /// 修改字典所属显示
        /// </summary>
        private void DataBindingComplete()
        {
            if (this.dgvDictioanry.Rows.Count != 0)
            {
                for (int i = 0; i < this.dgvDictioanry.Rows.Count; i++)
                {
                    string id = dgvDictioanry.Rows[i].Cells["Dictionary_OtherID"].Value.ToString();
                    if (id == "0")
                    {
                        dgvDictioanry.Rows[i].Cells["DicName"].Value = null;
                    }
                    else
                    {
                        string str = "select Dictionary_Name from Dictionary where Dictionary_ID=" + id;
                        string strvalue = LinQBaseDao.GetSingle(str).ToString();
                        dgvDictioanry.Rows[i].Cells["DicName"].Value = strvalue;
                    }
                }
            }
        }
    }
}
