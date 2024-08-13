using Microsoft.Graph.Models;

namespace Repositories.Interfaces;

public interface IAppRepository
{
    Task<string> GetAppIdByNameAsync(string appName);

    Task<IEnumerable<MobileApp>> GetAppsAsync();

    Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId);
    
    Task AssignAppToGroupAsync(string appId, string groupId, InstallIntent intent, string exclusionRule = "");

    Task RemoveAppFromGroupAsync(string appId, string groupId);

    Task RemoveAppAssignmentAsync(string appId, string assignmentId);

    Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync();

}