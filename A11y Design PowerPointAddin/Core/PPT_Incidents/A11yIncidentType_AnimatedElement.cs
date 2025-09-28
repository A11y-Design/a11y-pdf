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

namespace A11y_Design_PowerPointAddin.Core.PPT_Incidents
{
    internal class A11yIncidentType_AnimatedElement : IA11yIncidentCreator<Shape>
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_AnimatedElement).Assembly);
        private A11yIncidentTopic topic = new A11yIncidentTopic(resourceManager.GetString("AnimatedShape"), TopicLevel.WARNING);

        public IA11yIncidentTopic Topic => topic;

        public void Reset()
        {
        }

        public bool TryCreate(Shape item, ICollection<IA11yIncidentItem> errorItem)
        {
            if (item.AnimationSettings.Animate == Microsoft.Office.Core.MsoTriState.msoTrue)
            {
                errorItem.Add(new A11yIncidentItem(() =>
                {                    
                    item.SelectShape();
                    Controller.AppController.View.AccessibilityChanges.SetHint(resourceManager.GetString("AnimatedShapeDescription"));
                    ChangePaneController.SetTab(AccessibilityChangesPaneTabs.HINTS, false);
                },
                item.GetName(),
                topic));

                return true;
            }

            return false;
        }
    }
}
