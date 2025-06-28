# Qutora .NET SDK

[![NuGet](https://img.shields.io/nuget/v/Qutora.SDK.svg)](https://www.nuget.org/packages/Qutora.SDK/)
[![Downloads](https://img.shields.io/nuget/dt/Qutora.SDK.svg)](https://www.nuget.org/packages/Qutora.SDK/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0+-blue.svg)](https://dotnet.microsoft.com/download)
[![Build Status](https://img.shields.io/github/actions/workflow/status/qutora/qutora-sdk-dotnet/nuget-publish.yml)](https://github.com/qutora/qutora-sdk-dotnet/actions)

**Official .NET SDK for Qutora Document Management System**

This SDK provides access to core document management functionality for external application integration. For complete administrative capabilities, see [Qutora Admin SDK](https://github.com/qutora/qutora-admin-sdk-dotnet).

## 🌟 About Qutora

Qutora is a **document management system** that provides enterprise-grade document storage, metadata management, and API-first architecture. This **SDK** is freely available on GitHub under the MIT license for easy integration with any .NET application.

## ⚠️ Important Notice

**This SDK provides access to the Qutora API endpoints designed for external application integration.** It covers the core document management functionality that third-party applications typically need to integrate with any Qutora instance.

**For administrative operations** (user management, system configuration, advanced settings), please use the [Qutora Admin SDK](https://github.com/qutora/qutora-admin-sdk-dotnet) which provides comprehensive access to all administrative endpoints.

## 🎯 Purpose

This SDK is specifically designed for **external application integration** and covers the essential document management operations that third-party applications typically require:

- Document upload, download, and management
- Category browsing and organization  
- Metadata operations and search
- Storage provider access

**This is not a complete API wrapper** - it intentionally focuses on integration scenarios rather than administrative functions.

## 🚀 Quick Start

### Installation

Install the SDK via NuGet Package Manager:

```bash
dotnet add package Qutora.SDK
```

Or via Package Manager Console:

```powershell
Install-Package Qutora.SDK
```

### 🚀 Performance Features

- **Automatic Caching**: Categories, Metadata Schemas, and Storage Providers are cached by default (15 minutes)
- **Significant Performance Improvement**: 
  - Categories: ~75x faster (cached)
  - Metadata Schemas: ~67x faster (cached)
  - Storage Providers: ~100x faster (cached)
- **Zero Configuration**: Caching works out-of-the-box with sensible defaults
- **Flexible Cache Providers**: Memory, Distributed (Redis/SQL Server), or disabled
- **Smart Cache Invalidation**: Automatic cache clearing on Create/Update/Delete operations
- **Documents Not Cached**: Documents are too large/dynamic for caching

### Basic Usage

```csharp
using Qutora.SDK;
using Qutora.SDK.Models;
using Qutora.SDK.Extensions; // For DI registration

// Initialize the client
var client = new QutoraClient(
    baseUrl: "https://your-qutora-instance.com/api",
    apiKey: "your-api-key",
    apiSecret: "your-api-secret"
);

// Test connection
var isConnected = await client.TestConnectionAsync();
if (!isConnected)
{
    throw new Exception("Failed to connect to Qutora API");
}

// Get documents (Categories, Metadata, Storage are automatically cached)
var documents = await client.Documents.GetDocumentsAsync(page: 1, size: 10);
Console.WriteLine($"Found {documents.TotalCount} documents");

// Upload a document
var fileBytes = File.ReadAllBytes("document.pdf");
var createRequest = new CreateDocumentRequest
{
    Name = "My Important Document",
    CategoryId = "category-guid",
    BucketId = "bucket-guid"
};

var newDocument = await client.Documents.CreateDocumentAsync(
    fileName: "document.pdf",
    fileContent: fileBytes,
    request: createRequest
);

// Download a document
var documentContent = await client.Documents.DownloadDocumentAsync(newDocument.Id);
File.WriteAllBytes("downloaded-document.pdf", documentContent);

// Work with metadata
var metadata = await client.Metadata.CreateDocumentMetadataAsync(
    newDocument.Id,
    new CreateMetadataRequest
    {
        Values = new Dictionary<string, object>
        {
            ["title"] = "Important Document",
            ["department"] = "Finance",
            ["created_date"] = DateTime.UtcNow
        },
        Tags = new List<string> { "important", "finance", "2024" }
    }
);
```

## 📚 API Reference

### Document Operations

```csharp
// Get paginated documents
var documents = await client.Documents.GetDocumentsAsync(page: 1, size: 20);

// Get specific document with metadata
var document = await client.Documents.GetByIdAsync("document-id", includeMetadata: true);

// Upload document with full options
var createRequest = new CreateDocumentRequest
{
    Name = "Contract Document",
    CategoryId = "legal-category-id",
    BucketId = "secure-bucket-id",
    MetadataJson = JsonSerializer.Serialize(new { department = "Legal" }),
    CreateShare = true,
    ExpiresAfterDays = 30,
    AllowDownload = true,
    AllowPrint = false
};

var document = await client.Documents.CreateDocumentAsync(
    "contract.pdf", 
    fileBytes, 
    createRequest
);

// Update document
var updateRequest = new UpdateDocumentRequest
{
    Name = "Updated Document Name",
    CategoryId = "new-category-id",
    BucketId = "new-bucket-id"
};
var updated = await client.Documents.UpdateDocumentAsync("document-id", updateRequest);

// Download document
var content = await client.Documents.DownloadDocumentAsync("document-id");
// or as stream
var stream = await client.Documents.DownloadDocumentStreamAsync("document-id");

// Get documents by category
var categoryDocs = await client.Documents.GetDocumentsByCategoryAsync("category-id");

// Manage document versions
var versions = await client.Documents.GetDocumentVersionsAsync("document-id");
var newVersion = await client.Documents.CreateVersionAsync(
    "document-id", 
    newFileBytes, 
    "contract_v2.pdf", 
    "Updated terms and conditions"
);
```

### Category Operations

```csharp
// Get all categories (simple list) - Cached automatically
var categories = await client.Categories.GetAllCategoriesAsync();

// Get categories with pagination and search - Cached automatically
var pagedCategories = await client.Categories.GetCategoriesPagedAsync(
    page: 1, 
    pageSize: 20, 
    searchTerm: "legal"
);

// Get root categories (no parent)
var rootCategories = await client.Categories.GetRootCategoriesAsync();

// Get subcategories
var subcategories = await client.Categories.GetSubcategoriesAsync("parent-category-id");

// Get specific category
var category = await client.Categories.GetCategoryByIdAsync("category-id");

// Create new category
var newCategory = await client.Categories.CreateCategoryAsync(
    "Legal Documents", 
    "Category for legal and compliance documents"
);
```

### Metadata Operations

```csharp
// Get document metadata
var metadata = await client.Metadata.GetDocumentMetadataAsync("document-id");

// Create metadata with schema validation
var createRequest = new CreateMetadataRequest
{
    SchemaName = "document-schema",
    Values = new Dictionary<string, object>
    {
        ["title"] = "Project Proposal",
        ["author"] = "John Doe",
        ["created_date"] = DateTime.UtcNow,
        ["priority"] = "high"
    },
    Tags = new List<string> { "proposal", "project", "2024" }
};
var metadata = await client.Metadata.CreateDocumentMetadataAsync("document-id", createRequest);

// Update metadata
var updateRequest = new UpdateMetadataRequest
{
    Values = new Dictionary<string, object> { ["status"] = "approved" },
    Tags = new List<string> { "approved", "final" }
};
var updated = await client.Metadata.UpdateDocumentMetadataAsync("document-id", updateRequest);

// Search metadata with pagination
var searchResults = await client.Metadata.SearchMetadataPagedAsync(
    query: "department:legal AND status:active",
    page: 1,
    pageSize: 10
);

// Get all available tags
var tags = await client.Metadata.GetTagsAsync();

// Get metadata schemas - Cached automatically
var schemas = await client.Metadata.GetSchemasAsync(page: 1, pageSize: 50);
var specificSchema = await client.Metadata.GetSchemaByIdAsync("schema-id");

// Validate metadata against schema
var validateRequest = new ValidateMetadataRequest
{
    SchemaName = "document-schema",
    Values = new Dictionary<string, object> { ["title"] = "Test Document" }
};
var validation = await client.Metadata.ValidateMetadataAsync(validateRequest);
```

### Storage Operations

```csharp
// Get accessible buckets with pagination - Cached automatically
var buckets = await client.Storage.GetAccessibleBucketsAsync(page: 1, pageSize: 20);

// Get accessible storage providers - Cached automatically
var providers = await client.Storage.GetAccessibleProvidersAsync();
```

## ⚙️ Configuration

### Using Configuration Options

```csharp
var options = new QutoraClientOptions
{
    BaseUrl = "https://your-qutora-instance.com/api",
    ApiKey = "your-api-key",
    ApiSecret = "your-api-secret",
    TimeoutSeconds = 60,
    MaxRetryAttempts = 3,
    UserAgent = "MyApp/1.0.0",
    DefaultHeaders = new Dictionary<string, string>
    {
        ["X-Custom-Header"] = "CustomValue"
    },
    Cache = new QutoraCacheConfiguration
    {
        Enabled = true,
        ProviderType = CacheProviderType.Memory,
        DefaultDuration = TimeSpan.FromMinutes(15)
    }
};

var client = new QutoraClient(options);
```

### Using Dependency Injection (Recommended)

```csharp
// In Program.cs - Simple setup with automatic caching
services.AddQutoraSDK(
    baseUrl: configuration["Qutora:BaseUrl"]!,
    apiKey: configuration["Qutora:ApiKey"]!,
    apiSecret: configuration["Qutora:ApiSecret"]!
);

// Custom cache configuration
services.AddQutoraSDK(
    baseUrl: configuration["Qutora:BaseUrl"]!,
    apiKey: configuration["Qutora:ApiKey"]!,
    apiSecret: configuration["Qutora:ApiSecret"]!,
    enableCache: true,
    cacheMinutes: 30
);

// Distributed cache (Redis/SQL Server/etc.)
services.AddStackExchangeRedisCache(options => 
    options.Configuration = configuration.GetConnectionString("Redis"));
services.AddQutoraSDK(
    baseUrl: configuration["Qutora:BaseUrl"]!,
    apiKey: configuration["Qutora:ApiKey"]!,
    apiSecret: configuration["Qutora:ApiSecret"]!,
    useDistributedCache: true,
    cacheMinutes: 60
);

// Manual registration (advanced)
services.AddHttpClient<QutoraClient>();
services.AddSingleton(provider => new QutoraClientOptions
{
    BaseUrl = configuration["Qutora:BaseUrl"],
    ApiKey = configuration["Qutora:ApiKey"],
    ApiSecret = configuration["Qutora:ApiSecret"]
});
services.AddScoped<QutoraClient>();
```

### Configuration from appsettings.json

```json
{
  "Qutora": {
    "BaseUrl": "https://your-qutora-instance.com/api",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret",
    "TimeoutSeconds": 30,
    "MaxRetryAttempts": 3
  },
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
}
```

## 🔐 Authentication

The SDK uses API Key authentication with two headers:
- `X-QUTORA-Key`: Your API key
- `X-QUTORA-Secret`: Your API secret

Generate API credentials through the Qutora API or admin interface. Ensure your API key has appropriate permissions for the operations you need to perform.

## 🚨 Error Handling

```csharp
try
{
    var document = await client.Documents.GetByIdAsync("invalid-id");
}
catch (HttpRequestException ex) when (ex.Message.Contains("404"))
{
    Console.WriteLine("Document not found");
}
catch (HttpRequestException ex) when (ex.Message.Contains("401"))
{
    Console.WriteLine("Authentication failed - check your API credentials");
}
catch (HttpRequestException ex) when (ex.Message.Contains("403"))
{
    Console.WriteLine("Access forbidden - insufficient permissions");
}
catch (HttpRequestException ex) when (ex.Message.Contains("500"))
{
    Console.WriteLine("Server error - please try again later");
}
catch (TaskCanceledException ex)
{
    Console.WriteLine("Request timeout - check your network connection");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

## 📝 Logging

The SDK supports Microsoft.Extensions.Logging:

```csharp
using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder => 
    builder.AddConsole().SetMinimumLevel(LogLevel.Debug)
);
var logger = loggerFactory.CreateLogger<QutoraClient>();

var client = new QutoraClient(options, httpClient: null, logger: logger);
```

## 🔧 Advanced Usage

### Custom HttpClient Configuration

```csharp
var handler = new HttpClientHandler()
{
    UseCookies = false
};

var httpClient = new HttpClient(handler);
httpClient.DefaultRequestHeaders.Add("X-Custom-Header", "Value");

var client = new QutoraClient(options, httpClient);
```

### Connection Testing

```csharp
var isConnected = await client.TestConnectionAsync();
if (!isConnected)
{
    Console.WriteLine("Failed to connect to Qutora API - check your configuration");
}
```

### Retry Logic and Resilience

The SDK includes built-in retry logic for transient failures:

```csharp
var options = new QutoraClientOptions
{
    BaseUrl = "https://your-qutora-instance.com/api",
    ApiKey = "your-api-key",
    ApiSecret = "your-api-secret",
    MaxRetryAttempts = 5,  // Retry up to 5 times
    TimeoutSeconds = 120   // 2 minute timeout
};
```

## 📋 Supported API Endpoints

This SDK covers the following Qutora API endpoints:

### Documents (8 endpoints)
- `GET /api/documents` - List documents with filtering and pagination
- `POST /api/documents` - Upload new document with full options
- `GET /api/documents/{id}` - Get document details
- `PUT /api/documents/{id}` - Update document metadata
- `GET /api/documents/{id}/download` - Download document content
- `GET /api/documents/category/{categoryId}` - Get documents by category
- `POST /api/documents/{id}/versions` - Create new document version
- `GET /api/documents/{id}/versions` - Get document version history

### Categories (4 endpoints)
- `GET /api/categories` - List categories with pagination
- `GET /api/categories/all` - Get all categories (simple list)
- `POST /api/categories` - Create new category
- `GET /api/categories/{id}` - Get category details

### Metadata (8 endpoints)
- `GET /api/metadata/document/{documentId}` - Get document metadata
- `POST /api/metadata/document/{documentId}` - Create document metadata
- `PUT /api/metadata/document/{documentId}` - Update document metadata
- `GET /api/metadata/search` - Search documents by metadata
- `GET /api/metadata/tags` - Get all available tags
- `POST /api/metadata/validate` - Validate metadata against schema
- `GET /api/metadata/schemas` - Get metadata schemas
- `GET /api/metadata/schemas/{id}` - Get specific metadata schema

### Storage (2 endpoints)
- `GET /api/storage/buckets/my-accessible-buckets` - Get accessible storage buckets
- `GET /api/storage/providers/user-accessible` - Get accessible storage providers

**Total: 22 API endpoints supported**

## 🏗️ Architecture

The SDK follows clean architecture principles:

```
Qutora.SDK/
├── Interfaces/          # Service contracts
├── Services/            # API service implementations
├── Models/              # Data transfer objects
├── Constants/           # API endpoint definitions
├── QutoraClient.cs      # Main client facade
└── QutoraClientOptions.cs # Configuration options
```

## 🤝 Contributing

This SDK is open-source and we welcome contributions!

### Development Setup

1. Clone the SDK repository:
```bash
git clone https://github.com/qutora/qutora-sdk-dotnet.git
cd qutora-sdk-dotnet
```

2. Build the project:
```bash
dotnet build
```

3. Run tests:
```bash
dotnet test
```

### Contributing Guidelines

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Commit your changes (`git commit -m 'Add amazing feature'`)
7. Push to the branch (`git push origin feature/amazing-feature`)
8. Open a Pull Request

## 📄 License

This SDK is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support & Community

- **Documentation**: [SDK Documentation](https://github.com/qutora/qutora-sdk-dotnet/wiki)
- **Issues**: [GitHub Issues](https://github.com/qutora/qutora-sdk-dotnet/issues)
- **Discussions**: [GitHub Discussions](https://github.com/qutora/qutora-sdk-dotnet/discussions)

## 🚀 Getting Started with Qutora

To use this SDK, you'll need a running Qutora API instance. The Qutora backend provides the API endpoints that this SDK connects to.

### Qutora API Setup

For information about setting up the Qutora API backend, please refer to the [Qutora API documentation](https://github.com/qutora/qutora-api).

## 🔄 Version History

- **1.0.0** - Initial release
  - Complete API endpoint coverage (22 endpoints)
  - Document upload/download/management
  - Category operations with hierarchy support
  - Metadata management with schema validation
  - Storage bucket and provider access
  - Built-in retry logic and error handling
  - Comprehensive logging support
  - **Automatic caching system** with 2-5x performance improvement
  - **Dependency injection support** with simple setup
  - **Distributed cache support** (Redis, SQL Server, etc.)

## 🌟 Related Projects

- **[Qutora API](https://github.com/qutora/qutora-api)** - The main document management system backend
- **[Qutora Admin SDK](https://github.com/qutora/qutora-admin-sdk-dotnet)** - Complete administrative SDK with all API endpoints for system management

---

**Made with ❤️ for the .NET community. Star ⭐ the project on [GitHub](https://github.com/qutora/qutora-sdk-dotnet) if you find it useful!**