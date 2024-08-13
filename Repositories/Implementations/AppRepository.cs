using Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Repositories.Interfaces;
using GraphClientFactory = Helpers.GraphClientFactory;

namespace Repositories.Implementations;

public class AppRepository(IOptions<Configuration> config, IGroupRepository groupRepository) : IAppRepository
{
    private readonly GraphServiceClient _graphClient = GraphClientFactory.CreateGraphClient(config.Value);

    public async Task<string> GetAppIdByNameAsync(string appName)
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps
            .GetAsync(config => config.QueryParameters.Filter = $"displayName eq '{appName}'");

        return apps.Value.FirstOrDefault()?.Id;
    }

    public async Task<IEnumerable<MobileApp>> GetAppsAsync()
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps.GetAsync();
        return apps.Value;
    }

    public async Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId)
    {
        var assignments = await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments.GetAsync();
        return assignments.Value;
    }

    public async Task AssignAppToGroupAsync(string appId, string groupId, InstallIntent intent, string exclusionRule = "")
    {
        // Create the assignment
        var assignment = new MobileAppAssignment
        {
            Target = new GroupAssignmentTarget { GroupId = groupId },
            Intent = intent
        };

        await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments.PostAsync(assignment);

        // If exclusion rule is provided, update the group's membership rule
        if (!string.IsNullOrEmpty(exclusionRule))
        {
            await groupRepository.UpdateGroupMembershipRuleAsync(groupId, exclusionRule);
        }
    }

    public async Task RemoveAppFromGroupAsync(string appId, string groupId)
    {
        var assignments = await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments
            .GetAsync(config => config.QueryParameters.Filter = $"target/groupId eq '{groupId}'");

        foreach (var assignment in assignments.Value)
        {
            await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments[assignment.Id].DeleteAsync();
        }
    }

    public async Task RemoveAppAssignmentAsync(string appId, string assignmentId)
    {
        await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments[assignmentId].DeleteAsync();
    }

    public async Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync()
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps.GetAsync();
        var windowsApps = apps.Value.OfType<Win32LobApp>().ToList();
        return windowsApps;
    }
}