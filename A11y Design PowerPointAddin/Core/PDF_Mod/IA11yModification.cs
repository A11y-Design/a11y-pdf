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

namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// Interface for modifications of the presentation and PDF while exporting.
    /// A A11yModification is able to collect information and add markings durring an iteration over all slides trough the Visit-Methods.
    /// Modifications of the Exported PDF acan be done with ModifyPDFRoot(..) and ModifyPDFNode(..)
    /// </summary>
    internal interface IA11yModification
    {
        /// <summary>
        /// Oserves a slide of the presentation to be exportet
        /// </summary>
        /// <param name="slide"></param>
        void Visit(Slide slide);

        /// <summary>
        /// Observes a shape at a specific slide of the presentation to be exportet
        /// </summary>
        /// <param name="slide"></param>
        /// <param name="shape"></param>
        void Visit(Slide slide, Shape shape);

        /// <summary>
        /// Edits the PDF root-node (no further tree traversing shold be implemented here -> Use ModifyPDFNode)
        /// </summary>
        /// <param name="pdfDoc"></param>
        void ModifyPDFRoot(PdfDocument pdfDoc);

        /// <summary>
        /// Editing single Nodes in the PDF. Changes need to happen from parents to kids and not in the other direction.
        /// Do not delete the pointer multiple times. Never return pointers above the parent. Allways indicate the removal of a pointer.
        /// </summary>
        /// <param name="page">current pdf page, can be null</param>
        /// <param name="treePointer"></param>
        /// <returns>if node is still the same (correct indications are mandatory)</returns>
        bool ModifyPDFNode(PdfPage page,TagTreePointer treePointer);


    }
}