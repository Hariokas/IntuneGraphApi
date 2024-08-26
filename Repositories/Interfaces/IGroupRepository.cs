using Microsoft.Graph.Models;

namespace Repositories.Interfaces;

public interface IGroupRepository
{
    Task<Group> CreateGroupAsync(string displayName, string mailNickname, string description, bool mailEnabled = false,
        bool securityEnabled = true, List<string>? groupTypes = null);

    Task<IEnumerable<Group>> GetGroupsAsync();

    Task<string> GetGroupIdByNameAsync(string groupName);

    Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart);

    Task UpdateGroupMembershipRuleAsync(string groupId, string membershipRule);
}