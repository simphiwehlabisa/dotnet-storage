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

## Configuration via appsettings.json

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
