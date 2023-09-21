using HashGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HashGenerator.Interfaces
{
    internal interface IFileOperator
    {
        IEnumerable<string> GetValidFiles(string folderPath, string[] fileNames);
        Task<bool> ReadDataFromCsvAsync(string inputFolderName, string outputFolderName, string fileName, CancellationToken cancellationToken);
    }
}
