namespace EMEWE.CarManagement.SystemAdmin
{
    partial class ControlInfoForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tv_ControlInfo = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_ManagementStrategy = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_Empty = new System.Windows.Forms.Button();
            this.btn_Preservation = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboxType = new System.Windows.Forms.ComboBox();
            this.cob_ControlInfo_State = new System.Windows.Forms.ComboBox();
            this.txt_ControlInfo_IDValue = new System.Windows.Forms.TextBox();
            this.txt_ControlInfo_Value = new System.Windows.Forms.TextBox();
            this.txt_ControlInfo_Remark = new System.Windows.Forms.TextBox();
            this.txt_ControlInfo_Content = new System.Windows.Forms.TextBox();
            this.txt_ControlInfo_Rule = new System.Windows.Forms.TextBox();
            this.txt_ControlInfo_HeightName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_ControlInfo_Name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lb_CarTypeAttribute = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tv_ControlInfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 611);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "管控信息:";
            // 
            // tv_ControlInfo
            // 
            this.tv_ControlInfo.AllowDrop = true;
            this.tv_ControlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_ControlInfo.Location = new System.Drawing.Point(3, 17);
            this.tv_ControlInfo.Name = "tv_ControlInfo";
            this.tv_ControlInfo.Size = new System.Drawing.Size(256, 591);
            this.tv_ControlInfo.TabIndex = 0;
            this.tv_ControlInfo.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tv_ControlInfo_NodeMouseDoubleClick);
            this.tv_ControlInfo.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tv_ControlInfo_BeforeExpand);
            this.tv_ControlInfo.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tv_ControlInfo_BeforeCollapse);
            this.tv_ControlInfo.DragDrop += new System.Windows.Forms.DragEventHandler(this.ControlInfoForm_DragDrop);
            this.tv_ControlInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tv_ControlInfo_MouseDown);
            this.tv_ControlInfo.DragEnter += new System.Windows.Forms.DragEventHandler(this.TV_MenuInfo_DragEnter);
            this.tv_ControlInfo.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tv_ControlInfo_NodeMouseClick);
            this.tv_ControlInfo.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TV_MenuInfo_ItemDrag);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_ManagementStrategy);
            this.groupBox2.Controls.Add(this.btn_delete);
            this.groupBox2.Controls.Add(this.btn_Empty);
            this.groupBox2.Controls.Add(this.btn_Preservation);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(262, 368);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(730, 243);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // btn_ManagementStrategy
            // 
            this.btn_ManagementStrategy.Location = new System.Drawing.Point(586, 63);
            this.btn_ManagementStrategy.Name = "btn_ManagementStrategy";
            this.btn_ManagementStrategy.Size = new System.Drawing.Size(75, 23);
            this.btn_ManagementStrategy.TabIndex = 4;
            this.btn_ManagementStrategy.Text = "管控策略";
            this.btn_ManagementStrategy.UseVisualStyleBackColor = true;
            this.btn_ManagementStrategy.Click += new System.EventHandler(this.btn_ManagementStrategy_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(247, 62);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(75, 23);
            this.btn_delete.TabIndex = 5;
            this.btn_delete.Text = "删  除";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_Empty
            // 
            this.btn_Empty.Location = new System.Drawing.Point(142, 62);
            this.btn_Empty.Name = "btn_Empty";
            this.btn_Empty.Size = new System.Drawing.Size(75, 23);
            this.btn_Empty.TabIndex = 3;
            this.btn_Empty.Text = "清  空";
            this.btn_Empty.UseVisualStyleBackColor = true;
            this.btn_Empty.Click += new System.EventHandler(this.btn_Empty_Click);
            // 
            // btn_Preservation
            // 
            this.btn_Preservation.Location = new System.Drawing.Point(42, 63);
            this.btn_Preservation.Name = "btn_Preservation";
            this.btn_Preservation.Size = new System.Drawing.Size(75, 23);
            this.btn_Preservation.TabIndex = 2;
            this.btn_Preservation.Text = "保  存";
            this.btn_Preservation.UseVisualStyleBackColor = true;
            this.btn_Preservation.Click += new System.EventHandler(this.btn_Preservation_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboxType);
            this.groupBox3.Controls.Add(this.cob_ControlInfo_State);
            this.groupBox3.Controls.Add(this.txt_ControlInfo_IDValue);
            this.groupBox3.Controls.Add(this.txt_ControlInfo_Value);
            this.groupBox3.Controls.Add(this.txt_ControlInfo_Remark);
            this.groupBox3.Controls.Add(this.txt_ControlInfo_Content);
            this.groupBox3.Controls.Add(this.txt_ControlInfo_Rule);
            this.groupBox3.Controls.Add(this.txt_ControlInfo_HeightName);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txt_ControlInfo_Name);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(262, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(730, 368);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "管控基础信息：";
            // 
            // comboxType
            // 
            this.comboxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxType.FormattingEnabled = true;
            this.comboxType.Location = new System.Drawing.Point(492, 108);
            this.comboxType.Name = "comboxType";
            this.comboxType.Size = new System.Drawing.Size(172, 20);
            this.comboxType.TabIndex = 16;
            // 
            // cob_ControlInfo_State
            // 
            this.cob_ControlInfo_State.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cob_ControlInfo_State.FormattingEnabled = true;
            this.cob_ControlInfo_State.Location = new System.Drawing.Point(115, 144);
            this.cob_ControlInfo_State.Name = "cob_ControlInfo_State";
            this.cob_ControlInfo_State.Size = new System.Drawing.Size(187, 20);
            this.cob_ControlInfo_State.TabIndex = 16;
            // 
            // txt_ControlInfo_IDValue
            // 
            this.txt_ControlInfo_IDValue.Location = new System.Drawing.Point(492, 27);
            this.txt_ControlInfo_IDValue.Name = "txt_ControlInfo_IDValue";
            this.txt_ControlInfo_IDValue.Size = new System.Drawing.Size(172, 21);
            this.txt_ControlInfo_IDValue.TabIndex = 11;
            this.txt_ControlInfo_IDValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_ControlInfo_IDValue_KeyPress);
            // 
            // txt_ControlInfo_Value
            // 
            this.txt_ControlInfo_Value.Location = new System.Drawing.Point(492, 67);
            this.txt_ControlInfo_Value.Name = "txt_ControlInfo_Value";
            this.txt_ControlInfo_Value.Size = new System.Drawing.Size(172, 21);
            this.txt_ControlInfo_Value.TabIndex = 11;
            this.txt_ControlInfo_Value.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_ControlInfo_Value_KeyPress);
            // 
            // txt_ControlInfo_Remark
            // 
            this.txt_ControlInfo_Remark.Location = new System.Drawing.Point(115, 253);
            this.txt_ControlInfo_Remark.Multiline = true;
            this.txt_ControlInfo_Remark.Name = "txt_ControlInfo_Remark";
            this.txt_ControlInfo_Remark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_ControlInfo_Remark.Size = new System.Drawing.Size(549, 55);
            this.txt_ControlInfo_Remark.TabIndex = 10;
            // 
            // txt_ControlInfo_Content
            // 
            this.txt_ControlInfo_Content.Location = new System.Drawing.Point(115, 185);
            this.txt_ControlInfo_Content.Multiline = true;
            this.txt_ControlInfo_Content.Name = "txt_ControlInfo_Content";
            this.txt_ControlInfo_Content.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_ControlInfo_Content.Size = new System.Drawing.Size(549, 55);
            this.txt_ControlInfo_Content.TabIndex = 15;
            // 
            // txt_ControlInfo_Rule
            // 
            this.txt_ControlInfo_Rule.Location = new System.Drawing.Point(115, 108);
            this.txt_ControlInfo_Rule.Name = "txt_ControlInfo_Rule";
            this.txt_ControlInfo_Rule.ReadOnly = true;
            this.txt_ControlInfo_Rule.Size = new System.Drawing.Size(188, 21);
            this.txt_ControlInfo_Rule.TabIndex = 14;
            this.txt_ControlInfo_Rule.DoubleClick += new System.EventHandler(this.txt_ControlInfo_Rule_DoubleClick);
            // 
            // txt_ControlInfo_HeightName
            // 
            this.txt_ControlInfo_HeightName.Location = new System.Drawing.Point(114, 23);
            this.txt_ControlInfo_HeightName.Name = "txt_ControlInfo_HeightName";
            this.txt_ControlInfo_HeightName.ReadOnly = true;
            this.txt_ControlInfo_HeightName.Size = new System.Drawing.Size(188, 21);
            this.txt_ControlInfo_HeightName.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(430, 111);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 5;
            this.label9.Text = "管控类型：";
            // 
            // txt_ControlInfo_Name
            // 
            this.txt_ControlInfo_Name.Location = new System.Drawing.Point(115, 63);
            this.txt_ControlInfo_Name.Name = "txt_ControlInfo_Name";
            this.txt_ControlInfo_Name.Size = new System.Drawing.Size(188, 21);
            this.txt_ControlInfo_Name.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "管控信息状态：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(394, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "管控信息实际值：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(394, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "管控信息默认值：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "管控信息备注：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "管控信息描述:：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "管控信息默认规则：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "管控信息上级名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "管控信息名称：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lb_CarTypeAttribute);
            this.panel1.Location = new System.Drawing.Point(564, 108);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 244);
            this.panel1.TabIndex = 9;
            this.panel1.Visible = false;
            // 
            // lb_CarTypeAttribute
            // 
            this.lb_CarTypeAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_CarTypeAttribute.FormattingEnabled = true;
            this.lb_CarTypeAttribute.ItemHeight = 12;
            this.lb_CarTypeAttribute.Location = new System.Drawing.Point(0, 0);
            this.lb_CarTypeAttribute.Name = "lb_CarTypeAttribute";
            this.lb_CarTypeAttribute.ScrollAlwaysVisible = true;
            this.lb_CarTypeAttribute.Size = new System.Drawing.Size(245, 244);
            this.lb_CarTypeAttribute.TabIndex = 3;
            this.lb_CarTypeAttribute.DoubleClick += new System.EventHandler(this.lb_CarTypeAttribute_DoubleClick);
           
            // 
            // ControlInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 611);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "ControlInfoForm";
            this.Text = "管控信息";
            this.Load += new System.EventHandler(this.ControlInfoForm1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ControlInfoForm_DragDrop);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView tv_ControlInfo;
        private System.Windows.Forms.ComboBox cob_ControlInfo_State;
        private System.Windows.Forms.TextBox txt_ControlInfo_Value;
        private System.Windows.Forms.TextBox txt_ControlInfo_Remark;
        private System.Windows.Forms.TextBox txt_ControlInfo_Content;
        private System.Windows.Forms.TextBox txt_ControlInfo_Rule;
        private System.Windows.Forms.TextBox txt_ControlInfo_Name;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_ManagementStrategy;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_Empty;
        private System.Windows.Forms.Button btn_Preservation;
        private System.Windows.Forms.TextBox txt_ControlInfo_HeightName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_ControlInfo_IDValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lb_CarTypeAttribute;
        private System.Windows.Forms.ComboBox comboxType;
        private System.Windows.Forms.Label label9;
    }
}