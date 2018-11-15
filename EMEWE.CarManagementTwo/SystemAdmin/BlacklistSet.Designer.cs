namespace EMEWE.CarManagement.SystemAdmin
{
    partial class BlacklistSet
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPagex = new System.Windows.Forms.Button();
            this.btnPages = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.txtDictionary_Spare_int2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDictionary_Spare_int1 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBackContext = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tv_BackList = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnPagex);
            this.groupBox1.Controls.Add(this.btnPages);
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.txtDictionary_Spare_int2);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtDictionary_Spare_int1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtBackContext);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.tv_BackList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(539, 352);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "黑名单等级设置";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(427, 254);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 57;
            this.button4.Text = "删除";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(324, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 56;
            this.button1.Text = "修改";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnPagex
            // 
            this.btnPagex.Location = new System.Drawing.Point(175, 175);
            this.btnPagex.Name = "btnPagex";
            this.btnPagex.Size = new System.Drawing.Size(24, 25);
            this.btnPagex.TabIndex = 55;
            this.btnPagex.Text = "↓";
            this.btnPagex.UseVisualStyleBackColor = true;
            this.btnPagex.Click += new System.EventHandler(this.btnPagex_Click);
            // 
            // btnPages
            // 
            this.btnPages.Location = new System.Drawing.Point(176, 129);
            this.btnPages.Name = "btnPages";
            this.btnPages.Size = new System.Drawing.Size(24, 25);
            this.btnPages.TabIndex = 54;
            this.btnPages.Text = "↑";
            this.btnPages.UseVisualStyleBackColor = true;
            this.btnPages.Click += new System.EventHandler(this.btnPages_Click);
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(217, 254);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 53;
            this.btnSet.Text = "添加";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // txtDictionary_Spare_int2
            // 
            this.txtDictionary_Spare_int2.Location = new System.Drawing.Point(333, 89);
            this.txtDictionary_Spare_int2.MaxLength = 3;
            this.txtDictionary_Spare_int2.Name = "txtDictionary_Spare_int2";
            this.txtDictionary_Spare_int2.Size = new System.Drawing.Size(100, 21);
            this.txtDictionary_Spare_int2.TabIndex = 52;
            this.txtDictionary_Spare_int2.TextChanged += new System.EventHandler(this.txtDictionary_Spare_int2_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(249, 92);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 51;
            this.label15.Text = "每几次降级：";
            // 
            // txtDictionary_Spare_int1
            // 
            this.txtDictionary_Spare_int1.Location = new System.Drawing.Point(333, 53);
            this.txtDictionary_Spare_int1.MaxLength = 3;
            this.txtDictionary_Spare_int1.Name = "txtDictionary_Spare_int1";
            this.txtDictionary_Spare_int1.Size = new System.Drawing.Size(100, 21);
            this.txtDictionary_Spare_int1.TabIndex = 50;
            this.txtDictionary_Spare_int1.TextChanged += new System.EventHandler(this.txtDictionary_Spare_int1_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(250, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 49;
            this.label14.Text = "每几次升级：";
            // 
            // txtBackContext
            // 
            this.txtBackContext.Location = new System.Drawing.Point(333, 19);
            this.txtBackContext.Name = "txtBackContext";
            this.txtBackContext.Size = new System.Drawing.Size(100, 21);
            this.txtBackContext.TabIndex = 47;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(264, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 4;
            this.label12.Text = "等级名称：";
            // 
            // tv_BackList
            // 
            this.tv_BackList.Dock = System.Windows.Forms.DockStyle.Left;
            this.tv_BackList.ForeColor = System.Drawing.Color.Black;
            this.tv_BackList.Indent = 19;
            this.tv_BackList.ItemHeight = 15;
            this.tv_BackList.Location = new System.Drawing.Point(3, 17);
            this.tv_BackList.Name = "tv_BackList";
            this.tv_BackList.Size = new System.Drawing.Size(166, 332);
            this.tv_BackList.TabIndex = 1;
            this.tv_BackList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BackList_MouseClick);
            this.tv_BackList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tv_BackList_NodeMouseClick);
            this.tv_BackList.MouseLeave += new System.EventHandler(this.BackList_MouseLeave);
            // 
            // BlacklistSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 376);
            this.Controls.Add(this.groupBox1);
            this.Name = "BlacklistSet";
            this.Text = "黑名单等级设置";
            this.Load += new System.EventHandler(this.BlacklistSet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.TextBox txtDictionary_Spare_int2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDictionary_Spare_int1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtBackContext;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TreeView tv_BackList;
        private System.Windows.Forms.Button btnPagex;
        private System.Windows.Forms.Button btnPages;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
    }
}