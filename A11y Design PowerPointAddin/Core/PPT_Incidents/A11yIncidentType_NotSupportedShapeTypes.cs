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

using System;
using System.Collections.Generic;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;
using System.Resources;
using A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane;
using A11y_Design_PowerPointAddin.Helper;

namespace A11y_Design_PowerPointAddin
{
    class A11yIncidentType_NotSupportedShapeTypes : IA11yIncidentCreator<Shape>
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_ShapeAltText).Assembly);
        private A11yIncidentTopic topic = new A11yIncidentTopic(resourceManager.GetString("MissingAltTextForNewerVisualContent"), TopicLevel.ERROR);
        private A11yIncidentTopic Topic { get; }
        IA11yIncidentTopic IA11yIncidentCreator<Shape>.Topic { get; }



        public bool TryCreate(Shape item, ICollection<IA11yIncidentItem> errorItem)
        {
            dynamic shp = item as dynamic;
            //Check if type of item is in enum NotSupportShapeTypes
            if (Enum.IsDefined(typeof(NotSupportShapeTypes), (int)item.Type)
                                && item.AlternativeText.Length == 0  // no alternative text
                                && shp.Decorative == 0)             // not marked as decorative
            {
                errorItem.Add(new A11yIncidentItem(() =>
                {
                    select(item);                                                    

                },
                item.GetName(),
                topic));
                Helper.AlternativeText.RegisterAltTextShapeType(item.Type);
                return true;
            }
            else
            {
                
                return false;
            }

        }

        private static void select(Shape item)
        {
            Controller.ChangePaneController.SetTab(AccessibilityChangesPaneTabs.ALTERNATVETEXT, false);
            Controller.AppController.View.AccessibilityChanges.SetHint(resourceManager.GetString("HintAreaDefaultText"));
            Controller.AppController.View.AccessibilityChanges.SetAlternativeText("AlternativeTextInfoBox");
            item.SelectShape();
        }

        void IA11yIncidentCreator<Shape>.Reset()
        {

        }

        enum NotSupportShapeTypes
        {
            msoContentApp = 27,
            msoGraphic = 28,
            msoLinkedGraphic = 29,
            mso3DModel = 30,
            msoLinked3DModel = 31,
        };
    }
}
