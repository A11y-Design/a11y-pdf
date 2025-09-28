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

using A11y_Design_PowerPointAddin.Controller;
using A11y_Design_PowerPointAddin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;



namespace A11y_Design_PowerPointAddin.Controls
{
    public partial class ErrorListPane : UserControl
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources",
                                               typeof(ErrorListPane).Assembly);

        private readonly Image errorIcon = Resources.error_circle;
        private readonly Image hintIcon = Resources.hintIcon;

        public Button ActiveCategory
        {
            get { return this.errorsAccordeon.ActiveCategory; }
        }

        public Accordeon ErrorAccordeon => this.errorsAccordeon;

        /// <summary>
        /// This pane lists all accessibility issues
        /// </summary>
        public ErrorListPane()
        {
            InitializeComponent();
            AppController.Model.RegisterOnIncidentUpdate(SetIncidents);
            AppController.Model.UpdateIncidents("init");

            refreshButton.FlatAppearance.MouseOverBackColor = refreshButton.BackColor;
            refreshButton.BackColorChanged += (s, e) =>
            {
                refreshButton.FlatAppearance.MouseOverBackColor = refreshButton.BackColor;
            };

            refreshButton.MouseEnter += (s, e) =>
            {
                Color HoverColor = ColorTranslator.FromHtml("#fcc2b1");
                ((Button)s).BackColor = HoverColor;
            };
            refreshButton.MouseLeave += (s, e) => { ((Button)s).BackColor = Color.White; };

            refreshButton.MouseClick += (object sender, MouseEventArgs e) =>
            {
                AppController.Model.UpdateIncidents("refreshButton");
                DoUpdateSplitContainerSize();
            };
            errorsAccordeonHeadline.Text = A11y_Design_PowerPointAddin.Properties.Resources.ErrorsAccordeonHeadline;
            hintsAccordeonHeadline.Text = A11y_Design_PowerPointAddin.Properties.Resources.HintsAccordeonHeadline;
            noErrorsLabel.Text = A11y_Design_PowerPointAddin.Properties.Resources.NoErrorsLabel;
            noHintsLabel.Text = A11y_Design_PowerPointAddin.Properties.Resources.NoHintsLabel;


            // Set correct height of the errors Accordeon
            splitContainer_Incidents.SizeChanged += UpdateSplitContainerSize;
            errorsAccordeon.ContentSizeChanged += UpdateSplitContainerSize;
            hintsAccordeon.ContentSizeChanged += UpdateSplitContainerSize;
        }



        public void ShowEmptyListText(Label label, bool isEmpty)
        {
            label.Visible = isEmpty;
        }
        

        private void UpdateSplitContainerSize(object sender, EventArgs e)
        {
            DoUpdateSplitContainerSize();
        }



        private int GetPanelControlsHeight(SplitterPanel panel, Accordeon accordeon)
        {
            int totalHeight = 0;

            foreach (Control control in panel.Controls)
            {
                totalHeight += control.Height; // + control.Margin.Top + control.Margin.Bottom;

                if (control.Name == accordeon.Name)
                {
                    totalHeight = totalHeight - accordeon.Height + accordeon.TotalHeight;
                }
            }

            return totalHeight;
        }

        public void ForAccordeon()
        {
            SetIncidents();
            DoUpdateSplitContainerSize();
        }
        private void DoUpdateSplitContainerSize()
        {
            int panel1ControlsHeight = GetPanelControlsHeight(splitContainer_Incidents.Panel1, errorsAccordeon);
            int panel2ControlsHeight = GetPanelControlsHeight(splitContainer_Incidents.Panel2, hintsAccordeon);

            int containerClientsHeight = splitContainer_Incidents.Height - splitContainer_Incidents.SplitterWidth - 9;

            if (panel1ControlsHeight + panel2ControlsHeight > containerClientsHeight)
            {
                if (panel1ControlsHeight < containerClientsHeight / 2)
                {
                    splitContainer_Incidents.SplitterDistance = panel1ControlsHeight;
                }
                else if (panel2ControlsHeight < containerClientsHeight / 2)
                {
                    splitContainer_Incidents.SplitterDistance = containerClientsHeight - panel2ControlsHeight -
                        splitContainer_Incidents.SplitterWidth - 2;
                }
                else
                {
                    splitContainer_Incidents.SplitterDistance = containerClientsHeight / 2;
                }
            }
            else
            {
                splitContainer_Incidents.SplitterDistance = panel1ControlsHeight;
            }
        }

        private void SetIncidents()
        {
            bool hasNoErrorInList = true;
            bool hasNoHintInList = true;
            List<IA11yIncidentItem> incidents = AppController.Model.Incidents;
            Dictionary<String, List<String>> errorTextDic = new Dictionary<String, List<String>>();
            List<String> listOfErrorTexts = new List<String>(); //to avoid multiple error buttons with the same error
            Cursor.Current = Cursors.WaitCursor;

            //tableLayoutPanel.RowStyles.Clear();
            errorsAccordeon.Clear();
            hintsAccordeon.Clear();

            Accordeon currentAccordeon = null;
            foreach (var topic in incidents.GroupBy(i => i.Topic).Select(s => s.Key))
            {
                currentAccordeon = (topic.Level == TopicLevel.ERROR) ? errorsAccordeon : hintsAccordeon;                
                if (topic.IncidentsRO.Count > 0) // this case occurs if a single slide is checked
                {
                    if (topic.Level == TopicLevel.ERROR) hasNoErrorInList = false;
                    if (topic.Level == TopicLevel.WARNING) hasNoHintInList = false;
                    Button categoryButton = currentAccordeon.AddCategory(topic.Name);
                    foreach (var incident in topic.IncidentsRO)
                    {
                        string errorText = incident.Name;

#if DEBUG
                        // just for testing if the incidents are listed in more than one category
                        if (errorTextDic.ContainsKey(errorText))
                        {
                            errorTextDic[errorText].Add(topic.Name);
                        }
                        else
                        {
                            errorTextDic.Add(errorText, new List<string>() { topic.Name });
                        }
#endif
                        listOfErrorTexts.Add(errorText);
                        Button errorButton = CreateErrorButton((topic.Level == TopicLevel.WARNING ? hintIcon : errorIcon), errorText, () => incident.Select());

                        currentAccordeon.AddControlToCategory(categoryButton, errorButton);
                        currentAccordeon.SetSize();

                    }
                }
                currentAccordeon.SetPadding();
            }
            SuspendLayout();
            ResumeLayout(false);

            ShowEmptyListText(noErrorsLabel, hasNoErrorInList);
            ShowEmptyListText(noHintsLabel, hasNoHintInList);


            Cursor.Current = Cursors.Default;
        }


        private Button CreateErrorButton(Image icon, string errorText, Action action)
        {
            Button errorButton = new Button
            {
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#212121"),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "   " + errorText,
                ImageAlign = ContentAlignment.MiddleLeft,
                Image = icon,
                TextImageRelation = TextImageRelation.ImageBeforeText
            };

            errorButton.Click += (s, e) => { action(); };

            errorButton.FlatAppearance.MouseOverBackColor = errorButton.BackColor;
            errorButton.BackColorChanged += (s, e) =>
            {
                errorButton.FlatAppearance.MouseOverBackColor = errorButton.BackColor;
            };

            errorButton.MouseEnter += (s, e) =>
            {
                Color HoverColor = ColorTranslator.FromHtml("#fcc2b1");
                ((Button)s).BackColor = HoverColor;
            };
            errorButton.MouseLeave += (s, e) => { ((Button)s).BackColor = Color.White; };
            errorButton.FlatAppearance.BorderSize = 0;

            return errorButton;
        }

    }
}
