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
using System;
using System.Resources;

namespace A11y_Design_PowerPointAddin
{
    /// <summary>
    /// Extention of the generic A11yIncidentType for Incidents based on Shapes
    /// </summary>
    class A11yIncidentType_Shape : A11yIncidentType<Shape>
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_ShapeAltText).Assembly);
        public A11yIncidentType_Shape(Func<Shape, bool> condition, A11yIncidentTopic topic) 
            : base(condition, s => s.GetName(), select, topic)
        {            
            Helper.AlternativeText.RegisterAltTextShapeRule(condition);
        }
        private static void select(Shape s)
        {
            ChangePaneController.SetTab(AccessibilityChangesPaneTabs.ALTERNATVETEXT, false);
            s.SelectShape();

            switch (ShapeExtensions.GetNestedType(s)) 
            {
                case Microsoft.Office.Core.MsoShapeType.msoTextBox:
                    AppController.View.AccessibilityChanges.SetAlternativeText("TextBoxBorderHint");
                    break;
            }
           
        }

        }
}