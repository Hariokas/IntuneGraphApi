using Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController(IGroupService groupService) : ControllerBase
{
    // Endpoint to get all groups
    [HttpGet()]
    [ResponseCache(CacheProfileName = "5MinCache")]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await groupService.GetGroupsAsync();
        return Ok(groups);
    }

    [HttpGet("{namePart}")]
    [ResponseCache(CacheProfileName = "5MinCache")]
    public async Task<IActionResult> SearchGroupsByName(string namePart)
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

    // Endpoint to create a group
    [HttpPost("Create")]
    public async Task<IActionResult> CreateGroup([FromBody] GroupCreationDto groupCreationDto)
    {
        var group = await groupService.CreateGroupAsync(groupCreationDto.DisplayName, groupCreationDto.MailNickname,
            groupCreationDto.Description);
        return CreatedAtAction(nameof(GetGroups), new { groupId = group.Id }, group);
    }

    [HttpPost("Create/{appName}")]
    public async Task<IActionResult> CreateAppGroups(string appName)
    {
        await groupService.CreateAppGroupsAsync(appName);
        return Ok($"Groups for {appName} created successfully.");
    }

    [HttpPost("Deploy/{appName}")]
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
}