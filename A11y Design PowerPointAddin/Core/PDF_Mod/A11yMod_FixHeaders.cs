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
    class A11yMod_FixHeaders : IA11yModification
    {
        private int _slideCount = 0;
        private int _highest_header = 0;        

        /// <summary>
        ///  Fix Headers of SLides
        /// </summary>      
        public A11yMod_FixHeaders()
        {          

        }
        public  bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            //find first page

            if (treePointer.GetRole().ToLower() == "sect")
            {
                _slideCount++;
            }
            // can be optimized if one heading is found on slide there should not be another
            // make h2 to h1 on first slide or if there has been no h1 before
            if (treePointer.GetRole().ToUpper() == "H2")
            {
                if (_slideCount == 1 || _highest_header == 0)
                {
                    treePointer.SetRole("H1");
                    _highest_header = 1;                    
                }
                else
                    _highest_header = 2;

            } else if (treePointer.GetRole().ToUpper() == "H1" && _highest_header != 0) {
                treePointer.SetRole("H2");
                _highest_header = 2;
            }
                     


            if (treePointer.GetRole().ToUpper() == "H1" && _highest_header < 1)
            {
                _highest_header = 1;
            }
            return true;
        }

        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
        }

        public void Visit(Slide slide)
        {
            if (_slideCount > 0)
            {
                _slideCount = 0;
                _highest_header = 0;
            }
        }

        public void Visit(Slide slide, Shape shape)
        {
        }
    }
}
