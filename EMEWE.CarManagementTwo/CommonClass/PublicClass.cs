using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using GemBox.ExcelLite;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace EMEWE.CarManagement.CommonClass
{

    public class PublicClass
    {
        /// <summary>
        /// 打开新窗体
        /// </summary>
        /// <param name="fFrm"></param>
        public static void ShowChildForm(Form fFrm)
        {
            foreach (Form item in Application.OpenForms)
            {
                if (item.Name.Equals(fFrm.Name))
                {
                    item.Activate();
                    item.WindowState = FormWindowState.Normal;
                    item.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - item.Width) / 2, 55);
                    item.TopLevel = true;
                    return;
                }
            }
            fFrm.Show();
            fFrm.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - fFrm.Width) / 2, 55);
            fFrm.TopLevel = true;
        }
        /// <summary>
        /// 导出数据路径
        /// </summary>
        /// <returns></returns>
        public static string SetExportPath()
        {
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\车辆本地缓存数据\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\车辆本地缓存数据\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd");
            }
            return path;
        }
        /// <summary>
        /// 数据缓存到本地
        /// </summary>
        public void GetCacheData(string strsql)
        {
            try
            {
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheet sheet = excelFile.Worksheets.Add("CarInoutRecord");
                DataTable table = LinQBaseDao.Query(strsql).Tables[0];
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

                if (File.Exists(SetExportPath() + "\\" + "车辆通行记录.xls"))
                {
                    File.Delete(SetExportPath() + "\\" + "车辆通行记录.xls");
                    excelFile.SaveXls(SetExportPath() + "\\" + "车辆通行记录.xls");

                    //ExcelClass ec = new ExcelClass();

                    //ec.InExcel(table, columns, rows, SetExportPath() + "\\" + "车辆通行记录.xls");
                }
                else
                {
                    excelFile.SaveXls(SetExportPath() + "\\" + "车辆通行记录.xls");                  
                }   
            }
            catch (Exception err)
            {
                CommonalityEntity.WriteTextLog("车辆本地缓存数据异常："+ err.Message);
            } 
        }
        /// <summary>
        /// xml添加节点数据
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="fileName">备份后的文件名称</param>
        /// <returns></returns>
        private bool SaveXml(string name, string fileName,string url)
        {
            if (!Directory.Exists(url))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(url);
                    XmlNode root = xmlDoc.SelectSingleNode("config");//查找<config>
                    XmlNodeList nodeList = xmlDoc.SelectSingleNode("config").ChildNodes;
                    XmlElement xe1 = xmlDoc.CreateElement("ConfigurationName");//创建一个<ConfigurationName>节点
                    xe1.SetAttribute("name", name);//设置该节点genre属性
                    xe1.SetAttribute("fileName", fileName);//设置该节点ISBN属性
                    root.AppendChild(xe1);//添加到<bookstore>节点中
                    xmlDoc.Save(url);
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
        /// <summary>
        /// 显示工具提示  徐东冬
        /// </summary>
        /// <param name="tti">ToolTipIcon.Info、None、Warning、Error</param>
        /// <param name="str">工具提示标题（ToolTipTitle）</param>
        /// <param name="strMessage">提示信息</param>
        /// <param name="controlName">提示框显示的控件</param>
        /// <param name="form">提示框显示的窗体</param>
        public static void ShowToolTip(ToolTipIcon tti, string strTitle, string strMessage, Control controlName, Form form)
        {
            if (!form.IsDisposed)
            {
                ToolTip toolTip1 = new ToolTip();
                toolTip1.ToolTipIcon = tti;
                toolTip1.ToolTipTitle = strTitle;
                Point showLocation = new Point(
                    controlName.Location.X + controlName.Width,
                    controlName.Location.Y);
                toolTip1.Show(strMessage, form, showLocation, 5000);
                controlName.Focus();
            }
        }
    }
}
