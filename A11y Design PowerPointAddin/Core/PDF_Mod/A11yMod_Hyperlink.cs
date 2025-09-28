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

using System;
using System.Collections.Generic;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using System.Text.RegularExpressions;
using System.Text;

namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// Collects ScreenTips of all Hyperlinks
    /// </summary>
    class A11yMod_Hyperlink : IA11yModification
    {
        private int index = 0;
        private int linkNumber = 0;
        private Dictionary<string, string> LinkScreenTip { get; set; }
        private Dictionary<int, int> SlideNumSlideIdDict { get; set; }
        private Dictionary<string, int> PdfPageObjNum { get; set; }

        public A11yMod_Hyperlink()
        {
            LinkScreenTip = new Dictionary<string, string>();
            PdfPageObjNum = new Dictionary<string, int>(); // [ObjNum, pageNum]
        }

        /// <summary>
        /// Creates a key based an page number and uri
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string CreateKeyForLink(int pageNumber, string uri)
        {
            return pageNumber + "_" + uri;
        }
        public void Visit(PowerPoint.Slide slide)
        {
            foreach (PowerPoint.Hyperlink hyperlink in slide.Hyperlinks)
            {
                // hyperlink.address is only http://url.de/ 
                // links to ids with # are not working http://url.de/#heading 
                // hyperlink.SubAddress; contains the id without the hash eg. heading
                try
                {
                    string linkAddress = hyperlink.Address;
                    string linkToFile = @"^(\\{2}[\w.-]+(\\[\w\s.-]+)+|(\.\.\\)+([\w\s.-]+\\)*[\w\s.-]+\.(pdf|docx|txt|md|pptx|xlsx)|[\w\s.-]+\.(pdf|docx|txt|md|pptx|xlsx))$"; //regex
                    if (hyperlink.SubAddress == null && hyperlink.Address != null
                        && Regex.IsMatch(hyperlink.Address.Trim(), linkToFile))
                    {
                        hyperlink.ScreenTip = hyperlink.TextToDisplay;
                        linkAddress = hyperlink.Address.Replace(@"\", "/");
                        linkAddress = linkAddress.Replace(@"//", "file://");
                        LinkScreenTip.Add(CreateKeyForLink(slide.SlideNumber, linkAddress), hyperlink.ScreenTip);
                    }
                    else
                    {
                        if (hyperlink.SubAddress != null && hyperlink.Address != null) // urls http://url.de/#heading  with contains id
                        {
                            hyperlink.ScreenTip = $"{hyperlink.Address}#{hyperlink.SubAddress}";  // create tooltip lhttp://url.de/#heading
                            linkAddress = hyperlink.ScreenTip;
                        }

                        if (hyperlink.SubAddress != null && hyperlink.Address == null) // internal links within PP subAddress (SlideId, ObjektID) e.g. (257,2,)
                        {
                            string pattern = @"^\d+,\d+,$"; //regex
                            if (Regex.IsMatch(hyperlink.SubAddress.Trim(), pattern))
                            {
                                hyperlink.ScreenTip = hyperlink.TextToDisplay;
                                string linkedPage = hyperlink.SubAddress.Split(',')[1];
                                linkAddress = "intern_" + linkedPage;   // is needed otherwise the key is not correct key is pageNum_intern_linkedPageNum
                            }
                        }
                        LinkScreenTip.Add(CreateKeyForLink(slide.SlideNumber, linkAddress), hyperlink.ScreenTip);
                    }

                }
                catch (Exception e)
                {

                }
            }
        }


        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            return true;
        }

        /// <summary>
        /// A method to set the alternative text of a link
        /// </summary>
        private void FixLinkTag(PdfPage page)
        {
            PdfDocument pdfDoc = page.GetDocument();
            int pageNumber = pdfDoc.GetPageNumber(page);
            var annotations = page.GetAnnotations();
            string key = string.Empty;
            foreach (PdfAnnotation annotation in annotations)
            {
                key = string.Empty;
                if (annotation.GetSubtype().Equals(PdfName.Link))
                {
                    PdfLinkAnnotation linkAnnotation = (PdfLinkAnnotation)annotation;

                    // Check if the link annotation has an action and it is a URI action
                    PdfDictionary action = linkAnnotation.GetAction();
                    if (action != null && action.Get(PdfName.S).Equals(PdfName.URI))
                    {
                        // Extract and create key
                        string uri = action.Get(PdfName.URI).ToString();
                        uri = Encoding.UTF8.GetString(Encoding.Default.GetBytes(uri));
                        key = CreateKeyForLink(pageNumber, uri);                
                     
                    }
                    else // intern Link to another page of the PDF
                    {
                        String pdfObject = linkAnnotation.GetDestinationObject().ToString();
                        Match match = Regex.Match(pdfObject, @"\d+ \d+ R Modified;");
                        if (match.Success)
                        {
                            // create key based on pageNumber an linked page
                            int linkedPage = PdfPageObjNum[match.Value];
                            key = pageNumber + "_intern_" + linkedPage;                                                       
                        }
                    }

                    if (LinkScreenTip.ContainsKey(key))
                    {
                        string toolTip = LinkScreenTip[key];
                        linkAnnotation.Put(PdfName.Contents, new PdfString(toolTip));
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("no entry for key:" + key );
                    }
                }
            }
        }

        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
            // createn dictionary with pagenum and Obj number of the page 
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                var pageRef = page.GetPdfObject().GetIndirectReference();
                PdfPageObjNum.Add(pageRef.ToString(), i);
            }
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                FixLinkTag(pdfDoc.GetPage(i));
            }

        }

        public void Visit(PowerPoint.Slide slide, PowerPoint.Shape shape)
        {
        }
    }
}

