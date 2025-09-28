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
using System.Resources;
using Office = Microsoft.Office.Core;

namespace A11y_Design_PowerPointAddin.Core.PPT_Incidents
{
    internal class A11yIncidentType_Table : IA11yIncidentCreator<Shape>
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_Table).Assembly);
        private A11yIncidentTopic topic = new A11yIncidentTopic(resourceManager.GetString("IrregularTable"), TopicLevel.WARNING);

        IA11yIncidentTopic IA11yIncidentCreator<Shape>.Topic => topic;

        public void Reset()
        {
        }

        public bool TryCreate(Shape item, ICollection<IA11yIncidentItem> incidentItems)
        {
            bool bfound = false;
            bool bHasEmptyContentCell = false;
            IA11yIncidentItem errorItem;
            if (item.GetNestedType() == Office.MsoShapeType.msoTable)
            {
                int rowindex = 1;
                int colindex = 1;
                foreach (Row row in item.Table.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        var s = cell.Shape;
                        var cell_text = s.TextFrame.TextRange.Text;
                        if(cell_text.Trim().Length == 0)
                        {
                            string incidentText = "";
                            string incidentHint = "";
                            // empty header
                            if (item.Table.FirstRow && rowindex == 1 || item.Table.LastCol && colindex == 1)
                            {
                                incidentText = $"{resourceManager.GetString("EmptyHeaderCell")} {item.GetName()}";
                                incidentHint = resourceManager.GetString("EmptyHeaderCellDescription");
                            }
                            // empty cell
                            else if(!bHasEmptyContentCell)// empty cell
                            {
                                incidentText = $"{resourceManager.GetString("EmptyDataCell")} {item.GetName()}";
                                incidentHint = resourceManager.GetString("EmptyCellDescription");
                                bHasEmptyContentCell = true;

                            }
                            //skip
                            else
                            {
                                colindex++;
                                continue;
                            }

                            errorItem = new A11yIncidentItem(() => select(item, cell, incidentHint),
                                name: incidentText,
                                topic: topic);
                            incidentItems.Add(errorItem);
                            bfound = true;
                        }
                        colindex++;
                    }
                    colindex = 1;
                    rowindex++;
                }


            }
            return bfound;  
        }

        private void select(Shape shape, Cell cell, string hint)
        {
            shape.SelectShape();

            if (cell != null && cell.Parent == shape)
            {
                cell.Select();
            }

            ChangePaneController.SetTab(AccessibilityChangesPaneTabs.HINTS, false);
            Controller.AppController.View.AccessibilityChanges.SetHint(hint);
        }
    }


}
