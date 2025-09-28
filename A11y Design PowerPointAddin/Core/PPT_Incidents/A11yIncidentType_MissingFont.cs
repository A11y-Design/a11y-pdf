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

using A11y_Design_PowerPointAddin.Controller;
using A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane;
using A11y_Design_PowerPointAddin.Helper;
using Microsoft.Office.Interop.PowerPoint;
using System.Collections.Generic;
using System.Resources;
using Office = Microsoft.Office.Core;

namespace A11y_Design_PowerPointAddin.Core.PPT_Incidents
{
    internal class A11yIncidentType_MissingFont : IA11yIncidentCreator<Shape>
    {

        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_Table).Assembly);
        private A11yIncidentTopic topic = new A11yIncidentTopic(resourceManager.GetString("FontCannotembedInPDF"), TopicLevel.ERROR);
        private List<string> ProblemFonts = new List<string> { "Arial" };        
        public IA11yIncidentTopic Topic { get; }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public bool TryCreate(Shape shape, ICollection<IA11yIncidentItem> incidentItems)
        {
            bool found = false;
            string incidentText = resourceManager.GetString("incidentFontText");
            string incidentHint = resourceManager.GetString("incidentFontHint");
            IA11yIncidentItem errorItem;
            //checks whole Shape
            if (shape.HasTextFrame == Office.MsoTriState.msoTrue && (ProblemFonts.Contains(shape.TextFrame.TextRange.Font.Name)
                || shape.TextFrame.TextRange.Font.Name == null)) 
                // shape.TextFrame.TextRange.Font.Name = null if there are more than one font set for a TextRange
            {
                errorItem = new A11yIncidentItem(() => select(shape, incidentHint),
                                 name: incidentText,
                                 topic: topic);
                incidentItems.Add(errorItem);
                found = true;
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
                            errorItem = new A11yIncidentItem(() => select(s, incidentHint),
                                name: resourceManager.GetString("incidentFontTableName"),
                                topic: topic);
                            incidentItems.Add(errorItem);
                            found = true;
                        
                        }
                    }

                }
            }
            //check slide master
            bool changedFontInMaster = FontFix.ReplaceFontInMaster(Globals.ThisAddIn.Application.ActivePresentation);
            if (changedFontInMaster)
            {
                errorItem = new A11yIncidentItem(() => resourceManager.GetString("incidentFontReplaceInMasterHint"),
                                name: resourceManager.GetString("incidentFontInMaster"),
                                topic: topic);
                incidentItems.Add(errorItem);
                found = true;
            }

            return found;
        }

        private void select(Shape shape, string hint)
        {
            shape.SelectShape();
            FontFix.ReplaceAllFont(Globals.ThisAddIn.Application.ActivePresentation);
            ChangePaneController.SetTab(AccessibilityChangesPaneTabs.HINTS, false);
            AppController.View.AccessibilityChanges.SetHint(hint);
        }
    }
}
