namespace EMEWE.CarManagement.SystemAdmin
{
    partial class ControlPassForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtpwd2 = new System.Windows.Forms.TextBox();
            this.txtPwd1 = new System.Windows.Forms.TextBox();
            this.txtOlePwd = new System.Windows.Forms.TextBox();
            this.btnEditPwd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "确认新密码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "新密码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "旧密码：";
            // 
            // txtpwd2
            // 
            this.txtpwd2.Location = new System.Drawing.Point(93, 129);
            this.txtpwd2.Name = "txtpwd2";
            this.txtpwd2.PasswordChar = '*';
            this.txtpwd2.Size = new System.Drawing.Size(100, 21);
            this.txtpwd2.TabIndex = 10;
            // 
            // txtPwd1
            // 
            this.txtPwd1.Location = new System.Drawing.Point(93, 80);
            this.txtPwd1.Name = "txtPwd1";
            this.txtPwd1.PasswordChar = '*';
            this.txtPwd1.Size = new System.Drawing.Size(100, 21);
            this.txtPwd1.TabIndex = 9;
            // 
            // txtOlePwd
            // 
            this.txtOlePwd.Location = new System.Drawing.Point(93, 31);
            this.txtOlePwd.Name = "txtOlePwd";
            this.txtOlePwd.PasswordChar = '*';
            this.txtOlePwd.Size = new System.Drawing.Size(100, 21);
            this.txtOlePwd.TabIndex = 8;
            // 
            // btnEditPwd
            // 
            this.btnEditPwd.Location = new System.Drawing.Point(82, 174);
            this.btnEditPwd.Name = "btnEditPwd";
            this.btnEditPwd.Size = new System.Drawing.Size(75, 23);
            this.btnEditPwd.TabIndex = 7;
            this.btnEditPwd.Text = "确认修改";
            this.btnEditPwd.UseVisualStyleBackColor = true;
            this.btnEditPwd.Click += new System.EventHandler(this.btnEditPwd_Click);
            // 
            // ControlPassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 232);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtpwd2);
            this.Controls.Add(this.txtPwd1);
            this.Controls.Add(this.txtOlePwd);
            this.Controls.Add(this.btnEditPwd);
            this.Name = "ControlPassForm";
            this.Text = "手动控制密码修改";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtpwd2;
        private System.Windows.Forms.TextBox txtPwd1;
        private System.Windows.Forms.TextBox txtOlePwd;
        private System.Windows.Forms.Button btnEditPwd;
    }
}