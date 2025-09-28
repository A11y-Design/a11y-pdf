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

using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace A11y_Design_PowerPointAddin.Helper
{
    /// <summary>
    /// Singleton instance to handle alternative texts for shapes globally.
    /// <br>Provides helping fuctionalities.</br>
    /// </summary>
    class AlternativeText
    {
        public static AlternativeText Instance { get; } = new AlternativeText();
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(AlternativeText).Assembly);
        HashSet<MsoShapeType> AltTextShapeTypes;
        List<Func<PowerPoint.Shape, bool>> AltTextShapeRules;


        /// <summary>
        /// Register shpetype as eligible for alternative texts
        /// </summary>
        /// <param name="s"></param>
        public static void RegisterAltTextShapeType(MsoShapeType s)
        {
            Instance.AltTextShapeTypes.Add(s);
        }

        public static void RegisterAltTextShapeRule(Func<PowerPoint.Shape,bool> rule)
        {
            Instance.AltTextShapeRules.Add(rule);
        }

        public static void RegisterAltTextShapeRule(Func<PowerPoint.Shape, bool> rule, MsoShapeType s)
        {
            Instance.AltTextShapeRules.Add(rule);
            Instance.AltTextShapeTypes.Add(s);
        }

        public AlternativeText()
        {
            AltTextShapeTypes = new HashSet<MsoShapeType>();
            AltTextShapeRules = new List<Func<PowerPoint.Shape, bool>>();
        }

        /// <summary>
        /// Retrieve shapes eligible for alternative texts from slide
        /// </summary>
        /// <param name="slide"></param>
        /// <returns></returns>
        public static List<PowerPoint.Shape> SlideShapesValidForAltText(PowerPoint.Slide slide)
        {
            List<PowerPoint.Shape> altTextShapes = new List<PowerPoint.Shape>();

            foreach (PowerPoint.Shape shape in slide.Shapes)
            {
                if (shape.Name == resourceManager.GetString(("a11yPdfElement"))) continue; //needs no alt text is removed before export
                if (isAlternativeTextRequired(shape))
                {
                    altTextShapes.Add(shape);
                }

            }
            return altTextShapes;
        }

        /// <summary>
        /// Checks if a alternative text for an shape like link or image, maybe table needed
        /// </summary>
        /// <param name="shape">shape that should be checked</param>
        /// <returns></returns>
        public static bool isAlternativeTextRequired(PowerPoint.Shape shape)
        {
            bool shapeTypeNeedAlternativeText = Instance.AltTextShapeTypes.Any(s => s == shape.GetNestedType()) 
                                                && shape.GetNestedType() != MsoShapeType.msoTextBox 
                                                || Instance.AltTextShapeRules.Any(rule => rule.Invoke(shape));
            bool textBoxNeedAltText = (shape.Fill.Visible == MsoTriState.msoTrue || shape.Line.Visible == MsoTriState.msoTrue)
                                    && shape.TextFrame.HasText == MsoTriState.msoFalse ;
            bool value = shapeTypeNeedAlternativeText || textBoxNeedAltText;
            return value;
       
        }
    }


}
