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
    private string bucketName;

    /// <summary>
    /// Initializes a new instance of the AmazonS3StorageDriver class.
    /// </summary>
    /// <param name="storageSettings">The storage configuration settings.</param>
    public AmazonS3StorageDriver(IOptions<StorageConfiguration> storageSettings)
    {
        // Create AWS credentials using the access key and secret key from the storage settings
        var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(
            storageSettings.Value.AmazonS3.AccessKey,
            storageSettings.Value.AmazonS3.SecretKey);

        // Create client configuration for Amazon S3
        var clientConfig = new AmazonS3Config
        {
            // Set the authentication region from the storage settings
            AuthenticationRegion = storageSettings.Value.AmazonS3.Region,
            // Set the service URL from the storage settings
            ServiceURL = storageSettings.Value.AmazonS3.ServiceUrl,
            // Use path-style URLs for S3 requests
            ForcePathStyle = true
        };

        // Create a new instance of Amazon S3 client using the credentials and config
        s3Client = new AmazonS3Client(awsCredentials, clientConfig);

        // Set the bucket name from the storage settings
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

    public void ChangeBucketName(string newBucketName)
    {
        bucketName = newBucketName;
    }
}
