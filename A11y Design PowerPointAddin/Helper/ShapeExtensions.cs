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
using System.Diagnostics;
using System.Runtime.InteropServices;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
namespace A11y_Design_PowerPointAddin.Helper
{
    /// <summary>
    /// Zoo of helping fuctionlities for Shapes
    /// </summary>
    internal static class ShapeExtensions
    {
        /// <summary>
        /// Get Type while ignoring Placeholders
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Office.MsoShapeType GetNestedType(this Shape s)
        {
            if (s.Type == Office.MsoShapeType.msoPlaceholder) return s.PlaceholderFormat.ContainedType;
            else return s.Type;
        }

        /// <summary>
        /// Get Name of shape with optional palceholder-skipping and slidnumbers
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="skip_placeholders"></param>
        /// <param name="add_slide"></param>
        /// <returns></returns>
        public static string GetName(this Shape shape, bool skip_placeholders = true, bool add_slide = true)
        {
            int slideIndex = shape.Parent.SlideIndex;
            string elementName;
            Office.MsoShapeType type = shape.Type;
            if (type == Office.MsoShapeType.msoPlaceholder)
            {
                type = shape.PlaceholderFormat.ContainedType;
            }


            switch (type)
            {
                case Office.MsoShapeType.msoPicture:
                    elementName = Properties.Resources.ShapeTypeImage;
                    break;
                case Office.MsoShapeType.msoChart:
                    elementName = Properties.Resources.ShapeTypeChart;
                    break;
                case Office.MsoShapeType.msoGroup:
                    elementName = Properties.Resources.ShapeTypeGroup;
                    break;
                case Office.MsoShapeType.msoSmartArt:
                    elementName = Properties.Resources.ShapeTypeSmartArt;
                    break;
                case Office.MsoShapeType.msoTable:
                    elementName = Properties.Resources.ShapeTypeTable;
                    break;
                default:
                    elementName = "Element";
                    break;
            }
            if (add_slide)
                return elementName + " " + shape.Id + " (Folie " + slideIndex + ")";
            else
                return elementName + " " + shape.Id;
        }

        /// <summary>
        /// Go to slide of shape and select it
        /// </summary>
        /// <param name="shape"></param>
        public static void SelectShape(this Shape shape)
        {
            int slideIndex = shape.Parent.SlideIndex;

            try
            {
                Globals.ThisAddIn.Application.ActivePresentation.Slides[slideIndex].Select();
            }
            catch (COMException e)
            {
                Debug.Write(e.Message);
            }

            try
            {
                shape.Select();
            }
            catch (COMException e)
            {
                Debug.Write(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape"></param>
        /// <returns>true if shape in not deleted</returns>
        public static bool Exists(this Shape shape)
        {
            if(shape == null)return false;
            // horrible solution! is there any alternative to detect deleted shapes?
            try
            {
                return true;
            }
            catch (COMException)
            {
                return false;
            }
        }

        public static void ShowShapeType(bool showAllTypes)
        {
            Slides slides = Globals.ThisAddIn.Application.ActiveWindow.Presentation.Slides;
            foreach (Slide slide in slides)
            {                
                int count = slide.Shapes.Count;
                int i = 0;
                foreach(Shape shape in slide.Shapes)
                {
                    // check is needed because count of shapes changes because textboxes are added
                    if(count == i)
                    {
                        break;
                    }
                    else
                    {
                        i++;
                    }

                    if(showAllTypes) //show type textboxes
                    {
                        string placeHolder = string.Empty;
                        if(shape.Type == Office.MsoShapeType.msoPlaceholder){
                            placeHolder = shape.PlaceholderFormat.Type.ToString();
                        }
                        else
                        {
                            placeHolder = shape.Name;
                        }
                        string text = $"ST: {shape.Type.ToString()} | SN: {placeHolder}| SNT: {shape.GetNestedType()} |SAT: {shape.AutoShapeType} | Id : {shape.Id} | ZoP: {shape.ZOrderPosition}";
                        createTextbox(slide, shape.Left, shape.Top, shape.Width, shape.Height, text, System.Drawing.Color.Red);
                    }
                    else // hide all type textboxes 
                    {
                        removeAllShapeTypeTextboxes(); 
                    }

                }
            }
        }

        private static void createTextbox(Slide slide, float left, float top, float width, float height, string text, System.Drawing.Color color)
        {
            Shape textbox;
            textbox = slide.Shapes.AddTextbox(Microsoft.Office.Core.MsoTextOrientation.msoTextOrientationHorizontal, left, top, width, height);
            textbox.TextFrame.TextRange.Text = text;
            textbox.Fill.BackColor.RGB = 0x000000;
            int rgbColor = color.R + 0xFF *color.G + 0xFFFF *color.B;
            textbox.TextFrame.TextRange.Font.Color.RGB = rgbColor;
            textbox.Name = "TypeTextbox";
        }

        private static void removeAllShapeTypeTextboxes()
        {
            List<Shape> listTypeTB = GetAllTypeTB();
            foreach (Shape shape in listTypeTB)
            {
                shape.Delete();
            }
            listTypeTB.Clear();
        }

        private static List<Shape> GetAllTypeTB()
        {
            List<Shape> result = new List<Shape>();
            Slides slides = Globals.ThisAddIn.Application.ActiveWindow.Presentation.Slides;

            foreach (Slide sld in slides)
            {
                foreach (Shape shape in sld.Shapes)
                {
                    if (shape.Name == "TypeTextbox")
                    {
                        result.Add(shape);
                    }
                }
            }

            return result;
        }
    }
}
