using Microsoft.Graph;
using Microsoft.Graph.Devices.Item.GetMemberGroups;
using Microsoft.Graph.Models;
using Repositories.Interfaces;

namespace Repositories.Implementations;

public class DeviceRepository(IGraphClientFactory graphClientFactory) : IDeviceRepository
{
    private readonly GraphServiceClient _graphClient = graphClientFactory.CreateGraphClient();

    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        var devices = await _graphClient.Devices.GetAsync();
        return devices?.Value ?? Enumerable.Empty<Device>();
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

    public async Task<IEnumerable<Device>> SearchDevicesByNameAsync(string deviceName)
    {
        var devices = await _graphClient.Devices
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Filter = $"startsWith(displayName, '{deviceName}')";
            });

        return devices?.Value ?? Enumerable.Empty<Device>();
    }

    public async Task<IEnumerable<string>> GetDeviceGroupIdsAsync(string deviceId)
    {
        var requestBody = new GetMemberGroupsPostRequestBody { SecurityEnabledOnly = false };
        var groupIds = await _graphClient.Devices[deviceId].GetMemberGroups.PostAsGetMemberGroupsPostResponseAsync(requestBody);

        return groupIds?.Value ?? Enumerable.Empty<string>();
    }

}