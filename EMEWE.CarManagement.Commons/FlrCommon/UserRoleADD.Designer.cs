namespace EMEWE.CarManagement.Commons.FlrCommon
{
    partial class UserRoleADD
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chb_SelectAll = new System.Windows.Forms.CheckBox();
            this.chb_Menu_Visible = new System.Windows.Forms.CheckBox();
            this.btnCancle = new System.Windows.Forms.Button();
            this.chb_Menu_Enabled = new System.Windows.Forms.CheckBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tv_MenuInfo = new System.Windows.Forms.TreeView();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(238, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(234, 505);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chb_SelectAll);
            this.groupBox3.Controls.Add(this.chb_Menu_Visible);
            this.groupBox3.Controls.Add(this.btnCancle);
            this.groupBox3.Controls.Add(this.chb_Menu_Enabled);
            this.groupBox3.Controls.Add(this.btnUpdate);
            this.groupBox3.Location = new System.Drawing.Point(15, 37);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(193, 239);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "权限设置";
            // 
            // chb_SelectAll
            // 
            this.chb_SelectAll.AutoSize = true;
            this.chb_SelectAll.Location = new System.Drawing.Point(17, 38);
            this.chb_SelectAll.Name = "chb_SelectAll";
            this.chb_SelectAll.Size = new System.Drawing.Size(72, 16);
            this.chb_SelectAll.TabIndex = 1;
            this.chb_SelectAll.Text = "菜单全选";
            this.chb_SelectAll.UseVisualStyleBackColor = true;
            this.chb_SelectAll.Click += new System.EventHandler(this.chb_SelectAll_Click);
            // 
            // chb_Menu_Visible
            // 
            this.chb_Menu_Visible.AutoSize = true;
            this.chb_Menu_Visible.Checked = true;
            this.chb_Menu_Visible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_Menu_Visible.Location = new System.Drawing.Point(17, 79);
            this.chb_Menu_Visible.Name = "chb_Menu_Visible";
            this.chb_Menu_Visible.Size = new System.Drawing.Size(96, 16);
            this.chb_Menu_Visible.TabIndex = 5;
            this.chb_Menu_Visible.Text = "菜单是否可见";
            this.chb_Menu_Visible.UseVisualStyleBackColor = true;
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(98, 191);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 4;
            this.btnCancle.Text = "取    消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // chb_Menu_Enabled
            // 
            this.chb_Menu_Enabled.AutoSize = true;
            this.chb_Menu_Enabled.Checked = true;
            this.chb_Menu_Enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_Menu_Enabled.Location = new System.Drawing.Point(17, 132);
            this.chb_Menu_Enabled.Name = "chb_Menu_Enabled";
            this.chb_Menu_Enabled.Size = new System.Drawing.Size(108, 16);
            this.chb_Menu_Enabled.TabIndex = 5;
            this.chb_Menu_Enabled.Text = "菜单是否可操作";
            this.chb_Menu_Enabled.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(17, 191);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "确    认";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tv_MenuInfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 505);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "菜单列表";
            // 
            // tv_MenuInfo
            // 
            this.tv_MenuInfo.CheckBoxes = true;
            this.tv_MenuInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_MenuInfo.Location = new System.Drawing.Point(3, 17);
            this.tv_MenuInfo.Name = "tv_MenuInfo";
            this.tv_MenuInfo.Size = new System.Drawing.Size(232, 485);
            this.tv_MenuInfo.TabIndex = 0;
            this.tv_MenuInfo.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_MenuInfo_AfterCheck);
            // 
            // UserRoleADD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 505);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserRoleADD";
            this.Text = "权限配置";
            this.Load += new System.EventHandler(this.UserRoleADD_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox chb_SelectAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView tv_MenuInfo;
        private System.Windows.Forms.CheckBox chb_Menu_Enabled;
        private System.Windows.Forms.CheckBox chb_Menu_Visible;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}