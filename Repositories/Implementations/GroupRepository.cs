using Microsoft.Graph;
using Microsoft.Graph.Models;
using Repositories.Interfaces;
using Serilog;

namespace Repositories.Implementations;

public class GroupRepository(IGraphClientFactory graphClientFactory) : IGroupRepository
{
    private readonly GraphServiceClient _graphClient = graphClientFactory.CreateGraphClient();

    public async Task<Group> CreateSecurityGroupAsync(string displayName, string mailNickname, string description,
        bool? mailEnabled = false, bool? securityEnabled = true, List<string>? groupTypes = null)
    {
        groupTypes ??= [];

        var group = new Group
        {
            DisplayName = displayName,
            MailNickname = mailNickname,
            Description = description,
            MailEnabled = mailEnabled,
            SecurityEnabled = securityEnabled,
            GroupTypes = groupTypes
        };

        return await _graphClient.Groups.PostAsync(group);
    }

    public async Task<IEnumerable<Group>> GetGroupsAsync()
    {
        var groupList = new List<Group>();

        var groups = await _graphClient.Groups.GetAsync();
        var nextPageLink = groups?.OdataNextLink;

        groupList.AddRange(groups.Value);

        while (!string.IsNullOrEmpty(nextPageLink))
        {
            groups = await _graphClient.Groups.WithUrl(nextPageLink).GetAsync();
            nextPageLink = groups?.OdataNextLink;

            groupList.AddRange(groups.Value);
        }

        return groupList;
    }

    public async Task<Group> GetGroupByIdAsync(string groupId)
    {
        try
        {
            return await _graphClient.Groups[groupId].GetAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to fetch group with id: [{groupId}]");
            return null;
        }
    }

    public async Task<IEnumerable<Group>> GetGroupsByIdsAsync(IEnumerable<string> groupIds)
    {
        var groupList = new List<Group>();

        foreach (var groupId in groupIds)
        {
            var group = await GetGroupByIdAsync(groupId);
            if (group != null)
                groupList.Add(group);
        }

        return groupList;
    }

    public async Task<string> GetGroupIdByNameAsync(string groupName)
    {
        var groups = await _graphClient.Groups
            .GetAsync(config =>
            {
                config.QueryParameters.Filter = $"displayName eq '{groupName}'";
            });

        return groups.Value.FirstOrDefault()?.Id;
    }

    public async Task<IEnumerable<Device>> GetDevicesInGroupAsync(string groupId)
    {
        var devices = await _graphClient.Groups[groupId].Members.GetAsync();
        return devices?.Value?.OfType<Device>() ?? [];
    }

    public async Task<IEnumerable<Group>> SearchGroupsByNameAsync(string namePart)
    {
        var groups = await _graphClient.Groups
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Filter = $"startsWith(displayName, '{namePart}')";
            });

        return groups?.Value ?? Enumerable.Empty<Group>();
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