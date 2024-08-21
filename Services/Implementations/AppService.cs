using Microsoft.Graph.Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implementations;

public class AppService (IAppRepository appRepository) : IAppService
{
    public async Task<IEnumerable<MobileApp>> GetAppsAsync()
    {
        return await appRepository.GetAppsAsync();
    }

    public async Task<IEnumerable<Win32LobApp>> GetWindowsAppsAsync()
    {
        return await appRepository.GetWindowsAppsAsync();
    }

    public async Task<IEnumerable<MobileApp>> GetAppsByName(string appName)
    {
        try
        {
            var allApps = await appRepository.GetAppsAsync();

            var apps = allApps.Where(a =>
                a.DisplayName.Contains(appName, StringComparison.CurrentCultureIgnoreCase));

            return apps;
        }
        catch (Exception ex)
        {
            return [];
        }
    }

    public async Task<Win32LobApp> GetAppById(string appId)
    {
        try
        {
            var app = await appRepository.GetAppById(appId);
            return app;
        }
        catch
        {
            return null;
        }
    }

    public async Task<IEnumerable<MobileAppAssignment>> GetAppAssignmentsAsync(string appId)
    {
        return await appRepository.GetAppAssignmentsAsync(appId);
    }

    public async Task AssignAppToGroupAsync(string appId, string groupId, InstallIntent intent)
    {
        await appRepository.AssignAppToGroupAsync(appId, groupId, intent);
    }

    public async Task RemoveAppAssignmentAsync(string appId, string assignmentId)
    {
        await appRepository.RemoveAppAssignmentAsync(appId, assignmentId);
    }
}