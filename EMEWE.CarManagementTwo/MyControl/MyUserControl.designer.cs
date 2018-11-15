namespace EMEWE.CarManagement.MyControl
{
    partial class MyUserControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.txtJSYName = new System.Windows.Forms.TextBox();
            this.btnAddJSY = new System.Windows.Forms.Button();
            this.listboxStaff_Names = new System.Windows.Forms.ListBox();
            this.chkListBoxs = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtJSYName
            // 
            this.txtJSYName.Location = new System.Drawing.Point(6, 17);
            this.txtJSYName.Name = "txtJSYName";
            this.txtJSYName.Size = new System.Drawing.Size(121, 21);
            this.txtJSYName.TabIndex = 0;
            this.txtJSYName.Click += new System.EventHandler(this.txtJSYName_Click);
            this.txtJSYName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtJSYName_KeyUp);
            // 
            // btnAddJSY
            // 
            this.btnAddJSY.Location = new System.Drawing.Point(138, 16);
            this.btnAddJSY.Name = "btnAddJSY";
            this.btnAddJSY.Size = new System.Drawing.Size(70, 23);
            this.btnAddJSY.TabIndex = 1;
            this.btnAddJSY.Text = "新增";
            this.btnAddJSY.UseVisualStyleBackColor = true;
            this.btnAddJSY.Click += new System.EventHandler(this.btnAddJSY_Click);
            // 
            // listboxStaff_Names
            // 
            this.listboxStaff_Names.FormattingEnabled = true;
            this.listboxStaff_Names.ItemHeight = 12;
            this.listboxStaff_Names.Location = new System.Drawing.Point(6, 38);
            this.listboxStaff_Names.Name = "listboxStaff_Names";
            this.listboxStaff_Names.Size = new System.Drawing.Size(209, 208);
            this.listboxStaff_Names.TabIndex = 2;
            this.listboxStaff_Names.Visible = false;
            this.listboxStaff_Names.DoubleClick += new System.EventHandler(this.listboxStaff_Name_DoubleClick);
            // 
            // chkListBoxs
            // 
            this.chkListBoxs.CheckOnClick = true;
            this.chkListBoxs.FormattingEnabled = true;
            this.chkListBoxs.Location = new System.Drawing.Point(6, 36);
            this.chkListBoxs.Name = "chkListBoxs";
            this.chkListBoxs.Size = new System.Drawing.Size(209, 212);
            this.chkListBoxs.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkListBoxs);
            this.groupBox1.Controls.Add(this.listboxStaff_Names);
            this.groupBox1.Controls.Add(this.btnAddJSY);
            this.groupBox1.Controls.Add(this.txtJSYName);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 254);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "名称";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(82, 259);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "关闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(150, 259);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(58, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "清空";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // MyUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "MyUserControl";
            this.Size = new System.Drawing.Size(227, 286);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtJSYName;
        private System.Windows.Forms.Button btnAddJSY;
        private System.Windows.Forms.ListBox listboxStaff_Names;
        private System.Windows.Forms.CheckedListBox chkListBoxs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;



    }
}
