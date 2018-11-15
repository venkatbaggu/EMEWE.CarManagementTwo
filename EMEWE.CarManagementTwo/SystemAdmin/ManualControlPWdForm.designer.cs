namespace EMEWE.CarManagement.SystemAdmin
{
    partial class ManualControlPWdForm
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
            this.btnControlPwd = new System.Windows.Forms.Button();
            this.txtCurrentControlPWD = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnControlPwd
            // 
            this.btnControlPwd.Location = new System.Drawing.Point(109, 117);
            this.btnControlPwd.Name = "btnControlPwd";
            this.btnControlPwd.Size = new System.Drawing.Size(75, 23);
            this.btnControlPwd.TabIndex = 0;
            this.btnControlPwd.Text = "登录";
            this.btnControlPwd.UseVisualStyleBackColor = true;
          //  this.btnControlPwd.Click += new System.EventHandler(this.btnControlPwd_Click);
            // 
            // txtCurrentControlPWD
            // 
            this.txtCurrentControlPWD.Location = new System.Drawing.Point(125, 48);
            this.txtCurrentControlPWD.Name = "txtCurrentControlPWD";
            this.txtCurrentControlPWD.Size = new System.Drawing.Size(118, 21);
            this.txtCurrentControlPWD.TabIndex = 1;
            this.txtCurrentControlPWD.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "手动控制密码：";
            // 
            // ManualControlPWdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 213);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCurrentControlPWD);
            this.Controls.Add(this.btnControlPwd);
            this.Name = "ManualControlPWdForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "手动开闸管理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnControlPwd;
        private System.Windows.Forms.TextBox txtCurrentControlPWD;
        private System.Windows.Forms.Label label1;
    }
}