namespace EMEWE.CarManagement.SystemAdmin
{
    partial class PortFrom
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
            this.lbstr = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbstr
            // 
            this.lbstr.AutoSize = true;
            this.lbstr.BackColor = System.Drawing.Color.Transparent;
            this.lbstr.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbstr.Location = new System.Drawing.Point(3, 15);
            this.lbstr.Name = "lbstr";
            this.lbstr.Size = new System.Drawing.Size(0, 22);
            this.lbstr.TabIndex = 35;
            this.lbstr.Click += new System.EventHandler(this.lbstr_Click);
            // 
            // PortFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(76, 46);
            this.Controls.Add(this.lbstr);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.Name = "PortFrom";
            this.ShowInTaskbar = false;
            this.Text = "PortFrom";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PortFrom_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PortFrom_MouseUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PortFrom_MouseDown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortFrom_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PortFrom_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbstr;
    }
}