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

using System.Resources;
using A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane;
using A11y_Design_PowerPointAddin.Helper;
using Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office.Core;

namespace A11y_Design_PowerPointAddin
{
    class A11yIncidentType_Textbox : A11yIncidentType<Shape>
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_Textbox).Assembly);
        private A11yIncidentTopic Topic { get; }


        public A11yIncidentType_Textbox(Office.MsoShapeType shapeType, A11yIncidentTopic topic)
            : base(condition: condition,
                  itemNaming: s => s.GetName(), select,
                  topic: topic)
        {
            Helper.AlternativeText.RegisterAltTextShapeRule(condition, shapeType);
        }


        private static void select(Shape s)
        {
            Controller.ChangePaneController.SetTab(AccessibilityChangesPaneTabs.ALTERNATVETEXT, false);
            Controller.AppController.View.AccessibilityChanges.SetHint(resourceManager.GetString("HintAreaDefaultText"));
            s.SelectShape();

            switch (ShapeExtensions.GetNestedType(s))
            {
                case Microsoft.Office.Core.MsoShapeType.msoTextBox:
                    Controller.AppController.View.AccessibilityChanges.SetAlternativeText("TextBoxAltDescription");
                    break;

                default:
                    Controller.AppController.View.AccessibilityChanges.SetAlternativeText("TextBoxAltDescription");
                    break;
            }
        }

        private static bool condition(Shape s)
        {
            switch (s.GetNestedType())
            {
                case Office.MsoShapeType.msoDiagram:
                case Office.MsoShapeType.msoChart:
                case Office.MsoShapeType.msoGroup:
                    {
                        return false;
                    }
            }
            bool hasNotext = s.TextFrame.HasText == Office.MsoTriState.msoFalse ? true : false;
            bool hasNoAlternativeText = s.AlternativeText.Length == 0 ? true : false;
            bool notMarkedAsArtifact = !Helper.Artifact.IsIdMarkedAsArtifact(s) ? true : false;
            bool isLineVisible = false;
            try
            {
                isLineVisible = s.Line.Visible == Office.MsoTriState.msoTrue ? true : false;
            }
            catch (System.Exception ex)
            {

            }
            if (s.Name == resourceManager.GetString("a11yPdfElement")) return false;
            else
            {
                return (s.GetNestedType() != Office.MsoShapeType.msoTable && s.Fill.Visible == Office.MsoTriState.msoTrue && hasNotext && notMarkedAsArtifact && hasNoAlternativeText);
            }
        }

    }
}
