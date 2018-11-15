using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagementDAL;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class UserPasswordMg : Form
    {
        public UserPasswordMg()
        {
            InitializeComponent();
        }
        PageControl mf = new PageControl();
        private void UserPasswordMg_Load(object sender, EventArgs e)
        {
        }

        private void btnEditPwd_Click(object sender, EventArgs e)
        {
            if (txtPwd1.Text.Trim().ToLower() != txtpwd2.Text.Trim().ToLower())
            {
                mf.ShowToolTip(ToolTipIcon.Info, "提示", "新密码不一致!", txtPwd1, this);
                txtPwd1.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtpwd2.Text.Trim()))
            {
                mf.ShowToolTip(ToolTipIcon.Info, "提示", "新密码不能为空", txtpwd2, this);
                txtOlePwd.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtOlePwd.Text.Trim()))
            {
                mf.ShowToolTip(ToolTipIcon.Info, "提示", "旧密码不能为空", txtOlePwd, this);
                txtOlePwd.Focus();
                return;
            }

            if (LinQBaseDao.Query("select * from UserInfo where userid=" + CommonalityEntity.USERID + "").Tables[0].Rows.Count <= 0)
            {
                mf.ShowToolTip(ToolTipIcon.Info, "提示", "登录信息有误，请重新登录！", txtOlePwd, this);
                GC.Collect();
                Application.ExitThread();
                Application.Exit();
                Process.GetCurrentProcess().Kill();
                System.Environment.Exit(System.Environment.ExitCode);
                Application.ExitThread();
                return;
            }

            if ((LinQBaseDao.Query("select * from UserInfo where userid=" + CommonalityEntity.USERID + " and UserLoginId='" + CommonalityEntity.USERNAME + "' and UserLoginPwd='" + CommonalityEntity.EncryptDES(txtOlePwd.Text.Trim()) + "'").Tables[0].Rows.Count <= 0))
            {
                mf.ShowToolTip(ToolTipIcon.Info, "提示", "旧密码有误！", txtOlePwd, this);
                txtOlePwd.Focus();
                return;
            }

            Action<UserInfo> action = n =>
            {
                n.UserLoginPwd = CommonalityEntity.EncryptDES(txtpwd2.Text.Trim());
            };
            Expression<Func<UserInfo, bool>> funuser = n => n.UserId == CommonalityEntity.USERID;
            if (UserInfoDAL.Update(funuser, action) == true)//角色是否修改失败
            {
                MessageBox.Show("密码修改成功！");
                txtOlePwd.Text = "";
                txtPwd1.Text = "";
                txtpwd2.Text = "";
                CommonalityEntity.WriteLogData("修改", "用户" + CommonalityEntity.USERNAME + "修改密码", CommonalityEntity.USERNAME);//添加日志
            }
        }
    }
}
