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

using A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane;
using A11y_Design_PowerPointAddin.Core.PDF_Mod;
using A11y_Design_PowerPointAddin.Helper;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using iText.Kernel.XMP.Impl;
using iText.Kernel.XMP.Options;
using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// A class that exports the PPT presentation to PDF
    /// </summary>
    class PDFExporter 
    {
        A11yModificationHost PresentationModHost;

        private bool openPDF = false;        
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_ShapeAltText).Assembly);
        String reportMessage = resourceManager.GetString("TemplateBugReportMessage");

        public PDFExporter(){}

        /// <summary>
        /// The desctructor that deletes the copies of the presentations when PowerPoint is closed
        /// This is done because of the bug in PPT 2016 where the close methode does not release the lock on the presentation
        /// </summary>
        ~PDFExporter()
        {
            Helper.File.DeleteAllFiles(Globals.ThisAddIn.AppDataPath);           
        }

        /// <summary>
        /// Set the PDF file should be opened after the export
        /// </summary>
        public void SetOpenPDF(bool open)
        {
            this.openPDF = open;
        }

        /// <summary>
        /// The PDF export start.
        /// Displays the dialog for the Save path
        /// Continues with the export
        /// Displays the success feedback
        /// </summary>
        public void ExportClicked()
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();


            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.InitialDirectory = (Globals.ThisAddIn.Application.ActivePresentation.Path);

            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(Globals.ThisAddIn.Application.ActivePresentation.Name.Replace(" ", "_"));



            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var newPdfName = saveFileDialog1.FileName;

                // check access for PDF File
                try
                {
                    if (System.IO.File.Exists(newPdfName))
                        using (FileStream fileStream = System.IO.File.Open(newPdfName, FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { }
                }
                catch (IOException e)
                {
                    MessageBox.Show(resourceManager.GetString("ExportAbortFileIsOpened"));
                    return;
                }


                // Run Export
                bool sucess = false;
#if DEBUG
                //StartSW();
                //sucess = this.StartBuiltinGenerator(saveFileDialog1.FileName);
                //EndSW("StartBuiltinGenerator");
#endif

                try // safety for release
                {
                    StartSW();
                    sucess = this.StartBuiltinGenerator(saveFileDialog1.FileName);
                    EndSW("StartBuiltinGenerator");
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                    //sends report with StackTrace 
                    String stack = msg + e.StackTrace;
                    msg += "\n" + reportMessage;
                    //stack = stack.Replace("()", "()%0D%0A");
                    stack = Regex.Replace(stack, @"(bei)|(\sat\s)", "%0D%0A   $1$2");
                    stack = Regex.Replace(stack, @"(\sin\s)", "%0D%0A          $1");
                    if (MessageBox.Show(msg, "Interner Fehler", MessageBoxButtons.YesNo) == DialogResult.Yes) // code would be better without messageboxes here - FA                    
                    {
                        BugReport bugReport = new BugReport();
                        bugReport.btnEmail_Click(stack);
                    }
#if DEBUG
                    throw;              // only throw in Debug
#endif
                }
                if (sucess)
                {
                    string message = "Die PDF wurde erfolgreich generiert!";

                    string caption = global::A11y_Design_PowerPointAddin.Properties.Resources.AddInName;
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, caption, buttons);
                    if (openPDF)
                        System.Diagnostics.Process.Start(saveFileDialog1.FileName);
                }



            }
        }
        /// <summary>
        /// Checks if file is open by another process
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual bool isFileLocked(string fileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                using (FileStream fs = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
            }
            catch (IOException ex)
            {
                return true;
            }
            return false;
        }




        /// <summary>
        /// Creates a copy of the presentation
        /// Calls the PresentationPreparer functionality on the copy
        /// Generates a PDF with the built-in PDF generator
        /// Starts the modifications of this PDF file
        /// Finally, the copy is marked for deletion by the destructor
        /// </summary>
        /// <param name="newPdfName">Name of the PDF file</param>
        /// <returns>true is export was sucessful otherwise false</returns>
        private bool StartBuiltinGenerator(string newPdfName)
        {
            DocumentMetaData documentMeta = new DocumentMetaData();


            string presentation_filename = Path.GetFileNameWithoutExtension(Globals.ThisAddIn.Application.ActivePresentation.Name);
            string pdf_fullname_temp = System.IO.Path.Combine(Globals.ThisAddIn.AppDataPath, presentation_filename + ".pdf");
            string pdf_fullname_fix_tags = System.IO.Path.Combine(Globals.ThisAddIn.AppDataPath, presentation_filename + "fix_tags.pdf");
            string presentation_copy_fullname = System.IO.Path.Combine(Globals.ThisAddIn.AppDataPath, "copy_" + DateTime.Now.ToString().GetHashCode() + ".pptx");

            // Create and open copy of presentation
            Globals.ThisAddIn.Application.ActivePresentation.Save();
            Globals.ThisAddIn.Application.ActivePresentation.SaveCopyAs(presentation_copy_fullname, EmbedTrueTypeFonts: MsoTriState.msoTrue);

            { // Presentation presentation
                PowerPoint.Presentation presentation = Globals.ThisAddIn.Application.Presentations.Open(presentation_copy_fullname, WithWindow: MsoTriState.msoFalse);
                // Modify Powerpoint and collect information
                PresentationModHost = new A11yModificationHost();
                PresentationModHost.IteratePPT(presentation);

                // Create PDF
                object unknownType = Type.Missing;

                presentation.ExportAsFixedFormat(pdf_fullname_temp,
                    PowerPoint.PpFixedFormatType.ppFixedFormatTypePDF,
                    PowerPoint.PpFixedFormatIntent.ppFixedFormatIntentPrint,
                    Microsoft.Office.Core.MsoTriState.msoFalse, PowerPoint.PpPrintHandoutOrder.ppPrintHandoutVerticalFirst,
                    PowerPoint.PpPrintOutputType.ppPrintOutputSlides, Microsoft.Office.Core.MsoTriState.msoFalse, null,
                    PowerPoint.PpPrintRangeType.ppPrintAll, string.Empty, true, true, true,
                    false, false, unknownType);
                { //PdfDocument pdfDoc
#if LOAD_STATIC_PDF && DEBUG
                    pdf_fullname_temp = System.IO.Path.Combine("C:\\temp", "test.pdf");
                    pdf_fullname_fix_tags = pdf_debug_fix_tags;
                    MessageBox.Show("Es wird nur die PDF-Datei (Speicherort C:\\temp\\test.pdf) mit a11y pdf angepasst. \n Gespeichert wird die bearbeitete Datei im Ordner " + pdf_fullname_fix_tags);
#endif

                    PdfWriter writer = new PdfWriter(pdf_fullname_fix_tags);
                    PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdf_fullname_temp), writer);
                    // do Modifications

                    // set language
                    CultureInfo culture = new CultureInfo((int)Globals.ThisAddIn.Application.ActivePresentation.DefaultLanguageID);
                    pdfDoc.GetCatalog().SetLang(new PdfString(culture.Name));
                    pdfDoc.SetTagged();
                    string title = Helper.MetaData.GetByKey(Helper.MetaData.Key.Title);
                    pdfDoc.GetDocumentInfo().SetTitle(title);

                    if (pdfDoc.GetCatalog().GetViewerPreferences() == null)
                    {
                        PdfViewerPreferences viewerPref = new PdfViewerPreferences();
                        viewerPref.SetDisplayDocTitle(true);
                        pdfDoc.GetCatalog().SetViewerPreferences(viewerPref);
                    }
                    else
                    {
                        pdfDoc.GetCatalog().GetViewerPreferences().SetDisplayDocTitle(true);

                    }


                    // Run PDF Mods
                    PresentationModHost.IteratePDF(pdfDoc);
                    
                    pdfDoc.Close();
                 

                    #region set XMPMetadata
                    //set xmp metadata
                    PdfWriter writer1 = new PdfWriter(newPdfName, new WriterProperties().AddUAXmpMetadata().SetPdfVersion(PdfVersion.PDF_1_7));
                    PdfDocument pdfDoc1 = new PdfDocument(new PdfReader(pdf_fullname_fix_tags), writer1);
                    byte[] xmpMetadata = pdfDoc.GetXmpMetadata();
                    // in office 2016 xmpMetadata can be null                    
                    if (xmpMetadata != null)
                    {
                        // Parse the metadata                    
                        XMPMeta parser = ParseXmpMeta(xmpMetadata);


                        foreach (var change in documentMeta.getPendingChanges())
                        {
                            if (change.Key != "Notice") //Copy right status and url
                            {
                                if (change.Value != string.Empty) // empty string means no copyright or public domi
                                {
                                    parser.SetProperty(XMPConst.NS_XMP_RIGHTS, change.Key, change.Value);
                                }
                            }
                            else
                            {
                                if (change.Value != string.Empty)
                                {
                                    parser.SetLocalizedText(XMPConst.NS_DC, "rights", "", XMPConst.X_DEFAULT, change.Value);

                                }
                            }
                        }
                        pdfDoc1.SetXmpMetadata(parser);
                        pdfDoc1.GetWriter().Flush();
                    }
                    pdfDoc1.Close();
                    #endregion
                    #region fix not tagged paths
                    A11yMod_FixUntaggedPath.FixUntaggedPath(pdf_fullname_fix_tags);
                    // Fix multiple entries for authors
                    byte[] originalPdf = System.IO.File.ReadAllBytes(pdf_fullname_fix_tags);
                    byte[] updatedPdf = PdfMetaDataHelper.SetAuthorMetadata(originalPdf, presentation);
                    System.IO.File.WriteAllBytes(pdf_fullname_fix_tags, updatedPdf);                    
                    // finally set PDF/UA identifier
                    writer = new PdfWriter(newPdfName, new WriterProperties().AddUAXmpMetadata().SetPdfVersion(PdfVersion.PDF_1_7));
                    pdfDoc = new PdfDocument(new PdfReader(pdf_fullname_fix_tags), writer);
                    pdfDoc.SetTagged();
                    pdfDoc.Close();
                    #endregion                    
                }

                presentation.Save();
                presentation.Close();
#if !LOAD_STATIC_PDF
                System.IO.File.Delete(pdf_fullname_temp);
                System.IO.File.Delete(pdf_fullname_fix_tags);
#endif
                System.IO.File.Delete(presentation_copy_fullname);

            }
            Helper.File.DeleteAllFiles(Globals.ThisAddIn.AppDataPath);
            return true;
        }

       


        /// <summary>
        /// Performance Diagnostics
        /// </summary>
        Stopwatch stopWatch;

        private void StartSW()
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }

        private void EndSW(string comment = "time", bool stop = false)
        {
            // .. further performance diagnostics
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed; // Get the elapsed time as a TimeSpan value.
                                             // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Debug.WriteLine($"#######\t{comment}\t{elapsedTime}");
        }

        XMPMeta ParseXmpMeta(byte[] xmpBytes)
        {
            string xmpXml;
            XMPMeta parsedXmpMeta;
            xmpXml = Encoding.UTF8.GetString(xmpBytes);
            
            XmlDocument xmlDoc = new XmlDocument();
            xmpXml = SanitizeXml(xmpXml);
            xmlDoc.LoadXml(xmpXml);
            
            // Wenn gültig, parse mit iText
           parsedXmpMeta = XMPMetaParser.Parse(xmpXml, new ParseOptions());
            return parsedXmpMeta;
        }

      

        /// <summary>
        /// Mask unmasked letters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string SanitizeXml(string input)
        {            
            return System.Text.RegularExpressions.Regex.Replace(input, @"&(?!amp;|lt;|gt;|quot;|apos;|#x?[0-9A-Fa-f]+;)", "&amp;");
        }


    }



}