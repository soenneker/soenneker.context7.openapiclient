using Soenneker.Tests.HostedUnit;

namespace Soenneker.Context7.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class Context7OpenApiClientTests : HostedUnitTest
{
    public Context7OpenApiClientTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
