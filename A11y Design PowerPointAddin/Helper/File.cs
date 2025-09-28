// a11y pdf – A customizable PDF export tool for generating PDF files 
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
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;


namespace A11y_Design_PowerPointAddin.Helper
{
    class File
    {
        /// <summary>
        /// Delete all files in path
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteAllFiles(string path)
        {            
            DeleteFile(System.IO.Directory.GetFiles(path));
        }


        public static void DeleteFile(string[] files) {
            foreach (string file in files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
