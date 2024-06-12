using Microsoft.Graph.Models;
using Repositories.Implementations;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implementations;

public class GraphService(IGraphRepository graphRepository) : IGraphService
{
    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        return await graphRepository.GetDevicesAsync();
    }

    public async Task AddDeviceToGroupAsync(string groupId, string deviceId)
    {
        await graphRepository.AddDeviceToGroupAsync(groupId, deviceId);
    }

    public async Task RemoveDeviceFromGroupAsync(string groupId, string deviceId)
    {
        await graphRepository.RemoveDeviceFromGroupAsync(groupId, deviceId);
    }

    public async Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description)
    {
        return await graphRepository.CreateGroupAsync(displayName, mailNickname, description);
    }

    public async Task CreateAppGroupsAsync(string appName)
    {
        var requiredGroup = await graphRepository.CreateGroupAsync($"{appName} - Required", $"{appName}-required", $"Group for required installation of {appName}", false, true);
        var availableGroup = await graphRepository.CreateGroupAsync($"{appName} - Available", $"{appName}-available", $"Group for available installation of {appName}", false, true);
        var uninstallGroup = await graphRepository.CreateGroupAsync($"{appName} - Uninstall", $"{appName}-uninstall", $"Group for uninstalling {appName}", false, true);

        // Optionally, you can return or log the group IDs for further use.
    }

    public async Task DeployAppToGroupAsync(string appName, string requiredGroupId, string availableGroupId, string uninstallGroupId)
    {
        var appId = await GetAppIdByName(appName);

        if (appId == null)
        {
            throw new Exception($"App {appName} not found.");
        }

        // Assign to required group with Required intent
        await graphRepository.AssignAppToGroupAsync(appId, requiredGroupId, InstallIntent.Required);

        // Assign to available group with Available intent
        await graphRepository.AssignAppToGroupAsync(appId, availableGroupId, InstallIntent.Available);

        // Assign to uninstall group with Uninstall intent
        await graphRepository.AssignAppToGroupAsync(appId, uninstallGroupId, InstallIntent.Uninstall);
    }

    public async Task DeployAppToGroupAsync(string appName)
    {
        var requiredGroupName = $"{appName} - Required";
        var availableGroupName = $"{appName} - Available";
        var uninstallGroupName = $"{appName} - Uninstall";

        var requiredGroupId = await graphRepository.GetGroupIdByNameAsync(requiredGroupName);
        var availableGroupId = await graphRepository.GetGroupIdByNameAsync(availableGroupName);
        var uninstallGroupId = await graphRepository.GetGroupIdByNameAsync(uninstallGroupName);

        if (requiredGroupId == null || availableGroupId == null || uninstallGroupId == null)
        {
            throw new Exception($"One or more groups for {appName} not found.");
        }

        var appId = await graphRepository.GetAppIdByNameAsync(appName);

        if (appId == null)
        {
            throw new Exception($"App {appName} not found.");
        }

        // Assign to required group with Required intent
        await graphRepository.AssignAppToGroupAsync(appId, requiredGroupId, InstallIntent.Required);

        // Assign to available group with Available intent
        await graphRepository.AssignAppToGroupAsync(appId, availableGroupId, InstallIntent.Available);

        // Assign to uninstall group with Uninstall intent
        await graphRepository.AssignAppToGroupAsync(appId, uninstallGroupId, InstallIntent.Uninstall);
    }


    private async Task<string> GetAppIdByName(string appName)
    {
        var apps = await graphRepository.GetAppsAsync();
        var app = apps.FirstOrDefault(a => a.DisplayName.Equals(appName, StringComparison.OrdinalIgnoreCase));
        return app?.Id;
    }

    public async Task<IEnumerable<Group>> GetGroupsAsync()
    {
        return await graphRepository.GetGroupsAsync();
    }

    public async Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart)
    {
        return await graphRepository.SearchGroupsByNameAsync(namePart);
    }

    public async Task<IEnumerable<MobileApp>> GetAppsAsync()
    {
        return await graphRepository.GetAppsAsync();
    }

    public async Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId)
    {
        return await graphRepository.GetAppAssignmentsAsync(appId);
    }

    public async Task AssignAppToGroupAsync(string appId, string groupId, InstallIntent intent)
    {
        await graphRepository.AssignAppToGroupAsync(appId, groupId, intent);
    }

    public async Task RemoveAppAssignmentAsync(string appId, string assignmentId)
    {
        await graphRepository.RemoveAppAssignmentAsync(appId, assignmentId);
    }

    public async Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync()
    {
        return await graphRepository.GetWindowsAppsAsync();
    }
}