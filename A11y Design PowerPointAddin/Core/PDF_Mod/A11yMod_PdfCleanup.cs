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
    class A11yMod_PdfCleanup : IA11yModification
    {

        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            if (treePointer.GetRole().ToLower().Equals("document"))
            {
                return !checkForEmptyTags(treePointer);
            }
            else
            {
                return true;
            }
        }

        private bool checkForEmptyTags(TagTreePointer treePointer)
        {
            bool removedTags = false;
            for (int i = 0; i < treePointer.GetKidsCount(); i++)
            {
                if (!treePointer.GetKidsRoles()[i].ToLower().Equals("mcr"))
                {
                    checkForEmptyTags(treePointer.GetKidAsTagTreePointer(i));
                }
                removedTags = false;
            }
            if (treePointer.GetKidsCount() == 0)
            {
                treePointer.RemoveTag();
                removedTags = true;
            }

            return removedTags;
        }
        private bool _checkForEmptyTags(TagTreePointer treePointer)
        {
            bool removedEmptyTags = false;
            if (treePointer.GetKidsCount() == 0)
            {
                return false;
            }
            for (int i = 0; i < treePointer.GetKidsCount(); i++)
            {
                if (treePointer.GetKidsCount() > 1)
                {
                    removedEmptyTags = checkForEmptyTags(treePointer.MoveToKid(i));
                }
            }
            return removedEmptyTags;
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