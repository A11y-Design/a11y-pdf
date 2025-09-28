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



namespace A11y_Design_PowerPointAddin
{
    /// <summary>
    /// Implementation of the IA11yIncidentItem-Interface. 
    /// Should never be used in frontend.
    /// </summary>
    class A11yIncidentItem : IA11yIncidentItem 
    {
        private Action Select;
        private Action value;

        public string Name { get; }
        private A11yIncidentTopic RegisteredTopic { get; }
        public IA11yIncidentTopic Topic => RegisteredTopic;
        void IA11yIncidentItem.Select() => Select();


        public A11yIncidentItem(Action select, string name, A11yIncidentTopic topic)
        {
            Select = select;
            Name = name;
            RegisteredTopic = topic;
            RegisteredTopic.Incidents.Add(this);
        }

        public A11yIncidentItem(Action value)
        {
            this.value = value;
        }

        public void Dispose()
        {
            RegisteredTopic.Incidents.Remove(this);
        }

        public override string ToString()
        {
            return $"A11yIncidentItem {Name}, Topic: {RegisteredTopic.Name}";
        }

        ~A11yIncidentItem()
        {
            RegisteredTopic.Incidents.Remove(this);
        }
    }
}