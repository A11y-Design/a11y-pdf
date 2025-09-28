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
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace A11y_Design_PowerPointAddin.Controls
{
    public partial class LicenseInfoWindow : Form
    {
        private AddInRibbon buffer;
        private static LicenseInfoWindow _window = null;
        private static ResourceManager resourceManager = new ResourceManager("A11y_Design_PowerPointAddin.Properties.Resources", typeof(A11yIncidentType_ShapeAltText).Assembly);
        public LicenseInfoWindow()
        {
            InitializeComponent();            
            buffer = Globals.Ribbons.AddInRibbon;
            btnClose.Text = resourceManager.GetString("CloseText");
        }

        public static LicenseInfoWindow GetInstance()
        {
            if (_window == null)
            {
                _window = new LicenseInfoWindow();
                _window.Disposed += delegate { _window = null; };
            }
            return _window;
        }

        private void LicenseInfoWindow_Load(object sender, EventArgs e)
        {

            Screen screen = Screen.FromHandle(new IntPtr(Globals.ThisAddIn.Application.ActiveWindow.HWND));

            Location = screen.WorkingArea.Location;

            _window.Text = Properties.Resources.LicenseInfoWindowTitle;
            textVersionNr.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }


        private void textBoxLicenseKey_TextChanged(object sender, EventArgs e)
        {
            return;
            // Not required  anymore
            //if(textBoxLicenseKey.Text.Length >= 4)
            //{
            //    btnActivateLicense.Enabled = true;
            //}
            //else
            //{
            //    btnActivateLicense.Enabled = false;
            //}
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.a11y-design.de",
                UseShellExecute = true  // wichtig für neue .NET-Versionen
            });
        }
    }
}
