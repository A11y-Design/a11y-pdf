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

namespace A11y_Design_PowerPointAddin.Controller
{
    internal static class ChangePaneController
    {

        /// <summary>
        /// Set Tab in Change Pane
        /// </summary>
        /// <param name="tab">tab type</param>
        /// <param name="hide_if_same">hide pane if tab is already selected (toggle)</param>
        public static void SetTab(AccessibilityChangesPaneTabs tab, bool hide_if_same = true)
        {

            string tabName = tab.ToString();

            if (AppController.Instance.AccessibilityChanges.getSelectedTab()?.Name == tabName && hide_if_same)
                AppController.Instance.AccessibilityChangePane.Visible ^= true; // toggle bool
            else
            {
                AppController.Instance.AccessibilityChangePane.Visible = true;
                AppController.Instance.AccessibilityChanges.SetMetaData();
                AppController.Instance.AccessibilityChanges.rebuild_AlternativeTextList();
                //AppController.Instance.AccessibilityChanges.rebuild_ReadingOrderList();

            }
            AppController.Instance.AccessibilityChanges.SelectTab(tabName);

        }

    }
}
