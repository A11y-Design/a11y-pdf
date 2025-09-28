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
using System.Text.RegularExpressions;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using System.Net;
using System.Text;
using System.IO;
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
    /// Incident for Hyperlinks checks wether a links exist or not
    /// is not used now
    /// </summary>
    class A11yIncidentType_HyperlinkExists : IA11yIncidentCreator<PowerPoint.Hyperlink>
    {
        private A11yIncidentTopic Topic { get; }

        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_Table).Assembly);


        /// <summary>
        /// External topic without rights to add incidents
        /// </summary>
        IA11yIncidentTopic IA11yIncidentCreator<PowerPoint.Hyperlink>.Topic => Topic;

        public A11yIncidentType_HyperlinkExists(A11yIncidentTopic topic)
        {
            Topic = topic;
        }

        public bool TryCreate(PowerPoint.Hyperlink hyperlink, ICollection<IA11yIncidentItem> incidentItems)
        {

            IA11yIncidentItem errorItem = null;
            if (CheckIfLinkIsUrl(hyperlink))
            {
                if (hyperlink.ScreenTip.Equals(String.Empty))
                {
                    //hyperlink.ScreenTip = hyperlink.Address;
                    errorItem = CreateErrorItem(hyperlink, resourceManager.GetString("NoQuickInfoForLinkHint"));
                }
            }

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

        /// <summary>
        /// Checks if Link is an URL
        /// </summary>
        /// <param name="hyperlink"></param>
        /// <returns></returns>
        private static bool CheckIfLinkIsUrl(PowerPoint.Hyperlink hyperlink)
        {
            string subAddress = String.Empty;
            string textToDisplay = String.Empty;
            if (hyperlink != null)
            {

                subAddress = hyperlink.SubAddress;
                textToDisplay = hyperlink.TextToDisplay;
            }
            if (hyperlink.Address != null || // address is a URL or link to another file
                (subAddress != String.Empty && textToDisplay != String.Empty)) // internal link to another Slide
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CheckIfLinkExists(PowerPoint.Hyperlink hyperlink)
        {
            if (Regex.IsMatch(hyperlink.Address, @"^(http|https)://"))
            {
                string val = GetTitleOfUrl(hyperlink.Address);
                if (val != null)
                {
                    hyperlink.ScreenTip = hyperlink.ScreenTip.Length == 0 ? val : hyperlink.ScreenTip;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // link to file
                return CheckLinkToFile(hyperlink.Address);
            }
        }

        public static string GetTitleOfUrl(string url)
        {
            string title = String.Empty;
            using (MyClient client = new MyClient())
            {

                client.HeadOnly = true;
                string type;
                try
                {
                    type = client.ResponseHeaders["content-type"];
                    client.HeadOnly = false;
                }
                catch (WebException ex)
                {
                    return null;
                }

                // check 'tis not binary... we'll use text/, but could
                // check for text/html
                if (type.StartsWith(@"text/"))
                {
                    string text = client.DownloadString(url);
                    title = Regex.Match(text, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;

                    Console.WriteLine(text);
                }
            }
            byte[] bytes = Encoding.GetEncoding(0).GetBytes(title);
            string v = Encoding.UTF8.GetString(bytes);
            title = v;
            return title;
        }

        public static bool CheckLinkToFile(string url)
        {

            string pathPresentation = Globals.ThisAddIn.Application.ActivePresentation.Path;
            string pathFile = Path.Combine(pathPresentation, url);
            return System.IO.File.Exists(pathFile);
        }

        private IA11yIncidentItem CreateErrorItem(PowerPoint.Hyperlink hyperlink, string errorMessage)
        {
            PowerPoint.Shape shape = null;
            switch (hyperlink.Type)
            {
                case MsoHyperlinkType.msoHyperlinkShape: // get shape of parent
                    shape = hyperlink.Parent.Parent;
                    break;
                case MsoHyperlinkType.msoHyperlinkRange: // get shape of parent of parent - somehow?
                    shape = hyperlink.Parent.Parent.Parent.Parent;
                    break;
                default:
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

        internal class MyClient : WebClient
        {
            public bool HeadOnly { get; set; }
            protected override WebRequest GetWebRequest(Uri address)
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;
                WebRequest req = base.GetWebRequest(address);
                if (HeadOnly && req.Method == "GET")
                {
                    req.Method = "HEAD";
                }
                return req;
            }
        }
    }
}