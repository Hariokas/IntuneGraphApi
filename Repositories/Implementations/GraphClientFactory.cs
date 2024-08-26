using Azure.Identity;
using Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Repositories.Interfaces;

namespace Repositories.Implementations;

public class GraphClientFactory(IOptions<GraphApiConfiguration> config, ICertificateRepository certService) : IGraphClientFactory
{
    private static readonly string[] Scopes = ["https://graph.microsoft.com/.default"];

    public GraphServiceClient CreateGraphClient()
    {
        var certThumbprint = config.Value.CertificateThumbprint;
        var cert = certService.GetCertificateFromStore(certThumbprint);

        var clientCertCredential = new ClientCertificateCredential(config.Value.TenantId, config.Value.ClientId, cert);

        return new GraphServiceClient(clientCertCredential, Scopes);
    }
}