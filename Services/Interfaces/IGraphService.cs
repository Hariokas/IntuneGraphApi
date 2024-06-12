using Microsoft.Graph.Models;

namespace Services.Interfaces;

public interface IGraphService
{
    Task<IEnumerable<Device>> GetDevicesAsync();
    Task AddDeviceToGroupAsync(string groupId, string deviceId);
    Task RemoveDeviceFromGroupAsync(string groupId, string deviceId);
    Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description);
    Task CreateAppGroupsAsync(string appName);

    Task DeployAppToGroupAsync(string appName, string requiredGroupId, string availableGroupId,
        string uninstallGroupId);

    Task DeployAppToGroupAsync(string appName);
    Task<IEnumerable<Group>> GetGroupsAsync();
    Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart);
    Task<IEnumerable<MobileApp>> GetAppsAsync();
    Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId);
    Task AssignAppToGroupAsync(string appId, string groupId, InstallIntent intent);
    Task RemoveAppAssignmentAsync(string appId, string assignmentId);
    Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync();
}