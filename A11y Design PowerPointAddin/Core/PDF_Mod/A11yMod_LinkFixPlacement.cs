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
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;


namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// Links are not correct tagged. Its contains <sect>, <span>
    /// and layout could be inline instead of block
    /// </summary>

    class A11yMod_LinkFixPlacement : IA11yModification
    {
        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            bool treePointerNotChanged = true;
            if (treePointer.GetRole().Equals("Link"))
            {
                TagTreePointer child = new TagTreePointer(treePointer);
                treePointer.MoveToParent();
                treePointerNotChanged = false;
                if (treePointer.GetRole().Equals("Sect"))
                {
                    treePointer.MoveToPointer(child);
                    ChangePlacement(treePointer, PdfName.Block);
                    treePointerNotChanged = true;
                }                
            }
            return treePointerNotChanged;
        }

        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
        }

        public void Visit(PowerPoint.Slide slide)
        {

        }

        public void Visit(PowerPoint.Slide slide, PowerPoint.Shape shape)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treePointer"></param>
        /// <param name="placement"></param>
        private void ChangePlacement(TagTreePointer treePointer, PdfName placement)
        {
            PdfDictionary newDict = new PdfDictionary();

            if (placement == PdfName.Block || placement == PdfName.Inline)
            {
                newDict.Put(PdfName.O, PdfName.Layout);
                newDict.Put(PdfName.Placement, PdfName.Block);
                PdfStructureAttributes newAttribute = new PdfStructureAttributes(newDict);
                treePointer.GetProperties().AddAttributes(newAttribute);
            }
        }
    }
}
