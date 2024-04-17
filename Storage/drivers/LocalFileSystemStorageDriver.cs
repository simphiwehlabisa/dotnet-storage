

using Microsoft.Extensions.Options;
using Storage.contracts;

namespace Storage.drivers;
public class LocalFileSystemStorageDriver : IStorageDriver
{
    private readonly string rootPath;

    public LocalFileSystemStorageDriver(IOptions<StorageConfiguration> storageSettings)
    {
        if (storageSettings == null)
        {
            throw new ArgumentNullException(nameof(storageSettings));
        }
        this.rootPath = storageSettings.Value.LocalFileSystem.RootPath;
    }

    public async Task StoreAsync(string filePath, Stream fileStream)
    {
        var fullPath = Path.Combine(rootPath, filePath);

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        using (var file = File.Create(fullPath))
        {
            await fileStream.CopyToAsync(file);
        }
    }

    public async Task<Stream> GetAsync(string filePath)
    {
        var fullPath = Path.Combine(rootPath, filePath);

        if (File.Exists(fullPath))
        {
            return File.OpenRead(fullPath);
        }

        return null; // or throw an exception if the file doesn't exist
    }

    public async Task DeleteAsync(string filePath)
    {
        var fullPath = Path.Combine(rootPath, filePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public void ChangeBucketName(string newBucketName)
    {
        throw new NotImplementedException();
    }
}
