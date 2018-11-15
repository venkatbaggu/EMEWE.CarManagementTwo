using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GemBox.ExcelLite;
using EMEWE.CarManagement.DAL;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Diagnostics;
using System.Xml;
using System.Reflection;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class SystemConfig : Form
    {
        public SystemConfig()
        {
            InitializeComponent();
        }
        public string saveFileTitle = "";
        private void SystemConfig_Load(object sender, EventArgs e)
        {
            userContext();
        }
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnExport.Enabled = true;
                btnExport.Visible = true;
                btnImport.Enabled = true;
                btnImport.Visible = true;
            }
            else
            {
                btnExport.Visible = ControlAttributes.BoolControl("btnExport", "SystemConfig", "Visible");
                btnExport.Enabled = ControlAttributes.BoolControl("btnExport", "SystemConfig", "Enabled");

                btnImport.Visible = ControlAttributes.BoolControl("btnImport", "SystemConfig", "Visible");
                btnImport.Enabled = ControlAttributes.BoolControl("btnImport", "SystemConfig", "Enabled");

            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            string path = "";
            FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    saveFileDialog.SelectedPath = saveFileDialog.SelectedPath.Replace("\\备份数据", "");

                    path = saveFileDialog.SelectedPath + "\\备份数据\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMddHHmm");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                        path = saveFileDialog.SelectedPath + "\\备份数据\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMddHHmm") + 1;

                    if (!Directory.Exists(path + "\\CarPortSet.xml"))
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        //加入XML的声明段落
                        xmldoc.AppendChild(xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null));
                        //加入根元素
                        XmlElement xmlelem = xmldoc.CreateElement("", "config", "");
                        xmldoc.AppendChild(xmlelem);
                        //配置信息列表
                        XmlElement xmlelemFileName = xmldoc.CreateElement("ConfigLists");
                        XmlText xmltextFileName = xmldoc.CreateTextNode("配置信息列表");
                        xmlelemFileName.AppendChild(xmltextFileName);
                        xmldoc.ChildNodes.Item(1).AppendChild(xmlelemFileName);

                        try
                        {
                            xmldoc.Save(path + "\\CarPortSet.xml");
                            saveFileTitle = path + "\\CarPortSet.xml";
                        }
                        catch 
                        {
                           
                        }
                    }

                    CopConfig(path);

                    MessageBox.Show("导出成功！", "系统提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //打开导出文件夹目录
                    System.Diagnostics.Process.Start("explorer.exe", path);

                    CommonalityEntity.WriteLogData("系统配置", "配置信息导出成功！", CommonalityEntity.USERNAME);//添加日志
                }
                catch 
                {
                    MessageBox.Show("导出失败！", "系统提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonalityEntity.WriteLogData("系统配置", "配置信息导出失败！" , CommonalityEntity.USERNAME);//添加日志
                    DirectoryInfo dir = new DirectoryInfo(path);
                    if (dir.Exists)
                    {
                        DirectoryInfo[] childs = dir.GetDirectories();
                        foreach (DirectoryInfo child in childs)
                        {
                            child.Delete(true);
                        }
                        dir.Delete(true);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            bool sumbit = false;
            string path = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "xml配置文件(*.xml)|*.xml";
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = true;
            try
            {
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    path = openFileDialog.FileName.Replace("CarPortSet.xml", "");
                    if (openFileDialog.SafeFileName.ToString().Trim().ToLower() != "carportset.xml")
                    {
                        MessageBox.Show(openFileDialog.SafeFileName.ToString() + "不是有效的文件，请选择CarPortSet.xml文件", "系统提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    openFileDialog.FileName = openFileDialog.FileName.Replace("\\CarPortSet.xml", "");
                    ///读取xml文件中节点ConfigurationName中的数据
                    #region  系统配置信息
                    //系统配置
                    if (chkOther.Checked)
                    {
                        if (tableNames("SystemSet.xml", path))
                        {
                            string fileSystemSetpath = Application.StartupPath + "\\SystemSet.xml";
                            System.IO.File.Copy(path + "\\SystemSet.xml", fileSystemSetpath, true);
                        }
                    }

                    ///菜单信息
                    if (chkMenu.Checked)
                    {
                        if (tableNames("菜单信息.xls", path))
                        {
                            setExcelout("菜单信息.xls", "MenuInfo", path);
                        }
                    }

                    ///字典信息
                    if (chkDictionary.Checked)
                    {
                        if (tableNames("字典信息.xls", path))
                        {
                            setExcelout("字典信息.xls", "Dictionary", path);
                        }
                    }

                    if (chkSystem.Checked)
                    {
                        if (tableNames("EMEWE.CarManagement.exe.config", path))
                        {
                            string fileSystemSetpath = Application.StartupPath + "\\EMEWE.CarManagement.exe.config";
                            System.IO.File.Copy(path + "\\EMEWE.CarManagement.exe.config", fileSystemSetpath, true);
                        }

                        if (tableNames("EMEWE.CarManagement.vshost.exe.Config", path))
                        {
                            string fileSystemSetpath = Application.StartupPath + "\\EMEWE.CarManagement.vshost.exe.Config";
                            System.IO.File.Copy(path + "\\EMEWE.CarManagement.vshost.exe.Config", fileSystemSetpath, true);
                        }
                    }
                    #endregion

                    #region 基础配置信息

                    ///门岗信息
                    if (checkBoxPositionVoice.Checked)
                    {
                        if (tableNames("门岗信息.xls", path))
                        {
                            setExcelout("门岗信息.xls", "Position", path);
                        }
                    }
                    ///通道信息
                    if (checkBoxDriveway.Checked)
                    {
                        if (tableNames("通道信息.xls", path))
                        {
                            setExcelout("通道信息.xls", "Driveway", path);
                        }
                    }
                    ///摄像头信息
                    if (checkbixCamera.Checked)
                    {
                        if (tableNames("摄像头信息.xls", path))
                        {
                            setExcelout("摄像头信息.xls", "Camera", path);
                        }
                    }
                    ///地感信息
                    if (checkBoxFVNInfo.Checked)
                    {
                        if (tableNames("地感信息.xls", path))
                        {
                            setExcelout("地感信息.xls", "FVNInfo", path);
                        }
                    }
                    ///LED配置
                    if (chkLed.Checked)
                    {
                        if (tableNames("LED配置.xls", path))
                        {
                            setExcelout("LED配置.xls", "PositionLED", path);
                        }
                    }
                    ///呼叫配置
                    if (chkVoice.Checked)
                    {
                        if (tableNames("呼叫配置.xls", path))
                        {
                            setExcelout("呼叫配置.xls", "PositionVoice", path);
                        }
                    }
                    ///短信配置
                    if (chkSMS.Checked)
                    {
                        if (tableNames("短信配置.xls", path))
                        {
                            setExcelout("短信配置.xls", "PositionSMS", path);
                        }
                    }
                    ///打印设置
                    if (chkPrintInfo.Checked)
                    {
                        if (tableNames("打印设置.xls", path))
                        {
                            setExcelout("打印设置.xls", "PrintInfo", path);
                        }
                    }
                    #endregion

                    #region 用户配置信息
                    ///角色信息
                    if (chkroleInfo.Checked)
                    {
                        if (tableNames("角色信息.xls", path))
                        {
                            setExcelout("角色信息.xls", "RoleInfo", path);
                        }
                    }
                    ///用户信息
                    if (chkUserInfo.Checked)
                    {
                        if (tableNames("用户信息.xls", path))
                        {
                            setExcelout("用户信息.xls", "UserInfo", path);
                        }
                    }
                    ///用户权限
                    if (chkUserRolePer.Checked)
                    {
                        if (tableNames("用户权限.xls", path))
                        {
                            setExcelout("用户权限.xls", "PermissionsInfo", path);
                        }
                    }
                    #endregion

                    #region 车辆配置信息
                    ///车辆属性
                    if (chkCarAttribute.Checked)
                    {
                        if (tableNames("车辆属性.xls", path))
                        {
                            setExcelout("车辆属性.xls", "CarAttribute", path);
                        }
                    }
                    ///车辆类型
                    if (chkCartype.Checked)
                    {
                        if (tableNames("车辆类型.xls", path))
                        {
                            setExcelout("车辆类型.xls", "CarType", path);
                        }
                    }
                    ///车辆业务类型
                    if (chkBusType.Checked)
                    {
                        if (tableNames("车辆业务类型.xls", path))
                        {
                            setExcelout("车辆业务类型.xls", "BusinessType", path);
                        }
                    }
                    ///通行策略
                    if (chkDrivewayStrategy.Checked)
                    {
                        if (tableNames("通行策略.xls", path))
                        {
                            setExcelout("通行策略.xls", "DrivewayStrategy", path);
                        }
                    }
                    ///管控策略
                    if (chkManagementStrategy.Checked)
                    {
                        if (tableNames("管控策略.xls", path))
                        {
                            setExcelout("管控策略.xls", "ManagementStrategy", path);
                        }
                    }
                    ///管控信息
                    if (chkControlInfo.Checked)
                    {
                        if (tableNames("管控信息.xls", path))
                        {
                            setExcelout("管控信息.xls", "ControlInfo", path);
                        }
                    }
                    ///地磅基础信息
                    if (chkWeighInfo.Checked)
                    {
                        if (tableNames("地磅基础信息.xls", path))
                        {
                            setExcelout("地磅基础信息.xls", "WeighInfo", path);
                        }
                    }
                    ///地磅策略
                    if (chkWeighStrategy.Checked)
                    {
                        if (tableNames("地磅策略.xls", path))
                        {
                            setExcelout("地磅策略.xls", "WeighStrategy", path);
                        }
                    }
                    #endregion

                    #region IC卡配置信息
                    ///IC卡类型
                    if (chkICtype.Checked)
                    {
                        if (tableNames("IC卡类型.xls", path))
                        {
                            setExcelout("IC卡类型.xls", "ICCardType", path);
                        }
                    }
                    ///IC卡信息
                    if (chkICInfo.Checked)
                    {
                        if (tableNames("IC卡信息.xls", path))
                        {
                            setExcelout("IC卡信息.xls", "ICCard", path);
                        }
                    }
                    #endregion

                }
                CommonalityEntity.WriteLogData("系统配置", "配置信息导入成功！", CommonalityEntity.USERNAME);//添加日志
                MessageBox.Show(this, "导入成功，请退出系统重新启动");
                //if (MessageBox.Show(this, "配置信息已更改，是否重启！?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                //{
                //    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //    GC.Collect();
                //    Application.ExitThread();
                //    Application.Exit();
                //    Process.GetCurrentProcess().Kill();
                //    System.Environment.Exit(System.Environment.ExitCode);
                //    Application.ExitThread();
                //}
            }
            catch 
            {
                MessageBox.Show("导入失败！", "系统提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommonalityEntity.WriteLogData("系统配置", "配置信息导入失败！", CommonalityEntity.USERNAME);//添加日志
            }
        }
        /// <summary>
        /// 导出配置文件
        /// </summary>
        /// <param name="path">路径</param>
        private void CopConfig(string path)
        {
            #region 系统配置信息
            //系统配置
            if (chkOther.Checked)
            {
                string fileSystemSetpath = Application.StartupPath + "\\SystemSet.xml";
                System.IO.File.Copy(fileSystemSetpath, path + "\\SystemSet.xml", true);
                SaveXml("SystemSet", "SystemSet.xml");
            }



            //菜单信息
            if (chkMenu.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("MenuInfo");
                DataTable table = LinQBaseDao.Query("select * from MenuInfo").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "菜单信息.xls");
                SaveXml("MenuInfo", "菜单信息.xls");
            }

            //字典信息
            if (chkDictionary.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("Dictionary");
                DataTable table = LinQBaseDao.Query("select * from Dictionary").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "字典信息.xls");
                SaveXml("Dictionary", "字典信息.xls");
            }
            if (chkSystem.Checked)
            {
                string fileSystemSetpath = Application.StartupPath + "\\EMEWE.CarManagement.exe.config";
                System.IO.File.Copy(fileSystemSetpath, path + "\\EMEWE.CarManagement.exe.config", true);
                SaveXml("CarManagement", "EMEWE.CarManagement.exe.config");

                string fileSystemSetpath1 = Application.StartupPath + "\\EMEWE.CarManagement.vshost.exe.Config";
                System.IO.File.Copy(fileSystemSetpath1, path + "\\EMEWE.CarManagement.vshost.exe.Config", true);
                SaveXml("CarManagementvshost", "EMEWE.CarManagement.vshost.exe.Config");
            }


            #endregion

            #region 基础配置信息
            //门岗信息
            if (checkBoxPositionVoice.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("Position");
                DataTable table = LinQBaseDao.Query("select * from Position").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "门岗信息.xls");
                SaveXml("Position", "门岗信息.xls");
            }

            ///通道信息
            if (checkBoxDriveway.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("Driveway");
                DataTable table = LinQBaseDao.Query("select * from Driveway").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "通道信息.xls");
                SaveXml("Driveway", "通道信息.xls");
            }

            //摄像头信息
            if (checkbixCamera.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("Camera");
                DataTable table = LinQBaseDao.Query("select * from Camera").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "摄像头信息.xls");
                SaveXml("Camera", "摄像头信息.xls");
            }

            ///地感信息
            if (checkBoxFVNInfo.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("FVNInfo");
                DataTable table = LinQBaseDao.Query("select * from FVNInfo").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "地感信息.xls");
                SaveXml("FVNInfo", "地感信息.xls");
            }

            //LED配置
            if (chkLed.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("PositionLED");
                DataTable table = LinQBaseDao.Query("select * from PositionLED").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "LED配置.xls");
                SaveXml("PositionLED", "LED配置.xls");
            }

            //呼叫配置
            if (chkVoice.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("PositionVoice");
                DataTable table = LinQBaseDao.Query("select * from PositionVoice").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "呼叫配置.xls");
                SaveXml("PositionVoice", "呼叫配置.xls");
            }

            //短信配置
            if (chkSMS.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("PositionSMS");
                DataTable table = LinQBaseDao.Query("select * from PositionSMS").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "短信配置.xls");
                SaveXml("PositionSMS", "短信配置.xls");
            }

            //打印设置
            if (chkPrintInfo.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("PrintInfo");
                DataTable table = LinQBaseDao.Query("select * from PrintInfo").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "打印设置.xls");
                SaveXml("PrintInfo", "打印设置.xls");
            }
            #endregion

            #region 用户配置信息
            //角色信息
            if (chkroleInfo.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("RoleInfo");
                DataTable table = LinQBaseDao.Query("select * from RoleInfo").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "角色信息.xls");
                SaveXml("RoleInfo", "角色信息.xls");
            }

            //用户信息
            if (chkUserInfo.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("UserInfo");
                DataTable table = LinQBaseDao.Query("select * from UserInfo").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "用户信息.xls");
                SaveXml("UserInfo", "用户信息.xls");
            }

            //用户权限
            if (chkUserRolePer.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("PermissionsInfo");
                DataTable table = LinQBaseDao.Query("select * from PermissionsInfo").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "用户权限.xls");
                SaveXml("PermissionsInfo", "用户权限.xls");
            }
            #endregion

            #region  车辆配置信息
            //车辆属性
            if (chkCarAttribute.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("CarAttribute");
                DataTable table = LinQBaseDao.Query("select * from CarAttribute").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "车辆属性.xls");
                SaveXml("CarAttribute", "车辆属性.xls");
            }

            //车辆类型
            if (chkCartype.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("CarType");
                DataTable table = LinQBaseDao.Query("select * from CarType").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "车辆类型.xls");
                SaveXml("CarType", "车辆类型.xls");
            }

            //车辆业务类型
            if (chkBusType.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("BusinessType");
                DataTable table = LinQBaseDao.Query("select * from BusinessType").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "车辆业务类型.xls");
                SaveXml("BusinessType", "车辆业务类型.xls");
            }

            //通行策略
            if (chkDrivewayStrategy.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("DrivewayStrategy");
                DataTable table = LinQBaseDao.Query("select * from DrivewayStrategy").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "通行策略.xls");
                SaveXml("DrivewayStrategy", "通行策略.xls");
            }

            //管控策略
            if (chkManagementStrategy.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("ManagementStrategy");
                DataTable table = LinQBaseDao.Query("select * from ManagementStrategy").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "管控策略.xls");
                SaveXml("ManagementStrategy", "管控策略.xls");
            }

            //管控信息
            if (chkControlInfo.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("ControlInfo");
                DataTable table = LinQBaseDao.Query("select * from ControlInfo").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "管控信息.xls");
                SaveXml("ControlInfo", "管控信息.xls");
            }

            //地磅基础信息
            if (chkWeighInfo.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("WeighInfo");
                DataTable table = LinQBaseDao.Query("select * from WeighInfo").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "地磅基础信息.xls");
                SaveXml("WeighInfo", "地磅基础信息.xls");
            }

            //地磅策略
            if (chkWeighStrategy.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("WeighStrategy");
                DataTable table = LinQBaseDao.Query("select * from WeighStrategy").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "地磅策略.xls");
                SaveXml("WeighStrategy", "地磅策略.xls");
            }
            #endregion

            #region    IC卡配置信息
            //IC卡类型
            if (chkICtype.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("ICCardType");
                DataTable table = LinQBaseDao.Query("select * from ICCardType").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "IC卡类型.xls");
                SaveXml("ICCardType", "IC卡类型.xls");
            }

            //IC卡信息
            if (chkICInfo.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("ICCard");
                DataTable table = LinQBaseDao.Query("select * from ICCard").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "IC卡信息.xls");
                SaveXml("ICCard", "IC卡信息.xls");
            }

            //IC卡信息中为万能卡的
            if (chkICWNInfo.Checked)
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("ICCard");
                DataTable table = LinQBaseDao.Query("select ICCard.ICCard_Value,ICCardType.ICCardType_Name,ICCard.ICCard_Permissions,ICCard.ICCard_HasCount from ICCard left join ICCardType on ICCard.ICCard_ICCardType_ID =ICCardType.ICCardType_ID where ICCardType.ICCardType_Name = '万能卡'and ICCard.ICCard_State = '启动'").Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;

                for (int j = 0; j < columns; j++)
                {
                    sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        sheet.Cells[j + 1, i].Value = table.Rows[j][i].ToString();
                    }
                }
                excelFile.SaveXls(path + "\\" + "IC万能卡信息.xls");
                SaveXml("ICWNCard", "IC万能卡信息.xls");
            }

            #endregion
        }



        /// <summary>
        ///  将Excel中的数据导入到SQL数据库中
        /// </summary>
        /// <param name="fileName">文件名称（带后缀名）</param>
        /// <param name="tableName">表名</param>
        /// <param name="path">路径</param>
        private void setExcelout(string fileName, string tableName, string path)
        {
            string Countname = null;
            DataTable tableCountName = LinQBaseDao.Query("select name from Syscolumns where id=Object_id('" + tableName + "')").Tables[0];
            for (int i = 0; i < tableCountName.Rows.Count; i++)
            {
                if (i >= 1)
                {
                    Countname += "," + tableCountName.Rows[i][0].ToString();
                }
                if (i == 0)
                {
                    Countname += tableCountName.Rows[i][0].ToString();
                }
            }

            DataTable table = new DataTable();
            OleDbConnection dbcon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + fileName + ";Extended Properties=Excel 8.0");
            if (dbcon.State == ConnectionState.Closed)
            {
                dbcon.Open();
                LinQBaseDao.Query("truncate table " + tableName);
            }
            string sql = "select * from [" + tableName + "$]";
            OleDbCommand cmd = new OleDbCommand(sql, dbcon);
            OleDbDataReader sdr = cmd.ExecuteReader();
            table.Load(sdr);
            string strInsertComm;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                strInsertComm = "";
                strInsertComm = "set identity_insert " + tableName + " on ; Insert INTO " + tableName + "(" + Countname + ")";
                strInsertComm += " values(";
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (j >= 1)
                    {
                        strInsertComm += ",'" + table.Rows[i][j].ToString().Trim() + "'";
                    }
                    else
                    {
                        strInsertComm += "'" + table.Rows[i][j].ToString().Trim() + "'";
                    }
                }
                strInsertComm += ")  ;set identity_insert " + tableName + " off;";
                LinQBaseDao.ExecuteSql(strInsertComm);
            }

            if (dbcon.State == ConnectionState.Open)
            {
                dbcon.Close();
            }
        }

        public string FormatSQLIDs(string ids)
        {
            string[] arryIDs = GetArryIDs(ids);
            string tempStr = "";
            for (int i = 0; i < arryIDs.Length; i++)
                tempStr += arryIDs[i].ToString() + ",";
            if (tempStr != "")
                tempStr = tempStr.Remove(tempStr.Length - 1);
            return tempStr;
        }

        public string[] GetArryIDs(string ids)
        {
            string[] strIDs = ids.Split(',');
            string[] arryIDs = new string[strIDs.Length];
            for (int i = 0; i < strIDs.Length; i++)
                arryIDs[i] = strIDs[i];
            return arryIDs;
        }

        /// <summary>
        /// xml添加节点数据
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="fileName">备份后的文件名称</param>
        /// <returns></returns>
        private bool SaveXml(string name, string fileName)
        {
            if (!Directory.Exists(saveFileTitle))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(saveFileTitle);
                    XmlNode root = xmlDoc.SelectSingleNode("config");//查找<config>
                    XmlNodeList nodeList = xmlDoc.SelectSingleNode("config").ChildNodes;
                    XmlElement xe1 = xmlDoc.CreateElement("ConfigurationName");//创建一个<ConfigurationName>节点
                    xe1.SetAttribute("name", name);//设置该节点genre属性
                    xe1.SetAttribute("fileName", fileName);//设置该节点ISBN属性
                    root.AppendChild(xe1);//添加到<bookstore>节点中
                    xmlDoc.Save(saveFileTitle);
                    return true;
                }
                catch 
                {
                    return false;
                }
            }
            else
                return false;
        }

        //全选
        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked == true)
            {
                checkBoxPositionVoice.Checked = true;
                checkBoxDriveway.Checked = true;
                checkbixCamera.Checked = true;
                checkBoxFVNInfo.Checked = true;
                chkLed.Checked = true;
                chkVoice.Checked = true;
                chkSMS.Checked = true;
                chkPrintInfo.Checked = true;

                chkroleInfo.Checked = true;
                chkUserInfo.Checked = true;
                chkUserRolePer.Checked = true;

                chkCarAttribute.Checked = true;
                chkCartype.Checked = true;
                chkBusType.Checked = true;
                chkDrivewayStrategy.Checked = true;
                chkManagementStrategy.Checked = true;
                chkControlInfo.Checked = true;
                chkWeighInfo.Checked = true;
                chkWeighStrategy.Checked = true;

                chkICtype.Checked = true;
                chkICInfo.Checked = true;
            }
            else
            {
                checkBoxPositionVoice.Checked = false;
                checkBoxDriveway.Checked = false;
                checkbixCamera.Checked = false;
                checkBoxFVNInfo.Checked = false;
                chkLed.Checked = false;
                chkVoice.Checked = false;
                chkSMS.Checked = false;
                chkPrintInfo.Checked = false;

                chkroleInfo.Checked = false;
                chkUserInfo.Checked = false;
                chkUserRolePer.Checked = false;

                chkCarAttribute.Checked = false;
                chkCartype.Checked = false;
                chkBusType.Checked = false;
                chkDrivewayStrategy.Checked = false;
                chkManagementStrategy.Checked = false;
                chkControlInfo.Checked = false;
                chkWeighInfo.Checked = false;
                chkWeighStrategy.Checked = false;

                chkICtype.Checked = false;
                chkICInfo.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                chkMenu.Checked = true;
                chkOther.Checked = true;
                chkDictionary.Checked = true;
                chkSystem.Checked = true;
            }
            else
            {
                chkMenu.Checked = false;
                chkOther.Checked = false;
                chkDictionary.Checked = false;
                chkSystem.Checked = false;
            }
        }

        /// <summary>
        /// 判断当前xml文件是否存在信息，文件是否存在
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        private bool tableNames(string tablename, string path)
        {
            try
            {
                if (File.Exists(path + tablename))
                {
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}