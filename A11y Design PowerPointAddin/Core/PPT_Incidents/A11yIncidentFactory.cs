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

using System.Collections.Generic;



namespace A11y_Design_PowerPointAddin
{
    /// <summary>
    /// Takes items of Type T as an input and creates a collection of IA11yIncidentItems based on registered IA11yIncidentCreators.
    ///  
    /// </summary>
    /// <typeparam name="T"> Type of objects that are checked</typeparam>
    class A11yIncidentFactory<T>
    {

        // Internal collection of IA11yIncidentCreators
        private List<IA11yIncidentCreator<T>> IncidentTypes;

        /// <summary>
        /// Retrieved Incidents that where discovered by registered IA11yIncidentCreators
        /// </summary>
        public List<IA11yIncidentItem> Incidents { get; private set; }
        public A11yIncidentFactory()
        {
            IncidentTypes = new List<IA11yIncidentCreator<T>>();
            Incidents = new List<IA11yIncidentItem>();
        }

        public void Reset()
        {

            foreach (var item in Incidents)
            {
                item.Dispose();
            }
            Incidents = new List<IA11yIncidentItem>();
        }

        public void RemoveIncidentsForSlide(int slideNumber)
        {
            string look4De = $"(Folie {slideNumber})";
            string look4En = $"(Slide {slideNumber})";
            List<IA11yIncidentItem> incidentsNew = new List<IA11yIncidentItem>();
            foreach (var item in Incidents)
            {

                if (item.Name.Contains(look4De) || item.Name.Contains(look4En))
                {
                    item.Dispose();
                }
                else
                {
                    incidentsNew.Add(item);
                }
                Incidents = incidentsNew;
            }
        }

        /// <summary>
        /// Add a new IA11yIncidentCreator
        /// </summary>
        /// <param name="Type"></param>
        public void Register(IA11yIncidentCreator<T> Type)
        {
            IncidentTypes.Add(Type);
        }

        /// <summary>
        /// Create IA11yIncidentItems by showing this item to registered IA11yIncidentCreators
        /// </summary>
        /// <param name="item"></param>
        public void AppendIncidentsOnItem(T item)
        {
            List<IA11yIncidentItem> appedincidnets = new List<IA11yIncidentItem>();
            foreach (var type in IncidentTypes)
            {
                appedincidnets.Clear();
                if (type.TryCreate(item, appedincidnets))
                {
                    Incidents.AddRange(appedincidnets);

                }
            }
        }

    }
}