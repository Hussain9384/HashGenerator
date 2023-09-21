using CsvHelper;
using CsvHelper.Configuration;
using HashGenerator.Interfaces;
using HashGenerator.Models;
using System.Formats.Asn1;
using System.Globalization;

namespace HashGenerator.Services
{
    internal class HashingService : IHashingService
    {
        private readonly IFileOperator _fileOperator;
        private readonly IHashGenerator _hashGenerator;

        public HashingService(IFileOperator fileOperator, IHashGenerator hashGenerator)
        {
            _fileOperator = fileOperator;
            _hashGenerator = hashGenerator;
        }

        public async Task StartHashing(string[] args)
        {
            //if (args.Length != 1) { throw new Exception("Missing Inputs, Unable to proceed"); };

            //string folderPath = args[0];

            var folderPath = "F:\\Temp";

            var inputFolderPath = Path.Combine(folderPath, "Input");
            if (File.Exists(inputFolderPath)) { }
            Console.WriteLine($"input filePath: {inputFolderPath} exist");

            var outputFolderpath = Path.Combine(folderPath, "Output");
            if (File.Exists(outputFolderpath))
            Console.WriteLine($"output filePath: {outputFolderpath} exist");

            string[] fileNamesList = Directory.GetFiles(inputFolderPath);

            if (fileNamesList.Length == 0)
            {
                await Console.Out.WriteLineAsync("No files found to process");
                return;
            }

            var cores = Environment.ProcessorCount;

            Console.WriteLine($"Using {cores} Cores to process hashing");

            if (!fileNamesList.Any()) Console.WriteLine("No files found to process");

            var filesWithIndex = fileNamesList.Select((x, i) => new { fileName = x, index = i + 1 });

            await Parallel.ForEachAsync(filesWithIndex, new ParallelOptions()
            {
                MaxDegreeOfParallelism = cores,
            }, async (input, token) =>
            {
                var res = await _fileOperator.ReadDataFromCsvAsync(inputFolderPath, outputFolderpath, input.fileName, token);

                await Console.Out.WriteLineAsync($"File_Process_Status_{input.fileName}_{res}");
            });
        }
    }
}
