using System.Security.Cryptography.X509Certificates;
using Repositories.Interfaces;
using Serilog;

namespace Repositories.Implementations;

public class CertificateRepository : ICertificateRepository
{
    public X509Certificate2 GetCertificateFromStore(string thumbprint)
    {
        try
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);
            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            if (certCollection.Any())
                return certCollection[0];

            Log.Error($"Certificate with thumbprint [{thumbprint}] not found.");
            return null;
        }
        catch (Exception e)
        {
            Log.Error($"Error while getting certificate from store: {e.Message}");
            return null;
        }
    }
}