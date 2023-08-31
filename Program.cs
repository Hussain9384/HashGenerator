
using HashGenerator.Interfaces;
using HashGenerator.Processors;
using HashGenerator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

// Add your services here
builder.ConfigureServices((hostContext, services) =>
{
    services.AddSingleton<IFileOperator, FileOperator>();
    services.AddSingleton<IHashGenerator, HashGenerator.Processors.HashGenerator>();
    services.AddSingleton<IHashingService, HashingService>();
});

var app = builder.Build();

var hashingService = app.Services.GetRequiredService<IHashingService>();

Console.WriteLine("Starting Hashing ...");


//dotnet run F:\Temp\Book1.xlsx sheet1 100 F:\Temp\Bk.csv

Task.Run(async () => {
    await hashingService.StartHashing(args);
}).Wait();

Console.WriteLine("Hashing Done...");


