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
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using A11y_Design_PowerPointAddin.Controller;
using A11y_Design_PowerPointAddin.Core.PPT_Incidents;
using System.Resources;
using A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane;
using System.Collections.Generic;
using A11y_Design_PowerPointAddin.Helper;

namespace A11y_Design_PowerPointAddin
{
    /// <summary>
    /// Incident for Hyperlinks without ScreenTip
    /// </summary>
    class A11yIncidentType_HyperlinkAlternativeDescription : IA11yIncidentCreator<PowerPoint.Hyperlink>
    {
        private A11yIncidentTopic Topic { get; }

        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_Table).Assembly);


        /// <summary>
        /// External topic without rights to add incidents
        /// </summary>
        IA11yIncidentTopic IA11yIncidentCreator<PowerPoint.Hyperlink>.Topic => Topic;

        public A11yIncidentType_HyperlinkAlternativeDescription(A11yIncidentTopic topic)
        {
            Topic = topic;
        }

        public bool TryCreate(PowerPoint.Hyperlink hyperlink, ICollection<IA11yIncidentItem> incidentItems)
        {

            IA11yIncidentItem errorItem = null;

            if (hyperlink.ScreenTip.Equals(String.Empty))
            {
                //hyperlink.ScreenTip = hyperlink.Address;
                setScreenTip(hyperlink);
                errorItem = CreateErrorItem(hyperlink, resourceManager.GetString("NoQuickInfoForLinkHint"));
            }

            //if (CheckIfLinkExists(item))
            if (errorItem != null)
            {
                incidentItems.Add(errorItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void setScreenTip(PowerPoint.Hyperlink hyperlink)
        {
            if (hyperlink.ScreenTip.Equals(String.Empty))
            {
                if (hyperlink.Address != null)
                {
                    hyperlink.ScreenTip = hyperlink.Address;
                }
                else if (hyperlink.SubAddress != null)
                {
                    string subAddress = hyperlink.SubAddress;                    
                    string slideNumeber = subAddress.Split(',')[1];
                    hyperlink.ScreenTip = resourceManager.GetString("internalLinkToSlide") + slideNumeber;
                }
            }
        }


        private IA11yIncidentItem CreateErrorItem(PowerPoint.Hyperlink hyperlink, string errorMessage)
        {
            PowerPoint.Shape shape = null;
            switch (hyperlink.Type)
            {
                case MsoHyperlinkType.msoHyperlinkShape: // get shape of parent
                    shape = hyperlink.Parent.Parent;
                    break;
                case MsoHyperlinkType.msoHyperlinkRange: // get shape of parent of parend - somehow?
                    shape = hyperlink.Parent.Parent.Parent.Parent;
                    break;
                default: // if no shape can be found
                    break;
            }

            return new A11yIncidentItem(() =>
            {
                if (shape != null)
                {
                    shape.SelectShape();
                }

                Controller.AppController.View.AccessibilityChanges.SetHint(errorMessage);
                ChangePaneController.SetTab(AccessibilityChangesPaneTabs.HINTS);
            },
           resourceManager.GetString("Hyperlink"),
           Topic);
        }

        public void Reset()
        {
        }


    }
}