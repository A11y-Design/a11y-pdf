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

using A11y_Design_PowerPointAddin.Properties;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace A11y_Design_PowerPointAddin.Controls
{

    /// <summary>
    /// Progessbar that is show during scanning for issues
    /// </summary>
    public partial class ActivityWindow : Form
    {
        private int currentSlide = 0;
        private int slideCount = 0;
        private string templateStringCounter = Resources.LabelCheckingXSlidesOfY;
        private static readonly ActivityWindow instance = new ActivityWindow();
        
        /// <summary>
        /// This is the progress bar that is show during the accessibility check
        /// </summary>
        public ActivityWindow()
        {
            InitializeComponent();
            this.Text = Resources.FormTitleErrorChecking;
            CounterText(1);
            Shown += new EventHandler(ActivityWindow_Shown);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        public static ActivityWindow Instance { get { return instance; } }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;


        }

        private void ActivityWindow_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        public BackgroundWorker BackgroundWorker { get { return backgroundWorker1; } }

        public void CounterText(int currentSlide)
        {
            string text = Resources.LabelCheckingXSlidesOfY.Replace("{0}", currentSlide.ToString());
            text = text.Replace("{1}", slideCount.ToString());
            label1.Text = text;
            label1.Refresh();
        }
        public void setStep(int step)
        {
            slideCount = step;
            progressBar1.Step = step;
        }
    }
}