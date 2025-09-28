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

using System.Collections.Concurrent;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace A11y_Design_PowerPointAddin.Controller
{
    /// <summary>
    /// Single instance for handeling views of current ppt-window 
    /// </summary>
    internal class AppController
    {
        // Singleton instance
        public static AppController Instance { get; } = new AppController();

        // View
        public static View.AppView View { get =>Instance.view; }
        public Controls.AccessibilityChangesPane.AccessibilityChangesPane AccessibilityChanges { get => View.AccessibilityChanges; }
        public Microsoft.Office.Tools.CustomTaskPane AccessibilityChangePane { get => View.AccessibilityChangePane; }

        // Model

        public static Model.AppModel Model { get => Instance.model; }

        /// <summary>
        /// Remove window specific entries from dictionaries
        /// </summary>
        public void ReleaseHandles()
        {
            PowerPoint.DocumentWindow wnd = Globals.ThisAddIn.Application.ActiveWindow;
            Instance.appViews.TryRemove(wnd,out var _);
            Instance.appModels.TryRemove(wnd,out var _);
        }

        private AppController()
        {
            appViews = new ConcurrentDictionary<PowerPoint.DocumentWindow, View.AppView>();
            appModels = new ConcurrentDictionary<PowerPoint.DocumentWindow, Model.AppModel>();
        }

        private ConcurrentDictionary<PowerPoint.DocumentWindow, View.AppView> appViews;
        private ConcurrentDictionary<PowerPoint.DocumentWindow, Model.AppModel> appModels;
        private View.AppView view
        {
            get {
                if (!appViews.TryGetValue(Globals.ThisAddIn.Application.ActiveWindow, out var ac))
                {
                    ac = new View.AppView();
                    appViews.TryAdd(Globals.ThisAddIn.Application.ActiveWindow,ac); // will succeed unless two equal keys are added
                }
                return ac;
            }
        }

        private Model.AppModel model
        {
            get
            {
                if (!appModels.TryGetValue(Globals.ThisAddIn.Application.ActiveWindow, out var ac))
                {
                    ac =  new Model.AppModel();
                    appModels.TryAdd(Globals.ThisAddIn.Application.ActiveWindow,ac);
                }
                return ac;
            }
        }

    }
}
