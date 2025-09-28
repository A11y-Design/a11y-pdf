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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace A11y_Design_PowerPointAddin.Controls
{
    /// <summary>
    /// Implementation of the an accordeon
    /// </summary>
    public class Accordeon : UserControl
    {
        private TableLayoutPanel layoutPanel = new TableLayoutPanel();        

        private const int BUTTON_PADDING = 4;
        private const int BUTTON_HEIGHT = 40;
        private const int BUTTON_FONT_SIZE = 10;
        private const int CONTENT_ITEM_HEIGHT = 40;
        private const int CONTENT_ITEM_Y_PADDING = 0;
        private const int CONTENT_ITEM_X_PADDING = 2;
        private const int CONTENT_ITEM_FONT_SIZE = 8;
        private const string FONT_FAMILY = "Arial";
        private const string FONT_COLOR_HEX = "#000000";


        private int verticalScrollbarWidth = SystemInformation.VerticalScrollBarWidth;

        private readonly Image downIcon = Properties.Resources.chevron_down;
        private readonly Image upIcon = Properties.Resources.chevron_up;

        // set this to true to hide the category buttons
        private bool categoriesVisisble = true;


        // each category is identified by a button and contains a list of controls (= the items to be revealed)
        private Dictionary<Button, List<Control>> categories = new Dictionary<Button, List<Control>>();
        private Button activeCategory;

        public event EventHandler ContentSizeChanged;

        public int TotalHeight => GetTotalHeight();

        private Size colapsedSize = new Size();

        public Button ActiveCategory => activeCategory;

        
        public Accordeon()
        {
            GenerateLayoutPanel();
        }

        public Accordeon(bool categoriesVisisble)
        {
            categoriesVisisble = false;
            GenerateLayoutPanel();
        }

        /// <summary>
        /// Clear all entries from accordeon 
        /// </summary>
        public void Clear()
        {
            layoutPanel.Controls.Clear();

            categories.Clear();
            activeCategory = null;
        }

        public void ClearCategory(Button categoryButton)
        {
            List<Control> buttonsOfCategory;

            if (categories.TryGetValue(categoryButton, out buttonsOfCategory) && buttonsOfCategory != null)
            {

                foreach (Control button in buttonsOfCategory)
                {
                    layoutPanel.Controls.Remove(button);
                }

                // clear the list
                categories.Remove(categoryButton);
                categories.Add(categoryButton, new List<Control>());
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        private void GenerateLayoutPanel()
        {
            layoutPanel.HorizontalScroll.Visible = false;
            layoutPanel.Padding = new Padding(0, 0, verticalScrollbarWidth, 0);
            layoutPanel.ColumnCount = 1;
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));            
            layoutPanel.Dock = DockStyle.Fill;
            layoutPanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;            
            Padding = new Padding(5, 5, 0, 5);
            layoutPanel.AutoScroll = true;
            layoutPanel.PaddingChanged += LayoutPanel_PaddingChanged;
            Controls.Add(layoutPanel);            
        }

        private void LayoutPanel_PaddingChanged(object sender, EventArgs e)
        {
        }

        public Button AddCategory(string categoryName)
        {
            Button categoryButton = new Button
            {
                Text = "  " + categoryName,
                Padding = new Padding(BUTTON_PADDING),
                Height = BUTTON_HEIGHT,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml(FONT_COLOR_HEX),
                Font = new Font(FONT_FAMILY, BUTTON_FONT_SIZE, FontStyle.Regular),
                Anchor = (AnchorStyles.Top | AnchorStyles.Left),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            categoryButton.FlatAppearance.BorderSize = 0;
            categoryButton.Image = downIcon;

            categoryButton.FlatAppearance.MouseOverBackColor = categoryButton.BackColor;
            categoryButton.BackColorChanged += (s, e) =>
            {
                categoryButton.FlatAppearance.MouseOverBackColor = categoryButton.BackColor;
            };

            categoryButton.MouseEnter += (s, e) =>
            {
                Color HoverColor = ColorTranslator.FromHtml("#fcc2b1");
                ((Button)s).BackColor = HoverColor;
            };
            categoryButton.MouseLeave += (s, e) => { ((Button)s).BackColor = Color.White; };

            categoryButton.Click += CategoryButton_Click;

            if (categoriesVisisble)
            {
                // add new row in the layout panel for the category button
                layoutPanel.Controls.Add(categoryButton);
            }

            categories.Add(categoryButton, null);            
            return categoryButton;
        }

        public void SuspendLP()
        {
            layoutPanel.SuspendLayout();
        }

        public void ResumeLP()
        {
            layoutPanel.ResumeLayout();
        }
        public void AddControlToCategory(Button categoryButton, Control control)
        {

            List<Control> categoryItems = GetCategoryItemsByCategory(categoryButton);

            if (categoryItems == null)
            {
                categoryItems = new List<Control>();
            }

            control.Visible = (categoryButton == activeCategory);
            control.Padding = new Padding(CONTENT_ITEM_X_PADDING, CONTENT_ITEM_Y_PADDING, CONTENT_ITEM_X_PADDING, CONTENT_ITEM_Y_PADDING);
            control.Height = CONTENT_ITEM_HEIGHT;
            control.Font = new Font(FONT_FAMILY, CONTENT_ITEM_FONT_SIZE, FontStyle.Regular);
            control.Dock = DockStyle.Top;

            categoryItems.Add(control);
            categories[categoryButton] = categoryItems;
            layoutPanel.Controls.Add(control);            
        }


        public void SetPadding()
        {
            layoutPanel.Padding = new Padding(0, 0, verticalScrollbarWidth, 0);
        }


        public void SetSize()
        {
            colapsedSize = layoutPanel.Size;
            layoutPanel.VerticalScroll.Enabled = false;            
        }
        public void CopyPane()
        {
            ErrorListPane errorListPane = new ErrorListPane();
            errorListPane.ForAccordeon();
        }

        public void CategoryButton_Click(object sender, EventArgs e)
        {
            layoutPanel.SuspendLayout();
            var newcategory = (Button)sender;
            bool closed = false;            
            // hide expanded category
            if (activeCategory != null)
            {
                categories[activeCategory].ForEach(c => c.Visible = false);
                activeCategory.Image = downIcon;
                closed = true;                
            }
            // show new category
            if (activeCategory != newcategory)
            {
                if (categories.ContainsKey(newcategory))
                {
                    categories[newcategory].ForEach(c => c.Visible = true);
                    newcategory.Image = upIcon;
                    activeCategory = newcategory;
                }
                else
                // Reopen active error category after checking one slide
                {
                    foreach (Button btn in categories.Keys)
                    {
                        if (btn.Text.Equals(newcategory.Text) && categories[btn] != null)
                        {
                            categories[btn].ForEach(c => c.Visible = true);
                            btn.Image = upIcon;
                            activeCategory = btn;
                        }
                    }

                }
            }

            if (closed)
            {
                layoutPanel.Size = colapsedSize;
                activeCategory = null;
            }
            layoutPanel.ResumeLayout();
            
            this.Height = GetTotalHeight();
            ContentSizeChanged?.Invoke(this, EventArgs.Empty);
            if (layoutPanel.VerticalScroll.Visible)
            {
                layoutPanel.Padding = new Padding(5, 5, 0, 5);
            }
            else
            {
                layoutPanel.Padding = new Padding(5, 5, 17, 5);
            }            
        }

        private List<Control> GetCategoryItemsByCategory(Button categoryButton)
        {
            List<Control> categoryItems;

            if (!categories.TryGetValue(categoryButton, out categoryItems))
            {
                throw new System.Exception("Accordeon Category does not exist");
            }

            return categoryItems;
        }

        private int GetTotalHeight()
        {
            int height = 0;
            foreach (Control control in layoutPanel.Controls)
            {
                if (control.Visible)
                    height += control.Height + control.Margin.Top + control.Margin.Bottom;
            }
            return height;
        }
    }
}
