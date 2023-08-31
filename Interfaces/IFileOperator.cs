using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGenerator.Interfaces
{
    internal interface IFileOperator
    {
        bool IsFileAndWorkSheetExist(string filePath, string sheetName);
        double GetCount(string filePath, string sheetName);
        Task ReadExcelLinesAsync(string filePath, string sheetName, int batchSize, string targetFilePath);
    }
}
