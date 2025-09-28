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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office;
using System.Linq;
using System.Diagnostics;


namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// Collects number of shapes in Master slides to hide them later 
    /// </summary>
    class A11yMod_TagMasterArtifacts : IA11yModification
    {
        string officeProductName = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName).ProductName;
        private List<MasterSlideInfo> masterSlideInfos;
        private bool isOffice2016 = false;
        private bool isOfficeLTSC2021 = false;
        public A11yMod_TagMasterArtifacts()
        {
            masterSlideInfos = new List<MasterSlideInfo>();
            isOffice2016 = Globals.ThisAddIn.IsOffice2016();
            isOfficeLTSC2021 = Globals.ThisAddIn.IsOfficeLTSC2021();
        }

        public void Visit(PowerPoint.Slide slide)
        {
            //only for office 2016 and LTSC
            if (isOffice2016 || isOfficeLTSC2021)
            {
                // reset collection if new export
                if (slide.SlideNumber < masterSlideInfos.Count)
                    masterSlideInfos.Clear();

                MasterSlideInfo masterInfo = new MasterSlideInfo(slide);
                masterSlideInfos.Add(masterInfo);

                // There is aproblem with images containing alternative texts in master slide 
                foreach (PowerPoint.Shape item in slide.Master.Shapes)
                    if (item.Type == Office.Core.MsoShapeType.msoPicture)
                    {
                        item.Title = "";
                        item.AlternativeText = "";
                    }

                foreach (PowerPoint.Shape item in slide.CustomLayout.Shapes)
                    if (item.Type == Office.Core.MsoShapeType.msoPicture)
                    {
                        item.Title = "";
                        item.AlternativeText = "";
                    }
            }

        }

        public void ModifyPDFRoot(iText.Kernel.Pdf.PdfDocument pdfDoc)
        {
            //only for office 2016
            if (isOffice2016) return;

            // get treepointer to work with
            var partTag = new TagTreePointer(pdfDoc);
            // Get pointer to slides because indices change when objects are removed
            List<TagTreePointer> page_tp = new List<TagTreePointer>();
            var pagesToFix = masterSlideInfos
                .Where(si => !si.Hidden && si.NeedsArtifactFixing) // filter is adopted from old tested code
                .OrderBy(si => si.PptPageIndex).ToList(); // this should not be needed .. just in case shit gets messed up, will be olay calles once per export
            for (int i = 0; i < pagesToFix.Count; i++)
            {
                int pageIndex = pagesToFix[i].PptPageIndex; // this is needed because change can be on various sides
                if (partTag.GetKidsRoles()[pageIndex - 1] != null) // check the existance of a propper role for kid -- does not fix out of bounds
                    page_tp.Add(partTag.GetKidAsTagTreePointer(pageIndex - 1)); // .. intentional out of bounds failing (not failing results in odd behaviour without error)
            }

            // interate pages by pdf index
            for (int i = 0; i < page_tp.Count; i++)
            {
                var remove_n_Tags = pagesToFix[i].MasterArtifactShapeCount;
                if (remove_n_Tags <= 0) continue;

                RemoveElementsInTree(new TagTreePointer(page_tp[i]), remove_n_Tags, out var maxmcid);

                if (maxmcid > 0)
                {
                    // get PDF-Text
                    var pdfPage = pdfDoc.GetPage(pagesToFix[i].PptPageIndex); // PptPageIndex is needed because change can be on various sides
                    var contentStreamAsText = new PdfString(pdfPage.GetContentBytes()).ToString();

                    // edit PDF-Text with regular-expression patterns allows more flexibility when replacing muiltiple parts within a context
                    var pattern = new Regex(@"\/P <<\/MCID [0-" + (maxmcid) + "]>> BDC");
                    contentStreamAsText = pattern.Replace(contentStreamAsText, ArtifactMarker);
                    pdfPage.GetFirstContentStream().SetData(new PdfString(contentStreamAsText).GetValueBytes());
                }
            }
        }
        /// <summary>
        /// Helper function to pass as match-evaluator in order to change pdf content
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string ArtifactMarker(Match match)
        {
           return match.Value.Replace(match.ToString(), "/Artifact BMC");
        }

        /// <summary>
        /// Revove tag if no childs are present
        /// </summary>
        /// <param name="treePointer"></param>
        /// <returns></returns>
        private bool DeleteTagIfNoKids(TagTreePointer treePointer)
        {
            if (treePointer.GetKidsCount() == 0)
            {
                treePointer.RemoveTag();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes first n structure elements in tree
        /// </summary>
        /// <param name="treePointer"></param>
        /// <param name="numberofElements"></param>
        /// <returns></returns>
        private bool RemoveElementsInTree(TagTreePointer treePointer, int numberofElements, out int maxmcid)
        {
            maxmcid = -1;
            try
            {
                treePointer.GetKidsRoles();
            }
            catch (System.Exception) // sometimes there is a problem with nodes that have no role, mostly if other problems have occurred before
            {
                return false;
            }


            for (int kidIndex = 0; kidIndex < numberofElements; kidIndex++)
            {
                if (0 == treePointer.GetKidsRoles().Count) break;
                try
                {
                    //fix Office 2016 issue
                    int cmax = GetMaxMcid(treePointer.GetKidAsTagTreePointer(0));
                    if (cmax > maxmcid) maxmcid = cmax;

                }
                catch (System.Exception ex)
                {
                }


                treePointer.GetPdfStructureElem().RemoveKid(0);
            }
            DeleteTagIfNoKids(treePointer);

            return true;
        }


        /// <summary>
        /// Returns highest mcid found recursive in childs of treepointer
        /// </summary>
        /// <param name="treePointer"></param>
        /// <returns></returns>
        private int GetMaxMcid(TagTreePointer treePointer)
        {
            try
            {
                treePointer.GetKidsRoles();
            }
            catch (System.Exception) // sometimes there is a problem with nodes that have no role, mostly if other problems have occoured before
            {
                return 0;
            }

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
                    child_max = GetMaxMcid(treePointer.GetKidAsTagTreePointer(kidIndex));

                }
                if (child_max > max) max = child_max;


            }
            return max;

        }

        private void debugPrintTagTree(TagTreePointer tagTreePointer, int depth = 0)
        {
            try
            {
                tagTreePointer.GetKidsRoles();
            }
            catch (System.Exception) // sometimes there is a problem with nodes that have no role, mostly if other problems have occoured before
            {
                return;
            }



            for (int kidIndex = 0; kidIndex < tagTreePointer.GetKidsRoles().Count; kidIndex++)
            {
                string role = tagTreePointer.GetKidsRoles()[kidIndex];

                if (role == null)
                    continue;
                int mcid = tagTreePointer.GetMcid(kidIndex);

                // if node is not MCR (has kids), recurse
                if (role != "MCR")
                {
                    debugPrintTagTree(tagTreePointer.GetKidAsTagTreePointer(kidIndex), depth + 1);
                }
            }
        }

        public void Visit(PowerPoint.Slide slide, PowerPoint.Shape shape)
        {

        }

        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            return true;
        }

        #region InfoStruct

        private struct MasterSlideInfo
        {
            public bool NeedsArtifactFixing;
            public bool Hidden;
            public int MasterArtifactShapeCount;
            public int SlideShapeCount;
            public int PptPageIndex;

            public MasterSlideInfo(PowerPoint.Slide slide)
            {


                //there is a number of shapes to remove within the Master
                int m_s_count = countShapesToRemove(slide.Master.Shapes, slide);


                // a master has multiple layouts with their own shapes and placeholders
                int d_s_count = countShapesToRemove(slide.CustomLayout.Shapes, slide);

                //... additionally a layout can also inherit the shapes of its master as background (placeholders are not included)
                bool d_msback = slide.CustomLayout.DisplayMasterShapes == Office.Core.MsoTriState.msoTrue;


                // the number of displayed shapes, that are not changable within a slide
                MasterArtifactShapeCount = d_msback ? d_s_count + m_s_count : d_s_count;

                // save the number for artifact-marking within the PDF
                PptPageIndex = slide.SlideNumber;

                int placeholder_shapeCount = slide.Shapes.Placeholders.Count;
                SlideShapeCount = slide.Shapes.Count;
                Hidden = slide.SlideShowTransition.Hidden == Office.Core.MsoTriState.msoTrue;
                NeedsArtifactFixing = !Hidden && !(SlideShapeCount - placeholder_shapeCount == 0 && MasterArtifactShapeCount == 0);

            }

            private static int countShapesToRemove(PowerPoint.Shapes sh, PowerPoint.Slide slide)
            {
                int count = 0;
                foreach (PowerPoint.Shape item in sh)
                {
                    float height = slide.CustomLayout.Height;
                    float width = slide.CustomLayout.Width;
                    if (item.Top < 0 ||       // shape is above the presentation area
                        item.Left > width ||  // shape is on the left outside the presentation area
                        item.Top > height ||  // shape is below the presentation area
                        item.Left < 0)        // shape is on the left outside the presentation area
                    {
                        continue;
                    }

                    switch (item.Type)
                    {
                        case Office.Core.MsoShapeType.msoShapeTypeMixed:
                            break;
                        case Office.Core.MsoShapeType.msoAutoShape:
                            if (item.HasTextFrame == Office.Core.MsoTriState.msoTrue
                                && item.TextFrame.HasText == Office.Core.MsoTriState.msoTrue)
                                count++;
                            break;
                        case Office.Core.MsoShapeType.msoCallout:
                            break;
                        case Office.Core.MsoShapeType.msoChart:
                            break;
                        case Office.Core.MsoShapeType.msoComment:
                            break;
                        case Office.Core.MsoShapeType.msoFreeform:
                            break;
                        case Office.Core.MsoShapeType.msoGroup:
                            break;
                        case Office.Core.MsoShapeType.msoEmbeddedOLEObject:
                            break;
                        case Office.Core.MsoShapeType.msoFormControl:
                            break;
                        case Office.Core.MsoShapeType.msoLine:
                            break;
                        case Office.Core.MsoShapeType.msoLinkedOLEObject:
                            break;
                        case Office.Core.MsoShapeType.msoLinkedPicture:
                            break;
                        case Office.Core.MsoShapeType.msoOLEControlObject:
                            break;
                        case Office.Core.MsoShapeType.msoPicture:
                            break;
                        case Office.Core.MsoShapeType.msoPlaceholder:
                            break;
                        case Office.Core.MsoShapeType.msoTextEffect:
                            break;
                        case Office.Core.MsoShapeType.msoMedia:
                            break;
                        case Office.Core.MsoShapeType.msoTextBox:
                            if (item.HasTextFrame == Office.Core.MsoTriState.msoTrue
                            && item.TextFrame.HasText == Office.Core.MsoTriState.msoTrue)
                                count++;
                            break;
                        case Office.Core.MsoShapeType.msoScriptAnchor:
                            break;
                        case Office.Core.MsoShapeType.msoTable:
                            break;
                        case Office.Core.MsoShapeType.msoCanvas:
                            break;
                        case Office.Core.MsoShapeType.msoDiagram:
                            break;
                        case Office.Core.MsoShapeType.msoInk:
                            break;
                        case Office.Core.MsoShapeType.msoInkComment:
                            break;
                        case Office.Core.MsoShapeType.msoSmartArt:
                            break;
                        case Office.Core.MsoShapeType.msoSlicer:
                            break;
                        case Office.Core.MsoShapeType.msoWebVideo:
                            break;
                        default:
                            break;
                    }

                }
                return count;
            }

        }
        #endregion
    }
}
