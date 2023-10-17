namespace Storage.contracts;
public interface IStorageDriver
{
    Task StoreAsync(string filePath, Stream fileStream);
    Task<Stream> GetAsync(string filePath);
    Task DeleteAsync(string filePath);
}