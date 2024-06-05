using Microsoft.Graph.Models;

namespace Repositories.Interfaces;

public interface IGraphRepository
{
    Task<IEnumerable<Group>> GetGroupsAsync();
    Task<IEnumerable<MobileApp>> GetAppsAsync();
    Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId);
    Task AssignAppToGroupAsync(string appId, string groupId);
    Task RemoveAppAssignmentAsync(string appId, string assignmentId);
    Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync();
}