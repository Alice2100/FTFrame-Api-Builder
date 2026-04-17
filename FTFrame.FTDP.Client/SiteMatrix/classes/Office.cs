using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
 using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2016.Excel;

namespace FTDPClient.classes
{
    public class Excel :IDisposable
    {
        public MemoryStream SpreadsheetStream { get; set; } // The stream that the spreadsheet gets returned on
        private Worksheet currentWorkSheet { get { return spreadSheet.WorkbookPart.WorksheetParts.First().Worksheet; } }
        private SpreadsheetDocument spreadSheet;
        private Columns _cols;



        /// <summary>
        /// Create a basic spreadsheet template
        /// The structure of OpenXML spreadsheet is something like this from what I can tell:
        ///                        Spreadsheet
        ///                              |
        ///                         WorkbookPart
        ///                   /         |             \
        ///           Workbook WorkbookStylesPart WorksheetPart
        ///                 |          |               |
        ///            Sheets     StyleSheet        Worksheet
        ///                |                        /        \
        ///          (refers to               SheetData        Columns
        ///           Worksheetparts)            |
        ///                                     Rows
        ///
        /// Obviously this only covers the bits in this class!
        /// </summary>
        /// <returns></returns>
        public bool CreateSpreadsheet(string filename)
        {
            try
            {
                //SpreadsheetStream = new MemoryStream();

                // Create the spreadsheet on the MemoryStream
                spreadSheet =
                         SpreadsheetDocument.Create(filename, SpreadsheetDocumentType.Workbook);

                WorkbookPart wbp = spreadSheet.AddWorkbookPart();   // Add workbook part
                //WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>(); // Add worksheet part
                Workbook wb = new Workbook(); // Workbook
                FileVersion fv = new FileVersion();
                fv.ApplicationName = "Wibble Wobble";
                Worksheet ws = new Worksheet(); // Worksheet
                SheetData sd = new SheetData(); // Data on worksheet

                // Add stylesheet
                WorkbookStylesPart stylesPart = spreadSheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = GenerateStyleSheet();
                stylesPart.Stylesheet.Save();




                ws.Append(sd); // Add sheet data to worksheet




                spreadSheet.WorkbookPart.Workbook = wb;
                spreadSheet.WorkbookPart.Workbook.Save();

            }
            catch
            {
                return false;
            }

            return true;
        }
        public void MergeCells(string sheetName, string cell1Name, string cell2Name)
        {
            Worksheet worksheet = _getWorkSheet(sheetName);
            if (worksheet == null || string.IsNullOrEmpty(cell1Name) || string.IsNullOrEmpty(cell2Name))
            {
                return;
            }

            MergeCells mergeCells;
            if (worksheet.Elements<MergeCells>().Count() > 0)
            {
                mergeCells = worksheet.Elements<MergeCells>().First();
            }
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.
                if (worksheet.Elements<CustomSheetView>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                }
                else if (worksheet.Elements<DataConsolidate>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<DataConsolidate>().First());
                }
                else if (worksheet.Elements<SortState>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SortState>().First());
                }
                //else if (worksheet.Elements<AutoFilter>().Count() > 0)
                //{
                //    worksheet.InsertAfter(mergeCells, worksheet.Elements<AutoFilter>().First());
                //}
                else if (worksheet.Elements<Scenarios>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<Scenarios>().First());
                }
                else if (worksheet.Elements<ProtectedRanges>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<ProtectedRanges>().First());
                }
                else if (worksheet.Elements<SheetProtection>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetProtection>().First());
                }
                else if (worksheet.Elements<SheetCalculationProperties>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetCalculationProperties>().First());
                }
                else
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
                }
            }

            // Create the merged cell and append it to the MergeCells collection.
            MergeCell mergeCell = new MergeCell() { Reference = new StringValue(cell1Name + ":" + cell2Name) };
            mergeCells.Append(mergeCell);

            worksheet.Save();
        }

        public string AddSheet(string Name)
        {
            WorksheetPart wsp = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
            wsp.Worksheet = new Worksheet();

            wsp.Worksheet.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

            wsp.Worksheet.Save();

            UInt32 sheetId;

            // If this is the first sheet, the ID will be 1. If this is not the first sheet, we calculate the ID based on the number of existing
            // sheets + 1.
            if (spreadSheet.WorkbookPart.Workbook.Sheets == null)
            {
                spreadSheet.WorkbookPart.Workbook.AppendChild(new Sheets());
                sheetId = 1;
            }
            else
            {
                sheetId = Convert.ToUInt32(spreadSheet.WorkbookPart.Workbook.Sheets.Count() + 1);
            }

            // Create the new sheet and add it to the workbookpart
            spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = spreadSheet.WorkbookPart.GetIdOfPart(wsp),
                SheetId = sheetId,
                Name = Name
            }
            );

            _cols = new Columns(); // Created to allow bespoke width columns
            // Save our changes
            spreadSheet.WorkbookPart.Workbook.Save();

            return spreadSheet.WorkbookPart.GetIdOfPart(wsp);// wsp;
        }

        private Worksheet _getWorkSheet(string sheetId)
        {
            WorksheetPart wsp = (WorksheetPart)spreadSheet.WorkbookPart.GetPartById(sheetId);
            return wsp.Worksheet;
        }

        /// <summary>
        /// add the bespoke columns for the list spreadsheet
        /// </summary>
        public void CreateColumnWidth(string sheetId, uint startIndex, uint endIndex, double width)
        {
            // Find the columns in the worksheet and remove them all

            if (_getWorkSheet(sheetId).Where(x => x.LocalName == "cols").Count() > 0)
                _getWorkSheet(sheetId).RemoveChild<Columns>(_cols);

            // Create the column
            Column column = new Column();
            column.Min = startIndex;
            column.Max = endIndex;
            column.Width = width;
            column.CustomWidth = true;
            _cols.Append(column); // Add it to the list of columns

            // Make sure that the column info is inserted *before* the sheetdata

            _getWorkSheet(sheetId).InsertBefore<Columns>(_cols, _getWorkSheet(sheetId).Where(x => x.LocalName == "sheetData").First());
            _getWorkSheet(sheetId).Save();
            spreadSheet.WorkbookPart.Workbook.Save();
        }

        /// <summary>
        /// Close the spreadsheet
        /// </summary>
        public void CloseSpreadsheet()
        {
            spreadSheet.Close();
        }

        /// <summary>
        /// Pass a list of column headings to create the header row
        /// </summary>
        /// <param name="headers"></param>
        public List<Cell> AddHeader(string sheetId, List<string> headers,Func<string,uint> styleFunc = null )
        {
            // Find the sheetdata of the worksheet
            var cells = new List<Cell>();
            SheetData sd = (SheetData)_getWorkSheet(sheetId).Where(x => x.LocalName == "sheetData").First();
            Row header = new Row();
            // increment the row index to the next row
            header.RowIndex = Convert.ToUInt32(sd.ChildElements.Count()) + 1;
            sd.Append(header); // Add the header row

            foreach (string heading in headers)
            {
                cells.Add(AppendCell(header, header.RowIndex, heading, styleFunc==null? 1 : styleFunc(heading)));

            }

            // save worksheet

            _getWorkSheet(sheetId).Save();
            return cells;
        }


        /// <summary>
        /// Pass a list of data items to create a data row
        /// </summary>
        /// <param name="dataItems"></param>
        public List<Cell> AddRow(string sheetId, List<string> dataItems, Func<string, uint> styleFunc = null)
        {
            // Find the sheetdata of the worksheet
            var cells = new List<Cell>();
            SheetData sd = (SheetData)_getWorkSheet(sheetId).Where(x => x.LocalName == "sheetData").First();
            Row header = new Row();
            // increment the row index to the next row
            header.RowIndex = Convert.ToUInt32(sd.ChildElements.Count()) + 1;


            sd.Append(header);

            foreach (string item in dataItems)
            {
                cells.Add(AppendCell(header, header.RowIndex, item, styleFunc == null ? 0 : styleFunc(item)));

            }

            // save worksheet

            _getWorkSheet(sheetId).Save();
            return cells;
        }

        /// <summary>
        /// Add cell into the passed row.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        /// <param name="styleIndex"></param>
        private Cell AppendCell(Row row, uint rowIndex, string value, uint styleIndex)
        {
            Cell cell = new Cell();
            cell.DataType = CellValues.InlineString;
            cell.StyleIndex = styleIndex;  // Style index comes from stylesheet generated in GenerateStyleSheet()
            Text t = new Text();
            t.Text = value; 

            // Append Text to InlineString object
            InlineString inlineString = new InlineString();
            inlineString.AppendChild(t);

            // Append InlineString to Cell
            cell.AppendChild(inlineString);

            // Get the last cell's column
            string nextCol = "A";
            Cell c = (Cell)row.LastChild;
            if (c != null) // if there are some cells already there...
            {
                int numIndex = c.CellReference.ToString().IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });

                // Get the last column reference
                string lastCol = c.CellReference.ToString().Substring(0, numIndex);
                // Increment
                nextCol = IncrementColRef(lastCol);
            }

            cell.CellReference = nextCol + rowIndex;

            row.AppendChild(cell);
            return cell;
        }

        // Increment the column reference in an Excel fashion, i.e. A, B, C...Z, AA, AB etc.
        // Partly stolen from somewhere on the Net and modified for my use.
        private string IncrementColRef(string lastRef)
        {
            char[] characters = lastRef.ToUpperInvariant().ToCharArray();
            int sum = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                sum *= 26;
                sum += (characters[i] - 'A' + 1);
            }

            sum++;

            string columnName = String.Empty;
            int modulo;

            while (sum > 0)
            {
                modulo = (sum - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                sum = (int)((sum - modulo) / 26);
            }

            return columnName;


        }

        /// <summary>
        /// Return a stylesheet. Completely stolen from somewhere, possibly this guy's blog,
        /// although I can't find it on there:
        /// http://polymathprogrammer.com/. Thanks whoever it was, it would have been a
        /// nightmare trying to figure this one out!
        /// </summary>
        /// <returns></returns>
        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(                                                               // Index 0 - The default font.
                        new FontSize() { Val = 12 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 1 - The bold font.
                        new Bold(),
                        new FontSize() { Val = 12 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 - The Italic font.
                        //new Italic(),
                        new Bold(),
                        new FontSize() { Val = 12 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 - The Times Roman font. with 16 size
                        new FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(                                                           // Index 0 - The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(                                                           // Index 1 - The default fill of gray 125 (required)
                        new PatternFill(new BackgroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }) { PatternType = PatternValues.Gray125}),
                    new Fill(                                                           // Index 2 - The yellow fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FF00CCCC" } }
                        )
                        { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(                                                         // Index 0 - The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new RightBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new TopBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new BottomBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 - The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyFont = true },       // Index 1 - Bold
                    new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 2 - Italic
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 - Times Roman
                    new CellFormat() { FontId = 2, FillId = 2, BorderId = 1, ApplyFill = true },       // Index 4 - Yellow Fill
                    new CellFormat(                                                                   // Index 5 - Alignment
                        new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    )
                    { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 6 - Border
                )
            ); // return
        }

        public void Dispose()
        {
            CloseSpreadsheet();
        }
    }
}
