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

using A11y_Design_PowerPointAddin.Helper;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Resources;
using Office = Microsoft.Office;

namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// Converts vector shapes to bitmaps - freeforms, smartarts, vector figures
    /// Grouping are also converted to bitmap as they may contain vector parts
    /// Call as first when exporting
    /// </summary>
    class A11yMod_ConvertVector : IA11yModification
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yMod_ConvertVector).Assembly);
        public List<int> ContainedTypes { get; private set; }
        public List<int> LooseTypes { get; private set; }

        const int msoGraphic = 28;
        public A11yMod_ConvertVector()
        {

            ContainedTypes = new List<int>
            {
                (int)Office.Core.MsoShapeType.msoFreeform,
                (int)Office.Core.MsoShapeType.msoSmartArt,
                (int)Office.Core.MsoShapeType.msoChart,
                msoGraphic
            };

            LooseTypes = new List<int>
            {
                (int)Office.Core.MsoShapeType.msoGroup,
                (int)Office.Core.MsoShapeType.msoFreeform,
                (int)Office.Core.MsoShapeType.msoInk,
            (int)Office.Core.MsoShapeType.msoInkComment,
                (int)Office.Core.MsoShapeType.msoSmartArt,
                (int)Office.Core.MsoShapeType.msoChart,
                msoGraphic
            };
        }

        public void Visit(Slide slide, Shape shape)
        {
            if (LooseTypes.Contains((int)shape.Type) ||
                (shape.Type == Office.Core.MsoShapeType.msoPlaceholder
                    && ContainedTypes.Contains((int)shape.PlaceholderFormat.ContainedType))
                || (shape.GetNestedType() == Office.Core.MsoShapeType.msoPicture && (shape.Name.ToLower().StartsWith("camera") || shape.Name.ToLower().StartsWith("kamera"))))
            {
                if (shape.Type == Office.Core.MsoShapeType.msoAutoShape && shape.HasTextFrame == Office.Core.MsoTriState.msoTrue)
                {
                    return;
                }
                string altText = shape.AlternativeText;
                float rotation = 0;
                try
                {
                    rotation = shape.Rotation;
                    shape.Rotation = 0;
                }
                catch (ArgumentException e)
                {
                    //an ArgumentException is thrown if the shape cannot be rotated, e.g. SmartArt. Catch block to handle it
                }
                float x = shape.Left;
                float y = shape.Top;
                float width = shape.Width;
                float height = shape.Height;
                long oldZorder = shape.ZOrderPosition;
                bool oldShapeWasArtifact = Artifact.IsIdMarkedAsArtifact(shape); //checks first after cut shape is not avaible anymore
                // artifacts have not be replaced fixes issue with slide designer
                if (oldShapeWasArtifact) return;
                string altTextCamera = String.Empty;
                if ((shape.GetNestedType() == Office.Core.MsoShapeType.msoPicture || shape.GetNestedType().ToString() == "28") && (shape.Name.ToLower().StartsWith("came") || shape.Name.ToLower().StartsWith("kame")))
                {
                    altTextCamera = resourceManager.GetString("ScreenshotWebcam");
                }

                bool clipBoardCleared = clearClipBoard();
                while (!clipBoardCleared)
                {
                    clipBoardCleared = clearClipBoard();
                }
                shape.Cut();


                Shape pic;

                System.Windows.Forms.IDataObject dataObject = null;
                while (dataObject == null) //looping for avoiding exception
                {
                    dataObject = GetDataObject();
                }
                try
                {
                    pic = slide.Shapes.PasteSpecial(PpPasteDataType.ppPastePNG)[1];
                }
                catch (Exception e)
                {
                    try
                    {
                        pic = slide.Shapes.PasteSpecial(PpPasteDataType.ppPasteDefault)[2];
                    }
                    catch (Exception ex)
                    {
                        pic = slide.Shapes.PasteSpecial(PpPasteDataType.ppPasteDefault)[1];
                    }

                }
                if (oldShapeWasArtifact) // check if old id is marked as artifact
                {
                    pic.AlternativeText = "SetAsDecorative";
                }
                else
                {
                    if (altTextCamera.Equals(String.Empty))
                    {
                        pic.AlternativeText = altText;
                    }
                    else
                    {
                        pic.AlternativeText = altTextCamera;

                    }



                }

                pic.Left = x + (width - pic.Width) / 2;
                pic.Top = y + (height - pic.Height) / 2;
                try
                {
                    //Office 2016 issue
                    pic.Rotation = rotation;
                }
                catch (Exception ex)
                {

                }

                while (pic.ZOrderPosition != oldZorder)
                {
                    pic.ZOrder(Office.Core.MsoZOrderCmd.msoSendBackward);
                }



            }
        }

        // is needed to avoid problems with wrong places images (office 2016 issue)
        private Boolean clearClipBoard()
        {
            try
            {
                System.Windows.Forms.Clipboard.Clear();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private System.Windows.Forms.IDataObject GetDataObject()
        {
            System.Windows.Forms.IDataObject dataObject = null;
            try
            {
                dataObject = System.Windows.Forms.Clipboard.GetDataObject();
            }
            catch (Exception ex)
            {

            }
            return dataObject;

        }
        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
        }

        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {
            return true;
        }

        public void Visit(Slide slide)
        {
        }
    }
}
