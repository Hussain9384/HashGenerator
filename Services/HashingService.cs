using HashGenerator.Interfaces;

namespace HashGenerator.Services
{
    internal class HashingService : IHashingService
    {
        private readonly IFileOperator _fileOperator;

        public HashingService(IFileOperator fileOperator)
        {
            _fileOperator = fileOperator;
        }

        public async Task StartHashing(string[] args)
        {
            if (args.Length != 4 ) { throw new Exception("Missing Inputs, Unable to proceed"); };

            string filePath = args[0];
            string sheetName = args[1];
            int.TryParse(args[2], out int batchSize);
            string targetFilePath = args[3];


            Console.WriteLine($"filePath: {filePath}, sheetName : {sheetName}, batchSize : {batchSize}");

            if (_fileOperator.IsFileAndWorkSheetExist(filePath, sheetName)) { 
                await _fileOperator.ReadExcelLinesAsync(filePath, sheetName, batchSize, targetFilePath);
            }
        }
    }
}
