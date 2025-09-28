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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Linq;
using System.Windows.Forms;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using System.Diagnostics;
using A11y_Design_PowerPointAddin.Controller;

namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    public partial class AlternativeText : UserControl
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_ShapeAltText).Assembly);

        private Dictionary<Button, PowerPoint.Shape> slideShapes = new Dictionary<Button, PowerPoint.Shape>();
        private System.Timers.Timer altTextUpdateTimer; // time based update
        private PowerPoint.Shape currentShape;

        //for fixing problems with shape.decorative, only available in office365 and office2021+
        int majorVersion = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName).ProductMajorPart;
        string productionVersion = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName).ProductVersion;
        int minorVersion = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName).ProductMinorPart;
        string officeProductName = FileVersionInfo.GetVersionInfo(Process.GetCurrentProcess().MainModule.FileName).ProductName;

        private readonly Color activeButtonColor = ColorTranslator.FromHtml("#fcc2b1");
        private readonly Color nonActiveButtonColor = Color.White;
        private Button activeButton = null;
        private int minLengthAltText = 4;
        public bool bypassTextChanged = false;
        public bool bypassArtifactCheck = false;
        public AlternativeText()
        {
            altTextUpdateTimer = new System.Timers.Timer(500); 
            altTextUpdateTimer.AutoReset = false;
            altTextUpdateTimer.Elapsed += (sender, e) =>
            {
                if (currentShape != null && currentShape.AlternativeText != alternativeTextInput.Text)
                {
                    currentShape.AlternativeText = alternativeTextInput.Text;
                }
                UpdateMarkedArtifactCheckbox(currentShape);

            };
            InitializeComponent();

            lbAlternativeTextInfo.Text = resourceManager.GetString("AlternativeTextInfoBox");
            alternativeTextInputLabel.Text = Properties.Resources.AddAlternativeText;


            saveAltTextButton.MouseClick += (object sender, MouseEventArgs e) =>
            {
                PowerPoint.Shape shape = currentShape;

                saveAltText(shape);
            };
            refreshPanelButton.MouseClick += (object sender, MouseEventArgs e) =>
            {
                refresh();
            };


            refreshPanelButton.FlatAppearance.MouseOverBackColor = refreshPanelButton.BackColor;
            refreshPanelButton.BackColorChanged += (s, e) =>
            {
                refreshPanelButton.FlatAppearance.MouseOverBackColor = refreshPanelButton.BackColor;
            };
            refreshPanelButton.MouseEnter += (s, e) =>
            {
                Color HoverColor = ColorTranslator.FromHtml("#fcc2b1");
                ((Button)s).BackColor = HoverColor;
            };
            refreshPanelButton.MouseLeave += (s, e) => { ((Button)s).BackColor = Color.White; };

            saveAltTextButton.FlatAppearance.MouseOverBackColor = saveAltTextButton.BackColor;
            saveAltTextButton.BackColorChanged += (s, e) =>
            {
                saveAltTextButton.FlatAppearance.MouseOverBackColor = saveAltTextButton.BackColor;
            };
            saveAltTextButton.MouseEnter += (s, e) =>
            {
                Color HoverColor = ColorTranslator.FromHtml("#fcc2b1");
                ((Button)s).BackColor = HoverColor;
            };
            saveAltTextButton.MouseLeave += (s, e) => { ((Button)s).BackColor = Color.White; };
        }

        private void AlternativeTextInput_LostFocus(object sender, EventArgs e)
        {
            altTextUpdateTimer.Stop();
            if (currentShape != null && currentShape.AlternativeText != alternativeTextInput.Text)
            {
                currentShape.AlternativeText = alternativeTextInput.Text;
            }
        }

        /// <summary>
        /// Refreshes the element list
        /// </summary>
        private void refresh()
        {
            PowerPoint.Slide slide;
            try { slide = Globals.ThisAddIn.Application.ActiveWindow.View.Slide; }
            catch (Exception ex)
            {
                return;
            }
            update_AlternativeTextList(slide);
        }

        /// <summary>
        /// Saves the input of the textbox and refreshes the element list with their status
        /// </summary>
        private void saveAltText(PowerPoint.Shape shape)
        {
            PowerPoint.Slide slide;
            try { slide = Globals.ThisAddIn.Application.ActiveWindow.View.Slide; }
            catch (Exception ex)
            {
                return;
            }
            if (currentShape != null && currentShape.AlternativeText != alternativeTextInput.Text)
            {
                currentShape.AlternativeText = alternativeTextInput.Text;
            }
            update_AlternativeTextList(slide);
            setSelection(shape);
            AppController.Model.UpdateIncidents("altText", slide.SlideNumber);

        }
        /// <summary>
        /// Update state of checkbox of the selected shape
        /// </summary>
        /// <param name="shape">Selected shape</param>
        private void UpdateMarkedArtifactCheckbox(PowerPoint.Shape shape)
        {
            if (shape == null) return;
            //update marked_as_artifact
            dynamic shp = (dynamic)currentShape;
            if (ShapeExtensions.Exists(currentShape) && shp.Decorative != 0)
            {
                isDecorativeCheckBox.Invoke((MethodInvoker)delegate
                {
                    isDecorativeCheckBox.Checked = true;
                    Helper.Artifact.MarkAsArtifact(shape);
                });

            }
            else
            {
                isDecorativeCheckBox.Invoke((MethodInvoker)delegate
                {
                    Helper.Artifact.UnmarkAsArtifact(shape);
                    isDecorativeCheckBox.Checked = false;
                });
            }
        }

        private void AlternativeTextInput_GotFocus(object sender, EventArgs e)
        {
            if (ShapeExtensions.Exists(currentShape) && currentShape.AlternativeText != alternativeTextInput.Text)
            {
                alternativeTextInput.Text = currentShape.AlternativeText;
            }
            UpdateMarkedArtifactCheckbox(currentShape);
        }

        private void AlternativeTextInput_TextChanged(object sender, EventArgs e)
        {
            //Start timer to update text on changes
            altTextUpdateTimer.Stop();
            altTextUpdateTimer.Start();
            if (!bypassTextChanged)
            {
                if (currentShape != null && activeButton != null && alternativeTextInput.Text.Length >= minLengthAltText)
                {
                    changeIcon(activeButton, Properties.Resources.hasAltText);
                }
                else
                {
                    changeIcon(activeButton, Properties.Resources.hasNoAltText);
                }
            }
        }

        public void update_AlternativeTextList(PowerPoint.Slide slide, PowerPoint.Shape shapeError)
        {
            this.currentShape = shapeError;

            // shapes contains all image shapes that have no alternative text
            List<PowerPoint.Shape> shapes = Helper.AlternativeText.SlideShapesValidForAltText(slide);

            imageButtonsLayoutPanel.Controls.Clear();
            alternativeTextInput.Clear();

            if (shapes.Count == 0)
            {
                Label noImagesOnSlide = new Label();
                noImagesOnSlide.Text = Properties.Resources.noImagesOnSlide;
                noImagesOnSlide.AutoSize = false;
                noImagesOnSlide.Dock = DockStyle.Top;
                noImagesOnSlide.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));                
                noImagesOnSlide.Margin = new Padding(4);
                noImagesOnSlide.Name = "noImageOnSlide";
                noImagesOnSlide.Size = new Size(50, 100);
                noImagesOnSlide.TabIndex = 1;


                imageButtonsLayoutPanel.Controls.Add(noImagesOnSlide);

                return;
            }

            //add new list elements from AlternativeText shape list
            foreach (PowerPoint.Shape shape in shapes)
            {

                Button shapeButton = new Button();
                shapeButton.Text = shape.GetName(true, false);
                shapeButton.FlatStyle = FlatStyle.Flat;
                shapeButton.Click += shapeButton_Click;
                shapeButton.Dock = DockStyle.Top;
                shapeButton.Size = new Size(140, 30);
                shapeButton.Height = 30;
                shapeButton.MinimumSize = new Size(140, 30);
                shapeButton.MaximumSize = new Size(140, 30);
                shapeButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
                shapeButton.AutoSize = true;


                if (shape.Id == shapeError.Id)
                {
                    if (currentShape != null && !string.IsNullOrEmpty(currentShape.AlternativeText))
                    {                        
                        alternativeTextInput.Text = currentShape.AlternativeText;
                    }                    
                    isDecorativeCheckBox.Checked = Helper.Artifact.IsIdMarkedAsArtifact(currentShape);
                    currentShape.Select();
                    alternativeTextInput.Focus();
                    shapeButton.BackColor = activeButtonColor;
                }

                slideShapes.Add(shapeButton, shape);
                imageButtonsLayoutPanel.Controls.Add(shapeButton);
                imageButtonsLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            }
        }

        public void Change_AltTextLabel(string RessourceKey)
        {
            lbAlternativeTextInfo.Text = resourceManager.GetString(RessourceKey);
        }

        public void update_AlternativeTextList(PowerPoint.Slide slide)
        {
            alternativeTextInput.Text = "";
            currentShape = null; // this is needed otherwise the alternative text is reset, if the slide is changed
            // shapes contains all image shapes that have no alternative text
            List<PowerPoint.Shape> shapes = Helper.AlternativeText.SlideShapesValidForAltText(slide);
            imageButtonsLayoutPanel.Controls.Clear();
            if (shapes.Count == 0)
            {
                Label noImagesOnSlide = new Label();
                noImagesOnSlide.Text = Properties.Resources.noImagesOnSlide;
                noImagesOnSlide.AutoSize = false;
                noImagesOnSlide.Dock = DockStyle.Top;
                noImagesOnSlide.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));            
                noImagesOnSlide.Margin = new Padding(4);
                noImagesOnSlide.Name = "noImageOnSlide";
                noImagesOnSlide.Size = new Size(50, 100);
                noImagesOnSlide.TabIndex = 1;

                imageButtonsLayoutPanel.Controls.Add(noImagesOnSlide);

                return;
            }

            Button firstButton = null;
            int i = 0;
            slideShapes.Clear();

            //add new list elements from AlternativeText shape list
            foreach (PowerPoint.Shape shape in shapes)
            {
                Button shapeButton = new Button();
                shapeButton.Text = shape.GetName(true, false);
                shapeButton.FlatStyle = FlatStyle.Flat;
                shapeButton.Click += shapeButton_Click;
                shapeButton.Dock = DockStyle.Top;
                shapeButton.Size = new Size(140, 30);
                shapeButton.Height = 30;
                shapeButton.MinimumSize = new Size(140, 30);
                shapeButton.MaximumSize = new Size(140, 30);
                shapeButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
                shapeButton.AutoSize = true;
                shapeButton = addIconToButton(shapeButton, shape);


                slideShapes.Add(shapeButton, shape);

                imageButtonsLayoutPanel.Controls.Add(shapeButton);
                imageButtonsLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                // automatically select the first button by "simulating" a click later
                if (i++ == 0)
                {
                    firstButton = shapeButton;
                }
            }            
        }

        /// <summary>
        /// Changes the icon of the active button
        /// </summary>
        /// <param name="button"></param>
        /// <param name="bitmap"></param>
        private void changeIcon(Button button, Bitmap bitmap)
        {
            if (button != null)
            {
                int width = (int)(bitmap.Width / 2);
                int height = (int)(bitmap.Height / 2);
                button.Image = new Bitmap(bitmap, new Size(width, height));
                button.ImageAlign = ContentAlignment.MiddleRight;
            }
        }

        /// <summary>
        ///  Adds an Image at the end of the shapeButton depending on if it has an altText or not or if it is marked as an artifact
        /// </summary>
        /// <param name="shapeButton"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        private Button addIconToButton(Button shapeButton, PowerPoint.Shape shape)
        {
            Bitmap bitmap = null;//size is 31x31
            if (shape.AlternativeText != "" && !Artifact.IsIdMarkedAsArtifact(shape))
            {// has text
                bitmap = Properties.Resources.hasAltText;

            }
            else if (!Artifact.IsIdMarkedAsArtifact(shape) && shape.AlternativeText == "")
            { // has no text
                bitmap = Properties.Resources.hasNoAltText;
            }
            else
            {//is artifact 
                bitmap = Properties.Resources.isArtifact;
            }

            int width = (int)(bitmap.Width / 2);
            int height = (int)(bitmap.Height / 2);
            shapeButton.Image = new Bitmap(bitmap, new Size(width, height));



            shapeButton.ImageAlign = ContentAlignment.MiddleRight;
            return shapeButton;
        }

        /// <summary>
        /// Set selected button based on shape
        /// </summary>
        /// <param name="s">Selected shape</param>
        public void setSelection(PowerPoint.Shape s)
        {
            var button = slideShapes.FirstOrDefault(x => x.Value == s).Key;
            if (button != default)
            {
                currentShape = s;
                alternativeTextInput.Clear();
                //check if shape has AltText, if so display it in the alternativeTextInput field
                if (s != null && !string.IsNullOrEmpty(s.AlternativeText))
                {
                    alternativeTextInput.Text = s.AlternativeText;
                }
                //check or uncheck isDecorativeChecbox based on saved value                
                UpdateMarkedArtifactCheckbox(s);               
                highlightButton(button);
            }
        }
 
        private void highlightButton(Button b)
        {
            // reset all button background colors
            foreach (Button slideShapeButton in slideShapes.Keys)
            {
                if (b != slideShapeButton)
                    slideShapeButton.BackColor = nonActiveButtonColor;
                else
                {
                    slideShapeButton.BackColor = activeButtonColor;
                    activeButton = slideShapeButton;
                }
            }


        }

        public void shapeButton_Click(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;

            if (slideShapes.TryGetValue(senderButton, out this.currentShape)
                && ShapeExtensions.Exists(currentShape))  // detect deleted shapes
            {
                activeButton = senderButton;

                if (e != null) // bad practise, fixes a problem with exceptions on slide navigation
                {
                    currentShape.Select();
                    alternativeTextInput.Focus();
                    dynamic shp = currentShape as dynamic;
                    // not office 2016
                    if (!officeProductName.Contains("2016"))
                    {
                        if (shp.Decorative != 0)
                        {
                            isDecorativeCheckBox.Checked = true;
                        }
                        else
                        {
                            isDecorativeCheckBox.Checked = false;
                        }
                    }
                    else
                    {
                        // only for Office 2016
                        if (Helper.Artifact.IsIdMarkedAsArtifact(currentShape))
                        {
                            isDecorativeCheckBox.Checked = true;
                        }
                        else
                        {
                            isDecorativeCheckBox.Checked = false;
                        }
                    }
                }
            }
        }

        private void isDecorativeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            dynamic shp = currentShape as dynamic;
            if (isDecorativeCheckBox.Checked)
            {
                alternativeTextInput.ReadOnly = true;
                if (currentShape != null)
                {
                    Helper.Artifact.MarkAsArtifact(currentShape);
                    //not office 2016
                    if (!officeProductName.Contains("2016"))
                    {
                        shp.Decorative = true;
                    }
                }
            }
            else
            {
                alternativeTextInput.ReadOnly = false;
                if (currentShape != null)
                {
                    Helper.Artifact.UnmarkAsArtifact(currentShape);
                    // not office 2016
                    if (!officeProductName.Contains("2016"))
                    {
                        shp.Decorative = false;
                    }
                }
            }
            // changes icon
            if (!bypassArtifactCheck)
            {
                if (isDecorativeCheckBox.Checked)
                {
                    changeIcon(activeButton, Properties.Resources.isArtifact);
                }
                else
                {
                    if (alternativeTextInput.Text.Length > minLengthAltText)
                    {
                        changeIcon(activeButton, Properties.Resources.hasAltText);
                    }
                    else
                    {
                        changeIcon(activeButton, Properties.Resources.hasNoAltText);
                    }
                }
            }
        }
    }
}

