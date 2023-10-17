using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Storage.contracts;

namespace Storage.drivers;
public class AmazonS3StorageDriver : IStorageDriver
{
    private readonly IAmazonS3 s3Client;
    private readonly string bucketName;

    public AmazonS3StorageDriver(IOptions<StorageConfiguration> storageSettings)
    {
        var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(storageSettings.Value.AmazonS3.AccessKey, storageSettings.Value.AmazonS3.SecretKey);

        var clientConfig = new AmazonS3Config
        {
            AuthenticationRegion = storageSettings.Value.AmazonS3.Region,
            ServiceURL = storageSettings.Value.AmazonS3.ServiceUrl,
            ForcePathStyle = true
        };
        s3Client = new AmazonS3Client(awsCredentials, clientConfig);
        bucketName = storageSettings.Value.AmazonS3.BucketName;
    }

    public async Task StoreAsync(string filePath, Stream fileStream)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = filePath,
            InputStream = fileStream,
            ContentType = "application/octet-stream"
        };

        await s3Client.PutObjectAsync(request);
    }

    public async Task<Stream> GetAsync(string filePath)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = filePath
        };

        var response = await s3Client.GetObjectAsync(request);
        return response.ResponseStream;
    }

    public async Task DeleteAsync(string filePath)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = filePath
        };

        await s3Client.DeleteObjectAsync(request);
    }
}
