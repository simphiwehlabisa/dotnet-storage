namespace Storage.configs;
public class AmazonS3StorageConfig
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
    public string Region { get; set; }
    public string ServiceUrl { get; set; }
}