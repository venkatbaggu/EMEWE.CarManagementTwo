namespace EMEWE.CarManagement.SystemAdmin
{
    partial class SAPCarInfoForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboxCarType = new System.Windows.Forms.ComboBox();
            this.btnChkSAP = new System.Windows.Forms.Button();
            this.gbOne = new System.Windows.Forms.GroupBox();
            this.lvwUserList = new System.Windows.Forms.DataGridView();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.lblNumber = new System.Windows.Forms.Label();
            this.gbOne.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwUserList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "车辆类型:";
            // 
            // comboxCarType
            // 
            this.comboxCarType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxCarType.FormattingEnabled = true;
            this.comboxCarType.Items.AddRange(new object[] {
            "成品车辆",
            "送货车辆",
            "三废车辆"});
            this.comboxCarType.Location = new System.Drawing.Point(127, 21);
            this.comboxCarType.Name = "comboxCarType";
            this.comboxCarType.Size = new System.Drawing.Size(121, 20);
            this.comboxCarType.TabIndex = 2;
            this.comboxCarType.SelectedIndexChanged += new System.EventHandler(this.comboxCarType_SelectedIndexChanged);
            // 
            // btnChkSAP
            // 
            this.btnChkSAP.Location = new System.Drawing.Point(309, 55);
            this.btnChkSAP.Name = "btnChkSAP";
            this.btnChkSAP.Size = new System.Drawing.Size(84, 23);
            this.btnChkSAP.TabIndex = 4;
            this.btnChkSAP.Text = "SAP校验登记";
            this.btnChkSAP.UseVisualStyleBackColor = true;
            this.btnChkSAP.Click += new System.EventHandler(this.btnChkSAP_Click);
            this.btnChkSAP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnChkSAP_KeyDown);
            // 
            // gbOne
            // 
            this.gbOne.Controls.Add(this.lvwUserList);
            this.gbOne.Controls.Add(this.txtNumber);
            this.gbOne.Controls.Add(this.lblNumber);
            this.gbOne.Controls.Add(this.btnChkSAP);
            this.gbOne.Controls.Add(this.label1);
            this.gbOne.Controls.Add(this.comboxCarType);
            this.gbOne.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbOne.Location = new System.Drawing.Point(0, 0);
            this.gbOne.Name = "gbOne";
            this.gbOne.Size = new System.Drawing.Size(772, 445);
            this.gbOne.TabIndex = 7;
            this.gbOne.TabStop = false;
            this.gbOne.Text = "校验登记";
            // 
            // lvwUserList
            // 
            this.lvwUserList.AllowUserToAddRows = false;
            this.lvwUserList.AllowUserToDeleteRows = false;
            this.lvwUserList.AllowUserToResizeRows = false;
            this.lvwUserList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.lvwUserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvwUserList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lvwUserList.Location = new System.Drawing.Point(3, 92);
            this.lvwUserList.Name = "lvwUserList";
            this.lvwUserList.ReadOnly = true;
            this.lvwUserList.RowTemplate.Height = 23;
            this.lvwUserList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lvwUserList.Size = new System.Drawing.Size(766, 350);
            this.lvwUserList.TabIndex = 76;
            this.lvwUserList.DoubleClick += new System.EventHandler(this.lvwUserList_DoubleClick);
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(127, 55);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(121, 21);
            this.txtNumber.TabIndex = 9;
            this.txtNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNumber_KeyDown);
            // 
            // lblNumber
            // 
            this.lblNumber.AutoSize = true;
            this.lblNumber.Location = new System.Drawing.Point(47, 64);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(35, 12);
            this.lblNumber.TabIndex = 8;
            this.lblNumber.Text = "lbl1:";
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SAPCarInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 463);
            this.Controls.Add(this.gbOne);
            this.MaximizeBox = false;
            this.Name = "SAPCarInfoForm";
            this.Text = "SAP校验登记";
            this.Load += new System.EventHandler(this.SAPCarInfoForm_Load);
            this.gbOne.ResumeLayout(false);
            this.gbOne.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwUserList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboxCarType;
        private System.Windows.Forms.Button btnChkSAP;
        private System.Windows.Forms.GroupBox gbOne;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.DataGridView lvwUserList;
    }
}