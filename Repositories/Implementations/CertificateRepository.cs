using System.Security.Cryptography.X509Certificates;
using Repositories.Interfaces;

namespace Repositories.Implementations;

public class CertificateRepository : ICertificateRepository
{
    public X509Certificate2 GetCertificateFromStore(string thumbprint)
    {
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

        store.Open(OpenFlags.ReadOnly);
        var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

        return certCollection.Count > 0
            ? certCollection.First()
            : throw new Exception($"Certificate with thumbprint [{thumbprint}] not found.");
    }
}