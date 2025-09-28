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

namespace A11y_Design_PowerPointAddin.Model
{
    // This Model should contain instances for the presentation.
    // Theads for async checking can be hold here, as well as Facories, Hosts, etc. that need time to create
    internal class AppModel
    {
        private A11yIncidentHost a11YIncidentHost;


        private Action OnIncidentUpdate = delegate { }; // not thread-safe

        /// <summary>
        /// Add to event when incident list is updated
        /// </summary>
        /// <param name="a"></param>
        public void RegisterOnIncidentUpdate(Action a) => OnIncidentUpdate += a;

        /// <summary>
        /// Call Events registered on Incident Update
        /// </summary>
        public void CallOnIncidentUpdate() => OnIncidentUpdate.Invoke();

        public AppModel()
        {
            a11YIncidentHost = new A11yIncidentHost();
            Incidents = new List<IA11yIncidentItem>();
            ShowReadingOrder = false;
        }

        /// <summary>
        /// Update the listof incidents.
        /// Calls CallOnIncidentUpdate() when done.
        /// </summary>
        public void UpdateIncidents(string method="all", int slideNumber = 0) => a11YIncidentHost.UpdateIncidents(method, slideNumber);


        public List<IA11yIncidentItem> Incidents { get; } // not thread-safe TODO: restrict access

        /// <summary>
        /// Clean disposal of all incindents
        /// </summary>
        public void ClearIncidentList()
        {
            foreach (var item in Incidents)
            {
                item.Dispose();
            }
            Incidents.Clear();
        }

        internal bool ShowReadingOrder {get; set; }
    }
}
