namespace EMEWE.CarManagement.MyControl
{
	partial class MyUserTreeView
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnqueding = new System.Windows.Forms.Button();
            this.btncloes = new System.Windows.Forms.Button();
            this.tv_StaffInfo = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnqueding);
            this.groupBox1.Controls.Add(this.btncloes);
            this.groupBox1.Controls.Add(this.tv_StaffInfo);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 312);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "名称";
            // 
            // btnqueding
            // 
            this.btnqueding.Location = new System.Drawing.Point(26, 280);
            this.btnqueding.Name = "btnqueding";
            this.btnqueding.Size = new System.Drawing.Size(56, 23);
            this.btnqueding.TabIndex = 5;
            this.btnqueding.Text = "确定";
            this.btnqueding.UseVisualStyleBackColor = true;
            this.btnqueding.Click += new System.EventHandler(this.btnqueding_Click);
            // 
            // btncloes
            // 
            this.btncloes.Location = new System.Drawing.Point(132, 280);
            this.btncloes.Name = "btncloes";
            this.btncloes.Size = new System.Drawing.Size(56, 23);
            this.btncloes.TabIndex = 4;
            this.btncloes.Text = "关闭";
            this.btncloes.UseVisualStyleBackColor = true;
            this.btncloes.Click += new System.EventHandler(this.btncloes_Click);
            // 
            // tv_StaffInfo
            // 
            this.tv_StaffInfo.Location = new System.Drawing.Point(6, 20);
            this.tv_StaffInfo.Name = "tv_StaffInfo";
            this.tv_StaffInfo.Size = new System.Drawing.Size(213, 254);
            this.tv_StaffInfo.TabIndex = 2;
            this.tv_StaffInfo.DoubleClick += new System.EventHandler(this.tv_StaffInfo_DoubleClick);
            // 
            // MyUserTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "MyUserTreeView";
            this.Size = new System.Drawing.Size(236, 318);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnqueding;
        private System.Windows.Forms.Button btncloes;
        private System.Windows.Forms.TreeView tv_StaffInfo;
	}
}
