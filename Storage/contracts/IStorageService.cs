namespace Storage.contracts;
public interface IStorageService
{
    /// <summary>
    /// Stores a file with the given path and content.
    /// </summary>
    /// <param name="filePath">The path of the file to store.</param>
    /// <param name="fileStream">A stream containing the file content.</param>
    Task StoreAsync(string filePath, Stream fileStream);

    /// <summary>
    /// Retrieves a file stream for the specified file path.
    /// </summary>
    /// <param name="filePath">The path of the file to retrieve.</param>
    /// <returns>A stream containing the file content, or null if the file doesn't exist.</returns>
    Task<Stream> GetAsync(string filePath);

    /// <summary>
    /// Deletes the file at the specified path.
    /// </summary>
    /// <param name="filePath">The path of the file to delete.</param>
    Task DeleteAsync(string filePath);
}
