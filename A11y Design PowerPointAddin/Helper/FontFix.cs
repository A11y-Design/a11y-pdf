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
using Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;


namespace A11y_Design_PowerPointAddin.Helper
{

    internal static class FontFix
    {
        private static bool fixedMasterFonts = false;
        private static string _replaceFont = "Calibri";
        private static List<string> ProblemFonts = new List<string> { "Arial" };
        public static void ReplaceFont(Shape shape)
        {
            shape.TextFrame.TextRange.Font.Name = _replaceFont;
        }
        /// <summary>
        /// Replaces all font in master
        /// </summary>
        /// <param name="presentation"></param>
        public static bool ReplaceFontInMaster(Presentation presentation)
        {
            bool fontIsReplace = false;
            if (!fixedMasterFonts)
            {
                foreach (Slide sld in presentation.Slides)
                {
                    foreach (Shape shp in sld.Master.Shapes)
                    {
                        if (shp.HasTextFrame == Office.MsoTriState.msoTrue && ProblemFonts.Contains(shp.TextFrame.TextRange.Font.Name))
                        {
                            ReplaceFont(shp);
                            fontIsReplace = true;
                        }
                    }
                    foreach (Shape shp in sld.CustomLayout.Shapes)
                    {
                        if (shp.HasTextFrame == Office.MsoTriState.msoTrue && ProblemFonts.Contains(shp.TextFrame.TextRange.Font.Name))
                        {
                            ReplaceFont(shp);
                            fontIsReplace = true;
                        }
                    }
                }


            }
            return fontIsReplace;
        }

        public static void ReplaceAllFont(Presentation presentation)
        {
            foreach (Slide sld in presentation.Slides)
            {
                foreach (Shape shape in sld.Shapes)
                {
                    if (shape.HasTextFrame == Office.MsoTriState.msoTrue && (ProblemFonts.Contains(shape.TextFrame.TextRange.Font.Name)
                         || shape.TextFrame.TextRange.Font.Name == null))
                    {
                        ReplaceFont(shape);
                    }
                    if (shape.GetNestedType() == Office.MsoShapeType.msoTable)
                    {
                        foreach (Row row in shape.Table.Rows)
                        {
                            foreach (Cell cell in row.Cells)
                            {
                                var s = cell.Shape;
                                if (ProblemFonts.Contains(s.TextFrame.TextRange.Font.Name))
                                {
                                    ReplaceFont(s);
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}
