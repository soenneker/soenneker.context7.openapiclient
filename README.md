[![](https://img.shields.io/nuget/v/soenneker.context7.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.context7.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.context7.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.context7.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.context7.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.context7.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.context7.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.context7.openapiclient/actions/workflows/codeql.yml)

# Soenneker.Context7.OpenApiClient

A Kiota-generated .NET client for Context7's HTTP API.

## Install

```bash
dotnet add package Soenneker.Context7.OpenApiClient
```

## Recommended setup

For dependency injection, API-key configuration, and client reuse, install the companion utility:

```bash
dotnet add package Soenneker.Context7.OpenApiClientUtil
```

```csharp
using Soenneker.Context7.OpenApiClientUtil.Registrars;

services.AddContext7OpenApiClientUtilAsSingleton();
```

Configure `Context7:ApiKey`, inject `IContext7OpenApiClientUtil`, and call `Get` to obtain the generated client.

## Direct construction

```csharp
using System.Net.Http.Headers;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Context7.OpenApiClient;
using Soenneker.Context7.OpenApiClient.Models;

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", apiKey);

var authentication = new AnonymousAuthenticationProvider();
var adapter = new HttpClientRequestAdapter(authentication, httpClient: httpClient);
var context7 = new Context7OpenApiClient(adapter);

SearchResponse? response = await context7.V2.Libs.Search.GetAsync(
    request =>
    {
        request.QueryParameters.LibraryName = "react";
        request.QueryParameters.Query = "How do hooks manage state?";
    },
    cancellationToken);
```

`AnonymousAuthenticationProvider` is used because the bearer header is already applied to this dedicated `HttpClient`. Do not put a Context7 API key on a shared client that can send default headers to unrelated hosts.

## Navigating the client

The generated request builders mirror Context7's URL hierarchy:

- `context7.V2.Libs.Search` searches for a library.
- `context7.V2.Context` retrieves documentation context.
- `context7.V2.Libs.Metrics` accesses library metrics.
- `context7.V2.Add` exposes repository, website, OpenAPI, and other ingestion endpoints.
- `context7.V1.Refresh` requests a library refresh.

Endpoint methods accept a request-configuration callback for query parameters, headers, and middleware options, followed by a cancellation token.

## Practical notes

- Keep the `HttpClient`, request adapter, and generated client long-lived. The companion utility manages that lifecycle for dependency-injection applications.
- Context7 authenticates API requests with `Authorization: Bearer <api-key>`. Treat the key as a credential and redact authorization headers from logs and traces.
- Response models and endpoint return values may be nullable when the schema permits an empty response.
- Kiota surfaces mapped service failures as generated error models; other transport and serialization failures surface as Kiota or HTTP exceptions.
- Public names and response shapes follow the source OpenAPI description and can change when the client is regenerated.
- Files under `src/Soenneker.Context7.OpenApiClient` are generated. Keep application-specific behavior outside the generated project.
