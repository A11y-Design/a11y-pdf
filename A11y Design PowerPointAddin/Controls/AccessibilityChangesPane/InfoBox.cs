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

using System.Drawing;
using System.Windows.Forms;

namespace A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane
{
    public partial class InfoBox : UserControl
    {
        private TableLayoutPanel container;
        private Label infoLabel;
        private PictureBox infoIcon;

        private readonly Color BACKGROUND_COLOR = Color.WhiteSmoke;
        private readonly Padding PADDING = new Padding(5);

        private string _text = "";
        override public string Text
        {
            get => _text;
            set
            {
                _text = value;
                infoLabel.Text = value;
            }
        }

        private int _height = 45;
        public new int Height
        {
            get => _height;
        }

        public InfoBox()
        {
            GenerateInfoBox();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            Size containerPreferredSize = container.GetPreferredSize(constrainingSize);
            Size preferredSize = new Size(constrainingSize.Width, containerPreferredSize.Height);

            return preferredSize;
        }


        private void GenerateInfoBox()
        {
            container = new TableLayoutPanel();
            container.AutoSize = true;
            this.Controls.Add(container);
            infoIcon = new PictureBox();
            infoLabel = new Label();

            container.SuspendLayout();
            infoIcon.SuspendLayout();
            infoLabel.SuspendLayout();
            SuspendLayout();

            generateLayoutContainer();
            generateInfoLabel();
            generateInfoIcon();

            infoLabel.AutoSize = true;
            this.AutoSize = true;

            this.Padding = PADDING;
            this.Dock = DockStyle.Top;
            this.AccessibleDescription = "Information Box";

            infoIcon.ResumeLayout(true);
            infoLabel.ResumeLayout(true);
            container.ResumeLayout(true);
            ((System.ComponentModel.ISupportInitialize)(infoIcon)).EndInit();
            this.ResumeLayout(true);
        }

        private void generateLayoutContainer()
        {
            container.ColumnCount = 2;
            container.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30));
            container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            container.RowCount = 1;
            container.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            container.Controls.Add(infoIcon, 0, 0);
            container.Controls.Add(infoLabel, 1, 0);

            container.TabIndex = 0;

            container.Padding = new Padding(5, 0, 0, 5);
            container.BackColor = BACKGROUND_COLOR;
            container.Dock = DockStyle.Fill;

            container.AutoSize = true;
        }

        private void generateInfoIcon()
        {
            infoIcon.Image = Properties.Resources.error_circle;
            // convert ! to i
            infoIcon.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            infoIcon.Dock = DockStyle.Top;
            infoLabel.AutoSize = true;
            infoIcon.IsAccessible = false;
            infoIcon.MinimumSize = new Size(25, 25);
        }

        private void generateInfoLabel()
        {
            infoLabel.Text = Text;

            infoLabel.Dock = DockStyle.Fill;
            infoLabel.TextAlign = ContentAlignment.TopLeft;
            infoLabel.AutoSize = true;
            infoLabel.UseCompatibleTextRendering = true;
            infoLabel.Margin = new Padding(0, 5, 0, 5);
        }
    }
}   
