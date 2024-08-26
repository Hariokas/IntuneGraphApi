using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Serilog;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppController(IAppService appService, IGroupService groupService) : ControllerBase
{
    // Endpoint to get all apps
    [HttpGet]
    public async Task<IActionResult> GetApps()
    {
        try
        {
            Log.Information("Fetching all apps.");
            var apps = await appService.GetAppsAsync();
            Log.Information($"Successfully fetched {apps.Count()} apps.");
            return Ok(apps);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching apps.");
            return StatusCode(500, "An error occurred while fetching apps.");
        }
    }

    // Endpoint to get all Windows apps
    [HttpGet("Windows")]
    public async Task<IActionResult> GetWindowsApps()
    {
        try
        {
            Log.Information("Fetching all Windows apps.");
            var windowsApps = await appService.GetWindowsAppsAsync();
            Log.Information($"Successfully fetched {windowsApps.Count()} Windows apps.");
            return Ok(windowsApps);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching Windows apps.");
            return StatusCode(500, "An error occurred while fetching Windows apps.");
        }
    }

    // Endpoint to find AppID by its name
    [HttpGet("{appName}")]
    public async Task<IActionResult> GetAppIdByName(string appName)
    {
        if (string.IsNullOrEmpty(appName))
        {
            Log.Warning("GetAppIdByName called with an empty appName.");
            return BadRequest($"{nameof(appName)} cannot be empty.");
        }

        try
        {
            Log.Information($"Fetching apps with name: {appName}.");
            var apps = await appService.GetAppsByName(appName);

            if (!apps.Any())
            {
                Log.Information($"No apps found with name: {appName}.");
                return NotFound($"\"{appName}\" query did not return any results.");
            }

            Log.Information($"Found {apps.Count()} apps appName name: {appName}.");
            return Ok(apps);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error occurred while fetching apps by name: {appName}.");
            return StatusCode(500, "An error occurred while fetching apps by name.");
        }
    }

    // Endpoint to get assignments for a specific app
    [HttpGet("Assignments/{appId}")]
    public async Task<IActionResult> GetAppAssignments(string appId)
    {
        try
        {
            Log.Information($"Fetching assignments for appId: {appId}.");
            var assignments = await appService.GetAppAssignmentsAsync(appId);
            Log.Information($"Successfully fetched {assignments.Count()} assignments for appId: {appId}.");
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error occurred while fetching assignments for appId: {appId}.");
            return StatusCode(500, "An error occurred while fetching app assignments.");
        }
    }

    [HttpGet("{appId}/DeploymentGroups")]
    public async Task<IActionResult> GetAppDeploymentGroups(string appId)
    {
        try
        {
            var assignments = await appService.GetAppAssignmentsAsync(appId);

            var groupIds = assignments
                .Select(a => a.Target as GroupAssignmentTarget)
                .Where(t => t != null)
                .Select(t => t.GroupId)
                .Distinct()
                .ToList();

            var relatedGroups = await groupService.GetGroupsByIdsAsync(groupIds);

            return Ok(relatedGroups);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error occurred while fetching deployment groups for appId: {appId}.");
            return StatusCode(500, "An error occurred while fetching deployment groups.");
        }
    }

    // Endpoint to remove an app assignment
    [HttpDelete("Assignments/{assignmentId}/Remove/{appId}")]
    public async Task<IActionResult> RemoveAppAssignment(string appId, string assignmentId)
    {
        try
        {
            Log.Information($"Removing assignment with assignmentId: {assignmentId} for appId: {appId}.");
            await appService.RemoveAppAssignmentAsync(appId, assignmentId);

            Log.Information($"Successfully removed assignment with assignmentId: {assignmentId} for appId: {appId}.");
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex,
                $"Error occurred while removing assignment with assignmentId: {assignmentId} for appId: {appId}.");
            return StatusCode(500, "An error occurred while removing app assignment.");
        }
    }

    // Endpoint to assign an app to a group
    [HttpPost("Assignments/{assignmentId}/Add/{appId}")]
    public async Task<IActionResult> AssignAppToGroup(string appId, string assignmentId, InstallIntent intent)
    {
        try
        {
            Log.Information(
                $"Assigning appId: {appId} to group with assignmentId: {assignmentId} using intent: {intent}.");
            await appService.AssignAppToGroupAsync(appId, assignmentId, intent);

            Log.Information($"Successfully assigned appId: {appId} to group with assignmentId: {assignmentId}.");
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error occurred while assigning appId: {appId} to group with assignmentId: {assignmentId}.");
            return StatusCode(500, "An error occurred while assigning the app to a group.");
        }
    }
}