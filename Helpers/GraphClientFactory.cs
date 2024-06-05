using Azure.Identity;
using Microsoft.Graph;

namespace Helpers;

public static class GraphClientFactory
{
    private static readonly string[] Scopes = ["https://graph.microsoft.com/.default"];

    public static GraphServiceClient CreateGraphClient(Configuration config)
    {
        var clientSecretCredential = new ClientSecretCredential(config.TenantId, config.ClientId, config.ClientSecret);
        return new GraphServiceClient(clientSecretCredential, Scopes);
    }
}