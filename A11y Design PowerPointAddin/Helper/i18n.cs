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

using System.Resources;

namespace A11y_Design_PowerPointAddin.Helper
{
    public static class i18n
    {
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(i18n).Assembly);

        public static string GetTranslation(string key)
        {
            return resourceManager.GetString(key);
        }
    }
}
