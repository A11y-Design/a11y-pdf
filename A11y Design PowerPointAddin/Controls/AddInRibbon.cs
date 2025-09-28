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

using A11y_Design_PowerPointAddin.Controls;
using A11y_Design_PowerPointAddin.Helper;
using A11y_Design_PowerPointAddin.Properties;
using Microsoft.Office.Tools.Ribbon;
using System.Reflection;

namespace A11y_Design_PowerPointAddin
{
    public partial class AddInRibbon
    {        
        private static LicenseInfoWindow licenseInfoWindow;

        private void AddInRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            //set language
            grpCreatePdf.Label = Resources.GroupCreatePdf;
            grpA11yPdf.Label = Resources.GroupA11yPdf;
            grpTools.Label = Resources.GroupTools;

            CustomizeGuiElement.SetButtonImage(btnCheckDocument, "DataValidation");
            CustomizeGuiElement.SetButtonImage(btnAbout, "Info");
            CustomizeGuiElement.SetI18nText(btnAbout);            
                        
            btnMetadata.Label = global::A11y_Design_PowerPointAddin.Properties.Resources.btnMetadata;
            CustomizeGuiElement.SetButtonImage(btnMetadata, "ShowDetailsPage");
            btnReadingOrder.Label = global::A11y_Design_PowerPointAddin.Properties.Resources.btnReadingOrder;
            CustomizeGuiElement.SetButtonImage(btnReadingOrder, "TabOrder");     
            btnAlternativeTexts.Label = global::A11y_Design_PowerPointAddin.Properties.Resources.btnAlternativeTexts;
            CustomizeGuiElement.SetButtonImage(btnAlternativeTexts, "ControlImage");
            btnSendBugReport.Label = global::A11y_Design_PowerPointAddin.Properties.Resources.btnSendBugReport;
            btnCheckDocument.Label = global::A11y_Design_PowerPointAddin.Properties.Resources.btnCheckDocument;            
            btnPdfExport.Label = global::A11y_Design_PowerPointAddin.Properties.Resources.btnPdfExport;
            CustomizeGuiElement.SetButtonImage(btnPdfExport, "FileSaveAsPdfOrXps");
            btnDocumentHints.Label = global::A11y_Design_PowerPointAddin.Properties.Resources.btnDocumentHints;
            CustomizeGuiElement.SetButtonImage(btnDocumentHints, "LookUp");
#if !DEBUG //hide button Reset Marked Artifacts Custom Document Properties in Release
            btnCustomDocumentProperties.Visible = false;
            btnSendBugReport.Visible = false;
            cbShowShapeTypes.Visible = false;
#endif
        }

        private void btnOpenErrorListGui_Click(object sender, RibbonControlEventArgs e)
        {
            Controller.ErrorTaskPaneControlller.TogglePane();
        }

        private void btnMetadata_Click(object sender, RibbonControlEventArgs e)
        {
            Controller.AppController.Instance.AccessibilityChanges.SetMetaData();
            Controller.ChangePaneController.SetTab(Controls.AccessibilityChangesPane.AccessibilityChangesPaneTabs.METADATA);
        }

        private void btnDocumentHints_Click(object sender, RibbonControlEventArgs e)
        {
            Controller.ChangePaneController.SetTab(Controls.AccessibilityChangesPane.AccessibilityChangesPaneTabs.HINTS);
        }

        private void btnReadingOrder_Click(object sender, RibbonControlEventArgs e)
        {
            Controller.ChangePaneController.SetTab(Controls.AccessibilityChangesPane.AccessibilityChangesPaneTabs.READINGORDER);
        }

        private void btnAlternativeTexts_Click(object sender, RibbonControlEventArgs e)
        {
            Controller.ChangePaneController.SetTab(Controls.AccessibilityChangesPane.AccessibilityChangesPaneTabs.ALTERNATVETEXT);
        }

        private void btnAbout_Click(object sender, RibbonControlEventArgs e)
        {
            licenseInfoWindow = LicenseInfoWindow.GetInstance();
            licenseInfoWindow.Show();
        }

        private void btnPdfExport_Click(object sender, RibbonControlEventArgs e)
        {
            Core.PDFExporter exporter = new Core.PDFExporter();
            exporter.ExportClicked();
        }

        private void cbShowShapeTypes_Click(object sender, RibbonControlEventArgs e)
        {
            ShapeExtensions.ShowShapeType(cbShowShapeTypes.Checked);
        }

        public void EnableButtons(bool enable)
        {
            btnPdfExport.Enabled = enable;
            btnCheckDocument.Enabled = enable;
            btnAlternativeTexts.Enabled = enable;
            btnReadingOrder.Enabled = enable;
            btnMetadata.Enabled = enable;            
            btnAbout.Enabled = enable;            
            btnDocumentHints.Enabled = enable;
            cbShowShapeTypes.Enabled = enable;
        }

        private void btnSendBugReport_Click(object sender, RibbonControlEventArgs e)
        {            
            string stack = "";
            BugReport bugReport = new BugReport();
            bugReport.btnEmail_Click(stack);
        }

        private void btnCustomDocumentProperties_Click(object sender, RibbonControlEventArgs e)
        {
            Helper.Artifact.DeleteAllArtifactsFromDocumentProperties();
        }
    }
}
