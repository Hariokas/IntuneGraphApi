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

    public async Task<string> GetAppIdByName(string appName)
    {
        try
        {
            var apps = await appRepository.GetAppsAsync();
            var app = apps.FirstOrDefault(a => a.DisplayName.Contains(appName, StringComparison.OrdinalIgnoreCase));
            return app?.Id ?? "";
        }
        catch
        {
            return "";
        }
    }

    public async Task<Win32LobApp> GetAppById(string appId)
    {
        return null;
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