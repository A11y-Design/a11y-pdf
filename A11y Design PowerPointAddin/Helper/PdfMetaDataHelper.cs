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

using iText.Kernel.Pdf;
using iText.Kernel.XMP.Options;
using iText.Licensing.Base;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using System.IO;
using System.Linq;

namespace A11y_Design_PowerPointAddin.Helper
{
    class PdfMetaDataHelper
    {
        /// <summary>
        /// Replace all set authors by new authors in list
        /// </summary>
        /// <param name="pdf"></param>
        /// <param name="presentation"></param>
        /// <returns></returns>
        public static byte[] SetAuthorMetadata(byte[] pdf, PowerPoint.Presentation presentation )
        {
            string[] authors = new string[] {}; // all authors that are set in the powerpoint
            foreach (var property in presentation.BuiltInDocumentProperties)
            {                
                if (property.Name.ToLower().Equals("author"))
                {
                    string allAuthors = property.Value;
                    authors = allAuthors.Split(';').ToArray();
                }
            }


            var inputStream = new MemoryStream(pdf);
            var reader = new PdfReader(inputStream);
            var outputStream = new MemoryStream();
            var writer = new PdfWriter(outputStream);
            var pdfDoc = new PdfDocument(reader, writer);
            // 🧹 1. Info-Dictionary cleanup and set new author
            PdfDictionary infoDict = pdfDoc.GetTrailer().GetAsDictionary(PdfName.Info);
            if (infoDict != null)
            {                
                // remove old author entry, if it exists
                infoDict.Remove(PdfName.Author);             
                //infoDict.Put(PdfName.Author, new PdfString(string.Join("; ", authors)));
            }

            var xmp = iText.Kernel.XMP.XMPMetaFactory.Create();
            foreach (var author in authors)
            {
                xmp.AppendArrayItem(
                    "http://purl.org/dc/elements/1.1/",
                    "creator",
                    new PropertyOptions(PropertyOptions.ARRAY_ORDERED),
                    author,
                    null
                );
            }            
            pdfDoc.SetXmpMetadata(xmp, new SerializeOptions());

            pdfDoc.Close();
            return outputStream.ToArray();
        }
    }
}


