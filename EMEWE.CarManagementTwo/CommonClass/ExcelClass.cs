
using EMEWE.CarManagement.DAL;
using GemBox.ExcelLite;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;

namespace EMEWE.CarManagement.CommonClass
{
    public class ExcelClass
    {
        public void InExcel(System.Data.DataTable table,int columns,int rows,string filepath)
        {
            try
            {
                filepath = filepath.Replace("\\\\", "\\");

                //ExcelFile excelFile = new ExcelFile();
                //ExcelWorksheet sheet = excelFile.Worksheets.ActiveWorksheet;

                //int num = sheet.Rows.Count;


                //1.创建Applicaton对象
                Application xApp = new Application();

                //2.得到workbook对象，打开已有的文件
                Workbook xBook = xApp.Workbooks.Open(filepath,
                                      Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                      Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                      Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                //3.指定要操作的Sheet
                Worksheet xSheet = (Worksheet)xBook.Sheets[1];
            }
            catch (Exception err)
            {

            }

        }

    }
}
