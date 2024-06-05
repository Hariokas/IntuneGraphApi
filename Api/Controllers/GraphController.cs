// References:

// Microsoft Graph API for Intune: Overview of Microsoft Graph API for Intune
// https://learn.microsoft.com/en-us/graph/api/resources/intune-graph-overview?view=graph-rest-1.0

// Application Assignments:
// Win32LobAppAssignmentSettings https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-win32lobappassignmentsettings?view=graph-rest-1.0
// MobileAppAssignment https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-mobileappassignment?view=graph-rest-1.0
// Assignment Target https://learn.microsoft.com/en-us/graph/api/resources/intune-shared-assignmenttarget?view=graph-rest-1.0
// Includes groupAssignmentTarget and allDevicesAssignmentTarget.

using Microsoft.AspNetCore.Mvc;
using Services.Implementations;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GraphController(IGraphService graphService) : ControllerBase
{

    // Endpoint to add a device to a group
    // Documentation: https://learn.microsoft.com/en-us/graph/api/group-post-members?view=graph-rest-1.0&tabs=http
    [HttpPost("groups/{groupId}/devices/{deviceId}")]
    public async Task<IActionResult> AddDeviceToGroup(string groupId, string deviceId)
    {
        await graphService.AddDeviceToGroupAsync(groupId, deviceId);
        return NoContent();
    }

    // Endpoint to remove a device from a group
    // Documentation: https://learn.microsoft.com/en-us/graph/api/group-delete-members?view=graph-rest-1.0&tabs=http
    [HttpDelete("groups/{groupId}/devices/{deviceId}")]
    public async Task<IActionResult> RemoveDeviceFromGroup(string groupId, string deviceId)
    {
        await graphService.RemoveDeviceFromGroupAsync(groupId, deviceId);
        return NoContent();
    }

    // Endpoint to get all groups
    // https://learn.microsoft.com/en-us/graph/api/group-list?view=graph-rest-1.0
    [HttpGet("groups")]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await graphService.GetGroupsAsync();
        return Ok(groups);
    }

    // Endpoint to get all apps
    // https://learn.microsoft.com/en-us/graph/api/intune-apps-mobileapp-list?view=graph-rest-1.0
    [HttpGet("apps")]
    public async Task<IActionResult> GetApps()
    {
        var apps = await graphService.GetAppsAsync();
        return Ok(apps);
    }

    // Endpoint to get all Windows apps
    // Win32LobApp: https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-win32lobapp?view=graph-rest-1.0
    [HttpGet("apps/windows")]
    public async Task<IActionResult> GetWindowsApps()
    {
        var windowsApps = await graphService.GetWindowsAppsAsync();
        return Ok(windowsApps);
    }

    // Endpoint to get assignments for a specific app
    // https://learn.microsoft.com/en-us/graph/api/intune-apps-mobileappassignment-list?view=graph-rest-1.0
    [HttpGet("apps/{appId}/assignments")]
    public async Task<IActionResult> GetAppAssignments(string appId)
    {
        var assignments = await graphService.GetAppAssignmentsAsync(appId);
        return Ok(assignments);
    }

    // Endpoint to assign an app to a group
    // https://learn.microsoft.com/en-us/graph/api/intune-apps-mobileappassignment-create?view=graph-rest-1.0
    [HttpPost("apps/{appId}/assignments")]
    public async Task<IActionResult> AssignAppToGroup(string appId, [FromBody] string groupId)
    {
        await graphService.AssignAppToGroupAsync(appId, groupId);
        return NoContent();
    }

    // Endpoint to remove an app assignment
    // https://learn.microsoft.com/en-us/graph/api/intune-apps-mobileappassignment-delete?view=graph-rest-1.0
    [HttpDelete("apps/{appId}/assignments/{assignmentId}")]
    public async Task<IActionResult> RemoveAppAssignment(string appId, string assignmentId)
    {
        await graphService.RemoveAppAssignmentAsync(appId, assignmentId);
        return NoContent();
    }
}