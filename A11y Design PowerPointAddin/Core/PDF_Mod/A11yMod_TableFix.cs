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
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Office = Microsoft.Office;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf.Tagging;
using Microsoft.Office.Interop.PowerPoint;
using A11y_Design_PowerPointAddin.Helper;
using System.Linq;
using System;

namespace A11y_Design_PowerPointAddin.Core
{
    /// <summary>
    /// <strong>Fixes Problem with Tables:</strong>
    /// <list type="bullet">
    /// <item>Empty cells are detected and get Tags that are missing in the PDF</item>
    /// <item>Merged Cells can be found and their tags are adjusted</item>
    /// <item>Headers are corrected as it was defined in the PPT (TD becomes TH)</item>
    /// <item>Headers get a soce (row,colboth) assigned</item>
    /// </list>
    /// <para>
    /// <strong>Assumptions:</strong>
    /// This mod is dependent on the index of a table within the Presentation. 
    /// Therefore "table"-taga should not be deleted or moved by any other mods.
    /// </para>
    /// </summary>
    class A11yMod_TableFix : IA11yModification
    {
        /// <summary>
        /// Structure to store table information from PPT
        /// </summary>
        private struct TableInfo
        {
            public int ColCount { get; private set; }
            public int RowCount { get; private set; }
            public (bool FirstRow, bool LastRow, bool FirstCol, bool LastCol) Headers { get; private set; }
            public Dictionary<(int row, int col), (bool IsEmpty,int RowSpan, int ColSpan)> PptCellProperties { get; set; }

            /// <summary> Connections betwenn cell coordinates in pdf to ppt </summary>
            public Dictionary<(int pdfRow, int pdfCol), (int row, int col)> Pdf2PptCoords { get; set; } // hidden cells do not appear in pdf, therefore we need a mapping from pdf to ppt

            /// <summary> checks if colum of ppt coordinate is header </summary>
            /// <param name="col">col in ppt table</param>
            /// <returns></returns>
            public bool IsRowHeader(int col) => col == 1 && Headers.FirstRow || col == ColCount && Headers.LastRow;
            /// <summary> checks if row of ppt coordinate is header </summary>
            /// <param name="row">row in ppt table</param>
            /// <returns></returns>
            public bool IsColHeader(int row) => row == 1 && Headers.FirstCol || row == RowCount && Headers.LastCol;

            public TableInfo(PowerPoint.Table table)
            {
                ColCount = table.Columns.Count;
                RowCount = table.Rows.Count;
                Headers = (table.FirstRow, table.LastRow, table.FirstCol, table.LastCol);
                PptCellProperties = new Dictionary<(int row, int col), (bool IsEmpty, int rowSpan, int colSpan)>();
                Pdf2PptCoords = new Dictionary<(int pdfRow, int pdfCol), (int row, int col)>();

                RetrievePptCellProperties(table);
            }

            /// <summary> Finds empty cells and spans in Table and stores them in this struct.</summary>
            /// <param name="table"></param>
            private void RetrievePptCellProperties(PowerPoint.Table table)
            {
                // contains cell area and root-cell that has to get the span
                var cellDimensions = new Dictionary<(float, float, float, float), (int row, int col)> ();

                for (int r = 1; r <= RowCount; r++) //table indices start with 1
                {
                    int hiddenColCellCount = 0;
                    for (int c = 1; c <= ColCount; c++)
                    {
                        Cell cell = table.Cell(r, c); // get cell from index iteration
                        Shape s = cell.Shape;
                        bool isHidden = false;
                        var dim = ValueTuple.Create(cell.Shape.Top, cell.Shape.Left, cell.Shape.Width, cell.Shape.Height);
                        // check cell dimensions for existing cell on this position/area
                        if (cellDimensions.TryGetValue(dim, out var spancell)) // cell is in span
                        {
                            var spcellProp = PptCellProperties[spancell];
                            spcellProp.ColSpan = Math.Max(1 + Math.Abs(c - spancell.col), spcellProp.ColSpan);
                            spcellProp.RowSpan = Math.Max(1 + Math.Abs(r - spancell.row), spcellProp.RowSpan);
                            PptCellProperties[spancell] = spcellProp;

                            isHidden = true;
                            hiddenColCellCount++;
                        }
                        else
                            cellDimensions.Add(dim, (r, c));

                        //check empty
                        string text = s.TextFrame.TextRange.Text;
                        bool IsEmpty = text.Trim().Length == 0;
                        //add props
                        PptCellProperties.Add((r, c), (IsEmpty, 1, 1)); // storing all cell properties for now
                        if(!isHidden)
                        {
                            Pdf2PptCoords[(r, c - hiddenColCellCount)] = (r, c);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Collection of Table infos that are retrieved from PPT
        /// </summary>
        private Queue<TableInfo> TableInfos { get; set; }

        private (TableInfo Info, int Row, int Col) CurrentTable;


        public A11yMod_TableFix()
        {
            TableInfos = new Queue<TableInfo>();
        }

        public void Visit(PowerPoint.Slide slide, PowerPoint.Shape shape)
        {
            if (shape.GetNestedType() == Office.Core.MsoShapeType.msoTable)
            {
                var ti = new TableInfo(shape.Table);
                TableInfos.Enqueue(ti);
            }
        }



        
        public bool ModifyPDFNode(PdfPage page, TagTreePointer treePointer)
        {

            switch (treePointer.GetRole().ToUpper())
            {
                case "TR":
                    CurrentTable.Row++;
                    CurrentTable.Col = 0;
                    FixTRKids(treePointer);
                    break;
                
                case "TH":
                    {
                        CurrentTable.Col++;
                        treePointer.GetProperties().ClearAttributes();
                        PdfDictionary dict = new PdfDictionary();

                        FixMergedCells(treePointer, dict);
                        FixTHTag(treePointer, dict);

                        dict.Put(PdfName.O, PdfName.Table);
                        PdfStructureAttributes newAttribute = new PdfStructureAttributes(dict);
                        treePointer.GetProperties().AddAttributes(newAttribute);
                        break;
                    }


                case "TD":
                    {
                        CurrentTable.Col++;
                        treePointer.GetProperties().ClearAttributes();
                        PdfDictionary dict = new PdfDictionary();

                        FixMergedCells(treePointer, dict);
                        FixTDTag(treePointer, dict);

                        if(dict.Size() > 0)
                        {
                            dict.Put(PdfName.O, PdfName.Table);
                            PdfStructureAttributes newAttribute = new PdfStructureAttributes(dict);
                            treePointer.GetProperties().AddAttributes(newAttribute);
                        }

                        break;
                    }


                case "TABLE":
                    CurrentTable.Row = 0;
                    CurrentTable.Col = 0;
                    CurrentTable.Info = TableInfos.Dequeue();
                    FixTableSpan(treePointer, page);
                    break;

                case "TBODY":
                    return true;
                    
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Removes span-tags inside current tree pointer
        /// <para>in some versions of ppt there is a "span" inside "table". This sould not be.  </para>
        /// </summary>
        /// <param name="treePointer"></param>
        public void FixTableSpan(TagTreePointer treePointer, PdfPage page)
        {
            for (int i = 0; i < treePointer.GetKidsCount(); i++)
            {
                if (treePointer.GetKidsRoles()[i].ToUpper() == "TBODY" || treePointer.GetKidsRoles()[i].ToUpper() == "THEAD")
                {
                    treePointer.MoveToKid(i);
                    treePointer.RemoveTag();
                }
            }

            for (int i = 0; i < treePointer.GetKidsCount(); i++)
            {
                if (treePointer.GetKidsRoles()[i].ToUpper() == "SPAN")
                {
                    //var contentStreamAsText = new PdfString(pdfPage.GetContentBytes()).ToString();
                    treePointer.MoveToKid(i);
                    if (treePointer.GetKidsCount() > 0 && treePointer.GetKidsRoles()[0] == "MCR")
                    {
                        int id = treePointer.GetMcid(0);                       
                        var contentStreamAsText = new PdfString(page.GetContentBytes()).ToString();
                        contentStreamAsText = contentStreamAsText.Replace("/P <</MCID " + id + ">> BDC", "/Artifact BMC");
                        page.GetFirstContentStream().SetData(new PdfString(contentStreamAsText).GetValueBytes());
                        treePointer.GetPdfStructureElem().RemoveKid(0);
                    }
                    treePointer.RemoveTag();
                }
            }
        }  

        /// <summary>
        /// A method to convert the TD tag to a TH tag if necessary
        /// </summary>
        public void FixTDTag(TagTreePointer treePointer, PdfDictionary attrDict)
        {
            if (CurrentTable.Info.IsRowHeader(CurrentTable.Row) || CurrentTable.Info.IsColHeader(CurrentTable.Col))
            {
                treePointer.SetRole("TH");
                FixTHTag(treePointer, attrDict);
            }

        }

        /// <summary>
        /// Add missing kids of empty cells
        /// </summary>
        /// <param name="treePointer"></param>
        public void FixTRKids(TagTreePointer treePointer)
        {
            if(CurrentTable.Info.Pdf2PptCoords.Keys.Count(k => k.pdfRow == CurrentTable.Row) > treePointer.GetKidsCount()) //fix only when needed
            {
                // get coordinates of cells that are ...
                var emptyPptcells = CurrentTable.Info.Pdf2PptCoords
                    .Where(pdf2pptEntry => pdf2pptEntry.Key.pdfRow == CurrentTable.Row  // ..  in the current row ..
                        && CurrentTable.Info.PptCellProperties[pdf2pptEntry.Value].IsEmpty) // ... and marked empty ..
                        //&& !(CurrentTable.Info.IsColHeader(pdf2pptEntry.Value.row) || CurrentTable.Info.IsRowHeader(pdf2pptEntry.Value.col))) // .. and not a header .. 
                    .Select(e => e.Key).ToList(); // .. as list of pdf cell coordinates


                foreach (var item in emptyPptcells)
                {
                    treePointer.AddTag(item.pdfCol - 1, "TD"); // should this be a TH tag?? TD seems more likely
                    treePointer.MoveToParent();
                }
            }

           
        }

        /// <summary>
        /// A method to set the scope for TH tags
        /// </summary>
        public void FixTHTag(TagTreePointer treePointer, PdfDictionary attrDict)
        {

            if (CurrentTable.Info.IsRowHeader(CurrentTable.Col) && CurrentTable.Info.IsColHeader(CurrentTable.Row))
            {
                attrDict.Put(PdfName.Scope, PdfName.Both);
            }
            else if (CurrentTable.Info.IsColHeader(CurrentTable.Row))
            {
                attrDict.Put(PdfName.Scope, PdfName.Column);
            }
            else
            {
                attrDict.Put(PdfName.Scope, PdfName.Row);
            }

        }

        /// <summary>
        ///  Set row- and colspan of cell
        /// </summary>
        /// <param name="treePointer"></param>
        private void FixMergedCells(TagTreePointer treePointer, PdfDictionary attrDict)
        {
            var cellId = ValueTuple.Create(CurrentTable.Row, CurrentTable.Col);
            if (CurrentTable.Info.Pdf2PptCoords.TryGetValue(cellId, out var coords)
                && CurrentTable.Info.PptCellProperties.TryGetValue(coords, out var mc))
            {

                if (mc.RowSpan > 1)
                {
                    attrDict.Put(PdfName.RowSpan, new PdfNumber(mc.RowSpan));                    
                }
                if (mc.ColSpan > 1)
                {
                    attrDict.Put(PdfName.ColSpan, new PdfNumber(mc.ColSpan));
                }
            }
        }


        public void ModifyPDFRoot(PdfDocument pdfDoc)
        {
        }

        public void Visit(Slide slide)
        {
        }
    }

}
