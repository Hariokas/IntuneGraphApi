using Microsoft.Graph;
using Microsoft.Graph.Models;
using Repositories.Interfaces;

namespace Repositories.Implementations;

public class DeviceRepository(IGraphClientFactory graphClientFactory) : IDeviceRepository
{
    private readonly GraphServiceClient _graphClient = graphClientFactory.CreateGraphClient();

    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        var devices = await _graphClient.Devices.GetAsync();
        return devices.Value;
    }

    public async Task AddDeviceToGroupAsync(string groupId, string deviceId)
    {
        var referenceCreate = new ReferenceCreate
        {
            OdataId = $"https://graph.microsoft.com/v1.0/directoryObjects/{deviceId}"
        };

        await _graphClient.Groups[groupId].Members.Ref.PostAsync(referenceCreate);
    }

    public async Task RemoveDeviceFromGroupAsync(string groupId, string deviceId)
    {
        await _graphClient.Groups[groupId].Members[deviceId].Ref.DeleteAsync();
    }
}