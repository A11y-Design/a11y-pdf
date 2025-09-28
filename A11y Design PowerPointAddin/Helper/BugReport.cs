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
using System.Diagnostics;
using System.Resources;

namespace A11y_Design_PowerPointAddin.Helper
{
    internal class BugReport
    {

        
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_ShapeAltText).Assembly);
        String messageBody = resourceManager.GetString("SendReport");
        
        private string recipient = "bug@a11y-design.de";
        private string subject = "Bug Report";
        private string lb = "%0D%0A%0D%0A";
        private string msg = "Email erfolgreich verschickt";
        public void btnEmail_Click(String Stack)
            {
            String stack = Stack;
            //currently works by saving the pdf and powerpoint on the desktop which the user then has to manually attach            
            string command = "mailto:" + recipient + "?subject=" + subject + "&body=" + messageBody + lb + stack;
            Process.Start(command);
            }

    }
}
