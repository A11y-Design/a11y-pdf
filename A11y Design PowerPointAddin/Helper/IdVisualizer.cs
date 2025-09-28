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

using Microsoft.Office.Interop.PowerPoint;
using System.Collections.Generic;
using System.Resources;

namespace A11y_Design_PowerPointAddin.Helper
{
    public class IdVisualizer
    {
        Slide slide;
        const string IdTagName = "DYNAMICID_B328720C-A0F9-473D-925F-307CE239BC1E";
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(IdVisualizer).Assembly);
        public void SetSlide(Slide slide)
        {
            this.slide = slide;
        }

        public void AddIds()
        {
            DeleteIds();

            List<Shape> shapes = new List<Shape>();
            foreach (Shape shape in slide.Shapes)
            {
                shapes.Add(shape);
            }

            shapes.Sort((x, y) => x.ZOrderPosition.CompareTo(y.ZOrderPosition));
            int readingOrderCounter = 1;

            foreach (Shape shape in shapes)
            {
                // Calculate position for text box
                float width = 20;
                float height = 20;
                float x = shape.Left - width;
                float y = shape.Top - height/2;

                // Add text box
                Shape newShape = slide.Shapes.AddTextbox(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal,
                    x, y, width, height);

                newShape.Name = resourceManager.GetString("a11yPdfElement");
                newShape.TextFrame.TextRange.Text = readingOrderCounter.ToString();
                readingOrderCounter++;

                newShape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeShapeToFitText;
                newShape.TextFrame.TextRange.Font.Color.RGB = System.Drawing.Color.FromArgb(255, 255, 255).ToArgb();

                newShape.Fill.ForeColor.RGB = System.Drawing.Color.FromArgb(0, 0, 255).ToArgb();

                // Set text box tag 
                newShape.Tags.Add(IdTagName, "Dynamic ID");
            }
        }

        public void DeleteIds()
        {

            //foreach (Presentation presentation in Globals.ThisAddIn.Application.Presentations)
            //{
            Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
                foreach (Slide slide in presentation.Slides)
                {
                    for (int i = slide.Shapes.Count; i > 0; i--)
                    {
                        string tagVal = slide.Shapes[i].Tags[IdTagName];
                        if (!string.IsNullOrEmpty(tagVal))
                            slide.Shapes[i].Delete();
                    }
                }
            //}
        }


    }
}
