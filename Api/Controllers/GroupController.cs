using Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController(IGroupService groupService) : ControllerBase
{
    // Endpoint to create a group
    [HttpPost("groups")]
    public async Task<IActionResult> CreateGroup([FromBody] GroupCreationDto groupCreationDto)
    {
        var group = await groupService.CreateGroupAsync(groupCreationDto.DisplayName, groupCreationDto.MailNickname,
            groupCreationDto.Description);
        return CreatedAtAction(nameof(GetGroups), new { groupId = group.Id }, group);
    }

    [HttpPost("apps/{appName}/create-groups")]
    public async Task<IActionResult> CreateAppGroups(string appName)
    {
        await groupService.CreateAppGroupsAsync(appName);
        return Ok($"Groups for {appName} created successfully.");
    }

    [HttpPost("apps/{appName}/deploy")]
    public async Task<IActionResult> DeployApp(string appName)
    {
        try
        {
            await groupService.DeployAppToGroupAsync(appName);
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
        var groups = await groupService.GetGroupsAsync();
        return Ok(groups);
    }

    [HttpGet("groups/search")]
    public async Task<IActionResult> SearchGroupsByName([FromQuery] string namePart)
    {
        if (string.IsNullOrWhiteSpace(namePart)) return BadRequest("Name part cannot be empty.");

        var groups = await groupService.SearchGroupsByNameAsync(namePart);
        var result = groups.Select(g => new
        {
            g.Id,
            g.DisplayName
        });

        return Ok(result);
    }

}