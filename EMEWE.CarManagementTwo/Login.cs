using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// 导入引用
using System.Configuration;
using System.Xml;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using EMEWE.Common;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.SystemAdmin;
using EMEWE.CarManagementDAL;
using EMEWE.CarManagement.Commons;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Web;
using WindowsFormsApplication3;
using SAPLogonCtrl;

namespace EMEWE.CarManagement
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private string UserName;//用户名
        private string UserID;//用户ID
        public MainForm mf = null;

        /// <summary>
        /// 登录的窗体的加载显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginForm_Load(object sender, EventArgs e)
        {
            #region 读取配置文件
            // 初始化配置文件路径
            string filepath = System.IO.Directory.GetCurrentDirectory() + "\\SystemSet.xml";
            //string filepath = System.IO.Directory.GetParent("\\SystemSet.xml").ToString();
            string rstr = SystemClass.GetSystemSet(filepath);// 调用读取xml配置信息的方法
            if (!string.IsNullOrEmpty(rstr))
            {
                MessageBox.Show(rstr, "运行信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            GetDataSet();
            common.ISService = Internet.PingIpOrDomainName(common.SQLIP);
            #endregion
            ControlPLC.GetSystem();
            if (common.ISService)
            {
                SystemClass.GetPosition();
                ControlPLC.DownloadIC();
                ControlPLC.DownloadDri();
                fvnState();
            }
            else
            {
                txtLoginName.Text = "不能连接数据库，请检查网络";
            }
            txtLoginName.TabIndex = 0;
        }


        /// <summary>
        /// “登录”的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            UserLogin(); // 调用车辆管理登录的方法 
        }
        /// <summary>
        /// 车辆管理系统登录的的方法
        /// </summary>
        /// <returns></returns>
        private bool UserLogin()
        {
            bool rbool = false;
            try
            {
                //if (!Find())
                //{
                //    MessageBox.Show("请插入正确的加密锁！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return false;
                //}
                common.ISService = Internet.PingIpOrDomainName(common.SQLIP);
                //得到用户名、密码、门岗值
                string loginName = txtLoginName.Text.Trim();
                string loginPwd = CommonalityEntity.EncryptDES(txtLoginPwd.Text.Trim());
                if (string.IsNullOrEmpty(txtLoginName.Text.Trim()))
                {
                    MessageBox.Show("用户名不能为空", "提示");
                    txtLoginName.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(txtLoginPwd.Text.Trim()))
                {
                    MessageBox.Show("用户密码不能为空", "提示");
                    txtLoginPwd.Focus();
                    return false;
                }
                if (loginName == "不能连接数据库，请检查网络")
                {
                    MessageBox.Show("请输入用户名", "提示");
                    txtLoginName.Focus();
                    return false;
                }
                CommonalityEntity.USERNAME = loginName;
                if (!common.ISService)
                {
                    if (MessageBox.Show("无网络是否继续登录！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    {
                        return false;
                    }
                    if (mf == null)
                    {
                        mf = new MainForm();
                    }
                    this.Hide();
                    this.ShowInTaskbar = false;
                    mf.Show();
                    rbool = true;
                    txtLoginName.Clear();
                    txtLoginPwd.Clear();
                }
                else
                {
                    string str = "select * from UserInfo where UserLoginId='" + loginName + "' and UserLoginPwd='" + loginPwd + "' and UserSate = '启动'";
                    DataTable dt = LinQBaseDao.Query(str).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string userid = dt.Rows[0]["UserId"].ToString();
                        string Name = dt.Rows[0]["UserName"].ToString();
                        if (!string.IsNullOrEmpty(loginName)) // 登录名
                        {
                            common.USERNAME = loginName;
                            CommonalityEntity.USERNAME = loginName;
                        }
                        if (!string.IsNullOrEmpty(Name)) // 真实员工姓名
                        {
                            common.NAME = Name;
                        }
                        string Log_Content = String.Format("用户名：{0}", txtLoginName.Text.Trim());
                        CommonalityEntity.WriteLogData("用户登录", Log_Content, CommonalityEntity.USERNAME);//添加日志
                        common.USERID = userid;//用户编号
                        common.NAME = Name;//真实姓名
                        common.CurrentGate = SystemClass.PositionName;//当前门岗
                        common.CurrentGate_ID = SystemClass.PositionID;//门岗编
                        CommonalityEntity.USERID = Convert.ToInt32(userid); // 用户编号
                        CommonalityEntity.USERNAME = loginName;//真实姓名
                        CommonalityEntity.LoginTime = CommonalityEntity.GetServersTime();

                        string roid = dt.Rows[0]["USERRoleID"].ToString();
                        if (!string.IsNullOrEmpty(roid))
                        {
                            CommonalityEntity.USERRoleID = Convert.ToInt32(dt.Rows[0]["USERRoleID"].ToString());
                        }
                        CommonalityEntity.PositionLED_ID = SystemClass.PositionLED_ID;//LED显示指定门岗
                        //UserRoleInfo(); // 查出登录用户的权限号
                        if (mf == null)
                        {
                            mf = new MainForm();
                        }
                        this.Hide();
                        this.ShowInTaskbar = false;
                        mf.Show();  // 串口配置 Com串口
                        if (SystemClass.ISLED == "显示")
                        {
                            if (SystemClass.PositionName == "停车场")
                            {
                                LEDTingForm led = new LEDTingForm();
                                led.Show();
                            }
                            else
                            {
                                LEDPreviewForm led = new LEDPreviewForm();
                                led.Show();
                            }
                        }
                        rbool = true;
                        txtLoginName.Clear();
                        txtLoginPwd.Clear();
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码不可用，请重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch(Exception  ex)
            {
                this.Show();
                MessageBox.Show("未能加载系统组件，请注册组件"+ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return rbool;
        }
        int Rtn = 1;//Rtn函数方法调用后接收返回值，如果出现错误则返回值为错误码
        private bool Find()
        {
            string NTCode = SystemClass.NTCode;//获取到加密锁识别码

            Rtn = NT88API.NTFindFirst(NTCode);//查找指定加密锁识别码的加密锁，如果返回值为 0，表示加密锁存在。
            //如果返回值不为0，则可以通过返回值Rtn查看错误代码
            if (Rtn != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 把地感状态置为：0
        /// </summary>
        private void fvnState()
        {
            try
            {
                LinQBaseDao.Query("update DeviceControl set DeviceControl_FanSate=0 where DeviceControl_PositionValue='" + SystemClass.PosistionValue + "'");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("Login.fvnState");
            }
        }
        /// <summary>
        /// “重置”的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.txtLoginName.Text = "";
            this.txtLoginPwd.Text = "";
        }


        #region 键盘事件    Enter
        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UserLogin(); // 调用车辆管理登录的方法             
            }
        }
        private void txtLoginName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UserLogin(); // 调用车辆管理登录的方法             
            }
        }
        private void txtLoginPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UserLogin(); // 调用车辆管理登录的方法             
            }
        }
        private void cbxDoorMound_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UserLogin(); // 调用车辆管理登录的方法             
            }
        }
        #endregion

        private void txtLoginName_Click(object sender, EventArgs e)
        {
            if (txtLoginName.Text.Trim() == "不能连接数据库，请检查网络")
            {
                txtLoginName.Text = "";
            }
        }

        /// <summary>
        /// 初始化数据库连接字符串
        /// </summary>
        private void GetDataSet()
        {
            string oldSqlStr = System.Configuration.ConfigurationManager.ConnectionStrings["EMEWEQCConnectionString"].ToString();
            if (oldSqlStr != "")
            {
                string[] sqlStr = oldSqlStr.Split(';');
                if (sqlStr.Length > 1)
                {
                    foreach (string str1 in sqlStr)
                    {
                        if (str1 != "")
                        {
                            string[] str = str1.Split('=');
                            if (str.Length > 1)
                            {
                                if (str[0] == "Data Source")
                                {
                                    common.SQLIP = str[1].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
