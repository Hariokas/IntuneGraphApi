using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Microsoft.Graph;

namespace Helpers;

public static class GraphClientFactory
{
    private static readonly string[] Scopes = ["https://graph.microsoft.com/.default"];

    public static GraphServiceClient CreateGraphClient(Configuration config)
    {
        var certificateThumbprint = config.CertificateThumbprint;
        var certificate = GetCertificateFromStore(certificateThumbprint);

        var clientCertificateCredential =
            new ClientCertificateCredential(config.TenantId, config.ClientId, certificate);

        return new GraphServiceClient(clientCertificateCredential, Scopes);
    }

    private static X509Certificate2 GetCertificateFromStore(string thumbprint)
    {
        using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
        try
        {
            store.Open(OpenFlags.ReadOnly);
            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            return certCollection.Count > 0 ? certCollection.First() : throw new Exception("Certificate not found.");
        }
        finally
        {
            store.Close();
        }
    }
}