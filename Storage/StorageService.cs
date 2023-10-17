


using Microsoft.Extensions.Options;
using Storage.contracts;
using Storage.drivers;

namespace Storage;
public class StorageService : IStorageService
{
    private readonly IStorageDriver activeStorageDriver;

    public StorageService(IOptions<StorageConfiguration> storageConfig,
                          LocalFileSystemStorageDriver localFileSystemStorageDriver,
                          AmazonS3StorageDriver amazonS3StorageDriver)
    {
        var activeDriver = storageConfig.Value.ActiveDriver;

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

    public async Task StoreAsync(string filePath, Stream fileStream)
    {
        if (activeStorageDriver == null)
        {
            throw new InvalidOperationException("No active storage driver specified.");
        }

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
}
