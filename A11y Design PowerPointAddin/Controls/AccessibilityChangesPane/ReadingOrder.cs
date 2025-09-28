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

using A11y_Design_PowerPointAddin.Helper;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows.Forms;
using System.Drawing;

namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    public partial class ReadingOrder : UserControl
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources",
                                       typeof(ReadingOrder).Assembly);

        private IdVisualizer visualiser = new IdVisualizer();
        Slide slide;

        private Button selectedButton = null;
        private System.Drawing.Point originalButtonLocation;

        private Shape currentShape;
        private Button activeButton = null;
        private Dictionary<Button, Shape> slideShapes = new Dictionary<Button, Shape>();
        private System.Drawing.Point currentDropLocation;
        private System.Drawing.Point startLocation;
        private System.Drawing.Point endLocation;


        private TableLayoutPanelCellPosition draggedCellPosition;
        
        /// <summary>
        /// This is the user interface for the reading order
        /// </summary>
        public ReadingOrder()
        {
            InitializeComponent();
            SetHint(resourceManager.GetString("ReadingOrderDefaultText"));

            readingOrderButton.Text = resourceManager.GetString("ShowReadingOrder");

            readingOrderButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            readingOrderButton.BackColorChanged += (s, e) => {
                readingOrderButton.FlatAppearance.MouseOverBackColor = readingOrderButton.BackColor;
            };
            readingOrderButton.MouseEnter += (s, e) =>
            {
                Color HoverColor = ColorTranslator.FromHtml("#fcc2b1");
                ((Button)s).BackColor = HoverColor;
            };
            readingOrderButton.MouseLeave += (s, e) => { ((Button)s).BackColor = Color.White; };

        }

        /// <summary>
        /// Sets the text of the infobox of the readingOrderPanel
        /// </summary>
        /// <param name="hintText"></param>
        public void SetHint(string hintText)
        {
            hintInfoBox.Text = hintText;
        }

        private void readingOrderButton_Click(object sender, EventArgs e)
        {
            Globals.ThisAddIn.Application.CommandBars.ExecuteMso("SelectionPane");

        }
    }
}
