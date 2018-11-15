namespace EMEWE.CarManagement.SystemAdmin
{
    partial class PrintInfoForm
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
            this.btnsz = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDY = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnsz
            // 
            this.btnsz.Location = new System.Drawing.Point(54, 28);
            this.btnsz.Name = "btnsz";
            this.btnsz.Size = new System.Drawing.Size(75, 23);
            this.btnsz.TabIndex = 2;
            this.btnsz.Text = "打印设置";
            this.btnsz.UseVisualStyleBackColor = true;
            this.btnsz.Click += new System.EventHandler(this.btnsz_Click);
            this.btnsz.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnsz_KeyDown);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(162, 28);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "打印预览";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.button2_KeyDown);
            // 
            // btnDY
            // 
            this.btnDY.Location = new System.Drawing.Point(276, 28);
            this.btnDY.Name = "btnDY";
            this.btnDY.Size = new System.Drawing.Size(75, 23);
            this.btnDY.TabIndex = 0;
            this.btnDY.Text = "确认打印";
            this.btnDY.UseVisualStyleBackColor = true;
            this.btnDY.Click += new System.EventHandler(this.btnDY_Click);
            this.btnDY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnDY_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(32, 79);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 600);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // PrintInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 686);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnDY);
            this.Controls.Add(this.btnsz);
            this.Controls.Add(this.button2);
            this.Name = "PrintInfoForm";
            this.Text = "小票打印";
            this.Load += new System.EventHandler(this.PrintInfoForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PrintInfoForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnsz;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnDY;
        private System.Windows.Forms.Panel panel1;
    }
}