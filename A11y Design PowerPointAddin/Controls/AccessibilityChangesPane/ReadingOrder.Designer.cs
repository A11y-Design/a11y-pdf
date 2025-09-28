
namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    partial class ReadingOrder
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
            this.readingOrderPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelPanel = new System.Windows.Forms.TableLayoutPanel();
            this.readingOrderButton = new System.Windows.Forms.Button();
            this.ButtonPannel = new System.Windows.Forms.Panel();
            this.hintInfoBox = new A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane.InfoBox();
            this.ButtonPannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // readingOrderPanel
            // 
            this.readingOrderPanel.AllowDrop = true;
            this.readingOrderPanel.AutoScroll = true;
            this.readingOrderPanel.AutoSize = true;
            this.readingOrderPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.readingOrderPanel.ColumnCount = 1;
            this.readingOrderPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.readingOrderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.readingOrderPanel.Location = new System.Drawing.Point(7, 76);
            this.readingOrderPanel.Margin = new System.Windows.Forms.Padding(4, 17, 4, 17);
            this.readingOrderPanel.Name = "readingOrderPanel";
            this.readingOrderPanel.Padding = new System.Windows.Forms.Padding(0, 14, 347, 0);
            this.readingOrderPanel.Size = new System.Drawing.Size(761, 14);
            this.readingOrderPanel.TabIndex = 2;
            // 
            // labelPanel
            // 
            this.labelPanel.AllowDrop = true;
            this.labelPanel.AutoScroll = true;
            this.labelPanel.AutoSize = true;
            this.labelPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.labelPanel.ColumnCount = 1;
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.labelPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPanel.Location = new System.Drawing.Point(0, 76);
            this.labelPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelPanel.Name = "labelPanel";
            this.labelPanel.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.labelPanel.Size = new System.Drawing.Size(7, 388);
            this.labelPanel.TabIndex = 3;
            // 
            // readingOrderButton
            // 
            this.readingOrderButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.readingOrderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.readingOrderButton.Location = new System.Drawing.Point(0, 0);
            this.readingOrderButton.Margin = new System.Windows.Forms.Padding(20, 6, 3, 2);
            this.readingOrderButton.Name = "readingOrderButton";
            this.readingOrderButton.Size = new System.Drawing.Size(219, 37);
            this.readingOrderButton.TabIndex = 4;
            this.readingOrderButton.Text = "readingOrderButton";
            this.readingOrderButton.UseVisualStyleBackColor = true;
            this.readingOrderButton.Click += new System.EventHandler(this.readingOrderButton_Click);
            // 
            // ButtonPannel
            // 
            this.ButtonPannel.Controls.Add(this.readingOrderButton);
            this.ButtonPannel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ButtonPannel.Location = new System.Drawing.Point(7, 90);
            this.ButtonPannel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonPannel.MaximumSize = new System.Drawing.Size(219, 49);
            this.ButtonPannel.Name = "ButtonPannel";
            this.ButtonPannel.Size = new System.Drawing.Size(219, 37);
            this.ButtonPannel.TabIndex = 5;
            // 
            // hintInfoBox
            // 
            this.hintInfoBox.AccessibleDescription = "Information Box";
            this.hintInfoBox.AutoSize = true;
            this.hintInfoBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.hintInfoBox.Location = new System.Drawing.Point(0, 0);
            this.hintInfoBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hintInfoBox.Name = "hintInfoBox";
            this.hintInfoBox.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.hintInfoBox.Size = new System.Drawing.Size(768, 76);
            this.hintInfoBox.TabIndex = 0;
            // 
            // ReadingOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ButtonPannel);
            this.Controls.Add(this.readingOrderPanel);
            this.Controls.Add(this.labelPanel);
            this.Controls.Add(this.hintInfoBox);
            this.Margin = new System.Windows.Forms.Padding(13, 4, 4, 4);
            this.Name = "ReadingOrder";
            this.Size = new System.Drawing.Size(768, 464);
            this.ButtonPannel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private InfoBox hintInfoBox;
        private System.Windows.Forms.TableLayoutPanel readingOrderPanel;
        private System.Windows.Forms.TableLayoutPanel labelPanel;
        private System.Windows.Forms.Button readingOrderButton;
        private System.Windows.Forms.Panel ButtonPannel;
    }
}
