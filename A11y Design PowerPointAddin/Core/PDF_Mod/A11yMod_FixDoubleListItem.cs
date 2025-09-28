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

using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf;
using Microsoft.Office.Interop.PowerPoint;


namespace A11y_Design_PowerPointAddin.Core
{
    class A11yMod_FixDoubleListItem : IA11yModification
    {
        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            if(treePointer.GetRole().Equals("L")) //first <L> Tag
            {
                checkForNestedLists(treePointer);
              
            }
            return true;
                
        }
        
        private void checkForNestedLists(TagTreePointer treePointer)
        {
            if (treePointer.GetKidsCount() > 0 && treePointer.GetKidsRoles()[0].ToUpper() == "L")
            {
                treePointer.MoveToKid(0);
                treePointer.RemoveTag();
                checkForNestedLists(treePointer);
            }
        }
      

        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
        }

        public void Visit(Slide slide)
        {
        }

        public void Visit(Slide slide, Shape shape)
        {
        }
    }
}
