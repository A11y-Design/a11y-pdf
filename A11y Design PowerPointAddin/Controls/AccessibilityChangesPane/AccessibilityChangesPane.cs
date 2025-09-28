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
using System.Windows.Forms;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Interop.PowerPoint;

namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    /// <summary>
    /// Enums of table
    /// </summary>
    public enum AccessibilityChangesPaneTabs
    {
        METADATA,
        READINGORDER,
        ALTERNATVETEXT,
        HINTS
    }

    public partial class AccessibilityChangesPane : UserControl
    {

        private AlternativeText alternativeTextControl;
        private DocumentMetaData documentMetaDataControl;
        private HintArea hintAreaControl;
        private ReadingOrder readingOrderControl;

        /// <summary>
        /// internal counter for shapes to decide when list needs to be updated
        /// </summary>
        private int current_SlideShapeCount;

        public AccessibilityChangesPane()
        {
            current_SlideShapeCount = 0;
            InitializeComponent();

            documentMetaDataControl = new DocumentMetaData();
            documentMetaDataControl.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(documentMetaDataControl);

            readingOrderControl = new ReadingOrder();
            readingOrderControl.Dock = DockStyle.Fill;
            tabPage2.Controls.Add(readingOrderControl);

            alternativeTextControl = new AlternativeText();
            alternativeTextControl.Dock = DockStyle.Fill;
            tabPage3.Controls.Add(alternativeTextControl);

            hintAreaControl = new HintArea();
            hintAreaControl.Dock = DockStyle.Fill;
            tabPage4.Controls.Add(hintAreaControl);

            tabControlRevision.Appearance = TabAppearance.Normal;            

            tabPage1.Text = Properties.Resources.MetaData;
            tabPage2.Text = Properties.Resources.ReadingOrder;
            tabPage3.Text = Properties.Resources.Alternativetexts;
            tabPage4.Text = Properties.Resources.btnDocumentHints;
        }

        internal void SetAlternativeText(string text)
        {
            alternativeTextControl.Change_AltTextLabel(text);
        }

        internal void SetHint(string text)
        {
            hintAreaControl.SetHint(text);
        }


        public void SetMetaData()
        {
            if (Helper.MetaData.GetByKey(Helper.MetaData.Key.Title) != string.Empty)
            {
                documentMetaDataControl.setDocumentTitleTextBox(Helper.MetaData.GetByKey(Helper.MetaData.Key.Title));
            }

            if (Helper.MetaData.GetByKey(Helper.MetaData.Key.Author) != string.Empty)
            {
                documentMetaDataControl.setDocumentAuthorTextBox(Helper.MetaData.GetByKey(Helper.MetaData.Key.Author));
            }

            if (Helper.MetaData.GetByKey(Helper.MetaData.Key.Comments) != string.Empty)
            {
                documentMetaDataControl.setDocumentCommentsTextBox(Helper.MetaData.GetByKey(Helper.MetaData.Key.Comments));
            }

            if (Helper.MetaData.GetByKey(Helper.MetaData.Key.Keywords) != string.Empty)
            {
                documentMetaDataControl.setDocumentKeywordsTextBox(Helper.MetaData.GetByKey(Helper.MetaData.Key.Keywords));
            }
            documentMetaDataControl.setDocumentCopyrightTextBox();
            documentMetaDataControl.setDocumentCopyrightCombobox();
            documentMetaDataControl.setDocumentCopyrightUrlTextBox();
        }


      

        // proxy into UserControl - not exacly needed here
        public void SelectTab(string tab)
        {
            tabControlRevision.SelectedTab = tabControlRevision.TabPages[tab];
        }

        /// <summary>
        /// returns selected Tab of ChangesPane (can be null)
        /// </summary>
        /// <returns></returns>
        public TabPage getSelectedTab()
        {
            return tabControlRevision.SelectedTab;
        }

        /// <summary>
        /// Create new alternative text list based on current slide
        /// </summary>
        public void rebuild_AlternativeTextList()
        {
            PowerPoint.Slide slide;
            try { slide = Globals.ThisAddIn.Application.ActiveWindow.View.Slide; }
            catch (Exception ex)
            {
                return;
            }

            current_SlideShapeCount = getRelevantShapeCount();

            alternativeTextControl.update_AlternativeTextList(slide);
        }


        // on selection changed to shape
        internal void onSelectShape(PowerPoint.Shape s)
        {
            
            // when a new shape is added -> rebuild list
            if (current_SlideShapeCount != getRelevantShapeCount())
            {
                rebuild_AlternativeTextList();                
            }

            // set button highlight
            alternativeTextControl.bypassTextChanged = true;
            alternativeTextControl.bypassArtifactCheck = true;
            alternativeTextControl.setSelection(s);
            alternativeTextControl.bypassTextChanged = false;
            alternativeTextControl.bypassArtifactCheck = false;
        }


        // on selection changed to nothing
        internal void onSelectNothing()
        {
            if (current_SlideShapeCount != getRelevantShapeCount())
                rebuild_AlternativeTextList();
        }

        /// <summary>
        /// Get Number of shapes that are relevant for alttext list refresh.
        /// </summary>
        /// <returns></returns>
        private static int getRelevantShapeCount()
        {

            PowerPoint.Slide slide = Globals.ThisAddIn.Application.ActiveWindow.View.Slide;
            int ret = slide.Shapes.Count;
            foreach (PowerPoint.Shape item in slide.Shapes.Placeholders)
            {
                // Placeholders without tables or pictures have the type Autoshape
                if (item.PlaceholderFormat.ContainedType == Microsoft.Office.Core.MsoShapeType.msoAutoShape) ret--;
            }
            return ret;
        }


    }
}
