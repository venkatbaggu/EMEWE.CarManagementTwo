namespace EMEWE.CarManagement.SystemAdmin
{
    partial class CheckInfo
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
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtPrompt
            // 
            this.txtPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPrompt.Location = new System.Drawing.Point(0, 0);
            this.txtPrompt.Multiline = true;
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrompt.Size = new System.Drawing.Size(412, 252);
            this.txtPrompt.TabIndex = 2;
            // 
            // CheckInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 253);
            this.Controls.Add(this.txtPrompt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckInfo";
            this.Text = "放车提示信息";
            this.Load += new System.EventHandler(this.CheckInfo_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CheckInfo_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPrompt;
    }
}