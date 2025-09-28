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
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using System.Diagnostics;
using A11y_Design_PowerPointAddin.Core.PDF_Mod;

namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// A class that creates an plugin infrastructure for registered IA11yModification s.
    /// Modification-plugins are able to collect or modify the Powerpoint copy as well as the generated PDF.
    /// </summary>
    public class A11yModificationHost
    {

        #region Adding new Mods
        public A11yModificationHost()
        {


            // Register mods here that are executed without interruption bevore all other Modifications
            RegisteredExclusiveModifiers = new List<IA11yModification>()
            {                
                new A11yMod_ConvertVector(),
                new A11yMod_TextInFigureFix(),
                // new A11yMod_HyperlinkPPT(), // empty Quickinfo will get URL as alternative text
                new A11yMod_TableFigureFiX(), // some table are exported as figure
                new A11yMod_TabOrderFix(),
                new A11yMod_PictureFix(), 
                new A11yMod_LinkFixPlacement(),
            };
            // Register mods here that can be executed sequentially per item
            RegisteredModifiers = new List<IA11yModification>()
            {
                new A11yMod_TableFix(),
                new A11yMod_TagMasterArtifacts(),
                new A11yMod_SlideBookmarks(),
                new A11yMod_Hyperlink(),
                new A11yMod_FixHeaders(),
                new A11yMod_FixDoubleListItem(),
                new A11yMod_PictureFix(), 
                //multiple calls are needed to remove all empty figure
                new A11yMod_PdfCleanup(),
            };
        }

        #endregion
        #region Implementation

        /// <summary>
        /// IA11yModifications that need to be executed without interference of other IA11yModifications
        /// </summary>
        private readonly List<IA11yModification> RegisteredExclusiveModifiers;
        /// <summary>
        /// IA11yModification that can be executed with the interference of other IA11yModification
        /// </summary>
        private readonly List<IA11yModification> RegisteredModifiers;



        /// <summary>
        /// Interates over the presentation, collects data and makes modifications as provided by the registered modificators
        /// </summary>
        public void IteratePPT(PowerPoint.Presentation presentation)
        {
            //some changes need to be done completely before others, therefore a seperate interation in needed
            foreach (var item in RegisteredExclusiveModifiers)
                foreach (PowerPoint.Slide slide in presentation.Slides)
                {
                    item.Visit(slide);
                    foreach (PowerPoint.Shape shape in slide.Shapes)
                        item.Visit(slide, shape);
                }

            //iteration with the other modificators
            foreach (PowerPoint.Slide slide in presentation.Slides)
            {
                foreach (var item in RegisteredModifiers)
                    item.Visit(slide);

                foreach (PowerPoint.Shape shape in slide.Shapes)
                {
                    foreach (var item in RegisteredModifiers)
                        item.Visit(slide, shape);
                }

            }
        }

        private PdfDocument PdfDocument { get; set; }

        public void IteratePDF(PdfDocument pdfDoc)
        {
            PdfDocument = pdfDoc;
            //creating this pointer somehow creates the Document root tag. Do not delete!
            TagTreePointer treePointer = new TagTreePointer(pdfDoc);

            //get root node?
            if (treePointer.GetRole().Equals("Sect") || (treePointer.GetRole().Equals("Slide") && pdfDoc.GetNumberOfPages() == 1))
            {
                Debug.WriteLine("Document root");
                treePointer.SetRole("Document");
            }

            // modify root
            foreach (var modifier in RegisteredExclusiveModifiers)
            {
                modifier.ModifyPDFRoot(pdfDoc);
            }
            foreach (var modifier in RegisteredModifiers)
            {
                modifier.ModifyPDFRoot(pdfDoc);
            }

            //recursively modify tags

            ExamineTag("", null, treePointer, RegisteredExclusiveModifiers);
            ExamineTag("", null, treePointer, RegisteredModifiers);

            PdfDocument = null;

        }

        /// <summary>
        /// Recursive method that examines each tag in the tag tree
        /// Traverses the tag tree with DFS
        /// Calls the methods to fix the special tags 
        /// </summary>
        private void ExamineTag(string space, PdfPage page, TagTreePointer treePointer, List<IA11yModification> mods)
        {
            bool isroot = treePointer.GetRole().Equals("Document");

            TagTreePointer parent = !isroot ? new TagTreePointer(treePointer).MoveToParent() : null;

            // show this node to our mods
            //
            foreach (var mod in mods)
            {
                if (mod.ModifyPDFNode(page, treePointer))
                {
                    //pointer was not intended to be deleted
                    if (!isroot && treePointer.IsPointingToSameTag(parent))
                    {
                        // pointer was deleted anyway
                        throw new A11yModificationException($"A11y modification problem: {mod.GetType().Name} is not returning original TagTreePointer as indicated");
                    }
                    // else everything as planned
                }
                else
                {
                    //pointer was intended to be deleted
                    if (!isroot && treePointer.IsPointingToSameTag(parent))
                    {
                        //pointer was deleted - fine
                        return;
                    }
                    else
                    {
                        //pointer was not deleted - problem
                        throw new A11yModificationException($"A11y modification problem: {mod.GetType().Name} is not returning parent TagTreePointer as indicated");

                    }
                }
            }

            // visit kids ...
            //
            for (int i = 0; i < treePointer.GetKidsCount(); i++)
            {
                // skip kid if role is empty or a Markup content
                var kid_role = treePointer.GetKidsRoles()[i];                
                if (kid_role == null || kid_role.Equals("MCR"))
                    continue;

                // if the child is not a Markup content, examine it
                treePointer.MoveToKid(i);
                if (isroot && PdfDocument != null && PdfDocument.GetNumberOfPages() > i + 1) // if root, then pass page                    
                    ExamineTag(space + " ", PdfDocument.GetPage(i + 1), treePointer, mods);
                else
                {
                    PdfDictionary elem = treePointer.GetPdfStructureElem().GetPdfObject();
                    if (elem.GetAsDictionary(PdfName.Pg) != null) page = treePointer.GetDocument().GetPage(elem.GetAsDictionary(PdfName.Pg));
                    ExamineTag(space + " ", page, treePointer, mods);
                }
            }

            // move back to the parent, if the parent is root finish
            if (!isroot)
            {
                try
                {
                    treePointer.MoveToParent();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception: " + e);
                }
            }



        }
        #endregion
    }
}
