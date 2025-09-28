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

#define EVENT_DEBUG
//#define TEST_ENGLISH
//#define TEST_GERMAN
using A11y_Design_PowerPointAddin.Controller;
using System;
using System.Diagnostics;
using System.IO;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;


namespace A11y_Design_PowerPointAddin
{
    public partial class ThisAddIn
    {

        static string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "a11y PDF");
        private static PowerPoint.Application app;
        private static int buildNumber;
        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
#if TEST_ENGLISH
            CultureInfo info = new CultureInfo("en");
            Thread.CurrentThread.CurrentUICulture = info;
#endif
#if TEST_GERMAN
            CultureInfo info = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = info;
#endif
            //for get the current version office              
            #region get office version
            app = new PowerPoint.Application();
            buildNumber = int.Parse(app.Build);
            app.Quit();
            #endregion

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);

            }

#if EVENT_DEBUG
            Application.SlideSelectionChanged += (a) =>
            {
                Debug.WriteLine("SlideSelectionChanged");
            };
            Application.AfterShapeSizeChange += (s) =>
            {
                Debug.WriteLine("AfterShapeSizeChange");
            };
            Application.AfterDragDropOnSlide += (a, b, c) =>
            {
                Debug.WriteLine("AfterDragDropOnSlide");
            };
            Application.WindowSelectionChange += (a) =>
            {
                Debug.WriteLine("WindowSelectionChange");
            };
            Application.PresentationSync += (a, b) =>
            {
                Debug.WriteLine("PresentationSync");
            };
#endif


            Application.PresentationBeforeSave += PresentationBeforeSaveHandler;

            SelectionController.Attach(Application);
            Application.PresentationClose += p =>
            {
                AppController.Instance.AccessibilityChangePane.Visible = false;
                ErrorTaskPaneControlller.Hide();
                AppController.Instance.ReleaseHandles();
            };
        }

        /// <summary>
        /// check if app is Office 2016
        /// </summary>
        /// <returns></returns>

        public bool IsOffice2016()
        {
            return buildNumber < 14000;
        }

        /// <summary>
        /// check if app is Office365
        /// </summary>
        /// <returns></returns>

        public bool IsOffice365()
        {
            return buildNumber >= 18000;
        }

        public bool IsOfficeLTSC2021()
        {
            return buildNumber >= 14000 && buildNumber < 18000;
        }



        public string AppDataPath { get { return appDataPath; } }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {

            string[] tempFiles = Directory.GetFiles(appDataPath, "*.pptx");
            foreach (string fileName in tempFiles)
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception ex)
                {

                }
            }

        }

        private void PresentationBeforeSaveHandler(PowerPoint.Presentation presentation, ref bool Cancel)
        {

        }

        #region Von VSTO generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
            this.Application.ProtectedViewWindowOpen += Application_ProtectedViewWindowOpen;
            this.Application.ProtectedViewWindowDeactivate += Application_ProtectedViewWindowDeactivate;
            this.Application.PresentationOpen += Application_PresentationOpen;
        }

        private void Application_PresentationOpen(PowerPoint.Presentation Pres)
        {
            Helper.Artifact.LoadAllArtifacts(); // needed to load all marked artifacts from custom document properties
        }

        private void Application_ProtectedViewWindowDeactivate(PowerPoint.ProtectedViewWindow ProtViewWindow)
        {
            Globals.Ribbons.AddInRibbon.EnableButtons(true);
        }

        private void Application_ProtectedViewWindowOpen(PowerPoint.ProtectedViewWindow ProtViewWindow)
        {
            Globals.Ribbons.AddInRibbon.EnableButtons(false);
        }


        #endregion
    }
}