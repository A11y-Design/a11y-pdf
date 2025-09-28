using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    partial class DocumentMetaData
    {
        private void InitializeComponent()
        { 
            this.lblTitleOfDocument = new System.Windows.Forms.Label();
            this.lbDocumentAuthor = new System.Windows.Forms.Label();
            this.lbDocumentComments = new System.Windows.Forms.Label();
            this.tbDocumentTitle = new System.Windows.Forms.TextBox();
            this.tbDocumentAuthor = new System.Windows.Forms.TextBox();
            this.tbDocumentComments = new System.Windows.Forms.TextBox();
            this.lbDocumentKeywords = new System.Windows.Forms.Label();
            this.tbDocumentKeywords = new System.Windows.Forms.TextBox();
            this.tbDocumentCopyright = new System.Windows.Forms.TextBox();
            this.lbDocumentCopyright = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.spacing = new System.Windows.Forms.Label();
            this.cbDocumentCopyright = new System.Windows.Forms.ComboBox();
            this.tbCopyrightUrl = new System.Windows.Forms.TextBox();
            this.lbDocumentCopyrightNotice = new System.Windows.Forms.Label();
            this.lbDocumentCopyrightURL = new System.Windows.Forms.Label();
            this.infoBox1 = new A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane.InfoBox();
            this.SuspendLayout();
            // 
            // lblTitleOfDocument
            // 
            this.lblTitleOfDocument.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitleOfDocument.Location = new System.Drawing.Point(0, 61);
            this.lblTitleOfDocument.Name = "lblTitleOfDocument";
            this.lblTitleOfDocument.Padding = new System.Windows.Forms.Padding(0, 10, 0, 5);
            this.lblTitleOfDocument.Size = new System.Drawing.Size(665, 40);
            this.lblTitleOfDocument.TabIndex = 0;
            this.lblTitleOfDocument.Text = "Resources.TitleOfDocument";
            this.lblTitleOfDocument.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDocumentAuthor
            // 
            this.lbDocumentAuthor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDocumentAuthor.Location = new System.Drawing.Point(0, 121);
            this.lbDocumentAuthor.Name = "lbDocumentAuthor";
            this.lbDocumentAuthor.Padding = new System.Windows.Forms.Padding(0, 10, 0, 5);
            this.lbDocumentAuthor.Size = new System.Drawing.Size(665, 40);
            this.lbDocumentAuthor.TabIndex = 1;
            this.lbDocumentAuthor.Text = "label2";
            this.lbDocumentAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDocumentComments
            // 
            this.lbDocumentComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDocumentComments.Location = new System.Drawing.Point(0, 181);
            this.lbDocumentComments.Name = "lbDocumentComments";
            this.lbDocumentComments.Padding = new System.Windows.Forms.Padding(0, 10, 0, 5);
            this.lbDocumentComments.Size = new System.Drawing.Size(665, 40);
            this.lbDocumentComments.TabIndex = 2;
            this.lbDocumentComments.Text = "label3";
            this.lbDocumentComments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDocumentTitle
            // 
            this.tbDocumentTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbDocumentTitle.Location = new System.Drawing.Point(0, 101);
            this.tbDocumentTitle.MinimumSize = new System.Drawing.Size(0, 20);
            this.tbDocumentTitle.Name = "tbDocumentTitle";
            this.tbDocumentTitle.Size = new System.Drawing.Size(665, 20);
            this.tbDocumentTitle.TabIndex = 3;
            this.tbDocumentTitle.TextChanged += new System.EventHandler(this.tbDocumentTitle_TextChanged_1);
            // 
            // tbDocumentAuthor
            // 
            this.tbDocumentAuthor.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbDocumentAuthor.Location = new System.Drawing.Point(0, 161);
            this.tbDocumentAuthor.Name = "tbDocumentAuthor";
            this.tbDocumentAuthor.Size = new System.Drawing.Size(665, 20);
            this.tbDocumentAuthor.TabIndex = 4;
            this.tbDocumentAuthor.TextChanged += new System.EventHandler(this.tbDocumentAuthor_TextChanged);
            // 
            // tbDocumentComments
            // 
            this.tbDocumentComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbDocumentComments.Location = new System.Drawing.Point(0, 221);
            this.tbDocumentComments.Name = "tbDocumentComments";
            this.tbDocumentComments.Size = new System.Drawing.Size(665, 20);
            this.tbDocumentComments.TabIndex = 5;
            this.tbDocumentComments.TextChanged += new System.EventHandler(this.tbDocumentComments_TextChanged);
            // 
            // lbDocumentKeywords
            // 
            this.lbDocumentKeywords.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDocumentKeywords.Location = new System.Drawing.Point(0, 422);
            this.lbDocumentKeywords.Name = "lbDocumentKeywords";
            this.lbDocumentKeywords.Padding = new System.Windows.Forms.Padding(0, 10, 0, 5);
            this.lbDocumentKeywords.Size = new System.Drawing.Size(665, 40);
            this.lbDocumentKeywords.TabIndex = 6;
            this.lbDocumentKeywords.Text = "lbKeywords";
            this.lbDocumentKeywords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDocumentKeywords
            // 
            this.tbDocumentKeywords.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbDocumentKeywords.Location = new System.Drawing.Point(0, 462);
            this.tbDocumentKeywords.Name = "tbDocumentKeywords";
            this.tbDocumentKeywords.Size = new System.Drawing.Size(665, 20);
            this.tbDocumentKeywords.TabIndex = 7;
            this.tbDocumentKeywords.TextChanged += new System.EventHandler(this.tbDocumentKeywords_TextChanged);
            // 
            // tbDocumentCopyright
            // 
            this.tbDocumentCopyright.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbDocumentCopyright.Location = new System.Drawing.Point(0, 342);
            this.tbDocumentCopyright.Name = "tbDocumentCopyright";
            this.tbDocumentCopyright.Size = new System.Drawing.Size(665, 20);
            this.tbDocumentCopyright.TabIndex = 9;
            // 
            // lbDocumentCopyright
            // 
            this.lbDocumentCopyright.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDocumentCopyright.Location = new System.Drawing.Point(0, 241);
            this.lbDocumentCopyright.Name = "lbDocumentCopyright";
            this.lbDocumentCopyright.Padding = new System.Windows.Forms.Padding(0, 10, 0, 5);
            this.lbDocumentCopyright.Size = new System.Drawing.Size(665, 40);
            this.lbDocumentCopyright.TabIndex = 8;
            this.lbDocumentCopyright.Text = "Copyright Status";
            this.lbDocumentCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.White;
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.saveButton.Location = new System.Drawing.Point(545, 502);
            this.saveButton.Margin = new System.Windows.Forms.Padding(3, 15, 3, 15);
            this.saveButton.MaximumSize = new System.Drawing.Size(120, 30);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(120, 30);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "save";
            this.saveButton.UseVisualStyleBackColor = false;
            // 
            // spacing
            // 
            this.spacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.spacing.ForeColor = System.Drawing.SystemColors.Control;
            this.spacing.Location = new System.Drawing.Point(0, 482);
            this.spacing.Name = "spacing";
            this.spacing.Padding = new System.Windows.Forms.Padding(30, 0, 30, 0);
            this.spacing.Size = new System.Drawing.Size(665, 20);
            this.spacing.TabIndex = 11;
            this.spacing.Text = "label3";
            // 
            // cbDocumentCopyright
            // 
            this.cbDocumentCopyright.BackColor = System.Drawing.Color.White;
            this.cbDocumentCopyright.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbDocumentCopyright.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.cbDocumentCopyright.FormattingEnabled = true;
            this.cbDocumentCopyright.Items.AddRange(new object[] {
            "Unknown",
            "Copyrighted",
            "Public Domain"});
            this.cbDocumentCopyright.Location = new System.Drawing.Point(0, 281);
            this.cbDocumentCopyright.Margin = new System.Windows.Forms.Padding(0);
            this.cbDocumentCopyright.MaxDropDownItems = 3;
            this.cbDocumentCopyright.Name = "cbDocumentCopyright";
            this.cbDocumentCopyright.Size = new System.Drawing.Size(665, 21);
            this.cbDocumentCopyright.TabIndex = 12;
            // 
            // tbCopyrightUrl
            // 
            this.tbCopyrightUrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbCopyrightUrl.Location = new System.Drawing.Point(0, 402);
            this.tbCopyrightUrl.Name = "tbCopyrightUrl";
            this.tbCopyrightUrl.Size = new System.Drawing.Size(665, 20);
            this.tbCopyrightUrl.TabIndex = 13;
            // 
            // lbDocumentCopyrightNotice
            // 
            this.lbDocumentCopyrightNotice.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDocumentCopyrightNotice.Location = new System.Drawing.Point(0, 302);
            this.lbDocumentCopyrightNotice.Name = "lbDocumentCopyrightNotice";
            this.lbDocumentCopyrightNotice.Padding = new System.Windows.Forms.Padding(0, 10, 0, 5);
            this.lbDocumentCopyrightNotice.Size = new System.Drawing.Size(665, 40);
            this.lbDocumentCopyrightNotice.TabIndex = 14;
            this.lbDocumentCopyrightNotice.Text = "Copyright Notice";
            // 
            // lbDocumentCopyrightURL
            // 
            this.lbDocumentCopyrightURL.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDocumentCopyrightURL.Location = new System.Drawing.Point(0, 362);
            this.lbDocumentCopyrightURL.Name = "lbDocumentCopyrightURL";
            this.lbDocumentCopyrightURL.Padding = new System.Windows.Forms.Padding(0, 10, 0, 5);
            this.lbDocumentCopyrightURL.Size = new System.Drawing.Size(665, 40);
            this.lbDocumentCopyrightURL.TabIndex = 15;
            this.lbDocumentCopyrightURL.Text = "Copyright Info URL";
            // 
            // infoBox1
            // 
            this.infoBox1.AccessibleDescription = "Information Box";
            this.infoBox1.AutoSize = true;
            this.infoBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoBox1.Location = new System.Drawing.Point(0, 0);
            this.infoBox1.Name = "infoBox1";
            this.infoBox1.Padding = new System.Windows.Forms.Padding(5);
            this.infoBox1.Size = new System.Drawing.Size(665, 61);
            this.infoBox1.TabIndex = 1;
            // 
            // DocumentMetaData
            // 
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.spacing);
            this.Controls.Add(this.tbDocumentKeywords);
            this.Controls.Add(this.lbDocumentKeywords);
            this.Controls.Add(this.tbCopyrightUrl);
            this.Controls.Add(this.lbDocumentCopyrightURL);
            this.Controls.Add(this.tbDocumentCopyright);
            this.Controls.Add(this.lbDocumentCopyrightNotice);
            this.Controls.Add(this.cbDocumentCopyright);
            this.Controls.Add(this.lbDocumentCopyright);
            this.Controls.Add(this.tbDocumentComments);
            this.Controls.Add(this.lbDocumentComments);
            this.Controls.Add(this.tbDocumentAuthor);
            this.Controls.Add(this.lbDocumentAuthor);
            this.Controls.Add(this.tbDocumentTitle);
            this.Controls.Add(this.lblTitleOfDocument);
            this.Controls.Add(this.infoBox1);
            this.Name = "DocumentMetaData";
            this.Size = new System.Drawing.Size(665, 544);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label lblTitleOfDocument;
        private System.Windows.Forms.Label lbDocumentAuthor;
        private System.Windows.Forms.Label lbDocumentComments;
        private System.Windows.Forms.TextBox tbDocumentTitle;
        private System.Windows.Forms.TextBox tbDocumentAuthor;
        private System.Windows.Forms.TextBox tbDocumentComments;
        private InfoBox infoBox1;
        private System.Windows.Forms.Label lbDocumentKeywords;
        private System.Windows.Forms.TextBox tbDocumentKeywords;
        private System.Windows.Forms.TextBox tbDocumentCopyright;
        private System.Windows.Forms.Label lbDocumentCopyright;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label spacing;
        private System.Windows.Forms.ComboBox cbDocumentCopyright;
        private System.Windows.Forms.TextBox tbCopyrightUrl;
        private System.Windows.Forms.Label lbDocumentCopyrightNotice;
        private System.Windows.Forms.Label lbDocumentCopyrightURL;
    }
}

