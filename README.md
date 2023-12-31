# dotnet-storage

[![NuGet Version](https://img.shields.io/nuget/v/dotnet-storage)](https://www.nuget.org/packages/dotnet-storage/)
[![License](https://img.shields.io/github/license/YourGitHubUsername/YourPackageName)](https://github.com/simphiwehlabisa/dotnet-storage/blob/main/LICENSE)

dotnet-storage is a .NET library that provides file storage functionality for ASP.NET Core applications.

## Installation

You can install dotnet-storage via NuGet package manager. Use the following commands:

### .NET CLI

Open your terminal and navigate to your ASP.NET Core project directory. Run the following command:

```bash
dotnet add package dotnet-storage
```
### Configuration program.cs
It is Very important to configure the package in startup/program.cs.
This will allow the package to work properly
```csharp
// Configuration from appsettings.json
builder.Services.Configure<StorageConfiguration>(builder.Configuration.GetSection("StorageSettings"));

// Register your storage drivers
builder.Services.AddTransient<LocalFileSystemStorageDriver>();
builder.Services.AddTransient<AmazonS3StorageDriver>();

// Register the StorageService
builder.Services.AddTransient<IStorageService, StorageService>();
```

### Configuration via appsettings.json

You can configure dotnet-storage by adding settings to your `appsettings.json` file. The following settings are available:

### Sample appsettings.json Configuration

Here's a `appsettings.json` configuration for dotnet-storage:

### Local localfilesystem Configuration

```json
  "StorageSettings": {
    "ActiveDriver": "localfilesystem", 
    "LocalFileSystem": {
      "RootPath": ""
    },
  }
```

### Amazons3/minio Configuration

```json
  "StorageSettings": {
    "ActiveDriver": "amazons3", 
    "AmazonS3": {
      "AccessKey": "",
      "SecretKey": "",
      "BucketName": "",
      "Region": "",
      "ServiceURL": ""
    }
  }
```

## Example Usage in an ASP.NET Core Application

To demonstrate how to use dotnet-storage, let's create a simple ASP.NET Core controller that utilizes the package's functionality.

### Step 1: Create an ASP.NET Core Controller

In your ASP.NET Core application, create a new controller. For example, let's create a `SampleController`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Storage.contracts;
namespace example.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    private readonly IStorageService storageService;
    private readonly ILogger<SampleController> logger;

    public SampleController(IStorageService storageService, ILogger<SampleController> logger)
    {
        this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost(Name = "upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            // Generate a unique file name or use a predefined naming convention
            string fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = "uploads/" + uniqueFileName; // You can customize the storage path

            using (var stream = file.OpenReadStream())
            {
                await storageService.StoreAsync(filePath, stream);
            }

            return Ok(new { FilePath = filePath });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while uploading the file.");
            return StatusCode(500, "Internal server error.");
        }
    }
}
```

### Here's an api call using curl

```curl
curl -X 'POST' \
  'https://localhost:7151/api/Sample' \
  -H 'accept: */*' \
  -H 'Content-Type: multipart/form-data' \
  -F 'file=@8fb41467-6983-478a-9086-260cb0a12782.jpeg;type=image/jpeg'
```

### response

```json
{
  "filePath": "uploads/04bdc90d-ad2a-4079-9c11-424be8aa02e3.jpeg"
}
```

## License

The dotnet-storage is open-sourced software licensed under the [MIT license](LICENSE.md).