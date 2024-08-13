using Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Repositories.Interfaces;
using GraphClientFactory = Helpers.GraphClientFactory;

namespace Repositories.Implementations;

public class GroupRepository(IOptions<Configuration> config) : IGroupRepository
{
    private readonly GraphServiceClient _graphClient = GraphClientFactory.CreateGraphClient(config.Value);

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

    public async Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart)
    {
        var groups = await _graphClient.Groups.GetAsync();
        return groups.Value.Where(g => g.DisplayName.Contains(namePart, StringComparison.OrdinalIgnoreCase));
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
}