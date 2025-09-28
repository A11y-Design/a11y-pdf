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

namespace A11y_Design_PowerPointAddin.Controller
{
    internal static class ErrorTaskPaneControlller
    {
        /// <summary>
        /// Toggle visibility of ErrorTaskPane and update content if needed
        /// </summary>
        public static void TogglePane()
        {
            if (!AppController.View.ErrorTaskPane.Visible)
            {
                AppController.Model.UpdateIncidents("togglePane");
                //AppController.View.ErrorTaskPane.Visible ^= true;

            }
            else
                AppController.View.ErrorTaskPane.Visible ^= true;
        }
        public static void Show() {
            AppController.View.ErrorTaskPane.Visible ^= true;
        }

        public static void Hide()
        {
            AppController.View.ErrorTaskPane.Visible = false;
        }
    }
}
