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

using System.Collections.Generic;
using Office = Microsoft.Office;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf.Tagging;
using Microsoft.Office.Interop.PowerPoint;
using A11y_Design_PowerPointAddin.Helper;

namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// Handles decorative pictures
    /// </summary>
    class A11yMod_PictureFix : IA11yModification
    {
        public Dictionary<int, List<float>> BoundingBoxes { get; private set; }
        private int bbIndex = 0; 
        private int figureNumber = 0;

        public A11yMod_PictureFix()
        {
            BoundingBoxes = new Dictionary<int, List<float>>();
        }

        public void Visit(Slide slide, Shape shape)
        {
            if (Artifact.IsIdMarkedAsArtifact(shape))
            {
                shape.AlternativeText = "SetAsDecorative";
                return;
            }
            if (shape.AlternativeText.Contains("SetAsDecorative")) return;

            var newBB = new List<float> { shape.Left, shape.Left + shape.Width, shape.Top, shape.Top + shape.Height };
            switch (shape.GetNestedType())
            {
                case Office.Core.MsoShapeType.msoAutoShape:
                case Office.Core.MsoShapeType.msoPicture:
                    BoundingBoxes.Add(bbIndex++, newBB);
                    break;
                case Office.Core.MsoShapeType.msoMedia:
                case Office.Core.MsoShapeType.msoDiagram:
                case Office.Core.MsoShapeType.msoSmartArt:

                    if (shape.TextFrame.HasText == Office.Core.MsoTriState.msoTrue)
                        break;
                    BoundingBoxes.Add(bbIndex++, newBB);
                    break;                 
            }


        }

        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            if (treePointer.GetRole().Equals("Figure"))
            {
                return FixFigureTag(treePointer, page);
            }
            return true;
        }

        /// <summary>
        /// Method to fix the Figure tag
        /// If set as decorative element, convert to an artifact and return
        /// Compute bounding box
        /// Set missing Attributes
        /// </summary>
        public bool FixFigureTag(TagTreePointer treePointer, PdfPage page)
        {
            var altDesc = treePointer.GetProperties().GetAlternateDescription();

            if (altDesc != null) // altDesc is null if there is not text
            {
                if (altDesc.Contains("SetAsDecorative"))
                {                                
                    if (treePointer.GetRole().Equals("/Artifact"))
                    {
                        return true;
                    }
                    try
                    {
                        int id = treePointer.GetMcid(0);
                        var contentStreamAsText = new PdfString(page.GetContentBytes()).ToString();
                        contentStreamAsText = contentStreamAsText.Replace("/P <</MCID " + id + ">> BDC", "/Artifact BMC");
                        page.GetFirstContentStream().SetData(new PdfString(contentStreamAsText).GetValueBytes());
                        treePointer.GetPdfStructureElem().RemoveKid(0);
                        treePointer.RemoveTag();
                        return false;
                    }
                    catch (System.Exception ex)
                    {
                        return true;
                    }

                }
            }

            PdfDictionary newDict = new PdfDictionary();

            if (!treePointer.GetPdfStructureElem().GetPdfObject().ContainsKey(PdfName.A))
            {
                List<float> bBoxList = BoundingBoxes[figureNumber++];
                PdfArray array = new PdfArray(bBoxList.ToArray());
                newDict.Put(PdfName.BBox, array);
            }

            newDict.Put(PdfName.O, PdfName.Layout);
            newDict.Put(PdfName.Placement, PdfName.Block);
            PdfStructureAttributes newAttribute = new PdfStructureAttributes(newDict);
            treePointer.GetProperties().AddAttributes(newAttribute);
            return true;            
        }

        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
        }

        public void Visit(Slide slide)
        {
        }
    }
}
