using System.Xml;
using System.Collections;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Drawing;
using System.Data;
using Newtonsoft.Json;
using System.Web;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FTFrame.Server.Core.Office
{
    public class Excel : IDisposable
    {
        string sheetName = "sheet1";
        SpreadsheetDocument xl;
        OpenXmlWriter oxw;
        WorksheetPart wsp;
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="path">Excel文件名称(全路径)</param>
        /// <param name="rowCount">表格列数量</param>
        /// <param name="sheet">表格名称</param>
        public Excel(string path, string sheet = "sheet1")
        {
            this.sheetName = sheet;
            xl = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
            xl.AddWorkbookPart();
            if (xl.WorkbookPart == null) throw new ArgumentNullException();
            wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();
            oxw = OpenXmlWriter.Create(wsp);
            oxw.WriteStartElement(new Worksheet());
            oxw.WriteStartElement(new SheetData());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="rowCount"></param>
        /// <param name="sheet"></param>
        public Excel(Stream stream, string sheet = "sheet1")
        {
            this.sheetName = sheet;
            xl = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            xl.AddWorkbookPart();
            if (xl.WorkbookPart == null) throw new ArgumentNullException();
            wsp = xl.WorkbookPart.AddNewPart<WorksheetPart>();
            oxw = OpenXmlWriter.Create(wsp);
            oxw.WriteStartElement(new Worksheet());
            oxw.WriteStartElement(new SheetData());
        }
        /// <summary>
        /// 写入表格数据
        /// </summary>
        /// <param name="datas"></param>
        public void Write(object[] datas)
        {
            if (datas == null || datas.Length == 0) return;
            int colNum = datas.Length;
            //oxa = new List<OpenXmlAttribute>();
            // this is the row index
            //oxa.Add(new OpenXmlAttribute("r", null, i.ToString()));
            //oxw.WriteStartElement(new Row(), oxa);
            oxw.WriteStartElement(new Row());
            for (int j = 0; j < colNum; ++j)
            {
                var oxa = new List<OpenXmlAttribute>();
                // this is the data type ("t"), with CellValues.String ("str")
                oxa.Add(new OpenXmlAttribute("t", null, "str"));
                // it's suggested you also have the cell reference, but
                // you'll have to calculate the correct cell reference yourself.
                // Here's an example:
                //oxa.Add(new OpenXmlAttribute("r", null, "A1"));
                //c.Append(f);
                oxw.WriteStartElement(new Cell() { DataType = CellValues.InlineString }, oxa);
                //oxw.WriteStartElement(new Cell());
                oxw.WriteElement(new CellValue($"{datas[j]}"));
                // this is for Cell
                oxw.WriteEndElement();
            }
            // this is for Row
            oxw.WriteEndElement();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public void Write(DataTable dt)
        {
            object[] titles = new object[dt.Columns.Count];
            for (int i = 0; i < titles.Length; i++) titles[i] = dt.Columns[i].ColumnName;
            Write(titles);
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                Write(dt.Rows[row].ItemArray);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="maxColumns"></param>
        /// <param name="readHeader"></param>
        /// <returns></returns>
        public static DataTable ReadIntoDatatableFromExcel(string filePath, string sheetName = null, int maxColumns = 100, bool readHeader = true)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return ReadIntoDatatableFromExcel(stream, sheetName, maxColumns, readHeader);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sheetName"></param>
        /// <param name="maxColumns"></param>
        /// <param name="readHeader"></param>
        /// <returns></returns>
        public static DataTable ReadIntoDatatableFromExcel(Stream stream, string sheetName = null, int maxColumns = 100, bool readHeader = true)
        {
            var dt = new DataTable();
            for (int i = 0; i < maxColumns; i++) dt.Columns.Add();

            try
            {
                using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(stream, false))
                {
                    var workbookPart = spreadsheetDocument.WorkbookPart;
                    var workbook = workbookPart.Workbook;

                    /*get only unhide tabs*/
                    var sheets = workbook.Descendants<Sheet>().Where(e => e.State == null);
                    var sheet = sheetName == null ? sheets.FirstOrDefault() : sheets.Where(r => r.Name == sheetName).FirstOrDefault();
                    if (sheet == null) throw new Exception("Can not Find Sheet");
                    //foreach (var sheet in sheets)
                    //{
                    var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);

                    /*Remove empty sheets*/
                    List<Row> rows = worksheetPart.Worksheet.Elements<SheetData>().First().Elements<Row>()
                        .Where(r => r.InnerText != string.Empty).ToList();

                    if (rows.Count > 1)
                    {
                        OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);

                        int i = 0;
                        int BTR = 0;/*Break the reader while empty rows are found*/

                        while (reader.Read())
                        {
                            if (reader.ElementType == typeof(Row))
                            {
                                /*ignoring first row with headers and check if data is there after header*/
                                if (i < (readHeader ? 0 : 1))
                                {
                                    i++;
                                    continue;
                                }

                                reader.ReadFirstChild();

                                DataRow row = dt.NewRow();

                                int CN = 0;

                                if (reader.ElementType == typeof(Cell))
                                {
                                    do
                                    {
                                        Cell c = (Cell)reader.LoadCurrentElement();

                                        /*reader skipping blank cells so data is getting worng in datatable's rows according to header*/
                                        if (CN != 0)
                                        {
                                            int cellColumnIndex =
                                                GetColumnIndexFromName(
                                                    GetColumnName(c.CellReference));

                                            if (cellColumnIndex < maxColumns && CN < cellColumnIndex - 1)
                                            {
                                                do
                                                {
                                                    row[CN] = string.Empty;
                                                    CN++;
                                                } while (CN < cellColumnIndex - 1);
                                            }
                                        }

                                        /*stopping execution if first cell does not have any value which means empty row*/
                                        if (CN == 0 && c.DataType == null && c.CellValue == null)
                                        {
                                            BTR++;
                                            break;
                                        }

                                        string cellValue = GetCellValue(c, workbookPart);
                                        row[CN] = cellValue;
                                        CN++;

                                        /*if any text exists after T column (index 20) then skip the reader*/
                                        if (CN == maxColumns)
                                        {
                                            break;
                                        }
                                    } while (reader.ReadNextSibling());
                                }

                                /*reader skipping blank cells so fill the array upto 19 index*/
                                while (CN != 0 && CN < maxColumns)
                                {
                                    row[CN] = string.Empty;
                                    CN++;
                                }

                                if (CN == maxColumns)
                                {
                                    dt.Rows.Add(row);
                                }
                            }
                            /*escaping empty rows below data filled rows after checking 5 times */
                            if (BTR > 5)
                                break;
                        }
                        reader.Close();
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
        private static string GetCellValue(Cell c, WorkbookPart workbookPart)
        {
            string cellValue = string.Empty;
            if (c.DataType != null && c.DataType == CellValues.SharedString)
            {
                SharedStringItem ssi =
                    workbookPart.SharedStringTablePart.SharedStringTable
                        .Elements<SharedStringItem>()
                        .ElementAt(int.Parse(c.CellValue.InnerText));
                if (ssi.Text != null)
                {
                    cellValue = ssi.Text.Text;
                }
            }
            else
            {
                if (c.CellValue != null)
                {
                    cellValue = c.CellValue.InnerText;
                }
            }
            return cellValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnNameOrCellReference"></param>
        /// <returns></returns>
        public static int GetColumnIndexFromName(string columnNameOrCellReference)
        {
            int columnIndex = 0;
            int factor = 1;
            for (int pos = columnNameOrCellReference.Length - 1; pos >= 0; pos--)   // R to L
            {
                if (Char.IsLetter(columnNameOrCellReference[pos]))  // for letters (columnName)
                {
                    columnIndex += factor * ((columnNameOrCellReference[pos] - 'A') + 1);
                    factor *= 26;
                }
            }
            return columnIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellReference"></param>
        /// <returns></returns>
        public static string GetColumnName(string cellReference)
        {
            /* Advance from L to R until a number, then return 0 through previous position*/
            for (int lastCharPos = 0; lastCharPos <= 3; lastCharPos++)
                if (Char.IsNumber(cellReference[lastCharPos]))
                    return cellReference.Substring(0, lastCharPos);

            throw new ArgumentOutOfRangeException("cellReference");
        }

        void Close()
        {
            // this is for SheetData
            oxw.WriteEndElement();
            // this is for Worksheet
            oxw.WriteEndElement();
            oxw.Close();
            if (xl.WorkbookPart == null) throw new ArgumentNullException();
            oxw = OpenXmlWriter.Create(xl.WorkbookPart);
            oxw.WriteStartElement(new Workbook());
            oxw.WriteStartElement(new Sheets());
            // you can use object initialisers like this only when the properties
            // are actual properties. SDK classes sometimes have property-like properties
            // but are actually classes. For example, the Cell class has the CellValue
            // "property" but is actually a child class internally.
            // If the properties correspond to actual XML attributes, then you're fine.
            oxw.WriteElement(new Sheet()
            {
                Name = sheetName,
                SheetId = 1,
                Id = xl.WorkbookPart.GetIdOfPart(wsp)
            });
            // this is for Sheets
            oxw.WriteEndElement();
            // this is for Workbook
            oxw.WriteEndElement();
            oxw.Close();
            xl.Close();
        }
        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    Close();
                }
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                disposedValue = true;
            }
        }
        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~OpenXmlExt() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }
        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
