using Microsoft.Graph.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implementations;

public class GroupService(IGroupRepository groupRepository, IAppRepository appRepository) : IGroupService
{
    public async Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description)
    {
        return await groupRepository.CreateSecurityGroupAsync(displayName, mailNickname, description);
    }

    public async Task CreateAppGroupsAsync(string appName)
    {
        var requiredGroup = await groupRepository.CreateSecurityGroupAsync($"{appName} - Required", $"{appName}-required", $"Group for required installation of {appName}", false, true);
        var availableGroup = await groupRepository.CreateSecurityGroupAsync($"{appName} - Available", $"{appName}-available", $"Group for available installation of {appName}", false, true);
        var uninstallGroup = await groupRepository.CreateSecurityGroupAsync($"{appName} - Uninstall", $"{appName}-uninstall", $"Group for uninstalling {appName}", false, true);

        // Optionally, you can return or log the group IDs for further use.
    }

    public async Task DeployAppToGroupAsync(string appName, string requiredGroupId, string availableGroupId, string uninstallGroupId)
    {
        var appId = await appRepository.GetAppIdByNameAsync(appName);

        if (appId == null)
        {
            throw new Exception($"App {appName} not found.");
        }

        // Assign to required group with Required intent
        await appRepository.AssignAppToGroupAsync(appId, requiredGroupId, InstallIntent.Required);

        // Assign to available group with Available intent
        await appRepository.AssignAppToGroupAsync(appId, availableGroupId, InstallIntent.Available);

        // Assign to uninstall group with Uninstall intent
        await appRepository.AssignAppToGroupAsync(appId, uninstallGroupId, InstallIntent.Uninstall);
    }

    public async Task DeployAppToGroupAsync(string appName)
    {
        var requiredGroupName = $"{appName} - Required";
        var availableGroupName = $"{appName} - Available";
        var uninstallGroupName = $"{appName} - Uninstall";

        var requiredGroupId = await groupRepository.GetGroupIdByNameAsync(requiredGroupName);
        var availableGroupId = await groupRepository.GetGroupIdByNameAsync(availableGroupName);
        var uninstallGroupId = await groupRepository.GetGroupIdByNameAsync(uninstallGroupName);

        if (requiredGroupId == null || availableGroupId == null || uninstallGroupId == null)
        {
            throw new Exception($"One or more groups for {appName} not found.");
        }

        var appId = await appRepository.GetAppIdByNameAsync(appName);

        if (appId == null)
            throw new Exception($"App {appName} not found.");

        // Assign to required group with Required intent
        await appRepository.AssignAppToGroupAsync(appId, requiredGroupId, InstallIntent.Required);

        // Assign to available group with Available intent
        await appRepository.AssignAppToGroupAsync(appId, availableGroupId, InstallIntent.Available);

        // Assign to uninstall group with Uninstall intent
        await appRepository.AssignAppToGroupAsync(appId, uninstallGroupId, InstallIntent.Uninstall);
    }

    public async Task<IEnumerable<Group>> GetGroupsAsync()
    {
        return await groupRepository.GetGroupsAsync();
    }

    public async Task<Group> GetGroupByIdAsync(string groupId)
    {
        return await groupRepository.GetGroupByIdAsync(groupId);
    }

    public async Task<IEnumerable<Group>> GetGroupsByIdsAsync(IEnumerable<string> groupIds)
    {
        return await groupRepository.GetGroupsByIdsAsync(groupIds);
    }

    public async Task<IEnumerable<Device>> GetDevicesInGroupAsync(string groupId)
    {
        return await groupRepository.GetDevicesInGroupAsync(groupId);
    }

    public async Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart)
    {
        return await groupRepository.SearchGroupsByNameAsync(namePart);
    }
}