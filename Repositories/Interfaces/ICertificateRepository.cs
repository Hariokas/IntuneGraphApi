using System.Security.Cryptography.X509Certificates;

namespace Repositories.Interfaces;

public interface ICertificateRepository
{
    X509Certificate2 GetCertificateFromStore(string thumbprint);
}