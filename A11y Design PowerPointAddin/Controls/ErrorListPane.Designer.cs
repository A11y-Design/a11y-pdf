using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A11y_Design_PowerPointAddin.Controls
{
    partial class ErrorListPane
    {

        private void InitializeComponent()
        {
            this.splitContainer_Incidents = new System.Windows.Forms.SplitContainer();
            this.errorsAccordeon = new A11y_Design_PowerPointAddin.Controls.Accordeon();
            this.noErrorsLabel = new System.Windows.Forms.Label();
            this.errorsAccordeonHeadline = new System.Windows.Forms.Label();
            this.hintsAccordeon = new A11y_Design_PowerPointAddin.Controls.Accordeon();
            this.noHintsLabel = new System.Windows.Forms.Label();
            this.hintsAccordeonHeadline = new System.Windows.Forms.Label();
            this.refreshButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Incidents)).BeginInit();
            this.splitContainer_Incidents.Panel1.SuspendLayout();
            this.splitContainer_Incidents.Panel2.SuspendLayout();
            this.splitContainer_Incidents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_Incidents
            // 
            this.splitContainer_Incidents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Incidents.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Incidents.Name = "splitContainer_Incidents";
            this.splitContainer_Incidents.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Incidents.Panel1
            // 
            this.splitContainer_Incidents.Panel1.Controls.Add(this.errorsAccordeon);
            this.splitContainer_Incidents.Panel1.Controls.Add(this.noErrorsLabel);
            this.splitContainer_Incidents.Panel1.Controls.Add(this.errorsAccordeonHeadline);
            // 
            // splitContainer_Incidents.Panel2
            // 
            this.splitContainer_Incidents.Panel2.Controls.Add(this.hintsAccordeon);
            this.splitContainer_Incidents.Panel2.Controls.Add(this.noHintsLabel);
            this.splitContainer_Incidents.Panel2.Controls.Add(this.hintsAccordeonHeadline);
            this.splitContainer_Incidents.Size = new System.Drawing.Size(546, 362);
            this.splitContainer_Incidents.SplitterDistance = 141;
            this.splitContainer_Incidents.TabIndex = 0;
            // 
            // errorsAccordeon
            // 
            this.errorsAccordeon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsAccordeon.Location = new System.Drawing.Point(0, 32);
            this.errorsAccordeon.Name = "errorsAccordeon";
            this.errorsAccordeon.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.errorsAccordeon.Size = new System.Drawing.Size(546, 109);
            this.errorsAccordeon.TabIndex = 0;
            // 
            // noErrorsLabel
            // 
            this.noErrorsLabel.AutoSize = true;
            this.noErrorsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.noErrorsLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noErrorsLabel.Location = new System.Drawing.Point(0, 17);
            this.noErrorsLabel.Name = "noErrorsLabel";
            this.noErrorsLabel.Size = new System.Drawing.Size(291, 15);
            this.noErrorsLabel.TabIndex = 2;
            this.noErrorsLabel.Text = "There are no accessilbility issues in your document.";
            // 
            // errorsAccordeonHeadline
            // 
            this.errorsAccordeonHeadline.AutoSize = true;
            this.errorsAccordeonHeadline.Dock = System.Windows.Forms.DockStyle.Top;
            this.errorsAccordeonHeadline.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorsAccordeonHeadline.Location = new System.Drawing.Point(0, 0);
            this.errorsAccordeonHeadline.Name = "errorsAccordeonHeadline";
            this.errorsAccordeonHeadline.Size = new System.Drawing.Size(68, 17);
            this.errorsAccordeonHeadline.TabIndex = 1;
            this.errorsAccordeonHeadline.Text = "Error List";
            // 
            // hintsAccordeon
            // 
            this.hintsAccordeon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hintsAccordeon.Location = new System.Drawing.Point(0, 32);
            this.hintsAccordeon.Name = "hintsAccordeon";
            this.hintsAccordeon.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.hintsAccordeon.Size = new System.Drawing.Size(546, 185);
            this.hintsAccordeon.TabIndex = 0;
            // 
            // noHintsLabel
            // 
            this.noHintsLabel.AutoSize = true;
            this.noHintsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.noHintsLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noHintsLabel.Location = new System.Drawing.Point(0, 17);
            this.noHintsLabel.Name = "noHintsLabel";
            this.noHintsLabel.Size = new System.Drawing.Size(194, 15);
            this.noHintsLabel.TabIndex = 2;
            this.noHintsLabel.Text = "There are no more hints available.";
            // 
            // hintsAccordeonHeadline
            // 
            this.hintsAccordeonHeadline.AutoSize = true;
            this.hintsAccordeonHeadline.Dock = System.Windows.Forms.DockStyle.Top;
            this.hintsAccordeonHeadline.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hintsAccordeonHeadline.Location = new System.Drawing.Point(0, 0);
            this.hintsAccordeonHeadline.Name = "hintsAccordeonHeadline";
            this.hintsAccordeonHeadline.Size = new System.Drawing.Size(60, 17);
            this.hintsAccordeonHeadline.TabIndex = 1;
            this.hintsAccordeonHeadline.Text = "Hint List";
            // 
            // refreshButton
            // 
            this.refreshButton.AutoSize = true;
            this.refreshButton.BackColor = System.Drawing.Color.White;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.ForeColor = System.Drawing.Color.Black;
            this.refreshButton.Location = new System.Drawing.Point(0, 0);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(129, 28);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = global::A11y_Design_PowerPointAddin.Properties.Resources.ReloadErrorList;
            this.refreshButton.UseVisualStyleBackColor = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(7, 30);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.refreshButton);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer_Incidents);
            this.splitContainer1.Size = new System.Drawing.Size(546, 416);
            this.splitContainer1.TabIndex = 0;
            // 
            // ErrorListPane
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "ErrorListPane";
            this.Padding = new System.Windows.Forms.Padding(7, 30, 7, 6);
            this.Size = new System.Drawing.Size(560, 452);
            this.splitContainer_Incidents.Panel1.ResumeLayout(false);
            this.splitContainer_Incidents.Panel1.PerformLayout();
            this.splitContainer_Incidents.Panel2.ResumeLayout(false);
            this.splitContainer_Incidents.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Incidents)).EndInit();
            this.splitContainer_Incidents.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private SplitContainer splitContainer_Incidents;
        private Accordeon errorsAccordeon;
        private Accordeon hintsAccordeon;
        private Label errorsAccordeonHeadline;
        private Label hintsAccordeonHeadline;
        private Label noErrorsLabel;
        private Label noHintsLabel;
        private Button refreshButton;
        private SplitContainer splitContainer1;
    }
}
