using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class WeighInfoFrom : Form
    {
        public WeighInfoFrom()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        // 定义一个全局变量： 门岗编号
        int iPositionID = 0;
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = " 1=1 ";


        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeighInfo_Load(object sender, EventArgs e)
        {
            tscbxPageSize.SelectedIndex = 1;
            userContext();
            StetaBingMethod();
            txtUserName.Text = CommonalityEntity.USERNAME;//绑定当前登录人
            LogInfoLoad("");
        }


        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnupdate.Enabled = true;
                btnAdd.Enabled = true;
                btnDelete.Enabled = true;
                btnupdate.Visible = true;
                btnAdd.Visible = true;
                btnDelete.Visible = true;
            }
            else
            {
                btnupdate.Visible = ControlAttributes.BoolControl("btnupdate", "WeighInfoFrom", "Visible");
                btnupdate.Enabled = ControlAttributes.BoolControl("btnupdate", "WeighInfoFrom", "Enabled");

                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "WeighInfoFrom", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "WeighInfoFrom", "Enabled");

                btnDelete.Visible = ControlAttributes.BoolControl("btnDelete", "WeighInfoFrom", "Visible");
                btnDelete.Enabled = ControlAttributes.BoolControl("btnDelete", "WeighInfoFrom", "Enabled");
            }
        }

        /// <summary>
        /// 状态绑定
        /// </summary>
        private void StetaBingMethod()
        {
            try
            {
                var p = DictionaryDAL.GetValueStateDictionary("01");
                int intcount = p.Count();
                var Pcob_DrivewayStrategy_State = p.Where(n => n.Dictionary_Name != "全部").ToList();
                if (Pcob_DrivewayStrategy_State != null && Pcob_DrivewayStrategy_State.Count() > 0)
                {
                    cobWeighInfo_State.DataSource = Pcob_DrivewayStrategy_State;
                    this.cobWeighInfo_State.DisplayMember = "Dictionary_Name";
                    cobWeighInfo_State.ValueMember = "Dictionary_ID";
                    cobWeighInfo_State.SelectedIndex = 0;

                    cobseekSate.DataSource = Pcob_DrivewayStrategy_State;
                    this.cobseekSate.DisplayMember = "Dictionary_Name";
                    cobseekSate.ValueMember = "Dictionary_ID";
                    cobseekSate.SelectedIndex = 0;
                }


            }
            catch 
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.StetaBingMethod()" + "".ToString());
            }

        }



        /// <summary>
        /// 清空方法
        /// </summary>
        private void Empty()
        {
            btnAdd.Enabled = true;
            btnupdate.Enabled = false;
            btnDelete.Enabled = false;
            this.txtWeighInfo_Name.Text = "";
            this.cobseekSate.SelectedIndex = -1;
            txtRemark.Text = "";
            txtWeighInfo_Phone.Text = "";
            txtRemark.Text = "";
            txtseekName.Text = "";
            txtseekPhone.Text = "";
            cobWeighInfo_State.SelectedIndex = -1;
            cobseekSate.SelectedIndex = -1;
        }

        /// <summary>
        /// 双击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnempty_Click(object sender, EventArgs e)
        {
            Empty();
        }
        /// <summary>
        /// 单击保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtWeighInfo_Name.Text.Trim()))
                {
                    MessageBox.Show("请输入地磅房名称");
                    return;
                }
                if (string.IsNullOrEmpty(cobWeighInfo_State.Text.Trim()))
                {
                    MessageBox.Show("请选择启动状态");
                    return;
                }
                if (txtRemark.Text.Trim().Length > 200)
                {
                    MessageBox.Show("超过最大字符，限制200字内");
                    return;
                }
                //查重
                DataSet ds = LinQBaseDao.Query("select COUNT(1) from WeighInfo where WeighInfo_Name='" + txtWeighInfo_Name.Text.Trim() + "'");
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                {
                    MessageBox.Show("该磅房名称已存在");
                    return;
                }
                //进行新增操作
                WeighInfo info = new WeighInfo();
                info.WeighInfo_Name = txtWeighInfo_Name.Text.Trim();
                info.WeighInfo_Phone = txtWeighInfo_Phone.Text.Trim();
                info.WeighInfo_State = cobWeighInfo_State.Text.Trim();
                info.WeighInfo_CreateTime = CommonalityEntity.GetServersTime();
                info.WeighInfo_Remark = txtRemark.Text.Trim();
                if (WeighInfoDAL.InsertOneCamera(info))
                {
                    MessageBox.Show("保存成功");
                    CommonalityEntity.WriteLogData("新增", "新增过磅基础信息：" + txtWeighInfo_Name.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                    Empty();
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
            catch 
            {
                Console.WriteLine("");
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
        private void tslNotChecked()
        {
            for (int i = 0; i < this.dgvWeighInfo.Rows.Count; i++)
            {
                dgvWeighInfo.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvWeighInfo.Rows.Count; i++)
            {
                this.dgvWeighInfo.Rows[i].Selected = true;
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
                tslAllChecked();
                return;
            }
            if (e.ClickedItem.Name == "tslNotSelect")//取消全选
            {
                tslNotChecked();
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
        /// 加载发生
        /// </summary>
        private void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvWeighInfo, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "WeighInfo", "*", "WeighInfo_ID", "WeighInfo_ID", 0, sqlwhere, true);
        }
        #endregion

        /// <summary>
        /// 单击删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int j = 0;//删除标签
                if (this.dgvWeighInfo.SelectedRows.Count > 0)//选中行
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        for (int i = 0; i < dgvWeighInfo.SelectedRows.Count; i++)
                        {
                            Expression<Func<WeighInfo, bool>> funuserinfo = n => n.WeighInfo_ID == int.Parse(this.dgvWeighInfo.SelectedRows[i].Cells["WeighInfo_ID"].Value.ToString());

                            if (!WeighInfoDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                            }
                        }
                        if (j == 0)
                        {
                            MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("请选中要删除的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch 
            {
                MessageBox.Show("");
            }
            finally
            {
                LogInfoLoad("");
                Empty();
            }
        }

        /// <summary>
        /// 单击修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvWeighInfo.SelectedRows.Count > 0)//选中行
                {
                    if (dgvWeighInfo.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        //进行修改
                        WeighInfo info = new WeighInfo();
                        if (string.IsNullOrEmpty(txtWeighInfo_Name.Text.Trim()))
                        {
                            MessageBox.Show("地磅房名称不能为空");
                            return;
                        }
                        if (string.IsNullOrEmpty(cobWeighInfo_State.Text.Trim()))
                        {
                            MessageBox.Show("请选择启动状态");
                            return;
                        }
                        if (dgvWeighInfo.SelectedRows[0].Cells["WeighInfo_Name"].Value.ToString() != txtWeighInfo_Name.Text.Trim())
                        {
                            //查重
                            DataSet ds = LinQBaseDao.Query("select COUNT(1) from WeighInfo where WeighInfo_Name='" + txtWeighInfo_Name.Text.Trim() + "'");
                            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                            {
                                MessageBox.Show("该磅房名称已存在");
                                Empty();
                                return;
                            }
                        }
                        Expression<Func<WeighInfo, bool>> p = n => n.WeighInfo_ID == int.Parse(this.dgvWeighInfo.SelectedRows[0].Cells["WeighInfo_ID"].Value.ToString());
                        Action<WeighInfo> ap = s =>
                        {
                            s.WeighInfo_Name = txtWeighInfo_Name.Text.Trim();
                            s.WeighInfo_Phone = this.txtWeighInfo_Phone.Text.Trim();
                            s.WeighInfo_State = this.cobWeighInfo_State.Text.Trim();
                            s.WeighInfo_Remark = this.txtRemark.Text.Trim();
                        };
                        if (WeighInfoDAL.Update(p, ap))
                        {
                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Empty();
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
                CommonalityEntity.WriteTextLog("地磅房信息管理 btnupdate_Click()" + "".ToString());
            }
            finally
            {
                LogInfoLoad("");
            }

        }

        //双击选数据
        private void dgvWeighInfo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvWeighInfo.SelectedRows.Count > 0)//选中行
                {
                    if (dgvWeighInfo.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    txtWeighInfo_Name.Text = dgvWeighInfo.SelectedRows[0].Cells["WeighInfo_Name"].Value.ToString();
                    txtWeighInfo_Phone.Text = dgvWeighInfo.SelectedRows[0].Cells["WeighInfo_Phone"].Value.ToString();
                    cobWeighInfo_State.Text = dgvWeighInfo.SelectedRows[0].Cells["WeighInfo_State"].Value.ToString();
                    txtRemark.Text = dgvWeighInfo.SelectedRows[0].Cells["WeighInfo_Remark"].Value.ToString();
                }
            }
            catch 
            {
                MessageBox.Show("");
            }
            finally
            {
                btnupdate.Enabled = true;
                btnAdd.Enabled = false;
            }
        }

        /// <summary>
        /// 单击搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            sqlwhere = " 1=1";
            if (!string.IsNullOrEmpty(txtseekName.Text.Trim()))
            {
                sqlwhere += " and WeighInfo_Name like '%" + txtseekName.Text.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(txtseekPhone.Text.Trim()))
            {
                sqlwhere += " and WeighInfo_Phone like '%" + txtseekPhone.Text.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(cobseekSate.Text.Trim()))
            {
                sqlwhere += " and WeighInfo_State = '" + cobseekSate.Text.Trim() + "' ";
            }

            page.BindBoundControl(dgvWeighInfo, "", tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "WeighInfo", "*", "WeighInfo_ID", "WeighInfo_ID", 0, sqlwhere, true);
        }

        private void dgvWeighInfo_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;

        }
    }
}
