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

using Microsoft.Office.Interop.PowerPoint;



namespace A11y_Design_PowerPointAddin.Controller
{
    /// <summary>
    /// This controller registers to selection events that are called globally and redistributes them to a specific window element 
    /// </summary>
    internal static class SelectionController
    {
        internal static void Attach(Application app)
        {
            app.SlideSelectionChanged += Application_SlideSelectionChanged;
            app.WindowSelectionChange += Application_SelectionChange;
        }

        internal static void Detach(Application app)
        {
            app.SlideSelectionChanged -= Application_SlideSelectionChanged;
            app.WindowSelectionChange -= Application_SelectionChange;
        }

        
        internal static void Application_SlideSelectionChanged(SlideRange s)
        {
            if (s.Count == 1 && CanUpdateAlttext())
                AppController.Instance.AccessibilityChanges.rebuild_AlternativeTextList();
        }

        internal static void Application_SelectionChange(Selection s)
        {
            if (CanUpdateAlttext()) // Fix this exception Selection(unknown member) : Invalid request.  This view does not support selection.
            {
                if ((s.Type == PpSelectionType.ppSelectionShapes || s.Type == PpSelectionType.ppSelectionText)
                                && s.ShapeRange.Count == 1 && CanUpdateAlttext())
                {
                    AppController.Instance.AccessibilityChanges.onSelectShape(s.ShapeRange[1]);
                }
                // after a delete, the selction changes to nothing
                else if (s.Type == PpSelectionType.ppSelectionNone
                    && s.Application.ActiveWindow.ActivePane.ViewType == PpViewType.ppViewSlide
                    && CanUpdateAlttext())
                {
                    AppController.Instance.AccessibilityChanges.onSelectNothing();
                }
            }
        }

        private static bool CanUpdateAlttext()
        {
            // do not execute if AlternativetextPane is not visible does not work
            if (!AppController.Instance.AccessibilityChangePane.Visible)
                return false;
            // do not execute on selection on displayed line between slides 
            if (Globals.ThisAddIn.Application.ActiveWindow.ViewType != PpViewType.ppViewNormal)
                return false;

            switch (Globals.ThisAddIn.Application.ActiveWindow.Panes[2].ViewType)
            {
                case PpViewType.ppViewSlide:
                    return true;
                default:
                    return false;
            }
        }


    }
}
