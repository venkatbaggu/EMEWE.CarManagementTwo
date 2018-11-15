using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagementDAL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CameraForm : Form
    {
        public CameraForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;
        public static MainForm mf; // 主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<View_Camera_Driveway_Position, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        // 定义一个公共的变量： 摄像头编号
        public int iCameraID = 0;
        // 定义一个公共的变量： 门岗编号
        public int iDrivewayPositionID = 0;

        /// <summary>
        /// Load 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraForm_Load(object sender, EventArgs e)
        {
            try
            {
                userContext();
                btnUpdate.Enabled = false;
                btnSelect_Click(btnSelect, null);
                BindCamera();
                BindSearchCamera();
                BindCameraLoca();
                BindCameraLocaSearch();
                BindMenGang(); // 调用绑定门岗的方法
                BindMenGang1();
                DrivewayName();
                cmbDVI.SelectedIndex = 0;
                mf = new MainForm();
                LoadData(); // 调用显示DatagridView的方法

                if (iCameraID > 0)
                {
                    // 若通道管理界面中有选择“通道”再跳转的，则显示如下
                    Expression<Func<View_DrivewayPosition, bool>> funviewinto = n => n.Driveway_ID == iCameraID;
                    foreach (var n in DrivewayDAL.Query(funviewinto))
                    {
                        if (n.Position_Name != null)
                        {
                            this.cbxPositionName.Text = n.Position_Name;
                            this.cbxDrivewayName.Text = n.Driveway_Name + n.Driveway_Type;
                            string PName = n.Position_Name;  //获取数据库读取到的门岗名称
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("加载摄像头管理信息有误，请查看相关信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "CameraForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "CameraForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "CameraForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "CameraForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "CameraForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "CameraForm", "Enabled");
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvCamera.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvCamera.DataSource = null;
                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()");
            }
           
        }
        #region comboBox 下拉框的绑定
        /// <summary>
        /// 绑定摄像头状态
        /// </summary>
        private void BindCamera()
        {
            try
            {
                this.cbxCamera_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.cbxCamera_State.DataSource != null)
                {
                    this.cbxCamera_State.DisplayMember = "Dictionary_Name";
                    this.cbxCamera_State.ValueMember = "Dictionary_ID";
                    this.cbxCamera_State.SelectedValue = -1;
                }
                else
                {
                    MessageBox.Show("摄像头状态暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("摄像头管理“摄像头状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索--绑定摄像头状态
        /// </summary>
        private void BindSearchCamera()
        {
            try
            {
                this.cbxCameraState.DataSource = DictionaryDAL.GetValueStateDictionary("01");

                if (this.cbxCameraState.DataSource != null)
                {
                    this.cbxCameraState.DisplayMember = "Dictionary_Name";
                    this.cbxCameraState.ValueMember = "Dictionary_ID";
                    this.cbxCameraState.SelectedIndex = 3;
                }
                else
                {
                    MessageBox.Show("摄像头状态暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("摄像头管理“摄像头状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定门岗名称
        /// </summary>
        private void BindMenGang()
        {
            try
            {
                string sql = "select * from Position where Position_State='启动'";
                this.cbxPositionName.DataSource = PositionDAL.GetViewPosition(sql);

                if (PositionDAL.GetViewPosition(sql).Count() > 0)
                {
                    this.cbxPositionName.DisplayMember = "Position_Name";
                    this.cbxPositionName.ValueMember = "Position_ID";
                    cbxPositionName.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("门岗暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("摄像头管理“门岗”绑定有误，请查看门岗信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定摄像头位置
        /// </summary>
        private void BindCameraLoca()
        {
            try
            {
                this.cbxCamera_Location.DataSource = DictionaryDAL.GetValueDictionary("04").Where(n => n.Dictionary_Name != "全部").ToList();

                if (cbxCamera_Location.DataSource != null)
                {
                    this.cbxCamera_Location.DisplayMember = "Dictionary_Name";
                    cbxCamera_Location.ValueMember = "Dictionary_ID";
                    cbxCamera_Location.SelectedValue = -1;
                }
                else
                {
                    MessageBox.Show("摄像头位置暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("摄像头管理“摄像头位置”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索--绑定摄像头位置
        /// </summary>
        private void BindCameraLocaSearch()
        {
            try
            {
                this.cbxCameraLocation.DataSource = DictionaryDAL.GetValueStateDictionary("04");

                if (cbxCameraLocation.DataSource != null)
                {
                    this.cbxCameraLocation.DisplayMember = "Dictionary_Name";
                    cbxCameraLocation.ValueMember = "Dictionary_ID";
                    cbxCameraLocation.SelectedIndex = 2;
                }
                else
                {
                    MessageBox.Show("摄像头位置暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("摄像头管理“摄像头位置”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索 --绑定门岗
        /// </summary>
        private void BindMenGang1()
        {
            try
            {
                string sql = "select * from Position where Position_State='启动'";
                this.comboxMenGang.DataSource = PositionDAL.GetViewPosition(sql);

                if (PositionDAL.GetViewPosition(sql).Count() > 0)
                {
                    this.comboxMenGang.DisplayMember = "Position_Name";
                    this.comboxMenGang.ValueMember = "Position_ID";
                    comboxMenGang.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("门岗暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("摄像头管理“门岗”绑定有误，请查看门岗信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索 --绑定通道
        /// </summary>
        private void DrivewayName()
        {
            try
            {
                string sql = "select distinct Driveway_Name from Driveway";
                this.comboxTongDao.DataSource = LinQBaseDao.Query(sql).Tables[0];

                if (this.comboxTongDao.DataSource != null)
                {
                    this.comboxTongDao.DisplayMember = "Driveway_Name";
                    //this.comboxTongDao.ValueMember = "Driveway_ID";
                    comboxTongDao.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("通道暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("摄像头管理“通道”绑定有误，请查看通道信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion
        /// <summary>
        /// 在门岗名称上更改SelectedValueChanged的值时引发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPositionName_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbxDrivewayName.Items.Count > 0)
            {
                this.cbxDrivewayName.Items.Clear();
            }
            if (cbxPositionName.Text == "") { return; }

            string PositionName = this.cbxPositionName.Text;
            var name = DrivewayDAL.GetViewDrivewayName(String.Format("select * from View_DrivewayPosition where Position_Name='{0}' and Driveway_State='启动'", PositionName));
            if (name != null)
            {
                foreach (var item in name)
                {
                    if (item.Driveway_Name != null)
                    {
                        if (item.Driveway_Type != null)
                        {
                            cbxDrivewayName.Items.Add(item.Driveway_Name + item.Driveway_Type);
                            cbxDrivewayName.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 查重方法
        /// </summary>
        /// <returns></returns>
        private bool btnCheck()
        {
            bool rbool = true;
            try
            {
                string PositionName = this.cbxPositionName.Text.ToString();
                int lenth = cbxDrivewayName.Text.Trim().Length;
                string DrivewayName = cbxDrivewayName.Text.Trim().Substring(0, 4);
                string DrivewayType = cbxDrivewayName.Text.Trim().Substring(lenth - 1, 1);
                string CameraName = this.txtCamera_Name.Text.Trim();
                string CameraCardAdd = this.txtCamera_CardAdd.Text.Trim();
                //判断名称是否已存在
                Expression<Func<View_Camera_Driveway_Position, bool>> funviewCamera = n => n.Position_Name == PositionName && n.Driveway_Name == DrivewayName && n.Camera_Name == CameraName;
                if (CameraDAL.QueryView(funviewCamera).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该摄像头名称已存在", txtCamera_Name, this);
                    txtCamera_Name.Focus();
                    rbool = false; ;
                }
                Expression<Func<View_Camera_Driveway_Position, bool>> funviewCamera1 = n => n.Position_Name == PositionName && n.Driveway_Name == DrivewayName && n.Camera_AddCard == CameraCardAdd;
                if (CameraDAL.QueryView(funviewCamera1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该摄像头地址码已存在", txtCamera_CardAdd, this);
                    txtCamera_CardAdd.Focus();
                    rbool = false; ;
                }
                return rbool;

            }
            catch
            {
                CommonalityEntity.WriteTextLog("摄像头管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }
        /// <summary>
        /// 查重方法
        /// </summary>
        /// <returns></returns>
        private bool btnCheckupdate()
        {
            bool rbool = true;
            try
            {
                string PositionName = this.cbxPositionName.Text.ToString();
                int lenth = cbxDrivewayName.Text.Trim().Length;
                string DrivewayName = cbxDrivewayName.Text.Trim().Substring(0, 4);
                string DrivewayType = cbxDrivewayName.Text.Trim().Substring(lenth - 1, 1);
                string CameraName = this.txtCamera_Name.Text.Trim();
                string CameraCardAdd = this.txtCamera_CardAdd.Text.Trim();
                //判断名称是否已存在
                Expression<Func<View_Camera_Driveway_Position, bool>> funviewCamera = n => n.Position_Name == PositionName && n.Driveway_Name == DrivewayName && n.Camera_Name == CameraName && n.Camera_Name != this.dgvCamera.SelectedRows[0].Cells["Camera_Name"].Value.ToString();
                if (CameraDAL.QueryView(funviewCamera).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该摄像头名称已存在", txtCamera_Name, this);
                    txtCamera_Name.Focus();
                    rbool = false; ;
                }
                Expression<Func<View_Camera_Driveway_Position, bool>> funviewCamera1 = n => n.Position_Name == PositionName && n.Driveway_Name == DrivewayName && n.Camera_AddCard == CameraCardAdd && n.Camera_AddCard != this.dgvCamera.SelectedRows[0].Cells["Camera_AddCard"].Value.ToString();
                if (CameraDAL.QueryView(funviewCamera1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该摄像头地址码已存在", txtCamera_CardAdd, this);
                    txtCamera_CardAdd.Focus();
                    rbool = false; ;
                }
                return rbool;

            }
            catch
            {
                CommonalityEntity.WriteTextLog("摄像头管理 btnCheck()");
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
                if (this.txtCamera_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "摄像头名称不能为空！", txtCamera_Name, this);
                    return;
                }
                if (this.txtCamera_CardAdd.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "摄像头地址码不能为空！", txtCamera_CardAdd, this);
                    return;
                }

                string PositionName = this.cbxPositionName.Text.ToString();
                int lenth = cbxDrivewayName.Text.Trim().Length;
                string DrivewayName = cbxDrivewayName.Text.Trim().Substring(0, 4);
                string DrivewayType = cbxDrivewayName.Text.Trim().Substring(lenth - 1, 1);
                int DrivewayID = 0;
                var p = DrivewayDAL.GetViewDrivewayName(String.Format("select Driveway_ID from View_DrivewayPosition where Position_Name='{0}' and  Driveway_Name= '{1}' and Driveway_Type= '{2}'", PositionName, DrivewayName, DrivewayType));
                if (p != null)
                {
                    foreach (var n in p)
                    {
                        if (n.Driveway_ID > 0)
                        {
                            DrivewayID = n.Driveway_ID;
                        }
                        break;
                    }
                }
                if (!btnCheck()) return; // 去重复
                var Cameraadd = new Camera
                {
                    Camera_Driveway_ID = DrivewayID,
                    Camera_Name = this.txtCamera_Name.Text.Trim(),
                    Camera_AddCard = this.txtCamera_CardAdd.Text.Trim(),
                    Camera_State = this.cbxCamera_State.Text,
                    Camera_Location = this.cbxCamera_Location.Text,
                    Camera_Remark = this.txtCamera_Remark.Text.Trim(),
                    Camera_DVIName = cmbDVI.Text
                };

                if (CameraDAL.InsertOneCamera(Cameraadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonalityEntity.WriteLogData("新增", "新增摄像头名称为：" + txtCamera_Name.Text.Trim() + "的信息", CommonalityEntity.USERNAME);//添加操作日志

                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("摄像头管理 btnSave_Click()");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// “修改”  按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtCamera_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "摄像头名称不能为空！", txtCamera_Name, this);
                    return;
                }
                if (this.txtCamera_CardAdd.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "摄像头地址码不能为空！", txtCamera_CardAdd, this);
                    return;
                }

                string PositionName = this.cbxPositionName.Text.ToString();
                int lenth = cbxDrivewayName.Text.Trim().Length;
                string DrivewayName = cbxDrivewayName.Text.Trim().Substring(0, 4);
                string DrivewayType = cbxDrivewayName.Text.Trim().Substring(lenth - 1, 1);
                int DrivewayID = 0;
                var p = DrivewayDAL.GetViewDrivewayName(String.Format("select Driveway_ID from View_DrivewayPosition where Position_Name='{0}' and  Driveway_Name= '{1}' and Driveway_Type= '{2}'", PositionName, DrivewayName, DrivewayType));
                if (p != null)
                {
                    foreach (var n in p)
                    {
                        if (n.Driveway_ID > 0)
                        {
                            DrivewayID = n.Driveway_ID;
                        }
                        break;
                    }
                }
                if (this.dgvCamera.SelectedRows.Count > 0)//选中行
                {
                    if (dgvCamera.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (!btnCheckupdate()) return; // 去重复
                        Expression<Func<Camera, bool>> pc = n => n.Camera_ID == int.Parse(this.dgvCamera.SelectedRows[0].Cells["Camera_ID"].Value.ToString());
                        string id = "";
                        string strfront = "";
                        string strcontent = "";
                        Action<Camera> ap = s =>
                        {
                            strfront = s.Camera_Driveway_ID + "," + s.Camera_Name + "," + s.Camera_AddCard + "," + s.Camera_State + "," + s.Camera_Location + "," + s.Camera_Remark;
                            s.Camera_Driveway_ID = DrivewayID;
                            s.Camera_Name = this.txtCamera_Name.Text.Trim();
                            s.Camera_AddCard = this.txtCamera_CardAdd.Text.Trim();
                            s.Camera_State = this.cbxCamera_State.Text;
                            s.Camera_Location = this.cbxCamera_Location.Text;
                            s.Camera_Remark = this.txtCamera_Remark.Text.Trim();
                            s.Camera_DVIName = cmbDVI.Text;
                            strcontent = s.Camera_Driveway_ID + "," + s.Camera_Name + "," + s.Camera_AddCard + "," + s.Camera_State + "," + s.Camera_Location + "," + s.Camera_Remark;
                            id = s.Camera_ID.ToString();
                        };

                        if (CameraDAL.Update(pc, ap))
                        {
                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的摄像头信息；修改前：" + strfront + "；修改后：" + strcontent, CommonalityEntity.USERNAME);//添加操作日志
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
                CommonalityEntity.WriteTextLog("摄像头管理 btnUpdate_Click()");
            }
            finally
            {
                LogInfoLoad("");
                this.btnUpdate.Enabled = false;
                this.btnSave.Enabled = true;
            }
        }

        /// <summary>
        /// “删除” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            tslDelCamera(); // 调用 删除选中数据信息的方法
        }
        /// <summary>
        ///删除选中数据信息的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslDelCamera()
        {
            try
            {
                int j = 0;
                if (dgvCamera.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvCamera.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int cameraid = int.Parse(this.dgvCamera.SelectedRows[i].Cells["Camera_ID"].Value.ToString());
                            Expression<Func<Camera, bool>> funuserinfo = n => n.Camera_ID == cameraid;
                            string strContent = LinQBaseDao.Query("select Camera_Name from Camera where Camera_ID=" + cameraid).Tables[0].Rows[0][0].ToString();
                            if (CameraDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除摄像头名称为：" + strContent + "的信息", CommonalityEntity.USERNAME);//添加操作日志
                            }
                        }
                        if (j == count)
                        {
                            MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else//没有选中
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("摄像头管理 tslDelCamera()+");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// 清空 事件
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
            this.txtCamera_Name.Text = "";
            this.txtCamera_CardAdd.Text = "";
            this.cbxCamera_State.SelectedValue = 1;
            this.cbxCamera_Location.SelectedValue = 1;
            this.txtCamera_Remark.Text = "";
            this.cbxPositionName.SelectedValue = 1;
            this.cbxDrivewayName.SelectedValue = 1;
            this.comboxMenGang.SelectedIndex = -1;
            this.comboxTongDao.SelectedIndex = -1;
        }

        /// <summary>
        /// 搜 索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (btnSelect.Enabled)
            {
                btnSelect.Enabled = false;
                selectTJ();
                //LogInfoDAL.loginfoadd("查询","检测项目查询",Common.USERNAME);//操作日志
                btnSelect.Enabled = true;
            }
        }
        /// <summary>
        /// 搜索的方法
        /// </summary>
        private void selectTJ()
        {
            #region 注释
            //try
            //{
            //    if (expr == null)
            //    {
            //        expr = (Expression<Func<View_Camera_Driveway_Position, bool>>>)PredicateExtensionses.True<View_Camera_Driveway_Position>();
            //    }

            //    int i = 0;
            //    if (!string.IsNullOrEmpty(this.txtCamera_Name.Text.Trim()))//摄像头名称
            //    {
            //        expr = expr.And(n => n.Camera_Name.Contains(txtCamera_Name.Text.Trim()));

            //        i++;
            //    }
            //    if (this.cbxCameraState.Text != null)//摄像头状态
            //    {
            //        expr = expr.And(n => n.Camera_State.Contains(cbxCameraState.Text));

            //        i++;
            //    }
            //    if (this.cbxCameraLocation.Text != null)//摄像头位置
            //    {
            //        expr = expr.And(n => n.Camera_Location.Contains(cbxCameraLocation.Text));

            //        i++;
            //    }
            //    if (i == 0)
            //    {
            //        expr = null;
            //    }
            //}
            //catch 
            //{
            //    common.WriteTextLog("通道管理 selectTJ()" + "".ToString());
            //}
            //finally
            //{
            //    LogInfoLoad("");
            //}
            #endregion

            try
            {
                sqlwhere = "  1=1";
                string name = this.txtCameraName.Text.Trim();
                string state = this.cbxCameraState.Text;
                string location = this.cbxCameraLocation.Text;
                string DrivewayName = this.comboxTongDao.Text.Trim();
                string PositionName = this.comboxMenGang.Text.Trim();


                if (!string.IsNullOrEmpty(state))//摄像头状态
                {
                    if (state == "全部")
                    {
                        sqlwhere = "  1=1";
                    }
                    else
                    {
                        sqlwhere += String.Format(" and Camera_State like  '%{0}%'", state);
                    }
                }
                if (!string.IsNullOrEmpty(location))//摄像头位置
                {
                    if (location == "全部")
                    {
                        sqlwhere = "  1=1";
                    }
                    else
                    {
                        sqlwhere += String.Format(" and Camera_Location like  '%{0}%'", location);
                    }
                }
                if (!string.IsNullOrEmpty(PositionName))//门岗名称
                {
                    sqlwhere += String.Format(" and Position_Name like  '%{0}%'", PositionName);
                }
                if (!string.IsNullOrEmpty(DrivewayName))//通道名称
                {
                    sqlwhere += String.Format(" and Driveway_Name like  '%{0}%'", DrivewayName);
                }
                if (!string.IsNullOrEmpty(name))//摄像头名称
                {
                    sqlwhere += String.Format(" and Camera_Name like  '%{0}%'", name);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CameraForm.selectTJ异常:");
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
        private void tslNotSelect()
        {
            for (int i = 0; i < this.dgvCamera.Rows.Count; i++)
            {
                dgvCamera.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll()
        {
            for (int i = 0; i < dgvCamera.Rows.Count; i++)
            {
                this.dgvCamera.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslAllCheck")//全选
            {
                tslSelectAll();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                tslNotSelect();
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
            page.BindBoundControl(dgvCamera, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_Camera_Driveway_Position", "*", "Camera_ID", "Camera_ID", 0, sqlwhere, true);
        }
        #endregion

        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCamera_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvCamera.SelectedRows.Count > 0)//选中行
            {
                if (dgvCamera.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvCamera.SelectedRows[0].Cells["Camera_ID"].Value.ToString());
                    Expression<Func<View_Camera_Driveway_Position, bool>> funviewinto = n => n.Camera_ID == ID;
                    foreach (var n in CameraDAL.QueryView(funviewinto))
                    {
                        if (n.Position_Name != null)
                        {
                            // 门岗名称
                            this.cbxPositionName.Text = n.Position_Name;
                        }
                        if (n.Driveway_ID > 0)
                        {
                            //通道名称
                            this.cbxDrivewayName.Text = n.Driveway_Name + n.Driveway_Type;
                        }
                        if (n.Camera_AddCard != null)
                        {
                            // 摄像头地址码
                            this.txtCamera_CardAdd.Text = n.Camera_AddCard;
                        }
                        if (n.Camera_Name != null)
                        {
                            // 摄像头名称
                            this.txtCamera_Name.Text = n.Camera_Name;
                        }

                        if (n.Camera_State != null)
                        {
                            // 摄像头状态
                            this.cbxCamera_State.Text = n.Camera_State;
                        }
                        if (n.Camera_Location != null)
                        {
                            // 摄像头拍照位置
                            this.cbxCamera_Location.Text = n.Camera_Location;
                        }
                        if (n.Camera_Remark != null)
                        {
                            // 摄像头备注
                            this.txtCamera_Remark.Text = n.Camera_Remark;
                        }
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要修改的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtCamera_CardAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            //阻止从键盘输入键
            e.Handled = true;

            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {

                if ((e.KeyChar == (char)8))
                { e.Handled = false; return; }
                else
                {
                    int len = this.txtCamera_CardAdd.Text.Length;
                    if (len < 2)
                    {
                        if (len == 0 && e.KeyChar != '0')
                        {
                            e.Handled = false; return;
                        }
                        //else if (len == 0)
                        //{
                        //    MessageBox.Show("地感PLC值不能以0开头！"); return;
                        //}
                        e.Handled = false; return;
                    }
                    else
                    {
                        MessageBox.Show("地感PLC值最多只能输入2位数字！");
                    }
                }
            }
            else
            {
                MessageBox.Show("地感PLC值只能输入数字！");
            }
        }

        private void dgvCamera_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

       
    }
}
