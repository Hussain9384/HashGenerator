using CsvHelper.Configuration;
using CsvHelper;
using HashGenerator.Interfaces;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using HashGenerator.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace HashGenerator.Processors
{
    internal class FileOperator : IFileOperator
    {
        private readonly IHashGenerator _hashGenerator;

        public FileOperator(IHashGenerator hashGenerator)
        {
            _hashGenerator = hashGenerator;
        }

        public IEnumerable<string> GetValidFiles(string folderPath, string[] fileNames)
        {
            Console.WriteLine($"Executing_{nameof(GetValidFiles)}");

            var fileExistResult = fileNames.Where(s => string.IsNullOrWhiteSpace(s) && File.Exists(folderPath));

            if (fileExistResult.Count() != fileNames.Count()) {
                Console.WriteLine($"There are few missing files : { string.Join(",",fileNames.Except(fileExistResult)) }");
            }

            return fileExistResult;
        }

        public async Task<bool> ReadDataFromCsvAsync(string inputFolderName, string outputFolderName, string fileName, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Executing_{nameof(ReadDataFromCsvAsync)}");

            bool isjobDone = false;

            List<Customer> records;

            try
            {
                string csvFilePath = Path.Combine(inputFolderName, fileName);
                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    records = csv.GetRecords<Customer>().ToList();

                    records.ForEach(item =>
                    {
                        item.EmailAddress = _hashGenerator.GenerateHash(item.EmailAddress);
                    });

                     isjobDone = await WriteCsvAsync(outputFolderName, fileName, records, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return isjobDone;
        }

        public async Task<bool> WriteCsvAsync(string folderName, string fileName, IEnumerable<Customer> records, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Executing_{nameof(ReadDataFromCsvAsync)}");

            bool isjobDone = false;

            try
            {
                var outputFile = Path.GetFileName(fileName).Replace("Input", "Output");
                using (var writer = new StreamWriter(Path.Combine(folderName, outputFile)))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    await csv.WriteRecordsAsync(records);
                }

                isjobDone = true;   
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while writing the CSV file: " + ex.Message);
            }

            return isjobDone;
        }
    }
}
