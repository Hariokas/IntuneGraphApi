using Microsoft.Graph.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implementations;

public class GraphService(IGraphRepository repository) : IGraphService
{
    public Task AddDeviceToGroupAsync(string groupId, string deviceId)
    {
        return repository.AddDeviceToGroupAsync(groupId, deviceId);
    }

    public Task RemoveDeviceFromGroupAsync(string groupId, string deviceId)
    {
        return repository.RemoveDeviceFromGroupAsync(groupId, deviceId);
    }

    public Task<IEnumerable<Group>> GetGroupsAsync()
    {
        return repository.GetGroupsAsync();
    }

    public Task<IEnumerable<MobileApp>> GetAppsAsync()
    {
        return repository.GetAppsAsync();
    }

    public Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId)
    {
        return repository.GetAppAssignmentsAsync(appId);
    }

    public Task AssignAppToGroupAsync(string appId, string groupId)
    {
        return repository.AssignAppToGroupAsync(appId, groupId);
    }

    public Task RemoveAppAssignmentAsync(string appId, string assignmentId)
    {
        return repository.RemoveAppAssignmentAsync(appId, assignmentId);
    }

    public Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync()
    {
        return repository.GetWindowsAppsAsync();
    }
}