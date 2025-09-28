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

using Microsoft.Office.Tools;
using A11y_Design_PowerPointAddin.Controls;
using A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane;

namespace A11y_Design_PowerPointAddin.View
{
    /// <summary>
    /// Contains all GUI-Instances that can not be shared between ppt-windows
    /// AppView instances should only be held by the AppController 
    /// </summary>
    internal class AppView
    {
        public AccessibilityChangesPane AccessibilityChanges { get; }
        public ErrorListPane ErrorListPane { get; }

        public CustomTaskPane AccessibilityChangePane { get; }
        public CustomTaskPane ErrorTaskPane { get; }

        private static int defaultWidth = 500; 

        public AppView()
        {
            AccessibilityChanges = new AccessibilityChangesPane();
            AccessibilityChangePane = Globals.ThisAddIn.CustomTaskPanes.Add(AccessibilityChanges, Properties.Resources.LabelEditPane);
            AccessibilityChangePane.Width = defaultWidth;

            ErrorListPane = new ErrorListPane();
            ErrorTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(ErrorListPane, Properties.Resources.FoundProblems);
            ErrorTaskPane.Width = defaultWidth;

        }


    }
}
