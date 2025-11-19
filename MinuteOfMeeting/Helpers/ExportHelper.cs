using OfficeOpenXml;
using System.Data;

namespace MinuteOfMeeting.Helpers
{
    /// <summary>
    /// Export Helper Class
    /// Handles Excel export functionality using EPPlus
    /// </summary>
    public static class ExportHelper
    {
        /// <summary>
        /// Export DataTable to Excel
        /// </summary>
        /// <param name="dt">DataTable to export</param>
        /// <param name="sheetName">Name of the Excel sheet</param>
        /// <returns>Excel file as byte array</returns>
        public static byte[] ExportToExcel(DataTable dt, string sheetName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);

                // Add headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dt.Columns[i].ColumnName;
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Add data
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        object value = dt.Rows[row][col];

                        // Handle DBNull values
                        if (value == DBNull.Value)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = "";
                        }
                        else
                        {
                            worksheet.Cells[row + 2, col + 1].Value = value;
                        }

                        // Add border to data cells
                        worksheet.Cells[row + 2, col + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Add total row count
                worksheet.Cells[dt.Rows.Count + 3, 1].Value = $"Total Records: {dt.Rows.Count}";
                worksheet.Cells[dt.Rows.Count + 3, 1].Style.Font.Bold = true;

                // Add timestamp
                worksheet.Cells[dt.Rows.Count + 4, 1].Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                worksheet.Cells[dt.Rows.Count + 4, 1].Style.Font.Italic = true;

                return package.GetAsByteArray();
            }
        }

        /// <summary>
        /// Export List of objects to Excel
        /// </summary>
        /// <typeparam name="T">Type of objects</typeparam>
        /// <param name="data">List of objects to export</param>
        /// <param name="sheetName">Name of the Excel sheet</param>
        /// <returns>Excel file as byte array</returns>
        public static byte[] ExportToExcel<T>(List<T> data, string sheetName)
        {
            if (data == null || !data.Any())
                return ExportEmptyExcel(sheetName);

            DataTable dt = ConvertToDataTable(data);
            return ExportToExcel(dt, sheetName);
        }

        /// <summary>
        /// Create empty Excel file with headers only
        /// </summary>
        /// <param name="sheetName">Name of the Excel sheet</param>
        /// <returns>Excel file as byte array</returns>
        public static byte[] ExportEmptyExcel(string sheetName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);

                worksheet.Cells[1, 1].Value = "No data available";
                worksheet.Cells[1, 1].Style.Font.Italic = true;

                worksheet.Cells[2, 1].Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                worksheet.Cells[2, 1].Style.Font.Italic = true;

                return package.GetAsByteArray();
            }
        }

        /// <summary>
        /// Export meetings with custom formatting
        /// </summary>
        /// <param name="dt">Meetings DataTable</param>
        /// <returns>Excel file as byte array</returns>
        public static byte[] ExportMeetingsToExcel(DataTable dt)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Meetings");

                // Style for headers
                var headerStyle = worksheet.Cells[1, 1, 1, dt.Columns.Count].Style;
                headerStyle.Font.Bold = true;
                headerStyle.Font.Color.SetColor(System.Drawing.Color.White);
                headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkBlue);
                headerStyle.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                // Add headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = GetFormattedColumnName(dt.Columns[i].ColumnName);
                }

                // Add data
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        object value = dt.Rows[row][col];

                        // Special formatting for specific columns
                        if (dt.Columns[col].ColumnName.Contains("Date"))
                        {
                            if (DateTime.TryParse(value?.ToString(), out DateTime dateValue))
                            {
                                worksheet.Cells[row + 2, col + 1].Value = dateValue;
                                worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "yyyy-mm-dd hh:mm";
                            }
                        }
                        else if (dt.Columns[col].ColumnName.Contains("Cancelled"))
                        {
                            bool isCancelled = Convert.ToBoolean(value);
                            worksheet.Cells[row + 2, col + 1].Value = isCancelled ? "Yes" : "No";

                            if (isCancelled)
                            {
                                worksheet.Cells[row + 2, col + 1].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                            }
                        }
                        else
                        {
                            worksheet.Cells[row + 2, col + 1].Value = value ?? "";
                        }

                        // Add border
                        worksheet.Cells[row + 2, col + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Add summary section
                int summaryRow = dt.Rows.Count + 3;
                worksheet.Cells[summaryRow, 1].Value = "SUMMARY";
                worksheet.Cells[summaryRow, 1].Style.Font.Bold = true;
                worksheet.Cells[summaryRow, 1].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);

                worksheet.Cells[summaryRow + 1, 1].Value = $"Total Meetings: {dt.Rows.Count}";
                worksheet.Cells[summaryRow + 1, 1].Style.Font.Bold = true;

                // Count by status if available
                if (dt.Columns.Contains("MeetingStatus"))
                {
                    var statusCount = dt.AsEnumerable()
                        .GroupBy(row => row["MeetingStatus"])
                        .Select(g => new { Status = g.Key, Count = g.Count() });

                    int statusRow = summaryRow + 2;
                    foreach (var status in statusCount)
                    {
                        worksheet.Cells[statusRow, 1].Value = $"{status.Status}: {status.Count}";
                        statusRow++;
                    }
                }

                // Add metadata
                worksheet.Cells[dt.Rows.Count + 10, 1].Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                worksheet.Cells[dt.Rows.Count + 10, 1].Style.Font.Italic = true;
                worksheet.Cells[dt.Rows.Count + 10, 1].Style.Font.Size = 10;

                return package.GetAsByteArray();
            }
        }

        /// <summary>
        /// Convert List of objects to DataTable
        /// </summary>
        /// <typeparam name="T">Type of objects</typeparam>
        /// <param name="data">List of objects</param>
        /// <returns>DataTable</returns>
        private static DataTable ConvertToDataTable<T>(List<T> data)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Get properties
            var properties = typeof(T).GetProperties();

            // Create columns
            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Add rows
            foreach (var item in data)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item) ?? DBNull.Value;
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        /// <summary>
        /// Format column names for better readability
        /// </summary>
        /// <param name="columnName">Original column name</param>
        /// <returns>Formatted column name</returns>
        private static string GetFormattedColumnName(string columnName)
        {
            // Convert PascalCase to readable format
            return string.Join(" ", System.Text.RegularExpressions.Regex.Split(columnName, "(?=[A-Z])"))
                       .Replace("ID", "ID")
                       .Replace("MOM", "MOM");
        }

        /// <summary>
        /// Export multiple tables to different sheets
        /// </summary>
        /// <param name="tables">Dictionary of table names and DataTables</param>
        /// <returns>Excel file as byte array</returns>
        public static byte[] ExportMultipleSheets(Dictionary<string, DataTable> tables)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                foreach (var table in tables)
                {
                    var worksheet = package.Workbook.Worksheets.Add(table.Key);

                    // Add headers
                    for (int i = 0; i < table.Value.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = table.Value.Columns[i].ColumnName;
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    }

                    // Add data
                    for (int row = 0; row < table.Value.Rows.Count; row++)
                    {
                        for (int col = 0; col < table.Value.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = table.Value.Rows[row][col] ?? "";
                        }
                    }

                    worksheet.Cells.AutoFitColumns();
                }

                return package.GetAsByteArray();
            }
        }

        /// <summary>
        /// Create downloadable file result
        /// </summary>
        /// <param name="data">Excel data as byte array</param>
        /// <param name="fileName">File name without extension</param>
        /// <returns>FileContentResult</returns>
        public static Microsoft.AspNetCore.Mvc.FileContentResult CreateExcelFile(byte[] data, string fileName)
        {
            string fullFileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return new Microsoft.AspNetCore.Mvc.FileContentResult(
                data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fullFileName
            };
        }
    }
}