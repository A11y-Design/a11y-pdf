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

using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace A11y_Design_PowerPointAddin.Core.PDF_Mod
{
    class A11yMod_FixUntaggedPath
    {
        /// <summary>
        /// Finds untagged paths and tag them as artifact
        /// </summary>
        /// <param name="inputPdfPath"></param>
        public static void FixUntaggedPath(String inputPdfPath)
        {
            string pdfTemp = inputPdfPath.Replace(".pdf", "_copy.pdf");            
            File.Copy(inputPdfPath, pdfTemp, true);                        
            PdfReader reader = new PdfReader(pdfTemp);
            PdfWriter writer = new PdfWriter(inputPdfPath);
            PdfDocument pdfDoc = new PdfDocument(reader, writer);
            // Process each page
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                PdfPage page = pdfDoc.GetPage(i);                
                // Process the content using a custom listener
                PdfCanvasProcessor processor = new PdfCanvasProcessor(new CustomContentListener(page));
                processor.ProcessPageContent(page);
            }
            pdfDoc.Close();
            File.Delete(pdfTemp);
        }

    }


    // Custom content listener to find and tag untagged graphical content
    public class CustomContentListener : IEventListener
    {
        private bool insideText = false; // Track whether we're inside text content
        private bool insidePath = false; // Track if inside a graphical path
        private PdfPage page;
        private PdfDocument pdfDoc;

        public CustomContentListener(PdfPage page)
        {
            this.page = page;
            this.pdfDoc = page.GetDocument();
        }

        public void EventOccurred(IEventData data, EventType type)
        {
            switch (type)
            {
                case EventType.BEGIN_TEXT:
                    insideText = true;
                    break;

                case EventType.END_TEXT:
                    insideText = false;
                    break;

                case EventType.RENDER_PATH:
                    if (!insideText)
                    {
                        insidePath = true;
                        Console.WriteLine("Found untagged graphical content (RENDER_PATH).");
                        // Here you can process or flag the path as non-textual content
                        PathRenderInfo info = (PathRenderInfo)data;
                        var infos = info.GetCanvasTagHierarchy();
                        var contentStreamAsText = new PdfString(pdfDoc.GetPage(page.GetPdfObject()).GetContentBytes()).ToString();
                        if (infos.Count == 0) break;
                        if (infos[0].GetRole().ToString() != PdfName.Artifact.ToString())
                        {
                            int mcid = infos[0].GetMcid();
                            contentStreamAsText = new PdfString(pdfDoc.GetPage(page.GetPdfObject()).GetContentBytes()).ToString();
                            string pattern = @"(/P\s*<</MCID\s" + mcid + @".*BDC)([\s\S]*?EMC)";

                            string replacement = @"" + PdfName.Artifact + " BMC $2";
                            Match match = Regex.Match(contentStreamAsText, pattern);
                            string patternTextObject = @"BT[\s\S]*?ET";
                            string patternImgObject = @"/(Image|Im)\d+\s+Do"; // look for /ImageXX Do and /ImXX Do
                            Match matchTextObject = Regex.Match(match.Value, patternTextObject);
                            Match matchImageObject = Regex.Match(match.Value, patternImgObject);
                            if (match.Success && !matchTextObject.Success
                                && !match.Value.Contains("/Meta")  //Meta draws an image
                                && !matchImageObject.Success)
                            {
                                bool firstMatch = true;

                                // Replace only the first occurrence
                                string result = Regex.Replace(contentStreamAsText, pattern, match1 =>
                                {
                                    if (firstMatch)
                                    {
                                        firstMatch = false;
                                        return match1.Result(replacement);  // Replace using the captured group
                                    }
                                    return match1.Value;  // Return original value for subsequent matches
                                });

                                // save streamBeforeReplace to txt-files for debugging
                                page.GetFirstContentStream().SetData(new PdfString(result).GetValueBytes());
                            }
                        }                       
                    }
                    insidePath = false;
                    break;
                case EventType.CLIP_PATH_CHANGED:
                    break;
                case EventType.RENDER_IMAGE:                    
                    break;

                default:
                    break;
            }
        }

        // Function to handle tagging untagged graphical content as non-textual
        private void MarkAsNonTextual(PathRenderInfo infos)
        {
            Console.WriteLine("Graphical content marked as non-textual.");
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            // We are interested in text, image, and path events
            return new List<EventType>
        {
            EventType.BEGIN_TEXT,
            EventType.END_TEXT,
            EventType.RENDER_PATH,
            EventType.RENDER_IMAGE
        };
        }
    }
}
