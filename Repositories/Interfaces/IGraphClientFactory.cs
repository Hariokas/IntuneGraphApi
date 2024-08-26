using Microsoft.Graph;

namespace Repositories.Interfaces;

public interface IGraphClientFactory
{
    GraphServiceClient CreateGraphClient();
}