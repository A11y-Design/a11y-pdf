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
using Office = Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace A11y_Design_PowerPointAddin.Helper
{
    internal static class MetaData
    {
        /// <summary>
        /// Get metadata entry by key
        /// </summary>
        /// <param name="key">Key of metadata entry</param>
        /// <param name="pres">presentation</param>
        /// <returns></returns>
        // allowed keys https://docs.microsoft.com/en-us/dotnet/api/microsoft.office.core.documentproperty?view=office-pia
        public static String GetByKey(Key key, PowerPoint.Presentation pres = null)
        {
            if(pres == null)
                pres = Globals.ThisAddIn.Application.ActivePresentation;

            Office.DocumentProperties properties = (Office.DocumentProperties)pres.BuiltInDocumentProperties;


            if (properties[key.ToString()] == null || properties[key.ToString()].Value == null
                                                   || properties[key.ToString()] != null && properties[key.ToString()].Value.ToString() == String.Empty)
            {
                return String.Empty;
            }


            return properties[key.ToString()].Value.ToString();
        }
        /// <summary>
        /// Set metadata
        /// </summary>
        /// <param name="key">metadata key</param>
        /// <param name="value">to set</param>
        public static void SetMetaData(string key, string value, PowerPoint.Presentation pres = null)
        {
            if(pres == null)
                pres = Globals.ThisAddIn.Application.ActivePresentation;

            Office.DocumentProperties properties = (Office.DocumentProperties)pres.BuiltInDocumentProperties;            
            properties[key].Value = value;
        }


        public enum Key
        {
            Title,
            Subject,
            Author,
            Keywords,
            Comments
        }
    }
}
