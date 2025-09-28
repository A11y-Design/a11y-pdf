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

using Office = Microsoft.Office.Core;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Office.Interop.PowerPoint;
using System.Linq;


namespace A11y_Design_PowerPointAddin.Helper
{    
    class Artifact
    {
        static string prefix = "artifact";  //artifact_id
        static Office.DocumentProperties properties;
        static List<string> keys = new List<string>();

        /// <summary>
        /// Markes element as artifact 
        /// </summary>
        /// <param name="shape">of the element</param>
        public static void MarkAsArtifact(Shape shape)
        {
            properties = (Office.DocumentProperties)Globals.ThisAddIn.Application.ActivePresentation.CustomDocumentProperties;
            string keyId = createKey(shape);            
            if (!keys.Contains(keyId))
            {
                keys.Add(keyId);
                try
                {
                    properties.Add(keyId, false, Office.MsoDocProperties.msoPropertyTypeString, "artifact");
                    //save shapeName
                    properties.Add(createKeyShapeName(keyId), false, Office.MsoDocProperties.msoPropertyTypeString, shape.Name);
                    shape.Name = i18n.GetTranslation("a11yElementMarkedAsArtifact");
                    
                }
                catch (COMException e)
                {
                    Debug.Write("#### ERROR add keyid" + keyId + "\n to custom pro\n" + e.Message);
                }

            }
        }

        /// <summary>
        /// Unmarkes element as artifact 
        /// </summary>
        /// <param name="shape">of the element</param>
        public static void UnmarkAsArtifact(Shape shape)
        {
            string keyId = createKey(shape);
            // remove artifact entry
            if (keys.Contains(keyId)) { 
                keys.Remove(keyId);
                //remove save shape name           
                string keyShapeName = createKeyShapeName(keyId);                                
                string shapeName = readDocumentProperty(keyShapeName);
                if (shapeName != null)
                {
                    shape.Name = shapeName;
                    properties[keyShapeName].Delete();
                }
                if (readDocumentProperty(keyId) != null)
                {
                    properties[keyId].Delete();
                }      
            }
                       
        }

        private static string readDocumentProperty(string propertyName)
        {                        

            foreach (Office.DocumentProperty prop in properties)
            {
                if (prop.Name == propertyName)
                {
                    return prop.Value.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Delete all marked artifacts from Custom Document Properties only for development and testing
        /// </summary>
        public static void DeleteAllArtifactsFromDocumentProperties()
        {
            properties = (Office.DocumentProperties)Globals.ThisAddIn.Application.ActivePresentation.CustomDocumentProperties;
            foreach (Office.DocumentProperty prop in properties)
            {
                if (prop.Name.StartsWith("artifact_")) prop.Delete();
            }
            keys.Clear();
            Globals.ThisAddIn.Application.ActivePresentation.Save();            
        }

        /// <summary>
        /// Loads all marked artifacts
        /// </summary>
        public static void LoadAllArtifacts()
        {
            properties = (Office.DocumentProperties)Globals.ThisAddIn.Application.ActivePresentation.CustomDocumentProperties;
            foreach (Office.DocumentProperty prop in properties)
            {
                keys.Add(prop.Name);                
            }
            keys = keys.Distinct().ToList<string>();
        }

        /// <summary>
        /// Checks whether shape is marked as artifact
        /// </summary>
        /// <param name="shape"></param>
        /// <returns>true if id is marked as artifact</returns>
        public static bool IsIdMarkedAsArtifact(Shape shape)
        {
            return keys.Contains(createKey(shape));
        }

        /// <summary>
        /// Create Key for shape name base on keyId
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        private static string createKeyShapeName(string keyId)
        {
            return keyId + "_shapeName";
        }

        /// <summary>
        /// Create key based on prefix, slide number and shape Id
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        private static string createKey(Shape shape)
        {
            string key = prefix + "_" + ((Slide)shape.Parent).SlideNumber + "_" + shape.Id;
            return key;
        }

    }

}


