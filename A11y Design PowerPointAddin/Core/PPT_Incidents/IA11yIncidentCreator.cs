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
    /// Creates incidents based on observed items on an defined topic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IA11yIncidentCreator<T>
    {
        /// <summary>
        /// Oberserve item an create a IncidentItem if conditions of that incident are met.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="incidents"></param>
        /// <returns></returns>
        bool TryCreate(T item, ICollection<IA11yIncidentItem> incidents);

        void Reset();
        IA11yIncidentTopic Topic { get; }
    }
}