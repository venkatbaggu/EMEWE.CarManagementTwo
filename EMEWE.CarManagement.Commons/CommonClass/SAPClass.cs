using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPFunctionsOCX;
using SAPLogonCtrl;
using SAPTableFactoryCtrl;
using System.Data;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public class SAPClass
    {
        private string i_CRFLG;
        private string i_DATUM;
        private string i_EBELN;
        private string i_WTD_ID;
        private string i_ZCARNO;
        private string lV_CARNO;
        private string lV_EBELN;
        private string lV_VBELN;

        private DataTable table1;
        private DataTable table2;
        /// <summary>
        /// 成品SAP
        /// </summary>
        public void ChengPin()
        {
            try
            {
                Connection connection = (Connection)this.ReturnLogin().NewConnection();
                if (connection.Logon(0, true))
                {
                    int num2;
                    DataRow row;
                    SAPFunctionsClass class3 = new SAPFunctionsClass
                    {
                        Connection = connection
                    };
                    IFunction function = (IFunction)class3.Add("ZLE_RFC_VEHICLE_CHECKIN");
                    IParameter parameter = (IParameter)function.get_Exports("LV_VEHICLE_TYPE");
                    parameter.Value = "20";
                    IParameter parameter2 = (IParameter)function.get_Exports("LV_CARNO");
                    parameter2.Value = this.LV_CARNO;
                    function.Call();
                    Tables tables = (Tables)function.Tables;
                    Table table = (Table)tables.get_Item("OUT_TAB");
                    int rowCount = table.RowCount;
                    DataTable table2 = new DataTable();
                    for (num2 = 1; num2 <= rowCount; num2++)
                    {
                        row = table2.NewRow();
                        if (num2 == 1)
                        {
                            table2.Columns.Add("CARNO");
                            table2.Columns.Add("WTD_ID");
                            table2.Columns.Add("O_FLAG");
                            table2.Columns.Add("TEL_NUMBER");
                            table2.Columns.Add("HG");
                            table2.Columns.Add("XZ");
                            table2.Columns.Add("KDATB");
                            table2.Columns.Add("KDATE");
                            table2.Columns.Add("NAME1_C");
                            table2.Columns.Add("NAME1_P");
                            table2.Columns.Add("Prodh");
                        }
                        row["CARNO"] = table.get_Cell(num2, "CARNO");
                        row["WTD_ID"] = table.get_Cell(num2, "WTD_ID");
                        row["O_FLAG"] = table.get_Cell(num2, "O_FLAG");
                        row["TEL_NUMBER"] = table.get_Cell(num2, "TEL_NUMBER");
                        row["HG"] = table.get_Cell(num2, "HG");
                        row["XZ"] = table.get_Cell(num2, "XZ");
                        row["KDATB"] = table.get_Cell(num2, "KDATB");
                        row["KDATE"] = table.get_Cell(num2, "KDATE");
                        row["NAME1_C"] = table.get_Cell(num2, "NAME1_C");
                        row["NAME1_P"] = table.get_Cell(num2, "NAME1_P");
                        row["Prodh"] = table.get_Cell(num2, "Prodh");
                        table2.Rows.Add(row);
                    }
                    Tables tables2 = (Tables)function.Tables;
                    Table table3 = (Table)tables2.get_Item("Return");
                    int num3 = table3.RowCount;
                    DataTable table4 = new DataTable();
                    for (num2 = 1; num2 <= num3; num2++)
                    {
                        row = table4.NewRow();
                        if (num2 == 1)
                        {
                            table4.Columns.Add("type");
                            table4.Columns.Add("message");
                        }
                        row["type"] = table3.get_Cell(num2, "type").ToString();
                        row["message"] = table3.get_Cell(num2, "message").ToString();
                        table4.Rows.Add(row);
                    }
                    this.Table1 = table2;
                    this.Table2 = table4;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void FangXing()
        {
            try
            {
                Connection connection = (Connection)this.ReturnLogin().NewConnection();
                if (connection.Logon(0, true))
                {
                    SAPFunctionsClass class3 = new SAPFunctionsClass
                    {
                        Connection = connection
                    };
                    IFunction function = (IFunction)class3.Add("ZLE_RFC_ZMMWTD");
                    IParameter parameter = (IParameter)function.get_Exports("I_VSTEL");
                    parameter.Value = "3000";
                    IParameter parameter2 = (IParameter)function.get_Exports("I_ZMENID");
                    parameter2.Value = "CQ02";
                    IParameter parameter3 = (IParameter)function.get_Exports("I_ZCARNO");
                    parameter3.Value = this.I_ZCARNO;
                    IParameter parameter4 = (IParameter)function.get_Exports("I_EBELN");
                    parameter4.Value = this.I_EBELN;
                    function.Call();
                    Tables tables = (Tables)function.Tables;
                    Table table = (Table)tables.get_Item("Return");
                    int rowCount = table.RowCount;
                    DataTable table2 = new DataTable();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        DataRow row = table2.NewRow();
                        if (i == 1)
                        {
                            table2.Columns.Add("type");
                            table2.Columns.Add("message");
                        }
                        row["type"] = table.get_Cell(i, "type").ToString();
                        row["message"] = table.get_Cell(i, "message").ToString();
                        table2.Rows.Add(row);
                    }
                    this.Table2 = table2;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        /// <summary>
        /// 登录SAP
        /// </summary>
        /// <returns></returns>
        private SAPLogonControlClass ReturnLogin()
        {
            try
            {
                return new SAPLogonControlClass { System = Class1.strSystem, Client = Class1.strClient, MessageServer = Class1.strMessageServer, GroupName = Class1.strGroupName, Language = Class1.strLanguage, User = Class1.strUser, Password = Class1.strPassword };
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录SAP失败！");
                return null;
            }
        }
        /// <summary>
        /// 三废SAP
        /// </summary>
        public void SanFei()
        {
            try
            {
                Connection connection = (Connection)this.ReturnLogin().NewConnection();
                if (connection.Logon(0, true))
                {
                    int num2;
                    DataRow row;
                    SAPFunctionsClass class3 = new SAPFunctionsClass
                    {
                        Connection = connection
                    };
                    IFunction function = (IFunction)class3.Add("ZLE_RFC_VEHICLE_CHECKIN");
                    IParameter parameter = (IParameter)function.get_Exports("LV_VEHICLE_TYPE");
                    parameter.Value = "30";
                    IParameter parameter2 = (IParameter)function.get_Exports("LV_VBELN");
                    parameter2.Value = this.LV_VBELN;
                    function.Call();
                    Tables tables = (Tables)function.Tables;
                    Table table = (Table)tables.get_Item("OUT_TAB");
                    int rowCount = table.RowCount;
                    DataTable table2 = new DataTable();
                    for (num2 = 1; num2 <= rowCount; num2++)
                    {
                        row = table2.NewRow();
                        if (num2 == 1)
                        {
                            table2.Columns.Add("VBELN");
                            table2.Columns.Add("NAME1_C");
                            table2.Columns.Add("MAKTX");
                        }
                        row["VBELN"] = table.get_Cell(num2, "VBELN").ToString();
                        row["NAME1_C"] = table.get_Cell(num2, "NAME1_C").ToString();
                        row["MAKTX"] = table.get_Cell(num2, "MAKTX").ToString();
                        table2.Rows.Add(row);
                    }
                    Tables tables2 = (Tables)function.Tables;
                    Table table3 = (Table)tables2.get_Item("Return");
                    int num3 = table3.RowCount;
                    DataTable table4 = new DataTable();
                    for (num2 = 1; num2 <= num3; num2++)
                    {
                        row = table4.NewRow();
                        if (num2 == 1)
                        {
                            table4.Columns.Add("type");
                            table4.Columns.Add("message");
                        }
                        row["type"] = table3.get_Cell(num2, "type").ToString();
                        row["message"] = table3.get_Cell(num2, "message").ToString();
                        table4.Rows.Add(row);
                    }
                    this.Table1 = table2;
                    this.Table2 = table4;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void SaveData()
        {
            try
            {
                Connection connection = (Connection)this.ReturnLogin().NewConnection();
                if (connection.Logon(0, true))
                {
                    SAPFunctionsClass class3 = new SAPFunctionsClass
                    {
                        Connection = connection
                    };
                    IFunction function = (IFunction)class3.Add("ZLE_RFC_WTD");
                    IParameter parameter = (IParameter)function.get_Exports("I_WTD_ID");
                    parameter.Value = this.I_WTD_ID;
                    IParameter parameter2 = (IParameter)function.get_Exports("I_DATUM");
                    parameter2.Value = CommonalityEntity.GetServersTime().ToString();
                    IParameter parameter3 = (IParameter)function.get_Exports("I_CRFLG");
                    parameter3.Value = this.I_CRFLG;
                    function.Call();
                    Tables tables = (Tables)function.Tables;
                    Table table = (Table)tables.get_Item("Return");
                    int rowCount = table.RowCount;
                    DataTable table2 = new DataTable();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        DataRow row = table2.NewRow();
                        if (i == 1)
                        {
                            table2.Columns.Add("type");
                            table2.Columns.Add("message");
                        }
                        row["type"] = table.get_Cell(i, "type").ToString();
                        row["message"] = table.get_Cell(i, "message").ToString();
                        table2.Rows.Add(row);
                    }
                    this.Table2 = table2;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        /// <summary>
        /// 送货SAP
        /// </summary>
        public void SendGoods()
        {
            try
            {
                Connection connection = (Connection)this.ReturnLogin().NewConnection();
                if (connection.Logon(0, true))
                {
                    int num2;
                    DataRow row;
                    SAPFunctionsClass class3 = new SAPFunctionsClass
                    {
                        Connection = connection
                    };
                    IFunction function = (IFunction)class3.Add("ZLE_RFC_VEHICLE_CHECKIN");
                    IParameter parameter = (IParameter)function.get_Exports("LV_VEHICLE_TYPE");
                    parameter.Value = "10";
                    IParameter parameter2 = (IParameter)function.get_Exports("LV_EBELN");
                    parameter2.Value = this.LV_EBELN;
                    function.Call();
                    Tables tables = (Tables)function.Tables;
                    Table table = (Table)tables.get_Item("OUT_TAB");
                    int rowCount = table.RowCount;
                    DataTable table2 = new DataTable();
                    for (num2 = 1; num2 <= rowCount; num2++)
                    {
                        row = table2.NewRow();
                        if (num2 == 1)
                        {
                            table2.Columns.Add("EBELN");
                            table2.Columns.Add("NAME1_P");
                            table2.Columns.Add("MAKTX");
                            table2.Columns.Add("KDATB");
                            table2.Columns.Add("KDATE");
                        }
                        row["EBELN"] = table.get_Cell(num2, "EBELN").ToString();
                        row["NAME1_P"] = table.get_Cell(num2, "NAME1_P").ToString();
                        row["MAKTX"] = table.get_Cell(num2, "MAKTX").ToString();
                        row["KDATB"] = table.get_Cell(num2, "KDATB").ToString();
                        row["KDATE"] = table.get_Cell(num2, "KDATE").ToString();
                        table2.Rows.Add(row);
                    }
                    Tables tables2 = (Tables)function.Tables;
                    Table table3 = (Table)tables2.get_Item("Return");
                    int num3 = table3.RowCount;
                    DataTable table4 = new DataTable();
                    for (num2 = 1; num2 <= num3; num2++)
                    {
                        row = table4.NewRow();
                        if (num2 == 1)
                        {
                            table4.Columns.Add("type");
                            table4.Columns.Add("message");
                        }
                        row["type"] = table3.get_Cell(num2, "type").ToString();
                        row["message"] = table3.get_Cell(num2, "message").ToString();
                        table4.Rows.Add(row);
                    }
                    this.Table1 = table2;
                    this.Table2 = table4;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public string I_CRFLG
        {
            get
            {
                return this.i_CRFLG;
            }
            set
            {
                this.i_CRFLG = value;
            }
        }

        public string I_DATUM
        {
            get
            {
                return this.i_DATUM;
            }
            set
            {
                this.i_DATUM = value;
            }
        }

        public string I_EBELN
        {
            get
            {
                return this.i_EBELN;
            }
            set
            {
                this.i_EBELN = value;
            }
        }

        public string I_WTD_ID
        {
            get
            {
                return this.i_WTD_ID;
            }
            set
            {
                this.i_WTD_ID = value;
            }
        }

        public string I_ZCARNO
        {
            get
            {
                return this.i_ZCARNO;
            }
            set
            {
                this.i_ZCARNO = value;
            }
        }

        public string LV_CARNO
        {
            get
            {
                return this.lV_CARNO;
            }
            set
            {
                this.lV_CARNO = value;
            }
        }

        public string LV_EBELN
        {
            get
            {
                return this.lV_EBELN;
            }
            set
            {
                this.lV_EBELN = value;
            }
        }

        public string LV_VBELN
        {
            get
            {
                return this.lV_VBELN;
            }
            set
            {
                this.lV_VBELN = value;
            }
        }

        public DataTable Table1
        {
            get
            {
                return this.table1;
            }
            set
            {
                this.table1 = value;
            }
        }

        public DataTable Table2
        {
            get
            {
                return this.table2;
            }
            set
            {
                this.table2 = value;
            }
        }
    }
}