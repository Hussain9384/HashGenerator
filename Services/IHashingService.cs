namespace HashGenerator.Services
{
    internal interface IHashingService
    {
        Task StartHashing(string[] args);
    }
}