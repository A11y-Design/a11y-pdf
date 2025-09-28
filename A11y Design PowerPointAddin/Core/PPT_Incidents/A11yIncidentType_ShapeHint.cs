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
using System.Resources;

namespace A11y_Design_PowerPointAddin
{
    class A11yIncidentType_ShapeHint : A11yIncidentType<Shape>
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_ShapeAltText).Assembly);

        public A11yIncidentType_ShapeHint(Microsoft.Office.Core.MsoShapeType shapeType, A11yIncidentTopic topic)
            : base(condition: s => s.GetNestedType() == shapeType && s.AlternativeText.Length == 0,
                  itemNaming: s => s.GetName(), select,
                  topic: topic)
        {
            Helper.AlternativeText.RegisterAltTextShapeType(shapeType);
        }

        private static void select(Shape s)
        {
            ChangePaneController.SetTab(AccessibilityChangesPaneTabs.HINTS, false);
            s.SelectShape();

            AppController.View.AccessibilityChanges.SetHint(resourceManager.GetString("AnimatedShapeDescription"));

            switch (ShapeExtensions.GetNestedType(s))
            {
                case Microsoft.Office.Core.MsoShapeType.msoEmbeddedOLEObject:
                    ChangePaneController.SetTab(AccessibilityChangesPaneTabs.ALTERNATVETEXT);
                    AppController.View.AccessibilityChanges.SetAlternativeText("ErrorByEmbeddedObjectDescription");
                    break;

                case Microsoft.Office.Core.MsoShapeType.msoLinkedOLEObject:
                case Microsoft.Office.Core.MsoShapeType.msoOLEControlObject:
                    ChangePaneController.SetTab(AccessibilityChangesPaneTabs.ALTERNATVETEXT);
                    AppController.View.AccessibilityChanges.SetAlternativeText("ErrorByLinkedObjectDescription");
                    break;

                case Microsoft.Office.Core.MsoShapeType.msoInk:
                    ChangePaneController.SetTab(AccessibilityChangesPaneTabs.ALTERNATVETEXT);
                    AppController.View.AccessibilityChanges.SetAlternativeText("ErrorByInkDescription");
                    break;

                case Microsoft.Office.Core.MsoShapeType.msoInkComment:
                    ChangePaneController.SetTab(AccessibilityChangesPaneTabs.ALTERNATVETEXT);
                    AppController.View.AccessibilityChanges.SetAlternativeText("ErrorByInkCommentDescription");
                    break;

                case Microsoft.Office.Core.MsoShapeType.msoTextBox:
                    if (s.Line.Visible == Microsoft.Office.Core.MsoTriState.msoTrue)
                    {
                        AppController.View.AccessibilityChanges.SetHint(resourceManager.GetString("ErrorByInkCommentDescription"));
                        break;
                    }
                    else
                    {
                        break;
                    }


                default:
                    AppController.View.AccessibilityChanges.SetHint(resourceManager.GetString("HintAreaDefaultText"));
                    break;
            }
        }
    }
}