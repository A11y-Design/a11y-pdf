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

using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using A11y_Design_PowerPointAddin.Helper;

namespace A11y_Design_PowerPointAddin.Core
{
    class A11yMod_TableFigureFiX : IA11yModification
    {
        public void Visit(PowerPoint.Slide slide)
        {

        }

        public void Visit(PowerPoint.Slide slide, PowerPoint.Shape shape)
        {
            if (shape.GetNestedType() == Office.MsoShapeType.msoTable && shape.AlternativeText.Length > 0)
            {
               shape.AlternativeText = string.Empty;
            }
        }

        public bool ModifyPDFNode(TagTreePointer treePointer) => true;
        public void ModifyPDFRoot(PdfDocument pdfDoc) { }

        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            return true;
        }

        void IA11yModification.ModifyPDFRoot(PdfDocument pdfDoc) { }

        bool IA11yModification.ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            return true;
        }
    }
}
