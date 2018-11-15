namespace EMEWE.CarManagement.Commons.FlrCommon
{
    partial class MenuTypeManager
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
            this.gbxContext = new System.Windows.Forms.GroupBox();
            this.coboxMenuControlType = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnOperating = new System.Windows.Forms.Button();
            this.comboxMenuTypeOrder = new System.Windows.Forms.ComboBox();
            this.txtMenuTypeName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboxMenuState = new System.Windows.Forms.ComboBox();
            this.gbxContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxContext
            // 
            this.gbxContext.Controls.Add(this.coboxMenuControlType);
            this.gbxContext.Controls.Add(this.btnAdd);
            this.gbxContext.Controls.Add(this.btnOperating);
            this.gbxContext.Controls.Add(this.comboxMenuTypeOrder);
            this.gbxContext.Controls.Add(this.txtMenuTypeName);
            this.gbxContext.Controls.Add(this.label4);
            this.gbxContext.Controls.Add(this.label3);
            this.gbxContext.Controls.Add(this.label2);
            this.gbxContext.Controls.Add(this.label1);
            this.gbxContext.Controls.Add(this.comboxMenuState);
            this.gbxContext.Location = new System.Drawing.Point(3, 3);
            this.gbxContext.Name = "gbxContext";
            this.gbxContext.Size = new System.Drawing.Size(249, 201);
            this.gbxContext.TabIndex = 0;
            this.gbxContext.TabStop = false;
            this.gbxContext.Text = "修改菜单类型";
            this.gbxContext.UseCompatibleTextRendering = true;
            // 
            // coboxMenuControlType
            // 
            this.coboxMenuControlType.FormattingEnabled = true;
            this.coboxMenuControlType.Location = new System.Drawing.Point(118, 18);
            this.coboxMenuControlType.Name = "coboxMenuControlType";
            this.coboxMenuControlType.Size = new System.Drawing.Size(105, 20);
            this.coboxMenuControlType.TabIndex = 10;
            this.coboxMenuControlType.SelectionChangeCommitted += new System.EventHandler(this.coboxMenuControlType_SelectionChangeCommitted);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(131, 166);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 23);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "关闭窗体";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnOperating
            // 
            this.btnOperating.Location = new System.Drawing.Point(35, 166);
            this.btnOperating.Name = "btnOperating";
            this.btnOperating.Size = new System.Drawing.Size(75, 23);
            this.btnOperating.TabIndex = 8;
            this.btnOperating.Text = "确定修改";
            this.btnOperating.UseVisualStyleBackColor = true;
            this.btnOperating.Click += new System.EventHandler(this.btnOperating_Click);
            // 
            // comboxMenuTypeOrder
            // 
            this.comboxMenuTypeOrder.FormattingEnabled = true;
            this.comboxMenuTypeOrder.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.comboxMenuTypeOrder.Location = new System.Drawing.Point(118, 128);
            this.comboxMenuTypeOrder.Name = "comboxMenuTypeOrder";
            this.comboxMenuTypeOrder.Size = new System.Drawing.Size(105, 20);
            this.comboxMenuTypeOrder.TabIndex = 6;
            // 
            // txtMenuTypeName
            // 
            this.txtMenuTypeName.FormattingEnabled = true;
            this.txtMenuTypeName.Items.AddRange(new object[] {
            "TextBox",
            "Lable",
            "Button",
            "CheckBox"});
            this.txtMenuTypeName.Location = new System.Drawing.Point(118, 93);
            this.txtMenuTypeName.Name = "txtMenuTypeName";
            this.txtMenuTypeName.Size = new System.Drawing.Size(105, 20);
            this.txtMenuTypeName.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "菜单类型序号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "控件类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "菜单类型：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "菜单类型状态：";
            // 
            // comboxMenuState
            // 
            this.comboxMenuState.FormattingEnabled = true;
            this.comboxMenuState.Items.AddRange(new object[] {
            "启动",
            "暂停",
            "注销"});
            this.comboxMenuState.Location = new System.Drawing.Point(118, 56);
            this.comboxMenuState.Name = "comboxMenuState";
            this.comboxMenuState.Size = new System.Drawing.Size(105, 20);
            this.comboxMenuState.TabIndex = 0;
            // 
            // MenuTypeManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 209);
            this.Controls.Add(this.gbxContext);
            this.Name = "MenuTypeManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MenuInfoUpdate";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MenuInfoUpdate_Load);
            this.gbxContext.ResumeLayout(false);
            this.gbxContext.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxContext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboxMenuState;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboxMenuTypeOrder;
        private System.Windows.Forms.ComboBox txtMenuTypeName;
        private System.Windows.Forms.Button btnOperating;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox coboxMenuControlType;
    }
}