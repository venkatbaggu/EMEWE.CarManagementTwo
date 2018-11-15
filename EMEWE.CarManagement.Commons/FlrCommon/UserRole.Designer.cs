namespace EMEWE.CarManagement.Commons.FlrCommon
{
    partial class UserRole
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchName = new System.Windows.Forms.TextBox();
            this.lvwUserList = new System.Windows.Forms.DataGridView();
            this.流水号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role_Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
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
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tslNotSelect = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.tslSelectAll = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRoleName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblId = new System.Windows.Forms.Label();
            this.btnCancle = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboxState = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnDel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lvwUserList)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(413, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 65;
            this.btnSearch.Text = "搜   索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchName
            // 
            this.txtSearchName.Location = new System.Drawing.Point(128, 17);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(100, 21);
            this.txtSearchName.TabIndex = 64;
            // 
            // lvwUserList
            // 
            this.lvwUserList.AllowUserToAddRows = false;
            this.lvwUserList.AllowUserToDeleteRows = false;
            this.lvwUserList.AllowUserToResizeRows = false;
            this.lvwUserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvwUserList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.流水号,
            this.状态,
            this.Role_Id,
            this.Role_Name,
            this.Role_Remark});
            this.lvwUserList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lvwUserList.Location = new System.Drawing.Point(3, 220);
            this.lvwUserList.Name = "lvwUserList";
            this.lvwUserList.ReadOnly = true;
            this.lvwUserList.RowHeadersVisible = false;
            this.lvwUserList.RowTemplate.Height = 23;
            this.lvwUserList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lvwUserList.Size = new System.Drawing.Size(766, 303);
            this.lvwUserList.TabIndex = 67;
            this.lvwUserList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvwUserList_MouseClick);
            this.lvwUserList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwUserList_MouseDoubleClick);
            this.lvwUserList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.lvwUserList_RowPostPaint);
            this.lvwUserList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.lvwUserList_DataBindingComplete);
            // 
            // 流水号
            // 
            this.流水号.HeaderText = "流水号";
            this.流水号.Name = "流水号";
            this.流水号.ReadOnly = true;
            // 
            // 状态
            // 
            this.状态.DataPropertyName = "Role_State";
            this.状态.HeaderText = "状态";
            this.状态.Name = "状态";
            this.状态.ReadOnly = true;
            // 
            // Role_Id
            // 
            this.Role_Id.DataPropertyName = "Role_Id";
            this.Role_Id.HeaderText = "角色编号";
            this.Role_Id.Name = "Role_Id";
            this.Role_Id.ReadOnly = true;
            // 
            // Role_Name
            // 
            this.Role_Name.DataPropertyName = "Role_Name";
            this.Role_Name.HeaderText = "角色名称";
            this.Role_Name.Name = "Role_Name";
            this.Role_Name.ReadOnly = true;
            // 
            // Role_Remark
            // 
            this.Role_Remark.DataPropertyName = "Role_Remark";
            this.Role_Remark.HeaderText = "备注";
            this.Role_Remark.Name = "Role_Remark";
            this.Role_Remark.ReadOnly = true;
            this.Role_Remark.Width = 400;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(57, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 63;
            this.label9.Text = "角色名称：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.bindingNavigator1);
            this.groupBox2.Controls.Add(this.lvwUserList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(772, 526);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSearch);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtSearchName);
            this.groupBox3.Location = new System.Drawing.Point(0, 147);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(769, 44);
            this.groupBox3.TabIndex = 101;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "搜索";
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.AutoSize = false;
            this.bindingNavigator1.BackColor = System.Drawing.Color.Gainsboro;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
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
            this.toolStripSeparator15,
            this.toolStripLabel1,
            this.toolStripSeparator7,
            this.tslNotSelect,
            this.toolStripSeparator16,
            this.tslSelectAll,
            this.toolStripSeparator17});
            this.bindingNavigator1.Location = new System.Drawing.Point(3, 193);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(766, 27);
            this.bindingNavigator1.TabIndex = 100;
            this.bindingNavigator1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.bindingNavigator1_ItemClicked);
            // 
            // tslHomPage1
            // 
            this.tslHomPage1.Name = "tslHomPage1";
            this.tslHomPage1.Size = new System.Drawing.Size(29, 24);
            this.tslHomPage1.Text = "首页";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tslPreviousPage1
            // 
            this.tslPreviousPage1.Name = "tslPreviousPage1";
            this.tslPreviousPage1.Size = new System.Drawing.Size(41, 24);
            this.tslPreviousPage1.Text = "上一页";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(53, 24);
            this.toolStripLabel3.Text = "当前页数";
            // 
            // tstbPageCurrent
            // 
            this.tstbPageCurrent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tstbPageCurrent.Name = "tstbPageCurrent";
            this.tstbPageCurrent.Size = new System.Drawing.Size(50, 27);
            this.tstbPageCurrent.Text = "1";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(17, 24);
            this.toolStripLabel5.Text = "共";
            // 
            // tslPageCount
            // 
            this.tslPageCount.Name = "tslPageCount";
            this.tslPageCount.Size = new System.Drawing.Size(11, 24);
            this.tslPageCount.Text = "0";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(17, 24);
            this.toolStripLabel6.Text = "页";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(17, 24);
            this.toolStripLabel7.Text = "共";
            // 
            // tslNMax
            // 
            this.tslNMax.Name = "tslNMax";
            this.tslNMax.Size = new System.Drawing.Size(11, 24);
            this.tslNMax.Text = "0";
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(17, 24);
            this.toolStripLabel9.Text = "条";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // tslNextPage1
            // 
            this.tslNextPage1.Name = "tslNextPage1";
            this.tslNextPage1.Size = new System.Drawing.Size(41, 24);
            this.tslNextPage1.Text = "下一页";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // tslLastPage1
            // 
            this.tslLastPage1.Name = "tslLastPage1";
            this.tslLastPage1.Size = new System.Drawing.Size(29, 24);
            this.tslLastPage1.Text = "尾页";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel12
            // 
            this.toolStripLabel12.Name = "toolStripLabel12";
            this.toolStripLabel12.Size = new System.Drawing.Size(53, 24);
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
            this.tscbxPageSize.Size = new System.Drawing.Size(75, 27);
            this.tscbxPageSize.Text = "10";
            this.tscbxPageSize.SelectedIndexChanged += new System.EventHandler(this.tscbxPageSize_SelectedIndexChanged);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(53, 24);
            this.toolStripLabel1.Text = "权限配置";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
            // 
            // tslNotSelect
            // 
            this.tslNotSelect.Name = "tslNotSelect";
            this.tslNotSelect.Size = new System.Drawing.Size(53, 24);
            this.tslNotSelect.Text = "取消全选";
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(6, 27);
            // 
            // tslSelectAll
            // 
            this.tslSelectAll.Name = "tslSelectAll";
            this.tslSelectAll.Size = new System.Drawing.Size(29, 24);
            this.tslSelectAll.Text = "全选";
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(6, 27);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 98;
            this.label5.Text = "角色名称：";
            // 
            // txtRoleName
            // 
            this.txtRoleName.Location = new System.Drawing.Point(86, 31);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(100, 21);
            this.txtRoleName.TabIndex = 99;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(378, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 101;
            this.label3.Text = "备注：";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(413, 12);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(314, 72);
            this.txtRemark.TabIndex = 102;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(89, 17);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 109;
            this.btnUpdate.Text = "修    改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(197, 17);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 109;
            this.btnAdd.Text = "添   加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(303, 72);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(41, 12);
            this.lblId.TabIndex = 110;
            this.lblId.Text = "label1";
            this.lblId.Visible = false;
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(305, 17);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 109;
            this.btnCancle.Text = "清   空";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(192, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 112;
            this.label1.Text = "用户状态：";
            // 
            // comboxState
            // 
            this.comboxState.FormattingEnabled = true;
            this.comboxState.Items.AddRange(new object[] {
            "启动",
            "暂停",
            "注销"});
            this.comboxState.Location = new System.Drawing.Point(251, 31);
            this.comboxState.Name = "comboxState";
            this.comboxState.Size = new System.Drawing.Size(93, 20);
            this.comboxState.TabIndex = 113;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboxState);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblId);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtRoleName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(772, 97);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnDel);
            this.groupBox4.Controls.Add(this.btnCancle);
            this.groupBox4.Controls.Add(this.btnUpdate);
            this.groupBox4.Controls.Add(this.btnAdd);
            this.groupBox4.Location = new System.Drawing.Point(0, 98);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(772, 47);
            this.groupBox4.TabIndex = 102;
            this.groupBox4.TabStop = false;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(413, 17);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 110;
            this.btnDel.Text = "删   除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // UserRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 526);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "UserRole";
            this.Text = "角色权限管理";
            this.Load += new System.EventHandler(this.UserRole_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lvwUserList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchName;
        private System.Windows.Forms.DataGridView lvwUserList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboxState;
        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripLabel tslSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 流水号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role_Remark;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnDel;
    }
}