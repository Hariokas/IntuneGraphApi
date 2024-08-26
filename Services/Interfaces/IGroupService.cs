using Microsoft.Graph.Models;

namespace Services.Interfaces;

public interface IGroupService
{
    Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description);
    
    Task CreateAppGroupsAsync(string appName);

    Task DeployAppToGroupAsync(string appName, string requiredGroupId, string availableGroupId,
        string uninstallGroupId);

    Task DeployAppToGroupAsync(string appName);
    
    Task<IEnumerable<Group>> GetGroupsAsync();

    Task<Group> GetGroupByIdAsync(string groupId);

    Task<IEnumerable<Group>> GetGroupsByIdsAsync(IEnumerable<string> groupIds);

    Task<IEnumerable<Device>> GetDevicesInGroupAsync(string groupId);

    Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart);
}