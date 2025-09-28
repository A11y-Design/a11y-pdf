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
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Tagutils;
using Microsoft.Office.Interop.PowerPoint;


namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// Collects information about the Slides like Title and Number of Master 
    /// </summary>
    class A11yMod_SlideBookmarks : IA11yModification
    {
        private List<string> SlideTitels { get; set; }

        public A11yMod_SlideBookmarks()
        {
            SlideTitels = new List<string>();
        }

        public void Visit(Slide slide)
        {
            // clean up before export
            if (SlideTitels.Count > slide.SlideIndex)
                SlideTitels.Clear();

            try
            {
                if (slide.SlideShowTransition.Hidden != Microsoft.Office.Core.MsoTriState.msoTrue)
                    SlideTitels.Add(slide.Shapes.Title.TextEffect.Text);

            }
            catch (Exception e)
            {
                // is this to catch the null reference exception 
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }

        }

        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
            PdfOutline outlines = pdfDoc.GetOutlines(false);
            int positionOfOutline = 0; //necessary to add outlines in the correct order
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                //check first to avoid ArgumentOutOfRangeException 
                if (i - 1 >= SlideTitels.Count) break;
                PdfOutline ol = null;
                if (i == 1)
                {
                    if (SlideTitels[i - 1].Equals(string.Empty)) // if first slide has no title title of metadata is used
                    {
                        ol = outlines.AddOutline(Helper.MetaData.GetByKey(Helper.MetaData.Key.Title));
                    }
                    else
                    {
                        ol = outlines.AddOutline(SlideTitels[i - 1]);
                    }
                }
                else
                {
                    if (outlines.GetAllChildren().Count > 0)
                    {
                        ol = outlines.GetAllChildren()[0].AddOutline(SlideTitels[i - 1], positionOfOutline);
                        positionOfOutline++;
                    }
                }

                if (ol == null) continue;
                PdfArray array = new PdfArray();
                array.Add(pdfDoc.GetPage(i).GetPdfObject());
                array.Add(PdfName.Fit);
                try
                {
                    PdfDestination dest2 = PdfDestination.MakeDestination(array);
                    ol.AddAction(PdfAction.CreateGoTo(dest2));
                }
                catch (Exception e)
                {
                    // occurs with duplicates, as no new outline is created
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }
            }

        }



        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            return true;
        }

        public void Visit(Slide slide, Shape shape)
        {

        }
    }
}
