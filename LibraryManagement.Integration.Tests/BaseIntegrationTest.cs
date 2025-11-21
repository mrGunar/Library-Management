using Grpc.Net.Client;

namespace LibraryManagement.Integration.Tests;

public class BaseIntegrationTest : IClassFixture<TestWebApplicationFactory>
{
    protected readonly TestWebApplicationFactory Factory;
    protected readonly GrpcChannel Channel;

    public BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        Factory = factory;
        var client = factory.CreateClient();
        Channel = GrpcChannel.ForAddress(client.BaseAddress!, new GrpcChannelOptions
        {
            HttpClient = client
        });
    }
}

