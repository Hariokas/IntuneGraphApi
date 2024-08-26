using Azure.Identity;
using Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Repositories.Interfaces;
using Serilog;

namespace Repositories.Implementations;

public class GraphClientFactory(IOptions<GraphApiConfiguration> config, ICertificateRepository certService) : IGraphClientFactory
{
    private static readonly string[] Scopes = ["https://graph.microsoft.com/.default"];

    public GraphServiceClient CreateGraphClient()
    {
        var certThumbprint = config.Value.CertificateThumbprint;
        var cert = certService.GetCertificateFromStore(certThumbprint);

        if (cert != null)
        {
            Log.Information("Creating GraphClient using certificate");
            var clientCertCredential = new ClientCertificateCredential(config.Value.TenantId, config.Value.ClientId, cert);
            return new GraphServiceClient(clientCertCredential, Scopes);
        }

        Log.Warning("Certificate not found, falling back to client secret.");
        return CreateGraphClientWithClientSecret();
    }

    private GraphServiceClient CreateGraphClientWithClientSecret()
    {
        var clientSecretCredential = new ClientSecretCredential(config.Value.TenantId, config.Value.ClientId, config.Value.ClientSecret);

        return new GraphServiceClient(clientSecretCredential, Scopes);
    }

}