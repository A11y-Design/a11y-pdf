using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    internal partial class HintArea
    {
        private void InitializeComponent()
        {
            this.hintInfoBox = new A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane.InfoBox();
            this.SuspendLayout();
            // 
            // hintInfoBox
            // 
            this.hintInfoBox.AccessibleDescription = "Information Box";
            this.hintInfoBox.AutoSize = true;
            this.hintInfoBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.hintInfoBox.Location = new System.Drawing.Point(0, 0);
            this.hintInfoBox.Name = "hintInfoBox";
            this.hintInfoBox.Padding = new System.Windows.Forms.Padding(5);
            this.hintInfoBox.Size = new System.Drawing.Size(150, 61);
            this.hintInfoBox.TabIndex = 0;
            // 
            // HintArea
            // 
            this.Controls.Add(this.hintInfoBox);
            this.Name = "HintArea";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private InfoBox hintInfoBox;
    }
}
