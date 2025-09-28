
using System.Globalization;
using System.Threading;
using Tools = Microsoft.Office.Tools;

namespace A11y_Design_PowerPointAddin
{
    partial class AddInRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public AddInRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            //Helper.ResourcesReader.LoadResxFile();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureInfo.CurrentUICulture.Name);
            InitializeComponent();
#if !DEBUG 
            cbShowShapeTypes.Visible = false;
            btnSendBugReport.Visible = false;
            btnCustomDocumentProperties.Visible  = false;
#endif
        }

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">"true", wenn verwaltete Ressourcen gelöscht werden sollen, andernfalls "false".</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Append Text to Ribbon Tabname
        /// </summary>
        /// <param name="text"></param>
        public void AppendTextToRibbonTabname(string text)
        {
            this.tab1.Label += text;
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.grpCreatePdf = this.Factory.CreateRibbonGroup();
            this.btnCheckDocument = this.Factory.CreateRibbonButton();
            this.btnPdfExport = this.Factory.CreateRibbonButton();
            this.grpTools = this.Factory.CreateRibbonGroup();
            this.btnMetadata = this.Factory.CreateRibbonButton();
            this.btnReadingOrder = this.Factory.CreateRibbonButton();
            this.btnAlternativeTexts = this.Factory.CreateRibbonButton();
            this.btnDocumentHints = this.Factory.CreateRibbonButton();
            this.cbShowShapeTypes = this.Factory.CreateRibbonCheckBox();
            this.btnSendBugReport = this.Factory.CreateRibbonButton();
            this.btnCustomDocumentProperties = this.Factory.CreateRibbonButton();
            this.grpA11yPdf = this.Factory.CreateRibbonGroup();
            this.btnAbout = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.grpCreatePdf.SuspendLayout();
            this.grpTools.SuspendLayout();
            this.grpA11yPdf.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.grpCreatePdf);
            this.tab1.Groups.Add(this.grpTools);
            this.tab1.Groups.Add(this.grpA11yPdf);
            this.tab1.Label = "a11y pdf";
            this.tab1.Name = "tab1";
            // 
            // grpCreatePdf
            // 
            this.grpCreatePdf.Items.Add(this.btnCheckDocument);
            this.grpCreatePdf.Items.Add(this.btnPdfExport);
            this.grpCreatePdf.Label = "Create PDF";
            this.grpCreatePdf.Name = "grpCreatePdf";
            // 
            // btnCheckDocument
            // 
            this.btnCheckDocument.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnCheckDocument.Label = "";
            this.btnCheckDocument.Name = "btnCheckDocument";
            this.btnCheckDocument.ShowImage = true;
            this.btnCheckDocument.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnOpenErrorListGui_Click);
            // 
            // btnPdfExport
            // 
            this.btnPdfExport.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnPdfExport.Label = "PDF Export";
            this.btnPdfExport.Name = "btnPdfExport";
            this.btnPdfExport.ShowImage = true;
            this.btnPdfExport.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnPdfExport_Click);
            // 
            // grpTools
            // 
            this.grpTools.Items.Add(this.btnMetadata);
            this.grpTools.Items.Add(this.btnReadingOrder);
            this.grpTools.Items.Add(this.btnAlternativeTexts);
            this.grpTools.Items.Add(this.btnDocumentHints);
            this.grpTools.Items.Add(this.cbShowShapeTypes);
            this.grpTools.Items.Add(this.btnSendBugReport);
            this.grpTools.Items.Add(this.btnCustomDocumentProperties);
            this.grpTools.Label = "Tools";
            this.grpTools.Name = "grpTools";
            // 
            // btnMetadata
            // 
            this.btnMetadata.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnMetadata.Label = "Edit Metadata";
            this.btnMetadata.Name = "btnMetadata";
            this.btnMetadata.ShowImage = true;
            this.btnMetadata.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnMetadata_Click);
            // 
            // btnReadingOrder
            // 
            this.btnReadingOrder.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnReadingOrder.Label = "Edit Reading";
            this.btnReadingOrder.Name = "btnReadingOrder";
            this.btnReadingOrder.ShowImage = true;
            this.btnReadingOrder.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnReadingOrder_Click);
            // 
            // btnAlternativeTexts
            // 
            this.btnAlternativeTexts.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAlternativeTexts.Label = "Edit Alternative Texts";
            this.btnAlternativeTexts.Name = "btnAlternativeTexts";
            this.btnAlternativeTexts.ShowImage = true;
            this.btnAlternativeTexts.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAlternativeTexts_Click);
            // 
            // btnDocumentHints
            // 
            this.btnDocumentHints.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnDocumentHints.Label = "Hints";
            this.btnDocumentHints.Name = "btnDocumentHints";
            this.btnDocumentHints.ShowImage = true;
            this.btnDocumentHints.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDocumentHints_Click);
            // 
            // cbShowShapeTypes
            // 
            this.cbShowShapeTypes.Label = "show ShapeTypes";
            this.cbShowShapeTypes.Name = "cbShowShapeTypes";
            this.cbShowShapeTypes.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.cbShowShapeTypes_Click);
            // 
            // btnSendBugReport
            // 
            this.btnSendBugReport.Label = "Send Bug Report";
            this.btnSendBugReport.Name = "btnSendBugReport";
            this.btnSendBugReport.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSendBugReport_Click);
            // 
            // btnCustomDocumentProperties
            // 
            this.btnCustomDocumentProperties.Label = "Reset Marked Artifacts Custom Document Properties";
            this.btnCustomDocumentProperties.Name = "btnCustomDocumentProperties";
            this.btnCustomDocumentProperties.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCustomDocumentProperties_Click);
            // 
            // grpA11yPdf
            // 
            this.grpA11yPdf.Items.Add(this.btnAbout);
            this.grpA11yPdf.Label = "A11y PDF";
            this.grpA11yPdf.Name = "grpA11yPdf";
            // 
            // btnAbout
            // 
            this.btnAbout.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAbout.Label = "About";
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.ScreenTip = global::A11y_Design_PowerPointAddin.Properties.Resources.AboutScreenTip;
            this.btnAbout.ShowImage = true;
            this.btnAbout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnAbout_Click);
            // 
            // AddInRibbon
            // 
            this.Name = "AddInRibbon";
            this.RibbonType = "Microsoft.PowerPoint.Presentation";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.AddInRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.grpCreatePdf.ResumeLayout(false);
            this.grpCreatePdf.PerformLayout();
            this.grpTools.ResumeLayout(false);
            this.grpTools.PerformLayout();
            this.grpA11yPdf.ResumeLayout(false);
            this.grpA11yPdf.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpCreatePdf;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCheckDocument;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAbout;
        internal Tools.Ribbon.RibbonGroup grpTools;
        internal Tools.Ribbon.RibbonGroup grpA11yPdf;
        internal Tools.Ribbon.RibbonButton btnPdfExport;
        internal Tools.Ribbon.RibbonButton btnMetadata;
        internal Tools.Ribbon.RibbonButton btnDocumentHints;
        internal Tools.Ribbon.RibbonButton btnReadingOrder;
        internal Tools.Ribbon.RibbonButton btnAlternativeTexts;
        internal Tools.Ribbon.RibbonCheckBox cbShowShapeTypes;
        internal Tools.Ribbon.RibbonButton btnSendBugReport;
        internal Tools.Ribbon.RibbonButton btnCustomDocumentProperties;
    }



    partial class ThisRibbonCollection
    {
        internal AddInRibbon AddInRibbon
        {
            get { return this.GetRibbon<AddInRibbon>(); }
        }
    }
}
