# Trwn.Inspection.WebClient

A .NET Standard 2.1 client library for interacting with the Trwn.Inspection.Web API.

## Features

- Strongly typed client for all Inspection Reports API endpoints
- Support for CRUD operations on inspection reports
- Photo documentation management
- Comprehensive error handling with `ApiResponse<T>` wrappers
- Configurable HTTP client with dependency injection support
- Logging integration
- Async/await pattern throughout

## Installation

Add a project reference to `Trwn.Inspection.WebClient` and ensure you have the following NuGet packages:

- `System.Net.Http.Json` (>= 7.0.1)
- `Microsoft.Extensions.Http` (>= 7.0.0)

## Usage

### 1. Register the client in your DI container

```csharp
// In your Startup.cs or Program.cs
services.AddInspectionReportsClient("https://localhost:5001");

// Or with configuration
services.AddInspectionReportsClient(options =>
{
    options.BaseUrl = "https://localhost:5001";
    options.TimeoutSeconds = 60;
    options.ApiKey = "your-api-key";
    options.Headers.Add("Custom-Header", "value");
});
```

### 2. Inject and use the client

#### Basic Client (throws exceptions on errors)

```csharp
public class InspectionService
{
    private readonly IInspectionReportsClient _client;

    public InspectionService(IInspectionReportsClient client)
    {
        _client = client;
    }

    public async Task<List<InspectionReport>> GetAllReportsAsync()
    {
        return await _client.GetInspectionReportsAsync();
    }

    public async Task<InspectionReport?> GetReportAsync(string id)
    {
        return await _client.GetInspectionReportAsync(id);
    }

    public async Task<InspectionReport> CreateReportAsync(InspectionReport report)
    {
        return await _client.AddInspectionReportAsync(report);
    }

    public async Task DeleteReportAsync(string id)
    {
        await _client.DeleteInspectionReportAsync(id);
    }
}
```

#### Enhanced API Client (with ApiResponse wrappers)

```csharp
public class InspectionService
{
    private readonly IInspectionReportsApiClient _apiClient;

    public InspectionService(IInspectionReportsApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<InspectionReport>> GetAllReportsAsync()
    {
        var response = await _apiClient.GetInspectionReportsAsync();
        
        if (response.IsSuccess)
        {
            return response.Data ?? new List<InspectionReport>();
        }
        
        // Handle error
        throw new InvalidOperationException($"Failed to get reports: {response.ErrorMessage}");
    }

    public async Task<InspectionReport?> GetReportAsync(string id)
    {
        var response = await _apiClient.GetInspectionReportAsync(id);
        
        if (response.IsSuccess)
        {
            return response.Data;
        }
        
        if (response.StatusCode == 404)
        {
            return null; // Not found
        }
        
        // Handle other errors
        throw new InvalidOperationException($"Failed to get report: {response.ErrorMessage}");
    }

    public async Task<bool> DeleteReportAsync(string id)
    {
        var response = await _apiClient.DeleteInspectionReportAsync(id);
        return response.IsSuccess;
    }
}
```

### 3. Photo Documentation Management

```csharp
// Add photo documentation
var photoDoc = new PhotoDocumentation
{
    PhotoType = PhotoType.Major,
    Code = 1,
    Description = "Product defect",
    PicturePath = "/images/defect1.jpg"
};

var result = await _client.AddPhotoDocumentationAsync("report-id", photoDoc);

// Get specific photo
var photo = await _client.GetPhotoDocumentationAsync("report-id", 1);

// Get all photos for a report
var allPhotos = await _client.GetAllPhotoDocumentationAsync("report-id");

// Delete photo documentation
await _client.DeletePhotoDocumentationAsync("report-id", 1);
```

### 4. Complete CRUD Example

```csharp
public class ReportManager
{
    private readonly IInspectionReportsApiClient _client;

    public ReportManager(IInspectionReportsApiClient client)
    {
        _client = client;
    }

    public async Task<string> CreateAndManageReportAsync()
    {
        // Create new report
        var newReport = new InspectionReport
        {
            InspectionType = InspectionType.Final,
            ReportNo = "RPT-001",
            Client = "Test Client",
            ArticleName = "Test Product",
            InspectionDate = DateTime.UtcNow,
            InspectorName = "John Doe"
        };

        var createResponse = await _client.AddInspectionReportAsync(newReport);
        if (!createResponse.IsSuccess)
        {
            throw new Exception($"Failed to create report: {createResponse.ErrorMessage}");
        }

        var reportId = createResponse.Data.Id;

        // Add photo documentation
        var photo = new PhotoDocumentation
        {
            PhotoType = PhotoType.Major,
            Code = 1,
            Description = "Quality check",
            PicturePath = "/images/quality1.jpg"
        };

        var photoResponse = await _client.AddPhotoDocumentationAsync(reportId, photo);
        if (!photoResponse.IsSuccess)
        {
            throw new Exception($"Failed to add photo: {photoResponse.ErrorMessage}");
        }

        // Update report
        createResponse.Data.InspectionResult = InspectionResultType.Passes;
        var updateResponse = await _client.UpdateInspectionReportAsync(reportId, createResponse.Data);
        
        if (!updateResponse.IsSuccess)
        {
            throw new Exception($"Failed to update report: {updateResponse.ErrorMessage}");
        }

        return reportId;
    }

    public async Task<bool> CleanupReportAsync(string reportId)
    {
        // Get all photos first
        var photosResponse = await _client.GetAllPhotoDocumentationAsync(reportId);
        if (photosResponse.IsSuccess && photosResponse.Data != null)
        {
            // Delete all photos
            foreach (var photo in photosResponse.Data)
            {
                await _client.DeletePhotoDocumentationAsync(reportId, photo.Code);
            }
        }

        // Delete the report
        var deleteResponse = await _client.DeleteInspectionReportAsync(reportId);
        return deleteResponse.IsSuccess;
    }
}
```

## API Endpoints Covered

- `GET /api/InspectionReports` - Get all inspection reports
- `GET /api/InspectionReports/{id}` - Get specific inspection report
- `POST /api/InspectionReports` - Create new inspection report
- `PUT /api/InspectionReports/{id}` - Update inspection report
- `DELETE /api/InspectionReports/{id}` - Delete inspection report
- `POST /api/InspectionReports/{id}/foto` - Add photo documentation
- `GET /api/InspectionReports/{id}/foto/{fotoCode}` - Get specific photo
- `GET /api/InspectionReports/{id}/foto` - Get all photos for report
- `DELETE /api/InspectionReports/{id}/foto/{fotoCode}` - Delete photo documentation

## Error Handling

The library provides two approaches for error handling:

1. **Basic Client (`IInspectionReportsClient`)**: Throws exceptions for HTTP errors
2. **Enhanced Client (`IInspectionReportsApiClient`)**: Returns `ApiResponse<T>` with success/failure information

Choose the approach that best fits your application's error handling strategy.

## Configuration Options

- `BaseUrl`: The base URL of the Web API (required)
- `TimeoutSeconds`: Request timeout in seconds (default: 30)
- `ApiKey`: API key for authentication (optional)
- `Headers`: Additional headers to include in requests (optional)

## Dependencies

- .NET Standard 2.1
- System.Net.Http.Json (>= 7.0.1)
- Microsoft.Extensions.Http (>= 7.0.0)
- Microsoft.Extensions.Logging.Abstractions
- Trwn.Inspection.Models

## Client Interfaces

### IInspectionReportsClient
Basic client interface that throws exceptions on HTTP errors. Suitable for scenarios where you want simple error handling with try/catch blocks.

### IInspectionReportsApiClient
Enhanced client interface that returns `ApiResponse<T>` wrappers. Provides more detailed error information and allows for fine-grained error handling without exceptions.

Both clients provide the same functionality but with different error handling approaches. Choose the one that best fits your application architecture.