namespace EMEWE.CarManagement.Commons.FlrCommon
{
    partial class LogInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbSelect = new System.Windows.Forms.GroupBox();
            this.txtendTime = new System.Windows.Forms.DateTimePicker();
            this.txtbeginTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboxType = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtLog_Name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dgvLogInfo = new System.Windows.Forms.DataGridView();
            this.流水号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Log_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Log_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dictionary_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Log_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Log_Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Log_Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.tslHomPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslPreviousPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tstbPageCurrent = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.tslPageCount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.tslNMax = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tslNextPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tslLastPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel12 = new System.Windows.Forms.ToolStripLabel();
            this.tscbxPageSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tslNotSelect = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.tslSelectAll = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.gbSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSelect
            // 
            this.gbSelect.Controls.Add(this.txtendTime);
            this.gbSelect.Controls.Add(this.txtbeginTime);
            this.gbSelect.Controls.Add(this.label5);
            this.gbSelect.Controls.Add(this.label4);
            this.gbSelect.Controls.Add(this.comboxType);
            this.gbSelect.Controls.Add(this.btnSearch);
            this.gbSelect.Controls.Add(this.txtLog_Name);
            this.gbSelect.Controls.Add(this.label1);
            this.gbSelect.Controls.Add(this.label13);
            this.gbSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSelect.Location = new System.Drawing.Point(0, 0);
            this.gbSelect.Name = "gbSelect";
            this.gbSelect.Size = new System.Drawing.Size(770, 94);
            this.gbSelect.TabIndex = 1;
            this.gbSelect.TabStop = false;
            this.gbSelect.Text = "搜   索";
            // 
            // txtendTime
            // 
            this.txtendTime.Location = new System.Drawing.Point(382, 57);
            this.txtendTime.Name = "txtendTime";
            this.txtendTime.Size = new System.Drawing.Size(138, 21);
            this.txtendTime.TabIndex = 168;
            this.txtendTime.ValueChanged += new System.EventHandler(this.txtendTime_ValueChanged);
            this.txtendTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtendTime_KeyPress);
            // 
            // txtbeginTime
            // 
            this.txtbeginTime.Location = new System.Drawing.Point(105, 56);
            this.txtbeginTime.Name = "txtbeginTime";
            this.txtbeginTime.Size = new System.Drawing.Size(138, 21);
            this.txtbeginTime.TabIndex = 169;
            this.txtbeginTime.ValueChanged += new System.EventHandler(this.txtbeginTime_ValueChanged);
            this.txtbeginTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbeginTime_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(311, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 166;
            this.label5.Text = "结束时间：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 167;
            this.label4.Text = "开始时间：";
            // 
            // comboxType
            // 
            this.comboxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxType.FormattingEnabled = true;
            this.comboxType.Location = new System.Drawing.Point(382, 24);
            this.comboxType.Name = "comboxType";
            this.comboxType.Size = new System.Drawing.Size(138, 20);
            this.comboxType.TabIndex = 165;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(604, 56);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 21);
            this.btnSearch.TabIndex = 163;
            this.btnSearch.Text = "搜    索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtLog_Name
            // 
            this.txtLog_Name.Location = new System.Drawing.Point(105, 23);
            this.txtLog_Name.Name = "txtLog_Name";
            this.txtLog_Name.Size = new System.Drawing.Size(138, 21);
            this.txtLog_Name.TabIndex = 160;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 159;
            this.label1.Text = "操作人：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(311, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 159;
            this.label13.Text = "操作类型：";
            // 
            // dgvLogInfo
            // 
            this.dgvLogInfo.AllowUserToAddRows = false;
            this.dgvLogInfo.AllowUserToDeleteRows = false;
            this.dgvLogInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.流水号,
            this.Log_ID,
            this.Log_Name,
            this.Dictionary_Name,
            this.Log_Time,
            this.Log_Content,
            this.Log_Remark});
            this.dgvLogInfo.Location = new System.Drawing.Point(0, 121);
            this.dgvLogInfo.Name = "dgvLogInfo";
            this.dgvLogInfo.ReadOnly = true;
            this.dgvLogInfo.RowHeadersVisible = false;
            this.dgvLogInfo.RowTemplate.Height = 23;
            this.dgvLogInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogInfo.Size = new System.Drawing.Size(770, 406);
            this.dgvLogInfo.TabIndex = 90;
            this.dgvLogInfo.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvLogInfo_RowPostPaint);
            this.dgvLogInfo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvLogInfo_DataBindingComplete);
            // 
            // 流水号
            // 
            this.流水号.HeaderText = "流水号";
            this.流水号.Name = "流水号";
            this.流水号.ReadOnly = true;
            // 
            // Log_ID
            // 
            this.Log_ID.DataPropertyName = "Log_ID";
            this.Log_ID.HeaderText = "日志记录编号";
            this.Log_ID.Name = "Log_ID";
            this.Log_ID.ReadOnly = true;
            this.Log_ID.Width = 120;
            // 
            // Log_Name
            // 
            this.Log_Name.DataPropertyName = "Log_Name";
            this.Log_Name.HeaderText = "操作人";
            this.Log_Name.Name = "Log_Name";
            this.Log_Name.ReadOnly = true;
            // 
            // Dictionary_Name
            // 
            this.Dictionary_Name.DataPropertyName = "Dictionary_Name";
            this.Dictionary_Name.HeaderText = "操作类型";
            this.Dictionary_Name.Name = "Dictionary_Name";
            this.Dictionary_Name.ReadOnly = true;
            // 
            // Log_Time
            // 
            this.Log_Time.DataPropertyName = "Log_Time";
            this.Log_Time.HeaderText = "操作时间";
            this.Log_Time.Name = "Log_Time";
            this.Log_Time.ReadOnly = true;
            // 
            // Log_Content
            // 
            this.Log_Content.DataPropertyName = "Log_Content";
            this.Log_Content.HeaderText = "操作内容";
            this.Log_Content.Name = "Log_Content";
            this.Log_Content.ReadOnly = true;
            // 
            // Log_Remark
            // 
            this.Log_Remark.DataPropertyName = "Log_Remark";
            this.Log_Remark.HeaderText = "操作备注";
            this.Log_Remark.Name = "Log_Remark";
            this.Log_Remark.ReadOnly = true;
            this.Log_Remark.Width = 180;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.AutoSize = false;
            this.bindingNavigator1.BackColor = System.Drawing.Color.Gainsboro;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("宋体", 9F);
            this.bindingNavigator1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslHomPage1,
            this.toolStripSeparator1,
            this.tslPreviousPage1,
            this.toolStripSeparator2,
            this.toolStripLabel3,
            this.tstbPageCurrent,
            this.toolStripLabel5,
            this.tslPageCount,
            this.toolStripLabel6,
            this.toolStripSeparator3,
            this.toolStripLabel7,
            this.tslNMax,
            this.toolStripLabel9,
            this.toolStripSeparator4,
            this.tslNextPage1,
            this.toolStripSeparator5,
            this.tslLastPage1,
            this.toolStripSeparator6,
            this.toolStripLabel12,
            this.tscbxPageSize,
            this.toolStripSeparator7,
            this.tslNotSelect,
            this.toolStripSeparator16,
            this.tslSelectAll,
            this.toolStripSeparator17});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 94);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(770, 30);
            this.bindingNavigator1.TabIndex = 92;
            this.bindingNavigator1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.bdnInfo_ItemClicked);
            // 
            // tslHomPage1
            // 
            this.tslHomPage1.Name = "tslHomPage1";
            this.tslHomPage1.Size = new System.Drawing.Size(29, 27);
            this.tslHomPage1.Text = "首页";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // tslPreviousPage1
            // 
            this.tslPreviousPage1.Name = "tslPreviousPage1";
            this.tslPreviousPage1.Size = new System.Drawing.Size(41, 27);
            this.tslPreviousPage1.Text = "上一页";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(53, 27);
            this.toolStripLabel3.Text = "当前页数";
            // 
            // tstbPageCurrent
            // 
            this.tstbPageCurrent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tstbPageCurrent.Name = "tstbPageCurrent";
            this.tstbPageCurrent.Size = new System.Drawing.Size(50, 30);
            this.tstbPageCurrent.Text = "1";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(17, 27);
            this.toolStripLabel5.Text = "共";
            // 
            // tslPageCount
            // 
            this.tslPageCount.Name = "tslPageCount";
            this.tslPageCount.Size = new System.Drawing.Size(11, 27);
            this.tslPageCount.Text = "0";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(17, 27);
            this.toolStripLabel6.Text = "页";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(17, 27);
            this.toolStripLabel7.Text = "共";
            // 
            // tslNMax
            // 
            this.tslNMax.Name = "tslNMax";
            this.tslNMax.Size = new System.Drawing.Size(11, 27);
            this.tslNMax.Text = "0";
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(17, 27);
            this.toolStripLabel9.Text = "条";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 30);
            // 
            // tslNextPage1
            // 
            this.tslNextPage1.Name = "tslNextPage1";
            this.tslNextPage1.Size = new System.Drawing.Size(41, 27);
            this.tslNextPage1.Text = "下一页";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 30);
            // 
            // tslLastPage1
            // 
            this.tslLastPage1.Name = "tslLastPage1";
            this.tslLastPage1.Size = new System.Drawing.Size(29, 27);
            this.tslLastPage1.Text = "尾页";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripLabel12
            // 
            this.toolStripLabel12.Name = "toolStripLabel12";
            this.toolStripLabel12.Size = new System.Drawing.Size(53, 27);
            this.toolStripLabel12.Text = "每页条数";
            // 
            // tscbxPageSize
            // 
            this.tscbxPageSize.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "60",
            "65",
            "70",
            "90",
            "100",
            "200",
            "300",
            "500",
            "800",
            "1000"});
            this.tscbxPageSize.Name = "tscbxPageSize";
            this.tscbxPageSize.Size = new System.Drawing.Size(75, 30);
            this.tscbxPageSize.Text = "10";
            this.tscbxPageSize.SelectedIndexChanged += new System.EventHandler(this.tscbxPageSize_SelectedIndexChanged);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 30);
            // 
            // tslNotSelect
            // 
            this.tslNotSelect.Name = "tslNotSelect";
            this.tslNotSelect.Size = new System.Drawing.Size(53, 27);
            this.tslNotSelect.Text = "取消全选";
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(6, 30);
            // 
            // tslSelectAll
            // 
            this.tslSelectAll.Name = "tslSelectAll";
            this.tslSelectAll.Size = new System.Drawing.Size(29, 27);
            this.tslSelectAll.Text = "全选";
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(6, 30);
            // 
            // LogInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 524);
            this.Controls.Add(this.bindingNavigator1);
            this.Controls.Add(this.dgvLogInfo);
            this.Controls.Add(this.gbSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "LogInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "日志管理";
            this.Load += new System.EventHandler(this.LogInfoForm_Load);
            this.gbSelect.ResumeLayout(false);
            this.gbSelect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSelect;
        private System.Windows.Forms.DateTimePicker txtendTime;
        private System.Windows.Forms.DateTimePicker txtbeginTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboxType;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtLog_Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DataGridView dgvLogInfo;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel tslHomPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel tslPreviousPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox tstbPageCurrent;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripLabel tslPageCount;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripLabel tslNMax;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel tslNextPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel tslLastPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel12;
        private System.Windows.Forms.ToolStripComboBox tscbxPageSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel tslNotSelect;
        private System.Windows.Forms.ToolStripLabel tslSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.DataGridViewTextBoxColumn 流水号;
        private System.Windows.Forms.DataGridViewTextBoxColumn Log_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Log_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dictionary_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Log_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Log_Content;
        private System.Windows.Forms.DataGridViewTextBoxColumn Log_Remark;
    }
}