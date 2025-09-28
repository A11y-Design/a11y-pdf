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

namespace A11y_Design_PowerPointAddin
{
    /// <summary>
    /// Implmentation of the most general IncidentType.
    /// <br>Functions for the condition, naming and selecting are passed to the contructor</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class A11yIncidentType<T> : IA11yIncidentCreator<T>
    {
        private Func<T, bool> Condition;
        private Func<T, string> ItemNaming;
        private Action<T> SelectAction;
        private Type ObjType;

        /// <summary>
        /// internal topic with rights to add incidents
        /// </summary>
        private A11yIncidentTopic Topic { get;}

        /// <summary>
        /// External topic without rights to add incidents
        /// </summary>
        IA11yIncidentTopic IA11yIncidentCreator<T>.Topic => Topic;

        /// <summary>
        /// Incidents are created based on an item of type T when the <b>condition</b> is true 
        /// with the name from <b>ItemNaming</b> on the passed <b>topic</b>. When selected, the <b>selectAction</b> will be called.
        /// </summary>
        /// <param name="condition">An incident will be created, when the condition is true. Exapmle: t => true</param>
        /// <param name="itemNaming">Creates a custom name of the Incident. Example: t => t.toString()</param>
        /// <param name="selectAction">Action to be called, whe incident is selected. Example for Shape: shape => ShapeExtensions.SelectShape(shape)</param>
        /// <param name="topic">Topic of the created incidents. </param>
        public A11yIncidentType(Func<T, bool> condition, Func<T, string> itemNaming, Action<T> selectAction, A11yIncidentTopic topic)
        {
            ObjType = typeof(T);
            Condition = condition;
            ItemNaming = itemNaming;
            SelectAction = selectAction;
            Topic = topic;
        }

        /// <summary>
        /// Function for the Factory. Will be called with every observed item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="incidentItems"></param>
        /// <returns></returns>
        public bool TryCreate(T item, ICollection<IA11yIncidentItem> incidentItems)
        {

            IA11yIncidentItem errorItem;
            if(Condition(item))
            {
                errorItem = new A11yIncidentItem(()=>SelectAction(item), ItemNaming(item), Topic);
                incidentItems.Add(errorItem);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
        }
    }
}