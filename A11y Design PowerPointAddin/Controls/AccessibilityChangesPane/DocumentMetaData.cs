//  a11y pdf – A customizable PDF export tool for generating PDF files 
//  that meet the PDF/UA accessibility standard.
//  Copyright (C) 2025 a11y design GmbH, see <https://www.a11y-design.de/>.
//  This file is part of a11y pdf.
//
//  a11y pdf is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.
//
//  a11y pdf is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY

using Microsoft.Office.Core;
using Powerpoint = Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    public partial class DocumentMetaData : UserControl
    {
        public Dictionary<string, string> pendingChanges = new Dictionary<string, string>();
       
        /// <summary>
        /// This is the user interface for the metadata
        /// </summary>
        public DocumentMetaData()
        { 
            InitializeComponent();
            
            lbDocumentComments.Text = Properties.Resources.lbComments;
            lbDocumentAuthor.Text = Properties.Resources.lbAuthor;           
            lblTitleOfDocument.Text = Properties.Resources.TitleOfDocument;
            lbDocumentKeywords.Text = Properties.Resources.lbKeywords;
            lbDocumentCopyright.Text = Properties.Resources.lbCopyright;
            lbDocumentCopyrightNotice.Text = Properties.Resources.lbCopyNotice;
            lbDocumentCopyrightURL.Text = Properties.Resources.lbCopyUrl;
            saveButton.Text = Properties.Resources.SaveButton;
            infoBox1.Text = Properties.Resources.MetaDataInfoBox;           

            saveButton.MouseClick += (object sender, MouseEventArgs e) =>
            {               
                saveMetadata();

            };

            saveButton.FlatAppearance.MouseOverBackColor = saveButton.BackColor;
            saveButton.BackColorChanged += (s, e) => {
                saveButton.FlatAppearance.MouseOverBackColor = saveButton.BackColor;
            };
            saveButton.MouseEnter += (s, e) =>
            {
                Color HoverColor = ColorTranslator.FromHtml("#fcc2b1");
                ((Button)s).BackColor = HoverColor;
            };
            saveButton.MouseLeave += (s, e) => { ((Button)s).BackColor = Color.White; };
        }            
        /// <summary>
        /// function for saving all changed metadata via buttonclick
        /// </summary>
        private void saveMetadata()
        {
            string owner = "";
            if (cbDocumentCopyright.SelectedIndex == -1)
            {
                cbDocumentCopyright.SelectedIndex = 0;
                owner = cbDocumentCopyright.SelectedItem.ToString();
            }
            else
            {
                owner = cbDocumentCopyright.SelectedItem.ToString();
            }
            Powerpoint.Presentation pres;
            pres = Globals.ThisAddIn.Application.ActivePresentation;
            Helper.MetaData.SetMetaData(Helper.MetaData.Key.Title.ToString(), tbDocumentTitle.Text);
            Helper.MetaData.SetMetaData(Helper.MetaData.Key.Author.ToString(), tbDocumentAuthor.Text);
            Helper.MetaData.SetMetaData(Helper.MetaData.Key.Comments.ToString(), tbDocumentComments.Text);
            Helper.MetaData.SetMetaData(Helper.MetaData.Key.Keywords.ToString(), tbDocumentKeywords.Text);
           
            modifyOwner();

            if (pres.Saved == MsoTriState.msoFalse)
            {                
                pres.Save();
            }
            Controller.AppController.Model.UpdateIncidents("metaData", 1);
        }

        /// <summary>
        /// modifies the advanced property owner to store the copyright information
        /// </summary>
        private void modifyOwner()
        {
            string owner = "";
            if (cbDocumentCopyright.SelectedIndex == -1)
            {
                cbDocumentCopyright.SelectedIndex = 0;
                owner = "Status:" + cbDocumentCopyright.SelectedItem.ToString() + " Notice:" + tbDocumentCopyright.Text + " URL:" + 
                    tbCopyrightUrl.Text;
            }
            else
            {
                owner = "Status:" + cbDocumentCopyright.SelectedItem.ToString() + " Notice:" + tbDocumentCopyright.Text + " URL:" + 
                    tbCopyrightUrl.Text;
            }
            Powerpoint.Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;

            // Access the DocumentSummaryInformation
             Microsoft.Office.Core.DocumentProperties docProperties = presentation.CustomDocumentProperties;
            
            // Add or modify the "Owner" property
            if (CustomPropertyExists(docProperties, "Owner"))
            {
                docProperties["Owner"].Value = owner;
            }
            else
            {
                docProperties.Add("Owner", false, Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString, owner);
            }            
        }
        /// <summary>
        /// Check if CustomProperty exists
        /// </summary>
        /// <param name="docProperties">Properties of the document</param>
        /// <param name="propertyName">String of the property</param>
        /// <returns></returns>
        private bool CustomPropertyExists(Microsoft.Office.Core.DocumentProperties docProperties, string propertyName)
        {
            foreach (Microsoft.Office.Core.DocumentProperty property in docProperties)
            {
                if (property.Name == propertyName)
                {
                    return true; // The custom property exists
                }
            }
            return false; // The custom property does not exist
        }

        #region Setter for metadata values in gui
        public void setDocumentTitleTextBox(string documentTitle)
        {
            tbDocumentTitle.Text = documentTitle;
        }


        public void setDocumentAuthorTextBox(string documentAuthor)
        {
            tbDocumentAuthor.Text = documentAuthor;
        }

        public void setDocumentCommentsTextBox(string documentComments)
        {
            tbDocumentComments.Text = documentComments;
        }

        public void setDocumentKeywordsTextBox(string documentKeywords)
        {
            tbDocumentKeywords.Text = documentKeywords;
        }

        public void setDocumentCopyrightTextBox()
        {
            string notice;
            Powerpoint.Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
            Microsoft.Office.Core.DocumentProperties docProperties = presentation.CustomDocumentProperties;
            if (CustomPropertyExists(docProperties,"Owner"))
            {
                notice = docProperties["Owner"].Value;
                notice = notice.Trim();
                string[] parts = notice.Split(new string[] { "Notice:","Url:", " " }, StringSplitOptions.None);
                notice = parts[2];
            }
            else
            {
                notice = "";
            }
            tbDocumentCopyright.Text = notice;
        }
        public void setDocumentCopyrightCombobox()
        {
            string status;
            Powerpoint.Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
            Microsoft.Office.Core.DocumentProperties docProperties = presentation.CustomDocumentProperties;
            if (CustomPropertyExists(docProperties, "Owner"))
            {
                status = docProperties["Owner"].Value;
                status = status.Trim();
                string[] parts = status.Split(new string[] { "Status:", "Notice:", "Url:", " " }, StringSplitOptions.None);
                status = parts[1];
            }
            else
            {
                status = "";
            }
            switch (status)
            {
                case "Unknown":
                    cbDocumentCopyright.SelectedIndex = 0; 
                    break;
                case "Copyrighted":
                    cbDocumentCopyright.SelectedIndex = 1;
                    break;
                case "Public Domain":
                    cbDocumentCopyright.SelectedIndex = 2;
                    break;
            }
            
        }
        public void setDocumentCopyrightUrlTextBox()
        {
            string url;
            Powerpoint.Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
            Microsoft.Office.Core.DocumentProperties docProperties = presentation.CustomDocumentProperties;
            if (CustomPropertyExists(docProperties, "Owner"))
            {
                url = docProperties["Owner"].Value;
                url = url.Substring(url.IndexOf("URL:")).Remove(0,4);
            }
            else
            {
                url = "";
            }
            tbCopyrightUrl.Text = url;
        }
        #endregion
        #region Event Handler        
        // Event Handler
        private void tbDocumentTitle_TextChanged_1(object sender, EventArgs e)
        {

            Helper.MetaData.SetMetaData(Helper.MetaData.Key.Title.ToString(), tbDocumentTitle.Text);
        }

        private void tbDocumentAuthor_TextChanged(object sender, EventArgs e)
        {
            Helper.MetaData.SetMetaData(Helper.MetaData.Key.Author.ToString(), tbDocumentAuthor.Text);
        }

        private void tbDocumentComments_TextChanged(object sender, EventArgs e)
        {
            Helper.MetaData.SetMetaData(Helper.MetaData.Key.Comments.ToString(), tbDocumentComments.Text);
        }

        private void tbDocumentKeywords_TextChanged(object sender, EventArgs e)
        {
            Helper.MetaData.SetMetaData(Helper.MetaData.Key.Keywords.ToString(), tbDocumentKeywords.Text);
        }      

        public Dictionary<string, string> getPendingChanges()
        {
            string url = "";
            string status = "";
            string notice = "";
            Powerpoint.Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
            Microsoft.Office.Core.DocumentProperties docProperties = presentation.CustomDocumentProperties;
            if (CustomPropertyExists(docProperties, "Owner"))
            {
                url = docProperties["Owner"].Value;
                url = url.Substring(url.IndexOf("URL:")).Remove(0, 4);
                status = docProperties["Owner"].Value;
                status = status.Trim();
                string[] partsS = status.Split(new string[] { "Status:", "Notice:", "Url:", " " }, StringSplitOptions.None);
                status = partsS[1];
                if (partsS[1] == string.Empty)
                {
                    status = ""; // empty string means no copyright or public domain
                }
                if (partsS[1].ToLower().Equals("copyrighted"))
                {
                    status = "True"; //  copyright = true
                }

                if (partsS[1].ToLower().StartsWith("public")) // public domain = no copyright
                {
                    status = "False";
                }
                notice = docProperties["Owner"].Value;
                notice = notice.Trim();
                string[] partsN = notice.Split(new string[] { "Notice:", "Url:", " " }, StringSplitOptions.None);
                notice = partsN[2];
            }
                        
            pendingChanges.Add("Marked", status);
            pendingChanges.Add("Notice", notice);
            pendingChanges.Add("WebStatement", url);
            return this.pendingChanges;
        }
        #endregion
    }
}
