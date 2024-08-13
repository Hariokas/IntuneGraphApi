using Microsoft.AspNetCore.Mvc;
using Services.Implementations;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/DeviceController")]
[ApiController]
public class DeviceController (IDeviceService deviceService) : ControllerBase
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

}