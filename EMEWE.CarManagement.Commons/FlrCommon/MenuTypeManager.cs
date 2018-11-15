using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class MenuTypeManager : Form
    {
        public MenuTypeManager()
        {
            InitializeComponent();
        }

        #region --方法--
        private int Judgment()
        {
            if (MenuInfoManager.MenuTypeId == 1)//如果是修改
            {
                gbxContext.Text = "修改菜单类型";
                this.Text = "修改菜单类型";
                btnOperating.Text = "修改菜单";
                return 1;
            }

            if (MenuInfoManager.MenuTypeId == 3)//删除
            {
                gbxContext.Text = "删除菜单类型";
                this.Text = "删除菜单类型";
                btnOperating.Text = "删除菜单";
                return 3;
            }
            if (MenuInfoManager.MenuTypeId == 2)//添加
            {
                gbxContext.Text = "添加菜单类型";
                this.Text = "添加菜单类型";
                btnOperating.Text = "添加菜单";
                return 2;
            }
            else
            {
                CommonalityEntity.WriteTextLog("修改页面出现异常");
                this.Close();//异常 退出并记录日志
                return 0;
            }
        }

        /// <summary>
        /// 绑定菜单类型
        /// </summary>
        private void MenuControlType()
        {
            coboxMenuControlType.DataSource = MenuTypeDAL.Query();
            coboxMenuControlType.DisplayMember = "MenuType_Name";
            coboxMenuControlType.ValueMember = "MenuType_ID";
            if (coboxMenuControlType.DataSource != null)
            {
                coboxMenuControlType.SelectedIndex = -1;
            }
        }
        //查询数据并显示
        private void SearchMenuType(string menuTypeName)
        {
            comboxMenuState.Text = "";
            txtMenuTypeName.Text = "";
            comboxMenuTypeOrder.Text = "";



            Expression<Func<MenuType, bool>> menuinfoFunc = n => n.MenuType_Name.Trim() == menuTypeName;
            foreach (var n in MenuTypeDAL.Query(menuinfoFunc))
            {

                if (n.MenuType_State != null)
                {
                    comboxMenuState.Text = n.MenuType_State.ToString();
                }
                if (n.MenuType_Name != null)
                {
                    txtMenuTypeName.Text = n.MenuType_Name;
                }
                if (n.MenuType_Order != null)
                {
                    comboxMenuTypeOrder.SelectedText = n.MenuType_Order.ToString();
                }
            }
        }

        private bool SearchName(string menuTypeName)
        {
            try
            {
                Expression<Func<MenuType, bool>> funview_userinfo = n => n.MenuType_Name.Trim() == menuTypeName;
                if (MenuTypeDAL.Query(funview_userinfo).Count() > 0)
                {
                    return true;
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("角色管理 btnCheck()" );
            }
            return false;
        }

        private bool Add()
        {
            try
            {
                PageControl mf = new PageControl();
                if (string.IsNullOrEmpty(txtMenuTypeName.Text))
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "控件类型不能为空！", txtMenuTypeName, this);
                    return false;
                }
                if (string.IsNullOrEmpty(coboxMenuControlType.Text))
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "菜单名称不能为空！", coboxMenuControlType, this);
                    return false;
                }
                if (comboxMenuTypeOrder.Text.ToString() == "" || comboxMenuTypeOrder.Text.ToString() == null)
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "是否可见不能为空！", comboxMenuTypeOrder, this);
                    return false;
                }

                var MenuTypeAdd = new MenuType
                {
                    MenuType_State = Convert.ToInt32(comboxMenuState.SelectedValue),//状态
                    MenuType_Name = coboxMenuControlType.Text.ToString().Trim(),
                    MenuType_Remark = txtMenuTypeName.Text.Trim(),
                    MenuType_Order = Convert.ToInt32(comboxMenuTypeOrder.Text.Trim()),
                };
                int rint = 0;
                if (MenuTypeDAL.InsertOneQCRecord(MenuTypeAdd, out rint) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                MessageBox.Show(" 添加信息出现异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommonalityEntity.WriteTextLog("MenuInfoUpdate.btnOperating_Click()" );
            }
            return false;
        }

        private bool Delete()
        {
            try
            {
                label2.Text = coboxMenuControlType.Text.Trim();
                Expression<Func<MenuType, bool>> funMenuInfo = n => n.MenuType_Name == coboxMenuControlType.Text.Trim();
                if (MenuTypeDAL.DeleteToMany(funMenuInfo) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                MessageBox.Show(" 删除信息出现异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommonalityEntity.WriteTextLog("MenuInfoUpdate.btnOperating_Click()");
            }

            return false;
        }


        private bool Update()
        {
            try
            {
                PageControl mf = new PageControl();
                if (string.IsNullOrEmpty(txtMenuTypeName.Text))
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "控件类型不能为空！", txtMenuTypeName, this);
                    return false;
                }
                if (string.IsNullOrEmpty(coboxMenuControlType.Text))
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "菜单名称不能为空！", coboxMenuControlType, this);
                    return false;
                }
                if (comboxMenuTypeOrder.Text.ToString() == "" || comboxMenuTypeOrder.Text.ToString() == null)
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "是否可见不能为空！", comboxMenuTypeOrder, this);
                    return false;
                }

                //修改MenuType信息
                Action<MenuType> actionMY = M =>
                {
                    M.MenuType_Remark = coboxMenuControlType.Text;
                    M.MenuType_Name = txtMenuTypeName.Text;
                    M.MenuType_State = Convert.ToInt32(comboxMenuState.SelectedValue);
                    M.MenuType_Order = Convert.ToInt32(comboxMenuTypeOrder.Text);
                };
                Expression<Func<MenuType, bool>> funroleinfo = n => n.MenuType_Name.ToString() == txtMenuTypeName.Text;//执行数据
                if (MenuInfoDAL.Update(funroleinfo, actionMY) == true)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(" 修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            catch 
            {
                MessageBox.Show(" 修改信息出现异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommonalityEntity.WriteTextLog("MenuInfoUpdate.btnOperating_Click()" );
                return false;
            }
        }
        #endregion
        private void comboxMenuTypeOrderBind()
        {
            int maxID = Convert.ToInt32(LinQBaseDao.GetSingle("select max(MenuType_ID+1)  From dbo.MenuType"));
            int[] arr = new int[maxID + 1];
            for (int i = 0; i <= maxID; i++)
            {
                arr[i] = i + 1;
            }
            comboxMenuTypeOrder.DataSource = arr;
        }

        private void MenuInfoUpdate_Load(object sender, EventArgs e)
        {
            //1.判断是修改还是添加，控件的启用。如果是添加则传参为null（特定）
            //如果是修改则必须要传入参数，如果判断参数无效则关闭窗体，并记录日志
            //默认获取数据位|“string 1”

            try
            {
                if (MenuInfoManager.MenuTypeId == 1)//如果是修改
                {
                    gbxContext.Text = "修改菜单类型";
                    this.Text = "修改菜单类型";
                    btnOperating.Text = "修改菜单";
                    MenuControlType();
                    comboxMenuTypeOrderBind();
                    coboxMenuControlType.SelectedValue = Convert.ToInt32(MenuInfoManager.Cob_Menu_ControlTypes);
                }

                if (MenuInfoManager.MenuTypeId == 3)//删除
                {
                    gbxContext.Text = "删除菜单类型";
                    this.Text = "删除菜单类型";
                    btnOperating.Text = "删除菜单";
                    MenuControlType();
                    coboxMenuControlType.SelectedValue = Convert.ToInt32(MenuInfoManager.Cob_Menu_ControlTypes);
                }
                if (MenuInfoManager.MenuTypeId == 2)//添加
                {
                    gbxContext.Text = "添加菜单类型";
                    this.Text = "添加菜单类型";
                    btnOperating.Text = "添加菜单";
                    comboxMenuTypeOrderBind();
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("MenuInfoUpdate.MenuInfoUpdate_Load 修改页面出现异常");
                this.Close();//异常 退出并记录日志
            }
        }

        private void btnOperating_Click(object sender, EventArgs e)
        {
            try
            {
                if (Judgment() == 1)//修改
                {
                    if (Update() == true)
                    {
                        MessageBox.Show("修改成功！");
                        MenuControlType();
                    }
                }
                if (Judgment() == 3)//删除
                {
                    if (Delete() == true)
                    {
                        MessageBox.Show("删除成功！");
                        MenuControlType();
                    }
                }
                if (Judgment() == 2)//添加
                {
                    if (SearchName(coboxMenuControlType.SelectedText.Trim()) == false)//检车是否一次名称
                    {
                        MessageBox.Show("菜单类型已存在！");
                        return;
                    }
                    if (Add() == true)
                    {
                        MessageBox.Show("添加成功！"); return;
                    }
                }
            }
            catch
            {
                MessageBox.Show(" 操作出现异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommonalityEntity.WriteTextLog("MenuInfoUpdate.btnOperating_Click()" );
                this.Close();
            }
        }

        private void coboxMenuControlType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboxMenuState.Text = null;
            txtMenuTypeName.Text = null;
            comboxMenuTypeOrder.Text = null;
            if (coboxMenuControlType.SelectedText.Trim() != "")
            {
                SearchMenuType(coboxMenuControlType.SelectedText.Trim());
            }
            else
                return;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}