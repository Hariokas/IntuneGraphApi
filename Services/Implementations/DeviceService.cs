using Microsoft.Graph.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implementations;

public class DeviceService(IDeviceRepository deviceRepository, IAppRepository appRepository) : IDeviceService
{
    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        return await deviceRepository.GetDevicesAsync();
    }

    public async Task AddDeviceToGroupAsync(string groupId, string deviceId)
    {
        await deviceRepository.AddDeviceToGroupAsync(groupId, deviceId);
    }

    public async Task RemoveDeviceFromGroupAsync(string groupId, string deviceId)
    {
        await deviceRepository.RemoveDeviceFromGroupAsync(groupId, deviceId);
    }

    public async Task<IEnumerable<Device>> SearchDevicesByNameAsync(string deviceName)
    {
        return await deviceRepository.SearchDevicesByNameAsync(deviceName);
    }

    public async Task<IEnumerable<string>> GetDeviceGroupIdsAsync(string deviceId)
    {
        return await deviceRepository.GetDeviceGroupIdsAsync(deviceId);
    }

    public async Task<IEnumerable<MobileApp>> GetAssignedAppsAsync(string deviceId)
    {
        var groupIds = await deviceRepository.GetDeviceGroupIdsAsync(deviceId);
        var apps = await appRepository.GetAppsAssignedToGroupsAsync(groupIds);

        return apps;
    }
}