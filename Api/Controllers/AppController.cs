using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppController(IAppService appService) : ControllerBase
{
    // Endpoint to get all apps
    [HttpGet()]
    public async Task<IActionResult> GetApps()
    {
        var apps = await appService.GetAppsAsync();
        return Ok(apps);
    }

    // Endpoint to get all Windows apps
    [HttpGet("Windows")]
    public async Task<IActionResult> GetWindowsApps()
    {
        var windowsApps = await appService.GetWindowsAppsAsync();
        return Ok(windowsApps);
    }

    // Endpoint to find AppID by it's name
    [HttpGet("{appName}")]
    public async Task<IActionResult> GetAppIdByName(string appName)
    {
        if (string.IsNullOrEmpty(appName)) return BadRequest($"{nameof(appName)} cannot be empty.");

        var apps = await appService.GetAppsByName(appName);

        if (!apps.Any())
            return NotFound($"\"{appName}\" query did not return any results.");

        return Ok(apps);
    }

    // Endpoint to get assignments for a specific app
    [HttpGet("Assignments/{appId}")]
    public async Task<IActionResult> GetAppAssignments(string appId)
    {
        var assignments = await appService.GetAppAssignmentsAsync(appId);
        return Ok(assignments);
    }

    // Endpoint to remove an app assignment
    [HttpDelete("Assignments/{assignmentId}/Remove/{appId}")]
    public async Task<IActionResult> RemoveAppAssignment(string appId, string assignmentId)
    {
        await appService.RemoveAppAssignmentAsync(appId, assignmentId);
        return NoContent();
    }

    // Endpoint to assign an app to a group
    [HttpPost("Assignments/{assignmentId}/Add/{appId}")]
    public async Task<IActionResult> AssignAppToGroup(string appId, string assignmentId, InstallIntent intent)
    {
        await appService.AssignAppToGroupAsync(appId, assignmentId, intent);
        return NoContent();
    }
}