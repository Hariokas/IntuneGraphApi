﻿using Microsoft.Graph;
using Microsoft.Graph.Models;
using Repositories.Interfaces;
using Serilog;

namespace Repositories.Implementations;

public class AppRepository(IGroupRepository groupRepository, IGraphClientFactory graphClientFactory) : IAppRepository
{
    private readonly GraphServiceClient _graphClient = graphClientFactory.CreateGraphClient();

    public async Task<string> GetAppIdByNameAsync(string appName)
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps
            .GetAsync(config => config.QueryParameters.Filter = $"displayName eq '{appName}'");

        return apps?.Value?.FirstOrDefault()?.Id ?? "";
    }

    public async Task<IEnumerable<MobileApp>> GetAppsByNameAsync(string appName)
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps.GetAsync(config =>
        {
            config.QueryParameters.Filter = $"contains(tolower(displayName), tolower('{appName}'))";
        });

        return apps?.Value ?? Enumerable.Empty<MobileApp>();
    }

    public async Task<Win32LobApp> GetAppById(string appId)
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps.GetAsync();
        var windowsApps = apps?.Value?.OfType<Win32LobApp>().ToList().FirstOrDefault(a => a.Id == appId);

        return windowsApps ?? new Win32LobApp();
    }

    public async Task<IEnumerable<MobileApp>> GetAppsAsync()
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps.GetAsync();
        return apps?.Value ?? [];
    }

    public async Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId)
    {
        var assignments = await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments.GetAsync();
        return assignments?.Value ?? [];
    }

    public async Task AssignAppToGroupAsync(string appId, string groupId, InstallIntent intent, string exclusionRule = "")
    {
        // Create the assignment
        var assignment = new MobileAppAssignment
        {
            Target = new GroupAssignmentTarget { GroupId = groupId },
            Intent = intent
        };

        await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments.PostAsync(assignment);

        // If exclusion rule is provided, update the group's membership rule
        if (!string.IsNullOrEmpty(exclusionRule))
        {
            await groupRepository.UpdateGroupMembershipRuleAsync(groupId, exclusionRule);
        }
    }

    public async Task RemoveAppFromGroupAsync(string appId, string groupId)
    {
        var assignments = await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments
            .GetAsync(config => config.QueryParameters.Filter = $"target/groupId eq '{groupId}'");

        foreach (var assignment in assignments?.Value ?? [])
        {
            await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments[assignment.Id].DeleteAsync();
        }
    }

    public async Task RemoveAppAssignmentAsync(string appId, string assignmentId)
    {
        await _graphClient.DeviceAppManagement.MobileApps[appId].Assignments[assignmentId].DeleteAsync();
    }

    public async Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync()
    {
        var apps = await _graphClient.DeviceAppManagement.MobileApps.GetAsync();
        var windowsApps = apps.Value.OfType<Win32LobApp>().ToList();
        return windowsApps;
    }

    public async Task<IEnumerable<MobileApp>> GetAppsAssignedToGroupsAsync(IEnumerable<string> groupIds)
    {
        var allApps = new List<MobileApp>();

        var apps = await _graphClient.DeviceAppManagement.MobileApps.GetAsync();
        var mobileApps = apps?.Value ?? Enumerable.Empty<MobileApp>();

        foreach (var app in mobileApps)
        {
            var assignments = await _graphClient.DeviceAppManagement.MobileApps[app.Id].Assignments.GetAsync();

            var isAssigned = assignments?.Value?.Any(a =>
            a.Target is GroupAssignmentTarget groupTarget &&
            groupIds.Contains(groupTarget.GroupId)) ?? false;

            if (isAssigned)
                allApps.Add(app);
        }

        return allApps;
    }

}