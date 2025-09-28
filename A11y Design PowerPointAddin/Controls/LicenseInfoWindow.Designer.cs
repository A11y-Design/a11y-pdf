
using A11y_Design_PowerPointAddin.Properties;
using System.Globalization;

namespace A11y_Design_PowerPointAddin.Controls
{
    partial class LicenseInfoWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseInfoWindow));
            this.labelVersionNr = new System.Windows.Forms.Label();
            this.textVersionNr = new System.Windows.Forms.Label();
            this.lblCopyrightCompany = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelVersionNr
            // 
            this.labelVersionNr.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelVersionNr.AutoSize = true;
            this.labelVersionNr.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelVersionNr.Location = new System.Drawing.Point(29, 31);
            this.labelVersionNr.Name = "labelVersionNr";
            this.labelVersionNr.Size = new System.Drawing.Size(135, 20);
            this.labelVersionNr.TabIndex = 13;
            this.labelVersionNr.Text = "Versionnumber";
            // 
            // textVersionNr
            // 
            this.textVersionNr.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textVersionNr.AutoSize = true;
            this.textVersionNr.Location = new System.Drawing.Point(39, 51);
            this.textVersionNr.Name = "textVersionNr";
            this.textVersionNr.Size = new System.Drawing.Size(125, 15);
            this.textVersionNr.TabIndex = 14;
            this.textVersionNr.Text = "<< Versionnumber >>";
            // 
            // lblCopyrightCompany
            // 
            this.lblCopyrightCompany.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCopyrightCompany.AutoSize = true;
            this.lblCopyrightCompany.Location = new System.Drawing.Point(39, 108);
            this.lblCopyrightCompany.Name = "lblCopyrightCompany";
            this.lblCopyrightCompany.Size = new System.Drawing.Size(157, 15);
            this.lblCopyrightCompany.TabIndex = 16;
            this.lblCopyrightCompany.Text = "© 2025 a11y Design GmbH";
            // 
            // lblCopyright
            // 
            this.lblCopyright.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblCopyright.Location = new System.Drawing.Point(29, 82);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(89, 20);
            this.lblCopyright.TabIndex = 15;
            this.lblCopyright.Text = "Copyright";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::A11y_Design_PowerPointAddin.Properties.Resources.a11y_design_250x91;
            this.pictureBox1.Location = new System.Drawing.Point(239, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 107);
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(194, 156);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(43, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "www.a11y-design.de";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // LicenseInfoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 191);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblCopyrightCompany);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.textVersionNr);
            this.Controls.Add(this.labelVersionNr);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseInfoWindow";
            this.Text = "License information";
            this.Load += new System.EventHandler(this.LicenseInfoWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelVersionNr;
        private System.Windows.Forms.Label textVersionNr;
        private System.Windows.Forms.Label lblCopyrightCompany;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
    }
}