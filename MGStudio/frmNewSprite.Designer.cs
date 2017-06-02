namespace MGStudio
{
    partial class frmNewSprite
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.calcEdit1 = new DevExpress.XtraEditors.CalcEdit();
            this.calcEdit2 = new DevExpress.XtraEditors.CalcEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.calcEdit3 = new DevExpress.XtraEditors.CalcEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.calcEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcEdit3.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(162, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Width:";
            // 
            // calcEdit1
            // 
            this.calcEdit1.Location = new System.Drawing.Point(200, 12);
            this.calcEdit1.Name = "calcEdit1";
            this.calcEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.calcEdit1.Properties.Mask.EditMask = "n0";
            this.calcEdit1.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.calcEdit1.Size = new System.Drawing.Size(104, 20);
            this.calcEdit1.TabIndex = 2;
            // 
            // calcEdit2
            // 
            this.calcEdit2.Location = new System.Drawing.Point(200, 38);
            this.calcEdit2.Name = "calcEdit2";
            this.calcEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.calcEdit2.Properties.Mask.EditMask = "n0";
            this.calcEdit2.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.calcEdit2.Size = new System.Drawing.Size(104, 20);
            this.calcEdit2.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(159, 41);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(35, 13);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Height:";
            // 
            // calcEdit3
            // 
            this.calcEdit3.Location = new System.Drawing.Point(23, -195);
            this.calcEdit3.Name = "calcEdit3";
            this.calcEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.calcEdit3.Properties.Mask.EditMask = "n0";
            this.calcEdit3.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.calcEdit3.Size = new System.Drawing.Size(10, 20);
            this.calcEdit3.TabIndex = 10;
            // 
            // simpleButton1
            // 
            this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton1.Image = global::MGStudio.Properties.Resources.delete_16x16;
            this.simpleButton1.Location = new System.Drawing.Point(219, 104);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(85, 23);
            this.simpleButton1.TabIndex = 12;
            this.simpleButton1.Text = "&Cancel";
            // 
            // simpleButton5
            // 
            this.simpleButton5.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.simpleButton5.Image = global::MGStudio.Properties.Resources.checkbox_16x16;
            this.simpleButton5.Location = new System.Drawing.Point(12, 104);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(85, 23);
            this.simpleButton5.TabIndex = 11;
            this.simpleButton5.Text = "&OK";
            // 
            // frmNewSprite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 139);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.calcEdit3);
            this.Controls.Add(this.calcEdit2);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.calcEdit1);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewSprite";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create a new sprite";
            this.Load += new System.EventHandler(this.frmNewSprite_Load);
            ((System.ComponentModel.ISupportInitialize)(this.calcEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcEdit3.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.CalcEdit calcEdit3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        internal DevExpress.XtraEditors.CalcEdit calcEdit1;
        internal DevExpress.XtraEditors.CalcEdit calcEdit2;
    }
}