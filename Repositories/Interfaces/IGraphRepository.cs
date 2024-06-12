using Microsoft.Graph.Models;

namespace Repositories.Interfaces;

public interface IGraphRepository
{
    Task<IEnumerable<Device>> GetDevicesAsync();
    Task AddDeviceToGroupAsync(string groupId, string deviceId);
    Task RemoveDeviceFromGroupAsync(string groupId, string deviceId);
    Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description);
    Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description, bool mailEnabled,
        bool securityEnabled, List<string>? groupTypes = null);
    Task<IEnumerable<Group>> GetGroupsAsync();
    Task<string> GetGroupIdByNameAsync(string groupName);
    Task<string> GetAppIdByNameAsync(string appName);
    Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart);
    Task<IEnumerable<MobileApp>> GetAppsAsync();
    Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId);
    Task AssignAppToGroupAsync(string appId, string groupId, InstallIntent intent, string exclusionRule = "");
    Task RemoveAppFromGroupAsync(string appId, string groupId);
    Task RemoveAppAssignmentAsync(string appId, string assignmentId);
    Task UpdateGroupMembershipRuleAsync(string groupId, string membershipRule);
    Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync();
}