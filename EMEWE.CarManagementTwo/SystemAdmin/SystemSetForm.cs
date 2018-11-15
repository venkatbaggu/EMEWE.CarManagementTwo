using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.DAL;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;
using EMEWE.CarManagementDAL;
using EMEWE.CarManagement.CommonClass;
using System.Diagnostics;
using EMEWE.CarManagement.Commons;
using EMEWE.CarManagement.Commons.CommonClass;
using WindowsFormsApplication3;
using System.Threading;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class SystemSetForm : Form
    {
        public SystemSetForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 父窗体
        /// </summary>
        public MainForm mf;
        public LoginForm login;

        /// <summary>
        /// “系统设置” 的Load加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemSetForm_Load(object sender, EventArgs e)
        {
            mf = new MainForm();

            BindPositionName();

            GetDataSet(); // 调用初始化显示数据连接字符串的方法
            GetSystemSet();  // 调用获得系统设置信息SystemSet.xml
            GetSAPSet();
        }
        #region  门岗的绑定及联动事件
        /// <summary>
        /// 绑定门岗名称
        /// </summary>
        private void BindPositionName()
        {
            try
            {
                string sql = String.Format("select * from Position");
                this.cbxMenGangName.DataSource = PositionDAL.GetViewPosition(sql);

                if (PositionDAL.GetViewPosition(sql).Count() > 0)
                {
                    this.cbxMenGangName.DisplayMember = "Position_Name";
                    this.cbxMenGangName.ValueMember = "Position_ID";
                    cbxMenGangName.SelectedIndex = 0;
                }
            }
            catch
            {
                MessageBox.Show("系统设置“门岗”绑定有误，请查看门岗信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 在ListControl上更改SelectedValueChanged属性的值时引发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMenGangName_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.cbxPositionID.Items.Count > 0) // 门岗编号
            {
                this.cbxPositionID.Items.Clear();
            }
            if (this.cbxPositionValue.Items.Count > 0) // 门岗值
            {
                this.cbxPositionValue.Items.Clear();
            }
            if (this.cbxMenGangName.Text == "") { return; }

            string PositionName = this.cbxMenGangName.Text;
            var name = PositionDAL.GetViewPosition(String.Format("select * from Position where Position_Name='{0}'", PositionName));
            if (name != null)
            {
                foreach (var item in name)
                {
                    if (item.Position_ID != null)
                    {
                        if (item.Position_Value != null)
                        {
                            cbxPositionID.Items.Add(item.Position_ID);
                            cbxPositionID.SelectedIndex = 0;
                            cbxPositionValue.Items.Add(item.Position_Value);
                            cbxPositionValue.SelectedIndex = 0;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 绑定门岗编号
        /// </summary>
        private void BindPositionID()
        {
            try
            {
                string sql = String.Format("select * from Position");
                this.cbxPositionID.DataSource = PositionDAL.GetViewPosition(sql);

                if (PositionDAL.GetViewPosition(sql).Count() > 0)
                {
                    this.cbxPositionID.DisplayMember = "Position_ID";
                    this.cbxPositionID.ValueMember = "Position_ID";
                    cbxPositionID.SelectedIndex = -1;
                }
            }
            catch
            {
                MessageBox.Show("系统设置“门岗编号”绑定有误，请查看门岗信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定门岗值
        /// </summary>
        private void BindPositionValue()
        {
            try
            {
                string sql = String.Format("select * from Position");
                this.cbxPositionValue.DataSource = PositionDAL.GetViewPosition(sql);

                if (PositionDAL.GetViewPosition(sql).Count() > 0)
                {
                    this.cbxPositionValue.DisplayMember = "Position_Value";
                    this.cbxPositionValue.ValueMember = "Position_ID";
                    cbxPositionValue.SelectedIndex = -1;
                }
            }
            catch
            {
                MessageBox.Show("系统设置“门岗值”绑定有误，请查看门岗信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion


        /// <summary>
        /// 获得系统设置信息: 初始化设置   SystemSet.xml
        /// </summary>
        private void GetSystemSet()
        {
            try
            {
                string DVRIP = "";
                string DVRServerPort = "";
                string DVRLoginName = "";
                string DVRPwd = "";
                string DVRIPTwo = "";
                string DVRServerPortTwo = "";
                string DVRLoginNameTwo = "";
                string DVRPwdTwo = "";
                string SaveFiel = "";
                string CARD = "";
                string CARD2 = "";
                string PCL = "";
                string ReadValue = "";
                string PositionName = "";
                string PositionID = "";
                string PosistionValue = "";
                string HuJiaoJianGe = "";
                string SaveFile = "";
                string lChannel = "";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNode xn = xmlDoc.SelectSingleNode("param");//查找<bookstore>
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xnf in xnl)
                    {
                        XmlElement xe = (XmlElement)xnf;
                        //硬盘录像机一
                        DVRIP = xe.GetAttribute("DVRIP").ToString();  // 硬盘录像机的IP
                        DVRServerPort = xe.GetAttribute("DVRServerPort").ToString(); // 硬盘录像机的服务器端口号
                        DVRLoginName = xe.GetAttribute("DVRLoginName").ToString();   // 硬盘录像机的登录名称
                        DVRPwd = xe.GetAttribute("DVRPwd").ToString(); // 硬盘录像机的登录密码
                        //硬盘录像机二
                        DVRIPTwo = xe.GetAttribute("DVRIPTwo").ToString();  // 硬盘录像机的IP
                        DVRServerPortTwo = xe.GetAttribute("DVRServerPortTwo").ToString(); // 硬盘录像机的服务器端口号
                        DVRLoginNameTwo = xe.GetAttribute("DVRLoginNameTwo").ToString();   // 硬盘录像机的登录名称
                        DVRPwdTwo = xe.GetAttribute("DVRPwdTwo").ToString(); // 硬盘录像机的登录密码

                        SaveFiel = xe.GetAttribute("SaveFiel").ToString(); // 获取图片服务器地址
                        CARD = xe.GetAttribute("CARD").ToString(); // 读卡器1串口号
                        CARD2 = xe.GetAttribute("CARD2").ToString(); // 读卡器2串口号
                        PCL = xe.GetAttribute("PCL").ToString(); // PCL串口号
                        ReadValue = xe.GetAttribute("ReadValue").ToString(); // 登记IC卡读卡器通道值
                        PositionName = xe.GetAttribute("MenGangName").ToString();// 门岗名称
                        PositionID = xe.GetAttribute("PositionID").ToString(); // 门岗编号
                        PosistionValue = xe.GetAttribute("PosistionValue").ToString(); // 门岗值
                        HuJiaoJianGe = xe.GetAttribute("HuJiaoJianGe").ToString();
                        SaveFile = xe.GetAttribute("ServerImageLoad").ToString();//图片上传服务器地址
                        lChannel = xe.GetAttribute("lChannel").ToString();//登记拍照地址
                    }
                    //硬盘录像机一
                    if (!string.IsNullOrEmpty(DVRIP))
                    {
                        this.txtDVRIP.Text = DVRIP;
                        common.DVRIP = DVRIP;
                        SystemClass.DVRIPAddress = DVRIP;
                    }
                    if (!string.IsNullOrEmpty(DVRServerPort))
                    {
                        this.txtDVRServerPort.Text = DVRServerPort;
                        common.DVRServerPort = DVRServerPort;
                        SystemClass.DVRPortNumber = Common.Converter.ToShort(DVRServerPort);
                    }
                    if (!string.IsNullOrEmpty(DVRLoginName))
                    {
                        this.txtDVRLoginName.Text = DVRLoginName;
                        common.DVRLoginName = DVRLoginName;
                        SystemClass.DVRUserName = DVRLoginName;
                    }
                    if (!string.IsNullOrEmpty(DVRPwd))
                    {
                        this.txtDVRPwd.Text = DVRPwd;
                        common.DVRPwd = DVRPwd;
                        SystemClass.DVRPassword = DVRPwd;
                    }
                    //硬盘录像机二
                    if (!string.IsNullOrEmpty(DVRIPTwo))
                    {
                        this.txtDVRIPTwo.Text = DVRIPTwo;
                        common.DVRIPTwo = DVRIPTwo;
                        SystemClass.DVRIPAddressTwo = DVRIPTwo;
                    }
                    if (!string.IsNullOrEmpty(DVRServerPortTwo))
                    {
                        this.txtDVRServerPortTwo.Text = DVRServerPortTwo;
                        common.DVRServerPortTwo = DVRServerPortTwo;
                        SystemClass.DVRPortNumberTwo = Common.Converter.ToShort(DVRServerPortTwo);
                    }
                    if (!string.IsNullOrEmpty(DVRLoginNameTwo))
                    {
                        this.txtDVRLoginNameTwo.Text = DVRLoginNameTwo;
                        common.DVRLoginNameTwo = DVRLoginNameTwo;
                        SystemClass.DVRUserNameTwo = DVRLoginNameTwo;
                    }
                    if (!string.IsNullOrEmpty(DVRPwdTwo))
                    {
                        this.txtDVRPwdTwo.Text = DVRPwdTwo;
                        common.DVRPwdTwo = DVRPwdTwo;
                        SystemClass.DVRPasswordTwo = DVRPwdTwo;
                    }

                    if (!string.IsNullOrEmpty(SaveFiel))
                    {
                        this.txtSaveFiel.Text = SaveFiel;
                        common.ImageServerPath = SaveFiel;
                        SystemClass.SaveFile = SaveFiel;
                    }
                    if (!string.IsNullOrEmpty(CARD))
                    {
                        this.txtCardReadeOne.Text = CARD;
                        common.CARD = CARD;
                        SystemClass.CardReadComOne = int.Parse(CARD);
                    }
                    if (!string.IsNullOrEmpty(CARD2))
                    {
                        this.txtCardReadeTwo.Text = CARD2;
                        common.CARD2 = CARD2;
                        SystemClass.CardReadComTwo = int.Parse(CARD);
                    }
                    if (!string.IsNullOrEmpty(PCL))
                    {
                        this.txtPCLNumber.Text = PCL;
                        common.PCL = PCL;
                        SystemClass.PLCCom = int.Parse(PCL);
                    }
                    if (!string.IsNullOrEmpty(ReadValue))
                    {
                        this.txtReadValue.Text = ReadValue;
                        SystemClass.ReadValue = PosistionValue;
                    }
                    if (!string.IsNullOrEmpty(PositionName))
                    {
                        this.cbxMenGangName.Text = PositionName;
                        common.MenGangName = PositionName;
                        SystemClass.PositionName = PositionName;
                    }
                    if (!string.IsNullOrEmpty(PositionID))
                    {
                        this.cbxPositionID.Text = PositionID;
                        SystemClass.PositionID = int.Parse(PositionID);
                    }
                    if (!string.IsNullOrEmpty(PosistionValue))
                    {
                        this.cbxPositionValue.Text = PosistionValue;
                        SystemClass.PosistionValue = PosistionValue;
                    }
                    if (!string.IsNullOrEmpty(HuJiaoJianGe))
                    {
                        this.txtHuJiaoJianGe.Text = HuJiaoJianGe;
                        SystemClass.HuJiaoJianGe = int.Parse(HuJiaoJianGe);
                    }
                    if (!string.IsNullOrEmpty(SaveFile))
                    {
                        this.txtSaveFiel.Text = SaveFile;
                        SystemClass.SaveFile = SaveFile;
                    }
                    if (!string.IsNullOrEmpty(lChannel))
                    {
                        this.txtlChannel.Text = lChannel;
                        SystemClass.lChannel = Convert.ToInt32(lChannel);
                    }

                }
            }
            catch 
            {
                MessageBox.Show("获得系统设置失败!", "运行信息", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
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
                                    this.txtDataSource.Text = str[1].ToString();
                                }
                                else if (str[0] == "Initial Catalog")
                                {
                                    this.txtDatabase.Text = str[1].ToString();
                                }
                                else if (str[0] == "User ID")
                                {
                                    this.txtUserName.Text = str[1].ToString();
                                }
                                else if (str[0] == "Password")
                                {
                                    this.txtPwd.Text = str[1].ToString();
                                }

                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 初始化SAP设置
        /// </summary>
        private void GetSAPSet()
        {
            try
            {
                string System = "";
                string MessageServer = "";
                string GroupName = "";
                string Client = "";
                string Language = "";
                string SAPUser = "";
                string SAPPassword = "";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNode xn = xmlDoc.SelectSingleNode("param");//查找<bookstore>
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xnf in xnl)
                    {
                        XmlElement xe = (XmlElement)xnf;
                        System = xe.GetAttribute("System").ToString();
                        MessageServer = xe.GetAttribute("MessageServer").ToString();
                        GroupName = xe.GetAttribute("GroupName").ToString();
                        Client = xe.GetAttribute("Client").ToString();
                        Language = xe.GetAttribute("Language").ToString();
                        SAPUser = xe.GetAttribute("User").ToString();
                        SAPPassword = xe.GetAttribute("Password").ToString();


                    }
                    this.txtSystem.Text = System;
                    this.txtMessageServer.Text = MessageServer;
                    this.txtGroupName.Text = GroupName;
                    this.txtClient.Text = Client;
                    this.txtLanguage.Text = Language;
                    this.txtSAPUser.Text = SAPUser;
                    this.txtSAPPassword.Text = SAPPassword;
                    Class1.strSystem = System;
                    Class1.strMessageServer = MessageServer;
                    Class1.strGroupName = GroupName;
                    Class1.strClient = Client;
                    Class1.strLanguage = Language;
                    Class1.strUser = SAPUser;
                    Class1.strPassword = SAPPassword;

                }
            }
            catch 
            {
                MessageBox.Show("获得系统设置失败!", "运行信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// “数据源配置” 按钮的“保 存”单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveDB_Click(object sender, EventArgs e)
        {
            if (this.txtDataSource.Text == "")
            {
                MessageBox.Show("数据源不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtDatabase.Text == "")
            {
                MessageBox.Show("数据库不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtUserName.Text == "")
            {
                MessageBox.Show("数据库用户名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtPwd.Text == "")
            {
                MessageBox.Show("数据库密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string filepath = Application.ExecutablePath + ".config";
                string conString = "Data Source=" + this.txtDataSource.Text.Trim() + ";User ID=" + this.txtUserName.Text.Trim() + ";Password=" + this.txtPwd.Text.Trim() + ";Initial Catalog=" + this.txtDatabase.Text.Trim();
                string autoDataBaseString = "Data Source=" + this.txtDataSource.Text.Trim() + ";User ID=" + this.txtUserName.Text.Trim() + ";Password=" + this.txtPwd.Text.Trim() + ";Initial Catalog=master";
                bool boolCon = LinQBaseDao.DetectionConn(conString);
                if (!boolCon)
                {
                    MessageBox.Show("此数据源配置无法连接，重新输入正确的数据源配置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                XmlNodeList node = doc.SelectSingleNode("configuration").ChildNodes;
                if (node.Count > 0)
                {
                    // XmlNodeList dsf = doc.SelectSingleNode("connectionStrings").ChildNodes;
                    XmlElement ele = (XmlElement)node[1];
                    XmlNodeList nodeList = ele.ChildNodes;
                    if (nodeList.Count > 0)
                    {
                        XmlElement ele2 = (XmlElement)node[1];

                        XmlNodeList xnl = ele2.ChildNodes;
                        if (xnl.Count > 0)
                        {
                            XmlElement xe = (XmlElement)xnl[0];
                            xe.SetAttribute("connectionString", conString);
                            doc.Save(filepath);

                            btWriteReg(conString); // 调用写入连接数据源到 XML 的方法
                            if (MessageBox.Show("数据源修改成功！是否退出系统重新启动程序？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                            {
                                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                                Application.Restart();
                            }
                            CommonalityEntity.WriteLogData("修改", "数据源配置", common.NAME); //操作日志
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("修改数据配置失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 写入 连接数据源的方法
        /// </summary>
        /// <param name="conString"></param>
        private void btWriteReg(string conString)
        {
            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\EMEWE.CarManagement.exe.config";
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            XmlNodeList node = doc.SelectSingleNode("configuration").ChildNodes;
            if (node.Count > 0)
            {
                XmlElement ele = (XmlElement)node[1];
                XmlNodeList nodeList = ele.ChildNodes;
                if (nodeList.Count > 0)
                {
                    XmlElement ele2 = (XmlElement)nodeList[1];
                    XmlNodeList xnl = ele2.ChildNodes;
                    if (xnl.Count > 0)
                    {
                        XmlElement xe = (XmlElement)xnl[0];
                        xe.SetAttribute("connectionString", conString);
                        doc.Save(filepath);
                    }
                }
            }
        }
        /// <summary>
        /// “数据源配置” 按钮的“清 除”单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtDataSource.Text = "";
            this.txtDatabase.Text = "";
            this.txtUserName.Text = "";
            this.txtPwd.Text = "";
        }

        /// <summary>
        /// 硬盘录像机 --> 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextDVR_Click(object sender, EventArgs e)
        {
            this.tabControl2.SelectedTab = tabPage4;
        }
        /// <summary>
        /// 串口 --> 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNextPLC_Click(object sender, EventArgs e)
        {
            this.tabControl2.SelectedTab = tabPage9;
        }
        /// <summary>
        /// 上一步 --> DVR 硬盘录像机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreDVR_Click(object sender, EventArgs e)
        {
            this.tabControl2.SelectedTab = tabPage3;
        }
        /// <summary>
        /// 上一步 --> PLC 串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrePLC_Click(object sender, EventArgs e)
        {
            this.tabControl2.SelectedTab = tabPage9;
        }

        /// <summary>
        /// 门岗 --> “保 存” 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            StrIsNullOrEmpty(); // 调用判断系统初始化设置的各个文本框值是否为空的方法
            Save();     // 调用保存系统初始化设置的方法
            common.WriteLogData("修改", "初始化设置", common.USERNAME); //操作日志
            try
            {
                if (MessageBox.Show("是否退出系统重新读取配置信息？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    Application.Restart();
                }
                CommonalityEntity.WriteLogData("修改", "初始化设置", common.NAME); //操作日志
            }
            catch 
            {
                Process.GetCurrentProcess().Kill();
            }
        }
        string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\SystemSet.xml";

        /// <summary>
        /// 保存系统初始化设置的方法
        /// </summary>
        private void Save()
        {
            try
            {
                string DVRIP = this.txtDVRIP.Text.Trim();
                string DVRServerPort = this.txtDVRServerPort.Text.Trim();
                string DVRLoginName = this.txtDVRLoginName.Text.Trim();
                string DVRPwd = this.txtDVRPwd.Text.Trim();
                string SaveFiel = this.txtSaveFiel.Text.Trim();

                string DVRIPTwo = this.txtDVRIPTwo.Text.Trim();
                string DVRServerPortTwo = this.txtDVRServerPortTwo.Text.Trim();
                string DVRLoginNameTwo = this.txtDVRLoginNameTwo.Text.Trim();
                string DVRPwdTwo = this.txtDVRPwdTwo.Text.Trim();

                string PCL = this.txtPCLNumber.Text.Trim();
                string CARD = this.txtCardReadeOne.Text.Trim();
                string CARD2 = this.txtCardReadeTwo.Text.Trim();

                string ReadValue = this.txtReadValue.Text.Trim();

                string PositionName = this.cbxMenGangName.Text.Trim();
                string PositionID = this.cbxPositionID.Text.Trim();
                string PosistionValue = this.cbxPositionValue.Text.Trim();
                string HuJiaoJianGe = this.txtHuJiaoJianGe.Text.Trim();
                string SaveFile = this.txtSaveFiel.Text.Trim();
                string lChannel = this.txtlChannel.Text.Trim();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNode xn = xmlDoc.SelectSingleNode("param");//查找<bookstore>
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xnf in xnl)
                    {
                        XmlElement xe = (XmlElement)xnf;
                        //硬盘录像机一
                        xe.SetAttribute("DVRIP", DVRIP);  // 硬盘录像机的IP
                        xe.SetAttribute("DVRServerPort", DVRServerPort); // 硬盘录像机的服务器端口号
                        xe.SetAttribute("DVRLoginName", DVRLoginName);   // 硬盘录像机的登录名称
                        xe.SetAttribute("DVRPwd", DVRPwd); // 硬盘录像机的登录密码
                        //硬盘录像机二
                        xe.SetAttribute("DVRIPTwo", DVRIPTwo);  // 硬盘录像机的IP
                        xe.SetAttribute("DVRServerPortTwo", DVRServerPortTwo); // 硬盘录像机的服务器端口号
                        xe.SetAttribute("DVRLoginNameTwo", DVRLoginNameTwo);   // 硬盘录像机的登录名称
                        xe.SetAttribute("DVRPwdTwo", DVRPwdTwo); // 硬盘录像机的登录密码

                        xe.SetAttribute("ServerImageLoad", SaveFiel); //获取图片服务器地址
                        xe.SetAttribute("CARD", CARD); // 读卡器1串口号
                        xe.SetAttribute("CARD2", CARD2); // 读卡器2串口号
                        xe.SetAttribute("PCL", PCL); // PCL串口号
                        xe.SetAttribute("ReadValue", ReadValue); //登记IC卡读卡器通道值
                        xe.SetAttribute("MenGangName", PositionName); // 门岗名称
                        xe.SetAttribute("PositionID", PositionID); // 门岗编号
                        xe.SetAttribute("PosistionValue", PosistionValue); // 门岗值
                        xe.SetAttribute("HuJiaoJianGe", HuJiaoJianGe);
                        xe.SetAttribute("ServerImageLoad", SaveFile);//图片上传服务器地址
                        xe.SetAttribute("lChannel", lChannel);//登记拍照地址
                    }
                    xmlDoc.Save(filepath);

                    common.DVRIP = DVRIP;
                    common.DVRServerPort = DVRServerPort;
                    common.DVRLoginName = DVRLoginName;
                    common.DVRPwd = DVRPwd;

                    common.DVRIPTwo = DVRIPTwo;
                    common.DVRServerPortTwo = DVRServerPortTwo;
                    common.DVRLoginNameTwo = DVRLoginNameTwo;
                    common.DVRPwdTwo = DVRPwdTwo;
                    // common.ImageServerPath = SaveFiel;
                    common.PCL = PCL;
                    common.CARD = CARD;
                    common.CARD2 = CARD2;
                    common.MenGangName = PositionName;

                    SystemClass.DVRIPAddress = DVRIP; // 硬盘录像机IP地址
                    SystemClass.DVRPortNumber = Convert.ToInt16(DVRServerPort); // 硬盘录像机服务端口号
                    SystemClass.DVRUserName = DVRLoginName; // 硬盘录像机用户名
                    SystemClass.DVRPassword = DVRPwd; // 硬盘录像机密码

                    SystemClass.DVRIPAddressTwo = DVRIPTwo; // 硬盘录像机IP地址
                    SystemClass.DVRPortNumberTwo = Convert.ToInt16(DVRServerPortTwo); // 硬盘录像机服务端口号
                    SystemClass.DVRUserNameTwo = DVRLoginNameTwo; // 硬盘录像机用户名
                    SystemClass.DVRPasswordTwo = DVRPwdTwo; // 硬盘录像机密码

                    SystemClass.SaveFile = SaveFiel; //获取图片服务器地址
                    SystemClass.CardReadComOne = int.Parse(CARD);// 读卡器1串口号
                    SystemClass.CardReadComTwo = int.Parse(CARD2);// 读卡器2串口号
                    SystemClass.PLCCom = int.Parse(PCL);// PLC串口号
                    SystemClass.ReadValue = ReadValue; //登记IC卡读卡器通道值
                    SystemClass.PositionName = PositionName; // 门岗名称
                    SystemClass.PositionID = int.Parse(PositionID); // 门岗编号
                    SystemClass.PosistionValue = PosistionValue; // 门岗值;
                    SystemClass.HuJiaoJianGe = int.Parse(HuJiaoJianGe);
                    SystemClass.SaveFile = SaveFile;
                    SystemClass.lChannel = Convert.ToInt32(lChannel);
                    MessageBox.Show("系统初始化设置配置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonalityEntity.WriteLogData("修改", "修改系统初始化设置", CommonalityEntity.USERNAME);
                }
            }
            catch 
            {
                MessageBox.Show("系统初始化设置配置失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommonalityEntity.WriteTextLog(" SystemSetForm.Save()异常：" + "");
            }
        }
        /// <summary>
        /// 判断系统初始化设置的各个文本框值是否为空的方法
        /// </summary>
        private void StrIsNullOrEmpty()
        {
            if (this.txtDVRIP.Text == "")
            {
                MessageBox.Show("硬盘录像机的IP不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtDVRServerPort.Text == "")
            {
                MessageBox.Show("硬盘录像机的服务器端口号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtDVRLoginName.Text == "")
            {
                MessageBox.Show("硬盘录像机的登录名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtDVRPwd.Text == "")
            {
                MessageBox.Show("硬盘录像机的登录密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtCardReadeOne.Text == "")
            {
                MessageBox.Show("读卡器1串口号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtCardReadeTwo.Text == "")
            {
                MessageBox.Show("读卡器2串口号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtPCLNumber.Text == "")
            {
                MessageBox.Show("PCL串口号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.cbxMenGangName.Text == "")
            {
                MessageBox.Show("门岗名称不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.cbxPositionID.Text == "")
            {
                MessageBox.Show("门岗编号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.cbxPositionValue.Text == "")
            {
                MessageBox.Show("门岗值不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtHuJiaoJianGe.Text == "")
            {
                MessageBox.Show("呼叫间隔不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.tabControl2.SelectedTab = tabPage5;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            this.tabControl2.SelectedTab = tabPage4;
        }


        /// <summary>
        /// SAP连接地址设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSapSet_Click(object sender, EventArgs e)
        {
            try
            {
                string str = ISNUllorEmpty();
                if (!string.IsNullOrEmpty(str))
                {
                    MessageBox.Show(this, str);
                    return;
                }
                SAPSave();
                MessageBox.Show("SAP设置配置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch 
            {
                CommonalityEntity.WriteTextLog(" SystemSetForm.btnSapSet_Click()异常：" + "");
            }

        }

        private void SAPSave()
        {
            string System = txtSystem.Text.Trim();
            string MessageServer = txtMessageServer.Text.Trim();
            string GroupName = txtGroupName.Text.Trim();
            string Client = txtClient.Text.Trim();
            string Language = txtLanguage.Text.Trim();
            string SAPUser = txtSAPUser.Text.Trim();
            string SAPPassword = txtSAPPassword.Text.Trim();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNode xn = xmlDoc.SelectSingleNode("param");//查找<bookstore>
            XmlNodeList xnl = xn.ChildNodes;
            if (xnl.Count > 0)
            {
                foreach (XmlNode xnf in xnl)
                {
                    XmlElement xe = (XmlElement)xnf;
                    xe.SetAttribute("System", System);
                    xe.SetAttribute("MessageServer", MessageServer);
                    xe.SetAttribute("GroupName", GroupName);
                    xe.SetAttribute("Client", Client); // 
                    xe.SetAttribute("Language", Language);
                    xe.SetAttribute("User", SAPUser);
                    xe.SetAttribute("Password", SAPPassword);
                }
                xmlDoc.Save(filepath);

                Class1.strSystem = System;
                Class1.strMessageServer = MessageServer;
                Class1.strGroupName = GroupName;
                Class1.strClient = Client;
                Class1.strLanguage = Language;
                Class1.strUser = SAPUser;
                Class1.strPassword = SAPPassword;
                CommonalityEntity.WriteLogData("修改", "修改SAP连接地址", CommonalityEntity.USERNAME);
            }
        }


        private void btnSapEmpty_Click(object sender, EventArgs e)
        {
            txtSystem.Text = "";
            txtMessageServer.Text = "";
            txtGroupName.Text = "";
            txtClient.Text = "";
            txtLanguage.Text = "";
            txtSAPUser.Text = "";
            txtSAPPassword.Text = "";
        }

        private string ISNUllorEmpty()
        {
            string str = "";
            if (string.IsNullOrEmpty(txtSystem.Text.Trim()))
            {
                return str = "System不能为空！";
            }
            if (string.IsNullOrEmpty(txtMessageServer.Text.Trim()))
            {
                return str = "MessageServer不能为空！";
            }
            if (string.IsNullOrEmpty(txtGroupName.Text.Trim()))
            {
                return str = "GroupName不能为空！";
            }
            if (string.IsNullOrEmpty(txtClient.Text))
            {
                return str = "Client不能为空！";
            }
            if (string.IsNullOrEmpty(txtLanguage.Text.Trim()))
            {
                return str = "txtLanguage不能为空！";
            }
            if (string.IsNullOrEmpty(txtSAPUser.Text.Trim()))
            {
                return str = "User不能为空！";
            }
            if (string.IsNullOrEmpty(txtSAPPassword.Text.Trim()))
            {
                return str = "Password不能为空！";
            }
            return str;
        }
    }
}
