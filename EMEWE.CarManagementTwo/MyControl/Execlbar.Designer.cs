namespace EMEWE.CarManagement.MyControl
{
    partial class Execlbar
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
            this.components = new System.ComponentModel.Container();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnSet = new System.Windows.Forms.Button();
            this.lbfname = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 43);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(427, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(173, 73);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 1;
            this.btnSet.Text = "取消导出";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // lbfname
            // 
            this.lbfname.AutoSize = true;
            this.lbfname.Location = new System.Drawing.Point(24, 18);
            this.lbfname.Name = "lbfname";
            this.lbfname.Size = new System.Drawing.Size(53, 12);
            this.lbfname.TabIndex = 2;
            this.lbfname.Text = "正在导出";
            // 
            // Execlbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbfname);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.progressBar1);
            this.Name = "Execlbar";
            this.Size = new System.Drawing.Size(458, 99);
            this.Load += new System.EventHandler(this.Execlbar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Label lbfname;
        private System.Windows.Forms.Timer timer1;
    }
}
