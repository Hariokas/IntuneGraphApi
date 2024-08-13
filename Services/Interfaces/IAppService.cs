using Microsoft.Graph.Models;

namespace Services.Interfaces;

public interface IAppService
{
    Task<IEnumerable<MobileApp>> GetAppsAsync();

    Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync();

    Task<string> GetAppIdByName(string appName);

    Task<Win32LobApp> GetAppById(string appId);

    Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId);

    Task AssignAppToGroupAsync(string appId, string groupId, InstallIntent intent);

    Task RemoveAppAssignmentAsync(string appId, string assignmentId);
}