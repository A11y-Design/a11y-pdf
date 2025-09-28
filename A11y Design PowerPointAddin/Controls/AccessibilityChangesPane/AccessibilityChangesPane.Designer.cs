
namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    partial class AccessibilityChangesPane
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccessibilityChangesPane));
            this.tabControlRevision = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);

            this.tabControlRevision.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlRevision
            // 
            this.tabControlRevision.Controls.Add(this.tabPage1);
            this.tabControlRevision.Controls.Add(this.tabPage2);
            this.tabControlRevision.Controls.Add(this.tabPage3);
            this.tabControlRevision.Controls.Add(this.tabPage4);
            this.tabControlRevision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlRevision.ImageList = this.imageList1;
            this.tabControlRevision.Location = new System.Drawing.Point(5, 20);
            this.tabControlRevision.Margin = new System.Windows.Forms.Padding(10);
            this.tabControlRevision.MinimumSize = new System.Drawing.Size(200, 50);
            this.tabControlRevision.Name = "tabControlRevision";
            this.tabControlRevision.Padding = new System.Drawing.Point(10, 5);
            this.tabControlRevision.SelectedIndex = 0;
            this.tabControlRevision.Size = new System.Drawing.Size(592, 575);
            this.tabControlRevision.TabIndex = 5;
            this.tabControlRevision.Click += TabControlRevision_Click;

            // 
            // tabPage1
            // 
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = AccessibilityChangesPaneTabs.METADATA.ToString();
            this.tabPage1.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage1.Size = new System.Drawing.Size(584, 544);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Metadaten";
            this.tabPage1.UseVisualStyleBackColor = true;

            // 
            // tabPage2
            // 
            this.tabPage2.ImageIndex = 2;
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = AccessibilityChangesPaneTabs.READINGORDER.ToString();
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(584, 544);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Lesereihenfolge";
            this.tabPage2.UseVisualStyleBackColor = true;

            // 
            // tabPage3
            // 
            this.tabPage3.ImageIndex = 1;
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = AccessibilityChangesPaneTabs.ALTERNATVETEXT.ToString();
            this.tabPage3.Size = new System.Drawing.Size(584, 544);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Alternativtexte";
            this.tabPage3.UseVisualStyleBackColor = true;

            // 
            // tabPage4
            // 
            this.tabPage4.ImageIndex = 0;
            this.tabPage4.Location = new System.Drawing.Point(4, 27);
            this.tabPage4.Name = AccessibilityChangesPaneTabs.HINTS.ToString();
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(584, 544);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Hinweise";
            this.tabPage4.UseVisualStyleBackColor = true;

            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ShowDetailsPage.png");
            this.imageList1.Images.SetKeyName(1, "ControlImage.png");
            this.imageList1.Images.SetKeyName(2, "TabOrder.png");


            // 
            // AccessibilityChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlRevision);
            this.Name = "AccessibilityChanges";
            this.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.Size = new System.Drawing.Size(602, 600);
            this.tabControlRevision.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void TabControlRevision_Click(object sender, System.EventArgs e)
        {
           if(tabControlRevision.SelectedTab == tabPage1)
            Controller.AppController.Instance.AccessibilityChanges.SetMetaData();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControlRevision;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ImageList imageList1;
    }
}
