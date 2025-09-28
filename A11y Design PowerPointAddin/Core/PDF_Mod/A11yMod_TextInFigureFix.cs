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
using System;
using System.Text.RegularExpressions;

namespace A11y_Design_PowerPointAddin.Core
{
    class A11yMod_TextInFigureFix : IA11yModification
    {
        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            if (treePointer.GetRole().ToLower().Equals("figure"))
            {
                
                if (treePointer.GetKidsCount() > 0 && treePointer.GetKidsRoles()[0] != "MCR")
                {
                    // find span in figure
                    for (int i = 0; i < treePointer.GetKidsCount(); i++)
                    {
                        treePointer.MoveToKid(i);
                        if (treePointer.GetKidsCount() > 0 && treePointer.GetKidsRoles()[0] == "MCR")
                        {
                            int id = treePointer.GetMcid(0);
                            var contentStreamAsText = new PdfString(page.GetContentBytes()).ToString();
                            contentStreamAsText = contentStreamAsText.Replace("/P <</MCID " + id + ">> BDC", "/Artifact BMC");
                            page.GetFirstContentStream().SetData(new PdfString(contentStreamAsText).GetValueBytes());
                            treePointer.GetPdfStructureElem().RemoveKid(0);
                            treePointer.RemoveTag();
                            break;
                        }
                    }
                    for (int i = 0; i < treePointer.GetKidsCount(); i++)
                    {
                        var kid_role = treePointer.GetKidsRoles()[i].ToLower();
                        if (kid_role.Equals("textbox") || kid_role.Equals("p"))
                        {
                            treePointer.SetRole("Sect");
                        }
                    }
                }

            }
            return true;
        }

        private int GetMcid(TagTreePointer treePointer)
        {

            try
            {
                treePointer.GetKidsRoles();
            }
            catch (SystemException ex) { return 0; }

            int max = 0;
            for (int kidIndex = 0; kidIndex < treePointer.GetKidsRoles().Count; kidIndex++)
            {
                string role = treePointer.GetKidsRoles()[kidIndex];
                if (role == null)
                    continue;
                int child_max = treePointer.GetMcid(kidIndex);

                // if node is not MCR (has kids), recurse
                if (role != "MCR")
                {
                    child_max = GetMcid(treePointer.GetKidAsTagTreePointer(kidIndex));

                }
                if (child_max > max) max = child_max;
            }
            return max;
        }

        private static string ArtifactMarker(Match match)
        {
            return match.Value.Replace("/Figure <</MCID", "/Artifact <</Type /Pagination /MCID");
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
