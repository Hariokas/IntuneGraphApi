using Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
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

    public async Task<IEnumerable<Group>> GetGroupsAsync()
    {
        var groups = await _graphClient.Groups.GetAsync();
        return groups.Value;
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

    public async Task AssignAppToGroupAsync(string appId, string groupId)
    {
        var assignment = new MobileAppAssignment
        {
            Target = new GroupAssignmentTarget { GroupId = groupId },
            Intent = InstallIntent.Required
        };

        await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments.PostAsync(assignment);
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