using Microsoft.Graph.Models;

namespace Services.Interfaces;

public interface IDeviceService
{
    Task<IEnumerable<Device>> GetDevicesAsync();
    
    Task AddDeviceToGroupAsync(string groupId, string deviceId);
    
    Task RemoveDeviceFromGroupAsync(string groupId, string deviceId);
}