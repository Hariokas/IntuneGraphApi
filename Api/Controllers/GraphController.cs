// References:

// Microsoft Graph API for Intune: Overview of Microsoft Graph API for Intune
// https://learn.microsoft.com/en-us/graph/api/resources/intune-graph-overview?view=graph-rest-1.0

// Application Assignments:
// Win32LobAppAssignmentSettings https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-win32lobappassignmentsettings?view=graph-rest-1.0
// MobileAppAssignment https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-mobileappassignment?view=graph-rest-1.0
// Assignment Target https://learn.microsoft.com/en-us/graph/api/resources/intune-shared-assignmenttarget?view=graph-rest-1.0
// Includes groupAssignmentTarget and allDevicesAssignmentTarget.

using Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Services.Implementations;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GraphController(IGraphService graphService) : ControllerBase
{

    [HttpGet("devices")]
    public async Task<IActionResult> GetDevices()
    {
        var devices = await graphService.GetDevicesAsync();
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
    [HttpPost("groups/{groupId}/devices/{deviceId}")]
    public async Task<IActionResult> AddDeviceToGroup(string groupId, string deviceId)
    {
        await graphService.AddDeviceToGroupAsync(groupId, deviceId);
        return NoContent();
    }

    // Endpoint to remove a device from a group
    [HttpDelete("groups/{groupId}/devices/{deviceId}")]
    public async Task<IActionResult> RemoveDeviceFromGroup(string groupId, string deviceId)
    {
        await graphService.RemoveDeviceFromGroupAsync(groupId, deviceId);
        return NoContent();
    }

    // Endpoint to create a group
    [HttpPost("groups")]
    public async Task<IActionResult> CreateGroup([FromBody] GroupCreationDto groupCreationDto)
    {
        var group = await graphService.CreateGroupAsync(groupCreationDto.DisplayName, groupCreationDto.MailNickname, groupCreationDto.Description);
        return CreatedAtAction(nameof(GetGroups), new { groupId = group.Id }, group);
    }

    [HttpPost("apps/{appName}/create-groups")]
    public async Task<IActionResult> CreateAppGroups(string appName)
    {
        await graphService.CreateAppGroupsAsync(appName);
        return Ok($"Groups for {appName} created successfully.");
    }

    [HttpPost("apps/{appName}/deploy")]
    public async Task<IActionResult> DeployApp(string appName)
    {
        try
        {
            await graphService.DeployAppToGroupAsync(appName);
            return Ok($"{appName} deployment configured successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Endpoint to get all groups
    [HttpGet("groups")]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await graphService.GetGroupsAsync();
        return Ok(groups);
    }

    [HttpGet("groups/search")]
    public async Task<IActionResult> SearchGroupsByName([FromQuery] string namePart)
    {
        if (string.IsNullOrWhiteSpace(namePart))
        {
            return BadRequest("Name part cannot be empty.");
        }

        var groups = await graphService.SearchGroupsByNameAsync(namePart);
        var result = groups.Select(g => new
        {
            g.Id,
            g.DisplayName
        });

        return Ok(result);
    }

    // Endpoint to get all apps
    [HttpGet("apps")]
    public async Task<IActionResult> GetApps()
    {
        var apps = await graphService.GetAppsAsync();
        return Ok(apps);
    }

    // Endpoint to get all Windows apps
    [HttpGet("apps/windows")]
    public async Task<IActionResult> GetWindowsApps()
    {
        var windowsApps = await graphService.GetWindowsAppsAsync();
        return Ok(windowsApps);
    }

    // Endpoint to get assignments for a specific app
    [HttpGet("apps/{appId}/assignments")]
    public async Task<IActionResult> GetAppAssignments(string appId)
    {
        var assignments = await graphService.GetAppAssignmentsAsync(appId);
        return Ok(assignments);
    }

    // Endpoint to assign an app to a group
    [HttpPost("apps/{appId}/assignments")]
    public async Task<IActionResult> AssignAppToGroup(string appId, [FromBody] string groupId, InstallIntent intent)
    {
        await graphService.AssignAppToGroupAsync(appId, groupId, intent);
        return NoContent();
    }

    // Endpoint to remove an app assignment
    [HttpDelete("apps/{appId}/assignments/{assignmentId}")]
    public async Task<IActionResult> RemoveAppAssignment(string appId, string assignmentId)
    {
        await graphService.RemoveAppAssignmentAsync(appId, assignmentId);
        return NoContent();
    }
}