using ClosedXML.Excel;
using System.Data;

namespace MinuteOfMeeting.Helpers
{
    /// <summary>
    /// Export Helper Class
    /// Handles Excel export functionality using ClosedXML
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
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(sheetName);

                // Add headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = dt.Columns[i].ColumnName;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                // Add data
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        object value = dt.Rows[row][col];

                        // Handle DBNull values
                        var cell = worksheet.Cell(row + 2, col + 1);
                        if (value == DBNull.Value)
                        {
                            cell.Value = "";
                        }
                        else
                        {
                            if (value is string str)
                                cell.Value = str;
                            else if (value is int num)
                                cell.Value = num;
                            else if (value is double dbl)
                                cell.Value = dbl;
                            else if (value is DateTime dateValue)
                                cell.Value = dateValue;
                            else if (value is bool b)
                                cell.Value = b;
                            else
                                cell.Value = value?.ToString() ?? "";
                        }

                        // Add border to data cells
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Add total row count
                var totalCell = worksheet.Cell(dt.Rows.Count + 3, 1);
                totalCell.Value = $"Total Records: {dt.Rows.Count}";
                totalCell.Style.Font.Bold = true;

                // Add timestamp
                var timestampCell = worksheet.Cell(dt.Rows.Count + 4, 1);
                timestampCell.Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                timestampCell.Style.Font.Italic = true;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
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
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(sheetName);

                var cell1 = worksheet.Cell(1, 1);
                cell1.Value = "No data available";
                cell1.Style.Font.Italic = true;

                var cell2 = worksheet.Cell(2, 1);
                cell2.Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                cell2.Style.Font.Italic = true;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Export meetings with custom formatting
        /// </summary>
        /// <param name="dt">Meetings DataTable</param>
        /// <returns>Excel file as byte array</returns>
        public static byte[] ExportMeetingsToExcel(DataTable dt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Meetings");

                // Add and style headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = GetFormattedColumnName(dt.Columns[i].ColumnName);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                // Add data
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        object value = dt.Rows[row][col];
                        var cell = worksheet.Cell(row + 2, col + 1);

                        // Special formatting for specific columns
                        if (dt.Columns[col].ColumnName.Contains("Date"))
                        {
                            if (DateTime.TryParse(value?.ToString(), out DateTime dateValue))
                            {
                                cell.Value = dateValue;
                                cell.Style.DateFormat.Format = "yyyy-mm-dd hh:mm";
                            }
                        }
                        else if (dt.Columns[col].ColumnName.Contains("Cancelled"))
                        {
                            bool isCancelled = Convert.ToBoolean(value);
                            cell.Value = isCancelled ? "Yes" : "No";

                            if (isCancelled)
                            {
                                cell.Style.Font.FontColor = XLColor.Red;
                            }
                        }
                        else
                        {
                            var val = value ?? "";
                            if (val is string str)
                                cell.Value = str;
                            else if (val is int num)
                                cell.Value = num;
                            else if (val is double dbl)
                                cell.Value = dbl;
                            else if (val is DateTime dateTime)
                                cell.Value = dateTime;
                            else if (val is bool b)
                                cell.Value = b;
                            else
                                cell.Value = val.ToString();
                        }

                        // Add border
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Add summary section
                int summaryRow = dt.Rows.Count + 3;
                var summaryHeaderCell = worksheet.Cell(summaryRow, 1);
                summaryHeaderCell.Value = "SUMMARY";
                summaryHeaderCell.Style.Font.Bold = true;
                summaryHeaderCell.Style.Font.FontColor = XLColor.DarkBlue;

                var totalCell = worksheet.Cell(summaryRow + 1, 1);
                totalCell.Value = $"Total Meetings: {dt.Rows.Count}";
                totalCell.Style.Font.Bold = true;

                // Count by status if available
                if (dt.Columns.Contains("MeetingStatus"))
                {
                    var statusCount = dt.AsEnumerable()
                        .GroupBy(row => row["MeetingStatus"])
                        .Select(g => new { Status = g.Key, Count = g.Count() });

                    int statusRow = summaryRow + 2;
                    foreach (var status in statusCount)
                    {
                        var statusCell = worksheet.Cell(statusRow, 1);
                        statusCell.Value = $"{status.Status}: {status.Count}";
                        statusRow++;
                    }
                }

                // Add metadata
                var metadataCell = worksheet.Cell(dt.Rows.Count + 10, 1);
                metadataCell.Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                metadataCell.Style.Font.Italic = true;
                metadataCell.Style.Font.FontSize = 10;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
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
            using (var workbook = new XLWorkbook())
            {
                foreach (var table in tables)
                {
                    var worksheet = workbook.Worksheets.Add(table.Key);

                    // Add headers
                    for (int i = 0; i < table.Value.Columns.Count; i++)
                    {
                        var cell = worksheet.Cell(1, i + 1);
                        cell.Value = table.Value.Columns[i].ColumnName;
                        cell.Style.Font.Bold = true;
                    }

                    // Add data
                    for (int row = 0; row < table.Value.Rows.Count; row++)
                    {
                        for (int col = 0; col < table.Value.Columns.Count; col++)
                        {
                            var cell = worksheet.Cell(row + 2, col + 1);
                            var val = table.Value.Rows[row][col] ?? "";
                            if (val is string str)
                                cell.Value = str;
                            else if (val is int num)
                                cell.Value = num;
                            else if (val is double dbl)
                                cell.Value = dbl;
                            else if (val is DateTime dateTime)
                                cell.Value = dateTime;
                            else if (val is bool b)
                                cell.Value = b;
                            else
                                cell.Value = val.ToString();
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Export attendance data to Excel with custom formatting
        /// </summary>
        /// <param name="dt">Attendance DataTable</param>
        /// <returns>Excel file as byte array</returns>
        public static byte[] ExportAttendanceToExcel(DataTable dt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Attendance Report");

                // Add and style headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = GetFormattedColumnName(dt.Columns[i].ColumnName);
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.DarkGreen;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                // Add data
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        object value = dt.Rows[row][col];
                        var cell = worksheet.Cell(row + 2, col + 1);

                        // Special formatting for specific columns
                        if (dt.Columns[col].ColumnName.Contains("Date"))
                        {
                            if (DateTime.TryParse(value?.ToString(), out DateTime dateValue))
                            {
                                cell.Value = dateValue;
                                cell.Style.DateFormat.Format = "yyyy-mm-dd hh:mm";
                            }
                        }
                        else if (dt.Columns[col].ColumnName.Contains("Present"))
                        {
                            bool isPresent = Convert.ToBoolean(value);
                            cell.Value = isPresent ? "Present" : "Absent";

                            if (isPresent)
                            {
                                cell.Style.Font.FontColor = XLColor.Green;
                                cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                            }
                            else
                            {
                                cell.Style.Font.FontColor = XLColor.Red;
                                cell.Style.Fill.BackgroundColor = XLColor.LightPink;
                            }
                        }
                        else
                        {
                            var val = value ?? "";
                            if (val is string str)
                                cell.Value = str;
                            else if (val is int num)
                                cell.Value = num;
                            else if (val is double dbl)
                                cell.Value = dbl;
                            else if (val is DateTime dateTime)
                                cell.Value = dateTime;
                            else if (val is bool b)
                                cell.Value = b;
                            else
                                cell.Value = val.ToString();
                        }

                        // Add border
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    }
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Add summary section
                int summaryRow = dt.Rows.Count + 3;
                var summaryHeaderCell = worksheet.Cell(summaryRow, 1);
                summaryHeaderCell.Value = "ATTENDANCE SUMMARY";
                summaryHeaderCell.Style.Font.Bold = true;
                summaryHeaderCell.Style.Font.FontColor = XLColor.DarkGreen;

                var totalCell = worksheet.Cell(summaryRow + 1, 1);
                totalCell.Value = $"Total Records: {dt.Rows.Count}";
                totalCell.Style.Font.Bold = true;

                // Count attendance status if available
                if (dt.Columns.Contains("IsPresent"))
                {
                    int presentCount = dt.AsEnumerable().Count(row => Convert.ToBoolean(row["IsPresent"]));
                    int absentCount = dt.Rows.Count - presentCount;

                    var presentCell = worksheet.Cell(summaryRow + 2, 1);
                    presentCell.Value = $"Present: {presentCount}";
                    presentCell.Style.Font.FontColor = XLColor.Green;
                    presentCell.Style.Font.Bold = true;

                    var absentCell = worksheet.Cell(summaryRow + 3, 1);
                    absentCell.Value = $"Absent: {absentCount}";
                    absentCell.Style.Font.FontColor = XLColor.Red;
                    absentCell.Style.Font.Bold = true;

                    var percentageCell = worksheet.Cell(summaryRow + 4, 1);
                    double percentage = dt.Rows.Count > 0 ? (double)presentCount / dt.Rows.Count * 100 : 0;
                    percentageCell.Value = $"Attendance Rate: {percentage:F1}%";
                    percentageCell.Style.Font.Bold = true;
                }

                // Add metadata
                var metadataCell = worksheet.Cell(dt.Rows.Count + 10, 1);
                metadataCell.Value = $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                metadataCell.Style.Font.Italic = true;
                metadataCell.Style.Font.FontSize = 10;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
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
