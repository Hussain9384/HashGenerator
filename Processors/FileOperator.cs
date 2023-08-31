using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using HashGenerator.Interfaces;
using System.Globalization;
using System.Text;

namespace HashGenerator.Processors
{
    internal class FileOperator : IFileOperator
    {
        private readonly IHashGenerator _hashGenerator;

        public FileOperator(IHashGenerator hashGenerator)
        {
            _hashGenerator = hashGenerator;
        }
        public bool IsFileAndWorkSheetExist(string filePath, string sheetName)
        {
            if (File.Exists(filePath))
            {
                Console.WriteLine($"File exist in the path : '{filePath}'");

                using (var workbook = new XLWorkbook(filePath))
                {
                    try
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(sheetName);

                        if (worksheet != null)
                        {
                            Console.WriteLine($"Sheet '{sheetName}' exists in the workbook.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Sheet '{sheetName}' does not exist in the workbook.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"exception occurred during the check {nameof(IsFileAndWorkSheetExist)}, ex: {ex.Message} , inner_ex: {ex.InnerException?.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"File doesn't exist in the path : '{filePath}'");
            }
            return false;
        }
        public double GetCount(string filePath, string sheetName)
        {
            double rowCount = 0;

            using (var workbook = new XLWorkbook(filePath))
            {
                IXLWorksheet worksheet = workbook.Worksheet(sheetName);

                rowCount = worksheet.RowsUsed().Count();

                Console.WriteLine($"Row count: {rowCount}");
            }

            return rowCount;
        }

        public async Task ReadExcelLinesAsync(string filePath, string sheetName, int batchSize, string targetFilePath)
        {
            Console.WriteLine($"Started {nameof(ReadExcelLinesAsync)}");

            await Task.Run(() =>
            {
                using (var workbook = new XLWorkbook(filePath))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(sheetName);

                    var rows = worksheet.RowsUsed().Skip(1);

                    int processedCount = 0;

                    Console.WriteLine($"Rows_Count : {rows.Count()}");

                    while (processedCount < rows.Count())
                    {
                        var batch = rows.Skip(processedCount).Take(batchSize);

                        foreach (var row in batch)
                        {
                            using (StreamWriter writer = File.AppendText(targetFilePath))
                            {
                                StringBuilder sb = new StringBuilder();

                                foreach (var cell in row.Cells())
                                {
                                    var hash = CalculateHashIfNotEmpty(cell);

                                    sb.Append( cell.Value + "," );
                                    sb.Append(hash + ",");
                                }

                                writer.WriteLine( sb.ToString() );
                            }
                        }

                        processedCount += batch.Count();
                    }
                }
            });
        }

        private string CalculateHashIfNotEmpty(IXLCell xLCell) {

            if (xLCell.TryGetValue(out string cellValue) && !string.IsNullOrEmpty(cellValue))
            {
                return _hashGenerator.GenerateHash(cellValue);
            }

            return string.Empty;
        }
    }
}
