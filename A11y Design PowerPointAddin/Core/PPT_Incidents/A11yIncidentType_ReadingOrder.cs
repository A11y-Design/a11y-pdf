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
using A11y_Design_PowerPointAddin.Controls.AccessibilityChangesPane;
using A11y_Design_PowerPointAddin.Helper;
using Microsoft.Office.Interop.PowerPoint;
using System.Collections.Generic;
using System.Linq;

namespace A11y_Design_PowerPointAddin
{
    class A11yIncidentType_ReadingOrder : IA11yIncidentCreator<Slide>
    {

        private A11yIncidentTopic Topic { get; }
        IA11yIncidentTopic IA11yIncidentCreator<Slide>.Topic => Topic;

        private string Text;

        public A11yIncidentType_ReadingOrder(string text,A11yIncidentTopic topic)
        {
            Topic = topic;
            Text = text;
        }

        private List<Shape> readingOrderByCoord = new List<Shape>();

        public bool TryCreate(Slide slide, ICollection<IA11yIncidentItem> incidentItems)
        {
            readingOrderByCoord.Clear();
            foreach (Shape _shape in slide.Shapes)
            {
                readingOrderByCoord.Add(_shape);
            }

            List<Shape> sortList = readingOrderByCoord
                .OrderBy(p => (int)p.Top)
                .ThenBy(p => (int)p.Left)
                .ToList<Shape>();

            foreach (var item in sortList.Select((value, i) => new { i, value }))
            {
                Shape _shape = (Shape)item.value;
                int position = item.i + 1;

                if (position != _shape.ZOrderPosition)
                {
                    var i = new A11yIncidentItem(
                        () => {
                            _shape.SelectShape();
                            ChangePaneController.SetTab(AccessibilityChangesPaneTabs.READINGORDER, false);
                        },
                        Text.Replace("{0}", slide.SlideNumber.ToString()),
                        Topic);
                    incidentItems.Add(i);
                    return true;
                }
            }

            return false;

        }

        public void Reset()
        {
           readingOrderByCoord.Clear();
        }
    }
}