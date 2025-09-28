using A11y_Design_PowerPointAddin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{

    partial class AlternativeText
    {
        
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.refreshPanelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imageButtonsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.isDecorativeCheckBox = new System.Windows.Forms.CheckBox();
            this.saveAltTextButton = new System.Windows.Forms.Button();
            this.alternativeTextInput = new System.Windows.Forms.TextBox();
            this.alternativeTextInputLabel = new System.Windows.Forms.Label();
            this.lbAlternativeTextInfo = new A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane.InfoBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer.Location = new System.Drawing.Point(0, 61);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.refreshPanelButton);
            this.splitContainer.Panel1.Controls.Add(this.label2);
            this.splitContainer.Panel1.Controls.Add(this.label1);
            this.splitContainer.Panel1.Controls.Add(this.imageButtonsLayoutPanel);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(15, 20, 15, 15);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer.Panel2.Controls.Add(this.alternativeTextInput);
            this.splitContainer.Panel2.Controls.Add(this.alternativeTextInputLabel);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(15, 20, 15, 15);
            this.splitContainer.Size = new System.Drawing.Size(730, 546);
            this.splitContainer.SplitterDistance = 316;
            this.splitContainer.SplitterWidth = 6;
            this.splitContainer.TabIndex = 0;
            // 
            // refreshPanelButton
            // 
            this.refreshPanelButton.BackColor = System.Drawing.Color.White;
            this.refreshPanelButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.refreshPanelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshPanelButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.refreshPanelButton.Location = new System.Drawing.Point(18, 36);
            this.refreshPanelButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.refreshPanelButton.MaximumSize = new System.Drawing.Size(120, 30);
            this.refreshPanelButton.Name = "refreshPanelButton";
            this.refreshPanelButton.Size = new System.Drawing.Size(120, 30);
            this.refreshPanelButton.TabIndex = 0;
            this.refreshPanelButton.Text = global::A11y_Design_PowerPointAddin.Properties.Resources.RefreshButton;
            this.refreshPanelButton.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(15, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(3, 495);
            this.label2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // imageButtonsLayoutPanel
            // 
            this.imageButtonsLayoutPanel.AutoScroll = true;
            this.imageButtonsLayoutPanel.AutoSize = true;
            this.imageButtonsLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imageButtonsLayoutPanel.ColumnCount = 1;
            this.imageButtonsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.imageButtonsLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.imageButtonsLayoutPanel.Location = new System.Drawing.Point(15, 20);
            this.imageButtonsLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.imageButtonsLayoutPanel.Name = "imageButtonsLayoutPanel";
            this.imageButtonsLayoutPanel.Size = new System.Drawing.Size(286, 0);
            this.imageButtonsLayoutPanel.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(15, 267);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.isDecorativeCheckBox);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.splitContainer1.Panel1MinSize = 40;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.saveAltTextButton);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.splitContainer1.Size = new System.Drawing.Size(378, 264);
            this.splitContainer1.SplitterDistance = 191;
            this.splitContainer1.TabIndex = 9;
            // 
            // isDecorativeCheckBox
            // 
            this.isDecorativeCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.isDecorativeCheckBox.Location = new System.Drawing.Point(0, 20);
            this.isDecorativeCheckBox.Margin = new System.Windows.Forms.Padding(3, 15, 3, 15);
            this.isDecorativeCheckBox.Name = "isDecorativeCheckBox";
            this.isDecorativeCheckBox.Size = new System.Drawing.Size(191, 50);
            this.isDecorativeCheckBox.TabIndex = 6;
            this.isDecorativeCheckBox.Text = global::A11y_Design_PowerPointAddin.Properties.Resources.markAsArtifact;
            this.isDecorativeCheckBox.UseVisualStyleBackColor = true;
            this.isDecorativeCheckBox.CheckedChanged += new System.EventHandler(this.isDecorativeCheckBox_CheckedChanged);
            // 
            // saveAltTextButton
            // 
            this.saveAltTextButton.AutoSize = true;
            this.saveAltTextButton.BackColor = System.Drawing.Color.White;
            this.saveAltTextButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.saveAltTextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveAltTextButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.saveAltTextButton.Location = new System.Drawing.Point(0, 20);
            this.saveAltTextButton.Margin = new System.Windows.Forms.Padding(3, 15, 3, 15);
            this.saveAltTextButton.Name = "saveAltTextButton";
            this.saveAltTextButton.Size = new System.Drawing.Size(183, 30);
            this.saveAltTextButton.TabIndex = 7;
            this.saveAltTextButton.Text = global::A11y_Design_PowerPointAddin.Properties.Resources.SaveButton;
            this.saveAltTextButton.UseVisualStyleBackColor = false;
            // 
            // alternativeTextInput
            // 
            this.alternativeTextInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.alternativeTextInput.Location = new System.Drawing.Point(15, 60);
            this.alternativeTextInput.Multiline = true;
            this.alternativeTextInput.Name = "alternativeTextInput";
            this.alternativeTextInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.alternativeTextInput.Size = new System.Drawing.Size(378, 207);
            this.alternativeTextInput.TabIndex = 5;
            this.alternativeTextInput.TextChanged += new System.EventHandler(this.AlternativeTextInput_TextChanged);
            this.alternativeTextInput.GotFocus += new System.EventHandler(this.AlternativeTextInput_GotFocus);
            this.alternativeTextInput.LostFocus += new System.EventHandler(this.AlternativeTextInput_LostFocus);
            // 
            // alternativeTextInputLabel
            // 
            this.alternativeTextInputLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.alternativeTextInputLabel.Location = new System.Drawing.Point(15, 20);
            this.alternativeTextInputLabel.Name = "alternativeTextInputLabel";
            this.alternativeTextInputLabel.Size = new System.Drawing.Size(378, 40);
            this.alternativeTextInputLabel.TabIndex = 8;
            this.alternativeTextInputLabel.Text = "Add Alternative Text:";
            // 
            // lbAlternativeTextInfo
            // 
            this.lbAlternativeTextInfo.AccessibleDescription = "Information Box";
            this.lbAlternativeTextInfo.AutoSize = true;
            this.lbAlternativeTextInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbAlternativeTextInfo.Location = new System.Drawing.Point(0, 0);
            this.lbAlternativeTextInfo.Name = "lbAlternativeTextInfo";
            this.lbAlternativeTextInfo.Padding = new System.Windows.Forms.Padding(5);
            this.lbAlternativeTextInfo.Size = new System.Drawing.Size(730, 61);
            this.lbAlternativeTextInfo.TabIndex = 1;
            // 
            // AlternativeText
            // 
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.lbAlternativeTextInfo);
            this.Name = "AlternativeText";
            this.Size = new System.Drawing.Size(730, 594);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button refreshPanelButton;
        private System.Windows.Forms.TableLayoutPanel imageButtonsLayoutPanel;
        private InfoBox lbAlternativeTextInfo;
        private SplitContainer splitContainer1;
        private CheckBox isDecorativeCheckBox;
        private Button saveAltTextButton;
        private TextBox alternativeTextInput;
        private Label alternativeTextInputLabel;
        private Label label1;
        private Label label2;
    }
}
