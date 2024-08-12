using Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Serialization.Form;
using Repositories.Interfaces;
using GraphClientFactory = Helpers.GraphClientFactory;

namespace Repositories.Implementations;

public class GraphRepository : IGraphRepository
{
    private readonly GraphServiceClient _graphClient;

    public GraphRepository(IOptions<Configuration> config)
    {
        var settings = config.Value;
        _graphClient = GraphClientFactory.CreateGraphClient(settings);
    }

    public async Task<IEnumerable<Device>> GetDevicesAsync()
    {
        var devices = await _graphClient.Devices.GetAsync();
        return devices.Value;
    }


    public async Task AddDeviceToGroupAsync(string groupId, string deviceId)
    {
        var referenceCreate = new ReferenceCreate
        {
            OdataId = $"https://graph.microsoft.com/v1.0/directoryObjects/{deviceId}"
        };

        await _graphClient.Groups[groupId].Members.Ref.PostAsync(referenceCreate);
    }

    public async Task RemoveDeviceFromGroupAsync(string groupId, string deviceId)
    {
        await _graphClient.Groups[groupId].Members[deviceId].Ref.DeleteAsync();
    }

    public async Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description)
    {
        var group = new Group
        {
            DisplayName = displayName,
            MailNickname = mailNickname,
            Description = description,
            GroupTypes = new List<string> { "Unified" },
            MailEnabled = true,
            SecurityEnabled = false
        };

        return await _graphClient.Groups.PostAsync(group);
    }

    public async Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description, bool mailEnabled, bool securityEnabled, List<string>? groupTypes = null)
    {
        var group = new Group
        {
            DisplayName = displayName,
            MailNickname = mailNickname,
            Description = description,
            MailEnabled = mailEnabled,
            SecurityEnabled = securityEnabled,
            GroupTypes = groupTypes ?? []
        };

        return await _graphClient.Groups.PostAsync(group);
    }

    public async Task<IEnumerable<Group>> GetGroupsAsync()
    {
        var groups = await _graphClient.Groups.GetAsync();
        return groups.Value;
    }

    public async Task<string> GetGroupIdByNameAsync(string groupName)
    {
        var groups = await _graphClient.Groups
            .GetAsync(config => config.QueryParameters.Filter = $"displayName eq '{groupName}'");

        return groups.Value.FirstOrDefault()?.Id;
    }

    public async Task<string> GetAppIdByNameAsync(string appName)
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps
            .GetAsync(config => config.QueryParameters.Filter = $"displayName eq '{appName}'");

        return apps.Value.FirstOrDefault()?.Id;
    }

    public async Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart)
    {
        var groups = await _graphClient.Groups.GetAsync();
        return groups.Value.Where(g => g.DisplayName.Contains(namePart, StringComparison.OrdinalIgnoreCase));
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
            await UpdateGroupMembershipRuleAsync(groupId, exclusionRule);
        }
    }

    public async Task UpdateGroupMembershipRuleAsync(string groupId, string membershipRule)
    {
        var group = new Group
        {
            MembershipRule = membershipRule,
            MembershipRuleProcessingState = "On"
        };

        await _graphClient.Groups[groupId].PatchAsync(group);
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