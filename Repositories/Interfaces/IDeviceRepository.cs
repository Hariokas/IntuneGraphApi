﻿using Microsoft.Graph.Models;

namespace Repositories.Interfaces;

public interface IDeviceRepository
{
    Task<IEnumerable<Device>> GetDevicesAsync();

    Task AddDeviceToGroupAsync(string groupId, string deviceId);
    
    Task RemoveDeviceFromGroupAsync(string groupId, string deviceId);

    Task<IEnumerable<Device>> SearchDevicesByNameAsync(string deviceName);

    Task<IEnumerable<string>> GetDeviceGroupIdsAsync(string deviceId);
}