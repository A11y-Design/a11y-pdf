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

using System.Globalization;
using System.Resources;
using Tools = Microsoft.Office.Tools;


namespace A11y_Design_PowerPointAddin.Helper
{
    class CustomizeGuiElement
    {
        private static ResourceManager resourceManager;
        private static CultureInfo cultureInfo;
        private static void initI18n()
        {
            resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources",
                                               typeof(CustomizeGuiElement).Assembly);
            cultureInfo = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Setzt Icon mit Hilfe von ImageMso
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="iconName"></param>
        public static void SetButtonImage(Tools.Ribbon.RibbonButton btn, string iconName)
        {
            if (resourceManager == null) initI18n();
            btn.OfficeImageId = iconName;
            btn.ShowImage = true;
        }
        /// <summary>
        /// Set button label by btn.Name
        /// </summary>
        /// <param name="btn"></param>
        public static void SetI18nText(Tools.Ribbon.RibbonButton btn)
        {
            if (btn == null)
            {
                return;
            }

            btn.Label = resourceManager.GetString(btn.Name, cultureInfo);
        }
    }


}
