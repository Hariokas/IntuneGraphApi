﻿using Microsoft.Graph.Models;

namespace Services.Interfaces;

public interface IGraphService
{
    Task<IEnumerable<Group>> GetGroupsAsync();
    Task<IEnumerable<MobileApp>> GetAppsAsync();
    Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId);
    Task AssignAppToGroupAsync(string appId, string groupId);
    Task RemoveAppAssignmentAsync(string appId, string assignmentId);
    Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync();
}