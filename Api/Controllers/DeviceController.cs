using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Api.Controllers;

[Route("api/DeviceController")]
[ApiController]
public class DeviceController(IDeviceService deviceService) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetDevices()
    {
        var devices = await deviceService.GetDevicesAsync();
        var result = devices.Select(d => new
        {
            d.Id,
            d.DisplayName,
            d.DeviceId,
            d.OperatingSystem,
            d.OperatingSystemVersion,
            d.RegisteredOwners,
            d.RegisteredUsers,
            d.ApproximateLastSignInDateTime,
            d.OnPremisesLastSyncDateTime
        });

        return Ok(result);
    }

    // Endpoint to add a device to a group
    [HttpPost("AddToGroup/{groupId}/{deviceId}")]
    public async Task<IActionResult> AddDeviceToGroup(string groupId, string deviceId)
    {
        await deviceService.AddDeviceToGroupAsync(groupId, deviceId);
        return NoContent();
    }

    // Endpoint to remove a device from a group
    [HttpDelete("RemoveFromGroup/{groupId}/{deviceId}")]
    public async Task<IActionResult> RemoveDeviceFromGroup(string groupId, string deviceId)
    {
        await deviceService.RemoveDeviceFromGroupAsync(groupId, deviceId);
        return NoContent();
    }

    // Endpoint to search devices by name
    [HttpGet("Search/{deviceName}")]
    public async Task<IActionResult> SearchDevicesByName(string deviceName)
    {
        if (string.IsNullOrEmpty(deviceName)) return BadRequest("Device name cannot be empty.");

        var devices = await deviceService.SearchDevicesByNameAsync(deviceName);

        return devices.Count() > 0 ? Ok(devices) : NoContent();
    }

    // Endpoint to get group IDs that are assigned to a device
    [HttpGet("{deviceId}/groups")]
    public async Task<IActionResult> GetDeviceGroupIds(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId)) return BadRequest("Device ID cannot be empty.");

        var groupIds = await deviceService.GetDeviceGroupIdsAsync(deviceId);

        return groupIds.Count() > 0 ? Ok(groupIds) : NoContent();
    }

    // Endpoint to get assigned apps to a device
    [HttpGet("{deviceId}/apps")]
    public async Task<IActionResult> GetAssignedApps(string deviceId)
    {
        if (string.IsNullOrEmpty(deviceId)) return BadRequest("Device ID cannot be empty");

        var apps = await deviceService.GetAssignedAppsAsync(deviceId);

        return apps.Count() > 0 ? Ok(apps) : NoContent();
    }

}