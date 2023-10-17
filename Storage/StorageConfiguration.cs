using Storage.configs;

namespace Storage;
public class StorageConfiguration
{
    public string ActiveDriver { get; set; }
    public LocalFileSystemStorageConfig LocalFileSystem { get; set; }
    public AmazonS3StorageConfig AmazonS3 { get; set; }
}