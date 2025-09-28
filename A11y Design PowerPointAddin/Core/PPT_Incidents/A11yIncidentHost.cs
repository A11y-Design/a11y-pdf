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

using A11y_Design_PowerPointAddin.Controls;
using A11y_Design_PowerPointAddin.Core.PPT_Incidents;
using A11y_Design_PowerPointAddin.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;



namespace A11y_Design_PowerPointAddin
{
    /// <summary>
    /// Contains instances of A11yIncidentFactoríes and allows updating of Incidents within the AppModel
    /// <br><b>New types of incidents can be added here!</b></br>
    /// <para>(Replaces former PDFChecker)</para>
    /// </summary>
    public class A11yIncidentHost
    {
        // Instances of A11yIncidentFactories
        private A11yIncidentFactory<PowerPoint.Presentation> presentation_factory { get; }
        private A11yIncidentFactory<PowerPoint.Slide> slide_factory { get; }
        private A11yIncidentFactory<PowerPoint.Hyperlink> hyperlink_factory { get; }
        private A11yIncidentFactory<PowerPoint.Shape> shape_factory { get; }

        private ActivityWindow activityWindow;
        private Task IncidentUpdateTask { get; }

        // Additional factories are required, if incidents on new types of elements are added

        public A11yIncidentHost()
        {
            // IncidentUpdateTask = new Task(() => collectIncidents());
            // Create factories and resource manager ...
            var _rm = new ResourceManager(typeof(Properties.Resources));
            presentation_factory = new A11yIncidentFactory<PowerPoint.Presentation>();
            shape_factory = new A11yIncidentFactory<PowerPoint.Shape>();
            hyperlink_factory = new A11yIncidentFactory<PowerPoint.Hyperlink>();
            slide_factory = new A11yIncidentFactory<PowerPoint.Slide>();
            // added factories ...




            // ###############################
            // # REGISTER NEW INCIDENTS HERE #
            // ###############################
            //
            // You can add incidents in multiple ways...
            // #1 simple incidents can be added by using the bare A11yIncidentType implementation
            //    Delegates (functions) for checking the incident, naming, and selection need to be passed
            //    You can define simple methods ...
            var t_metadata = new A11yIncidentTopic("Fehlende Metadaten", TopicLevel.ERROR);
            presentation_factory.Register(new A11yIncidentType<PowerPoint.Presentation>(
                condition: p => Helper.MetaData.GetByKey(Helper.MetaData.Key.Title, p) == String.Empty,
                itemNaming: p => "Titel",
                selectAction: p =>
                {
                    Controller.AppController.Instance.AccessibilityChanges.SetMetaData();
                    Controller.ChangePaneController.SetTab(Controls.AccessibilityChangesPane.AccessibilityChangesPaneTabs.METADATA, false);
                },
                topic: t_metadata
            ));

            presentation_factory.Register(new A11yIncidentType<PowerPoint.Presentation>(
                condition: p => Helper.MetaData.GetByKey(Helper.MetaData.Key.Author, p) == String.Empty,
                itemNaming: p => "Autor",
                selectAction: p =>
                {
                    Controller.AppController.Instance.AccessibilityChanges.SetMetaData();
                    Controller.ChangePaneController.SetTab(Controls.AccessibilityChangesPane.AccessibilityChangesPaneTabs.METADATA, false);
                },
                topic: t_metadata
            )); 



            // #2 Create a special IncidentType by inheriting from A11yIncidentType, if certain elements of the A11yIncidentType are the same with a multiple incidents
            shape_factory.Register(new A11yIncidentType_Shape(
                condition: s => s.GetNestedType() == Office.MsoShapeType.msoTextBox
                && s.Line.Visible == Office.MsoTriState.msoTrue && s.AlternativeText == string.Empty && s.TextFrame2.TextRange.Text == String.Empty,
                topic: new A11yIncidentTopic(_rm.GetString("TextBoxBorder"), TopicLevel.WARNING)));

            // Optional: Create indipendent Toppics
            var t_altImg = new A11yIncidentTopic(_rm.GetString("MissingAltTextForImage"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoPicture, t_altImg));
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoLinkedPicture, t_altImg));
            shape_factory.Register(new A11yIncidentType_NotSupportedShapeTypes());

            var t_altSArt = new A11yIncidentTopic(_rm.GetString("MissingAltTextForSmartArt"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoSmartArt, t_altSArt));


            var t_altMedia = new A11yIncidentTopic(_rm.GetString("MissingAltTextForMedia"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoMedia, t_altMedia));
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoWebVideo, t_altMedia));

            var t_altForm = new A11yIncidentTopic(_rm.GetString("MissingAltTextForForm"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_AutoShapeAltText(t_altForm));
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoLine, t_altForm));
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoFreeform, t_altForm));

            var t_altObj = new A11yIncidentTopic(_rm.GetString("MissingAltTextForObject"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoShapeTypeMixed, t_altObj));

            var t_altDia = new A11yIncidentTopic(_rm.GetString("MissingAltTextForDiagram"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoChart, t_altDia));
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoDiagram, t_altDia));

            var t_altGrp = new A11yIncidentTopic(_rm.GetString("MissingAltTextForGroup"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_ShapeAltText(Office.MsoShapeType.msoGroup, t_altGrp));

            var t_byObj = new A11yIncidentTopic(_rm.GetString("ErrorByObject"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_ShapeHint(Office.MsoShapeType.msoEmbeddedOLEObject, t_byObj));
            shape_factory.Register(new A11yIncidentType_ShapeHint(Office.MsoShapeType.msoLinkedOLEObject, t_byObj));
            shape_factory.Register(new A11yIncidentType_ShapeHint(Office.MsoShapeType.msoOLEControlObject, t_byObj));

            var t_byInk = new A11yIncidentTopic(_rm.GetString("ErrorByInk"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_ShapeHint(Office.MsoShapeType.msoInk, t_byInk));
            shape_factory.Register(new A11yIncidentType_ShapeHint(Office.MsoShapeType.msoInkComment, t_byInk));

            var t_textBox = new A11yIncidentTopic(_rm.GetString("TextBoxFilled"), TopicLevel.ERROR);
            shape_factory.Register(new A11yIncidentType_Textbox(Office.MsoShapeType.msoTextBox, t_textBox));


            // #3 Creating own implementaions of the generic IA11yIncidentCreator-Interface allows the creation of complicated IncidentTypes
            hyperlink_factory.Register(new A11yIncidentType_HyperlinkAlternativeDescription(
                topic: new A11yIncidentTopic(_rm.GetString("AlternativeDescriptionForLinkNeeded"), TopicLevel.WARNING)));

            slide_factory.Register(new A11yIncidentType_ReadingOrder(_rm.GetString("CheckReadingOrder"),
                new A11yIncidentTopic(_rm.GetString("ReadingOrder"), TopicLevel.WARNING)));


            shape_factory.Register(new A11yIncidentType_Table());
            shape_factory.Register(new A11yIncidentType_AnimatedElement());
            shape_factory.Register(new A11yIncidentType_MissingFont());



        }

        private void UpdateMetaDataIncident()
        {
            PowerPoint.Presentation pres = Globals.ThisAddIn.Application.ActivePresentation;
            presentation_factory.AppendIncidentsOnItem(pres);

            List<IA11yIncidentItem> incidents = new List<IA11yIncidentItem>();
            incidents.AddRange(presentation_factory.Incidents);
            incidents.AddRange(slide_factory.Incidents);
            incidents.AddRange(hyperlink_factory.Incidents);
            incidents.AddRange(shape_factory.Incidents);
            Controller.AppController.Model.Incidents.AddRange(incidents);
            Button activeCategoryBtn = Controller.AppController.View.ErrorListPane.ActiveCategory;
            Controller.AppController.Model.CallOnIncidentUpdate();
            Controller.AppController.View.ErrorListPane.ErrorAccordeon.CategoryButton_Click(activeCategoryBtn, new EventArgs());
        }

        /// <summary>
        /// Update Incidents of a slide
        /// </summary>
        /// <param name="slideNumber">Slide number that should be checked for incidents</param>
        public void UpdateAltTextIncidents(int slideNumber)
        {
            PowerPoint.Presentation pres = Globals.ThisAddIn.Application.ActivePresentation;
            shape_factory.RemoveIncidentsForSlide(slideNumber);
            presentation_factory.AppendIncidentsOnItem(pres);
            collectIncidents("altText", slideNumber);
            List<IA11yIncidentItem> incidents = new List<IA11yIncidentItem>();
            incidents.AddRange(presentation_factory.Incidents);
            incidents.AddRange(slide_factory.Incidents);
            incidents.AddRange(hyperlink_factory.Incidents);
            incidents.AddRange(shape_factory.Incidents);
            Controller.AppController.Model.Incidents.AddRange(incidents);
            Button activeCategoryBtn = Controller.AppController.View.ErrorListPane.ActiveCategory;
            Controller.AppController.Model.CallOnIncidentUpdate();
            Controller.AppController.View.ErrorListPane.ErrorAccordeon.CategoryButton_Click(activeCategoryBtn, new EventArgs());
        }

        /// <summary>
        /// Go through all elements of the current presentation and show the to the factories.
        /// Add retrieved incidents to the AppModel.
        /// </summary>
        public void UpdateIncidents(String method = "default", int slideNumber = 0)
        {
            // checking performance to evaluate async execution ..      
            if (method.Equals("altText"))
            {
                UpdateAltTextIncidents(slideNumber);
                return;
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PowerPoint.Presentation pres = Globals.ThisAddIn.Application.ActivePresentation;
            if (!method.Equals("init") && !method.Equals("metaData"))
            {
                activityWindow = new ActivityWindow();
                activityWindow.Show();
                activityWindow.setStep(pres.Slides.Count);
            }
            presentation_factory.Reset();

            shape_factory.Reset();
            hyperlink_factory.Reset();
            slide_factory.Reset();





            presentation_factory.AppendIncidentsOnItem(pres);

            collectIncidents(method, slideNumber);


            TimeSpan ts = stopWatch.Elapsed; // Get the elapsed time as a TimeSpan value.
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Debug.WriteLine($"# Incident collection done at: {elapsedTime}");

            //... and collect their incidents
            List<IA11yIncidentItem> incidents = new List<IA11yIncidentItem>();
            incidents.AddRange(presentation_factory.Incidents);
            incidents.AddRange(shape_factory.Incidents);
            incidents.AddRange(hyperlink_factory.Incidents);
            incidents.AddRange(slide_factory.Incidents);


            // Set incidents on AppModel
            Controller.AppController.Model.ClearIncidentList();
            Controller.AppController.Model.Incidents.AddRange(incidents);
            Controller.AppController.Model.CallOnIncidentUpdate();

            //  further performance diagnostics
            stopWatch.Stop();
            ts = stopWatch.Elapsed; // Get the elapsed time as a TimeSpan value.
            // Format and display the TimeSpan value.
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Debug.WriteLine("# Total update time: " + elapsedTime);
            if (method.Equals("togglePane") || method.Equals("refreshButton"))
            {
                if (!method.Equals("refreshButton")) Controller.ErrorTaskPaneControlller.Show();
                activityWindow.Dispose();
            }
        }

        void collectIncidents(string method = "default", int slideNumber = 0)
        {

            PowerPoint.Presentation pres = Globals.ThisAddIn.Application.ActivePresentation;
            //Do not forget to call your new factories here!
            int slideCount = pres.Slides.Count;

            if (!method.Equals("init"))
            {
                if (activityWindow == null)
                {
                    activityWindow = new ActivityWindow();
                }
                activityWindow.setStep(slideCount);
            }
            foreach (PowerPoint.Slide _slide in pres.Slides)
            {
                if (method.Equals("altText") && slideNumber != _slide.SlideNumber)
                {
                    continue;
                }

                slide_factory.AppendIncidentsOnItem(_slide);

                foreach (PowerPoint.Hyperlink _hyperlink in _slide.Hyperlinks)
                {
                    hyperlink_factory.AppendIncidentsOnItem(_hyperlink);
                }
                // Check Shapes
                foreach (PowerPoint.Shape _shape in _slide.Shapes)
                {
                    shape_factory.AppendIncidentsOnItem(_shape);
                }
                if (method.Equals("init") || method.Equals("altText")) break; //only for 1 slide ord update alt text            
                activityWindow.CounterText(_slide.SlideNumber);
                activityWindow.BackgroundWorker.ReportProgress(_slide.SlideNumber * 100 / slideCount);

            }

        }

    }
}