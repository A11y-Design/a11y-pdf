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

using A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane;
using A11y_Design_PowerPointAddin.Helper;
using Microsoft.Office.Interop.PowerPoint;
using System.Resources;

namespace A11y_Design_PowerPointAddin
{
    class A11yIncidentType_AutoShapeAltText : A11yIncidentType<Shape>
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_ShapeAltText).Assembly);

        /// <summary>
        /// Dedicated implementation for incident on Forms without alternative text.
        /// Respects that autoshapes with text are converted to text-boxes in export.
        /// </summary>
        public A11yIncidentType_AutoShapeAltText(A11yIncidentTopic topic)
            : base(condition:condition,
                  itemNaming: s => s.GetName(), select,
                  topic: topic)
        {
            Helper.AlternativeText.RegisterAltTextShapeRule(condition);
        }

        private static void select(Shape s)
        {
            Controller.ChangePaneController.SetTab(AccessibilityChangesPaneTabs.ALTERNATVETEXT, false);
            s.SelectShape();

            Controller.AppController.View.AccessibilityChanges.SetAlternativeText("AutoShapeNoAltTextDescription");
        }

        private static bool condition(Shape s)
        {
            if((s.GetNestedType() == Microsoft.Office.Core.MsoShapeType.msoAutoShape ||
               s.GetNestedType() == Microsoft.Office.Core.MsoShapeType.msoTextBox) &&
               // Placeholders without content also match the previous conditions but have a noPrimitive type (aka not supported)
               s.AutoShapeType != Microsoft.Office.Core.MsoAutoShapeType.msoShapeNotPrimitive)
            {
                if (s.TextFrame.HasText == Microsoft.Office.Core.MsoTriState.msoTrue)
                {
                    return false;
                }
                if(s.AlternativeText.Length == 0 && !Helper.Artifact.IsIdMarkedAsArtifact(s) && s.Fill.Visible == Microsoft.Office.Core.MsoTriState.msoFalse)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

            //return (s.GetNestedType() == Microsoft.Office.Core.MsoShapeType.msoAutoShape ||
            //   s.GetNestedType() == Microsoft.Office.Core.MsoShapeType.msoTextBox)
            //   && (s.TextFrame.HasText == Microsoft.Office.Core.MsoTriState.msoFalse
            //   || (s.AlternativeText.Length == 0 && !Helper.Artifact.IsIdMarkedAsArtifact(s)))
            //   // Placeholders without content also match the previous conditions but have a noPrimitive type (aka not supported)
            //   && s.AutoShapeType != Microsoft.Office.Core.MsoAutoShapeType.msoShapeNotPrimitive;


        }
    }
}