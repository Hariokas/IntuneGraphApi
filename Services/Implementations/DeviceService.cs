using Microsoft.Graph.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implementations;

public class DeviceService(IDeviceRepository deviceRepository) : IDeviceService
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
}