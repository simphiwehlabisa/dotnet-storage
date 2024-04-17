


using Microsoft.Extensions.Options;
using Storage.contracts;
using Storage.drivers;

namespace Storage;
public class StorageService : IStorageService
{
    private readonly IStorageDriver activeStorageDriver;

    /// <summary>
    /// Constructs a new StorageService instance.
    /// </summary>
    /// <param name="storageConfig">The storage configuration options.</param>
    /// <param name="localFileSystemStorageDriver">The local file system storage driver.</param>
    /// <param name="amazonS3StorageDriver">The Amazon S3 storage driver.</param>
    public StorageService(IOptions<StorageConfiguration> storageConfig,
                          LocalFileSystemStorageDriver localFileSystemStorageDriver,
                          AmazonS3StorageDriver amazonS3StorageDriver)
    {
        // Get the active storage driver from the configuration options
        var activeDriver = storageConfig.Value.ActiveDriver;

        // Set the active storage driver based on the active driver name
        switch (activeDriver.ToLower())
        {
            case "localfilesystem":
                activeStorageDriver = localFileSystemStorageDriver;
                break;
            case "amazons3":
                activeStorageDriver = amazonS3StorageDriver;
                break;
            default:
                throw new InvalidOperationException($"Unknown storage driver: {activeDriver}");
        }
    }

    /// <summary>
    /// Stores the file at the specified file path using the active storage driver.
    /// </summary>
    /// <param name="filePath">The path where the file will be stored.</param>
    /// <param name="fileStream">The stream containing the file data.</param>
    public async Task StoreAsync(string filePath, Stream fileStream)
    {
        // Check if an active storage driver is specified
        if (activeStorageDriver == null)
        {
            throw new InvalidOperationException("No active storage driver specified.");
        }

        // Store the file using the active storage driver
        await activeStorageDriver.StoreAsync(filePath, fileStream);
    }

    public async Task<Stream> GetAsync(string filePath)
    {
        if (activeStorageDriver == null)
        {
            throw new InvalidOperationException("No active storage driver specified.");
        }

        return await activeStorageDriver.GetAsync(filePath);
    }

    public async Task DeleteAsync(string filePath)
    {
        if (activeStorageDriver == null)
        {
            throw new InvalidOperationException("No active storage driver specified.");
        }

        await activeStorageDriver.DeleteAsync(filePath);
    }

    public void ChangeBucketName(string newBucketName)
    {
        if (activeStorageDriver is AmazonS3StorageDriver amazonS3StorageDriver)
        {
            amazonS3StorageDriver.ChangeBucketName(newBucketName);
        }
    }

}
