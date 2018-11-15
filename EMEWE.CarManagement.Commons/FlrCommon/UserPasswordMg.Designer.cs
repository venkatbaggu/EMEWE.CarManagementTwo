namespace EMEWE.CarManagement.Commons.FlrCommon
{
    partial class UserPasswordMg
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
            this.btnEditPwd = new System.Windows.Forms.Button();
            this.txtOlePwd = new System.Windows.Forms.TextBox();
            this.txtPwd1 = new System.Windows.Forms.TextBox();
            this.txtpwd2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEditPwd
            // 
            this.btnEditPwd.Location = new System.Drawing.Point(86, 168);
            this.btnEditPwd.Name = "btnEditPwd";
            this.btnEditPwd.Size = new System.Drawing.Size(75, 23);
            this.btnEditPwd.TabIndex = 0;
            this.btnEditPwd.Text = "确认修改";
            this.btnEditPwd.UseVisualStyleBackColor = true;
            this.btnEditPwd.Click += new System.EventHandler(this.btnEditPwd_Click);
            // 
            // txtOlePwd
            // 
            this.txtOlePwd.Location = new System.Drawing.Point(97, 25);
            this.txtOlePwd.Name = "txtOlePwd";
            this.txtOlePwd.PasswordChar = '*';
            this.txtOlePwd.Size = new System.Drawing.Size(100, 21);
            this.txtOlePwd.TabIndex = 1;
            // 
            // txtPwd1
            // 
            this.txtPwd1.Location = new System.Drawing.Point(97, 74);
            this.txtPwd1.Name = "txtPwd1";
            this.txtPwd1.PasswordChar = '*';
            this.txtPwd1.Size = new System.Drawing.Size(100, 21);
            this.txtPwd1.TabIndex = 2;
            // 
            // txtpwd2
            // 
            this.txtpwd2.Location = new System.Drawing.Point(97, 123);
            this.txtpwd2.Name = "txtpwd2";
            this.txtpwd2.PasswordChar = '*';
            this.txtpwd2.Size = new System.Drawing.Size(100, 21);
            this.txtpwd2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "旧密码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "新密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "确认新密码";
            // 
            // UserPasswordMg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 213);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtpwd2);
            this.Controls.Add(this.txtPwd1);
            this.Controls.Add(this.txtOlePwd);
            this.Controls.Add(this.btnEditPwd);
            this.Name = "UserPasswordMg";
            this.Text = "密码修改";
            this.Load += new System.EventHandler(this.UserPasswordMg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEditPwd;
        private System.Windows.Forms.TextBox txtOlePwd;
        private System.Windows.Forms.TextBox txtPwd1;
        private System.Windows.Forms.TextBox txtpwd2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}