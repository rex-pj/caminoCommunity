namespace Camino.Infrastructure.Files.Contracts
{
    public interface IFileStore
    {
        Task<string> CreateFileAsync(Stream inputStream);
    }
}
