namespace JLMDSAP
{
    partial class SAPCS
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbCarType = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.txttent = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtCarNo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(12, 160);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(823, 263);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "选择";
            this.Column1.Name = "Column1";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(24, 78);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 18;
            this.label18.Text = "提示信息：";
            // 
            // cmbCarType
            // 
            this.cmbCarType.FormattingEnabled = true;
            this.cmbCarType.Items.AddRange(new object[] {
            "10",
            "20",
            "30"});
            this.cmbCarType.Location = new System.Drawing.Point(394, 30);
            this.cmbCarType.Name = "cmbCarType";
            this.cmbCarType.Size = new System.Drawing.Size(121, 20);
            this.cmbCarType.TabIndex = 17;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(626, 34);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(311, 35);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(77, 12);
            this.label19.TabIndex = 15;
            this.label19.Text = "车辆类型值：";
            // 
            // txttent
            // 
            this.txttent.Location = new System.Drawing.Point(95, 75);
            this.txttent.Multiline = true;
            this.txttent.Name = "txttent";
            this.txttent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txttent.Size = new System.Drawing.Size(420, 54);
            this.txttent.TabIndex = 14;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(36, 34);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 13;
            this.label20.Text = "车牌号：";
            // 
            // txtCarNo
            // 
            this.txtCarNo.Location = new System.Drawing.Point(95, 30);
            this.txtCarNo.Name = "txtCarNo";
            this.txtCarNo.Size = new System.Drawing.Size(100, 21);
            this.txtCarNo.TabIndex = 12;
            // 
            // SAPCS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 436);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.cmbCarType);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.txttent);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.txtCarNo);
            this.Controls.Add(this.dataGridView1);
            this.Name = "SAPCS";
            this.Text = "SAP对接测试";
            this.Load += new System.EventHandler(this.SAPCS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cmbCarType;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txttent;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtCarNo;
    }
}