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
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using System.Resources;

namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// Ist not used now
    /// </summary>
    class A11yMod_HyperlinkPPT : IA11yModification
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yMod_ConvertVector).Assembly);
        public A11yMod_HyperlinkPPT() { }

        public void Visit(PowerPoint.Slide slide)
        { }

        public void Visit(PowerPoint.Slide slide, PowerPoint.Shape shape)
        {
            foreach (PowerPoint.Hyperlink link in slide.Hyperlinks) { 
                if(link.ScreenTip.Equals(String.Empty))
                {
                    if(link.Address != null) { 
                        link.ScreenTip = link.Address;
                    }
                    else if(link.SubAddress != null)
                    {
                        link.ScreenTip = resourceManager.GetString("internalLinkToSlide");
                    }
                }
            }
        }
        public bool ModifyPDFNode(TagTreePointer treePointer) => true;
        public void ModifyPDFRoot(PdfDocument pdfDoc) { }

        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            return true;
        }
    }

}
