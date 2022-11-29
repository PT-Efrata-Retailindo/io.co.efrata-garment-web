using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Barebone.Util
{
    public static class Excel
    {
        /// <summary>
        /// Create an excel file using MemoryStream.
        /// File name is assigned later in Response.AddHeader() when you want to download.
        /// Each DataTable will be rendered in its own sheet, so you need to supply its sheet name as well.
        /// </summary>
        /// <param name="dtSourceList">A List of KeyValuePair of DataTable and its sheet name</param>
        /// <param name="styling">Default style is set to False</param>
        /// <returns>MemoryStream object to be written into Response.OutputStream</returns>

        public static MemoryStream CreateExcel(List<KeyValuePair<DataTable, String>> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();

            foreach (KeyValuePair<DataTable, String> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);
                sheet.Cells["A1"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcel(List<(DataTable dataTable, string sheetName, List<(string cells, Enum hAlign, Enum vAlign)> mergeCells)> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            foreach ((DataTable dataTable, string sheetName, List<(string, Enum, Enum)> mergeCells) in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(sheetName);
                sheet.Cells["A1"].LoadFromDataTable(dataTable, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                foreach ((string cells, Enum hAlign, Enum vAlign) in mergeCells)
                {
                    sheet.Cells[cells].Merge = true;
                    sheet.Cells[cells].Style.HorizontalAlignment = (OfficeOpenXml.Style.ExcelHorizontalAlignment)hAlign;
                    sheet.Cells[cells].Style.VerticalAlignment = (OfficeOpenXml.Style.ExcelVerticalAlignment)hAlign;
                }
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }
    }
}
