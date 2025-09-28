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

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using Microsoft.Office.Interop.PowerPoint;

namespace A11y_Design_PowerPointAddin.Core.PDF_Mod
{
    /// <summary>
    /// Fix Boundingbox Issue for filled/bordered textboxes
    /// </summary>
    class A11yMod_BorderedEmptyTextboxFix : IA11yModification
    {
        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            return true; 
        }

        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
            // Bounding box is fixed in A11yMod_PictureFix.cs
        }

        public void Visit(Slide slide)
        {
            // bbox is fixed  in another way       
        }

        public void Visit(Slide slide, Shape shape)
        {
            
        }
    }
}
