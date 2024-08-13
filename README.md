# IntuneGraphApi

IntuneGraphApi is a .NET Core Web API project designed to interact with Microsoft Graph API for Intune. This API allows you to manage Intune resources such as groups, apps, and app assignments using various endpoints.

## Features

- List all Azure AD groups.
- List all Intune mobile apps.
- List all Windows Line-of-Business (LOB) apps.
- Retrieve app assignments for a specific app.
- Assign an app to a group.
- Remove an app assignment from a group.
- Add a device to a group.
- Remove a device from a group.

## Endpoints

### Get Groups

- **URL:** `GET /api/graph/groups`
- **Description:** Retrieves a list of all Azure AD groups.
- **Documentation:** [Microsoft Graph: List groups](https://learn.microsoft.com/en-us/graph/api/group-list?view=graph-rest-1.0)

### Get Apps

- **URL:** `GET /api/graph/apps`
- **Description:** Retrieves a list of all Intune mobile apps.
- **Documentation:** [Microsoft Graph: List mobile apps](https://learn.microsoft.com/en-us/graph/api/intune-apps-mobileapp-list?view=graph-rest-1.0)

### Get Windows Apps

- **URL:** `GET /api/graph/apps/windows`
- **Description:** Retrieves a list of all Windows Line-of-Business (LOB) apps.
- **Documentation:** [Microsoft Graph: Win32LobApp resource type](https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-win32lobapp?view=graph-rest-1.0)

### Get App Assignments

- **URL:** `GET /api/graph/apps/{appId}/assignments`
- **Description:** Retrieves a list of assignments for a specific app.
- **Documentation:** [Microsoft Graph: List mobile app assignments](https://learn.microsoft.com/en-us/graph/api/intune-apps-mobileappassignment-list?view=graph-rest-1.0)

### Assign App to Group

- **URL:** `POST /api/graph/apps/{appId}/assignments`
- **Description:** Assigns an app to a specified group.
- **Documentation:** [Microsoft Graph: Create mobile app assignment](https://learn.microsoft.com/en-us/graph/api/intune-apps-mobileappassignment-create?view=graph-rest-1.0)

### Remove App Assignment

- **URL:** `DELETE /api/graph/apps/{appId}/assignments/{assignmentId}`
- **Description:** Removes an app assignment from a specified group.
- **Documentation:** [Microsoft Graph: Delete mobile app assignment](https://learn.microsoft.com/en-us/graph/api/intune-apps-mobileappassignment-delete?view=graph-rest-1.0)

### Add Device to Group

- **URL:** `POST /api/graph/groups/{groupId}/devices/{deviceId}`
- **Description:** Adds a device to a specified group.
- **Documentation:** [Microsoft Graph: Add member to group](https://learn.microsoft.com/en-us/graph/api/group-post-members?view=graph-rest-1.0&tabs=http)

### Remove Device from Group

- **URL:** `DELETE /api/graph/groups/{groupId}/devices/{deviceId}`
- **Description:** Removes a device from a specified group.
- **Documentation:** [Microsoft Graph: Remove member from group](https://learn.microsoft.com/en-us/graph/api/group-delete-members?view=graph-rest-1.0&tabs=http)

## Setup

1. **Clone the repository:**

    ```bash
    git clone https://azuredevops.danskenet.net/Main/Application%20Integration/_git/AppZone
    ```

2. **Navigate to the project directory:**

    ```bash
    cd IntuneGraphApi
    ```

3. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

4. **Update `appsettings.json` with your Azure AD details (excluding the ClientSecret):**

    ```json
    {
        "GraphApi": {
            "TenantId": "YOUR_TENANT_ID",
            "ClientId": "YOUR_CLIENT_ID"
        }
    }
    ```

5. **Add the ClientSecret to user secrets:**

    In Visual Studio, right-click on the project in Solution Explorer and select "Manage User Secrets". Add the following to the `secrets.json` file:

    ```json
    {
        "GraphApi": {
            "ClientSecret": "YOUR_CLIENT_SECRET"
        }
    }
    ```

    Alternatively, use the .NET CLI:

    ```bash
    dotnet user-secrets init
    dotnet user-secrets set "GraphApi:ClientSecret" "YOUR_CLIENT_SECRET"
    ```

6. **Run the application:**

    ```bash
    dotnet run --project IntuneGraphApi.Api
    ```

## Required API Permissions

To interact with Microsoft Graph API for Intune, you need to configure your Azure AD application with the necessary permissions. Below are the required permissions:

<details>
<summary><b>Group Read All</b></summary>

**Permission Name:** `Group.Read.All`

**Description:** Allows the app to read group memberships and properties without a signed-in user.

**Why It Is Required:** This permission is necessary for the application to retrieve information about all Azure AD groups, including their memberships and properties. This functionality is essential for listing groups and their details in the application.

**What It Does:** `Group.Read.All` allows the application to:
- Retrieve information about all groups in the directory.
- Read the membership details of each group.
- Access properties such as group name, description, and group members.

**Other Uses:**
- Fetching all groups to display them in an administrative dashboard.
- Auditing group memberships and properties.
- Building custom reporting tools that involve group data.

</details>

<details>
<summary><b>Device Management Apps Read Write All</b></summary>

**Permission Name:** `DeviceManagementApps.ReadWrite.All`

**Description:** Allows the app to read and write Intune-managed apps and their assignments.

**Why It Is Required:** This permission is essential for managing Intune applications. It enables the application to list, read details, and assign/unassign apps to groups.

**What It Does:** `DeviceManagementApps.ReadWrite.All` allows the application to:
- Retrieve a list of all Intune-managed apps.
- Read detailed information about each app.
- Create, update, and delete app assignments to groups.
- Manage properties and settings of the managed apps.

**Other Uses:**
- Automating the deployment of applications across the organization.
- Managing application settings and configurations programmatically.
- Integrating with other systems to automate workflows involving app assignments.

</details>

<details>
<summary><b>Device Management Managed Devices Read Write All</b></summary>

**Permission Name:** `DeviceManagementManagedDevices.ReadWrite.All`

**Description:** Allows the app to read and write the properties of managed devices.

**Why It Is Required:** This permission is necessary for managing the devices that are enrolled in Intune. It enables the application to read and modify device properties, which is crucial for tasks like assigning devices to groups or managing device compliance.

**What It Does:** `DeviceManagementManagedDevices.ReadWrite.All` allows the application to:
- Retrieve information about all managed devices.
- Read and update device properties such as compliance status, device health, and configuration.
- Perform actions on devices, like remote wipe or reboot.

**Other Uses:**
- Automating device compliance checks and remediation.
- Integrating with monitoring tools to track device health and status.
- Implementing custom device management solutions tailored to organizational needs.

</details>

<details>
<summary><b>Device Management Configuration Read Write All</b></summary>

**Permission Name:** `DeviceManagementConfiguration.ReadWrite.All`

**Description:** Allows the app to read and write Intune configuration policies and settings.

**Why It Is Required:** This permission is necessary for managing configuration policies in Intune. It enables the application to create, read, update, and delete policies that configure devices and apps.

**What It Does:** `DeviceManagementConfiguration.ReadWrite.All` allows the application to:
- Retrieve a list of all configuration policies.
- Read detailed information about each policy.
- Create, update, and delete configuration policies.
- Manage settings related to device compliance, security, and app configurations.

**Other Uses:**
- Automating the deployment and management of configuration policies.
- Ensuring consistent policy application across the organization.
- Integrating with other IT management tools to provide a unified management experience.

</details>

<details>
<summary><b>Group Read Write All</b></summary>

**Permission Name:** `Group.ReadWrite.All`

**Description:** Allows the app to read and write group memberships and properties.

**Why It Is Required:** This permission is necessary for managing Azure AD groups. It enables the application to read group information and modify group memberships, which is crucial for tasks like adding or removing devices from groups.

**What It Does:** `Group.ReadWrite.All` allows the application to:
- Retrieve information about all groups.
- Read and update group properties such as name and description.
- Add or remove members from groups.
- Create and delete groups.

**Other Uses:**
- Automating group membership management based on organizational roles.
- Integrating with HR systems to sync group memberships.
- Managing group-based access control policies programmatically.

</details>

<details>
<summary><b>Directory Read Write All</b></summary>

**Permission Name:** `Directory.ReadWrite.All`

**Description:** Allows the app to read and write directory data.

**Why It Is Required:** This permission is necessary for managing directory objects within Azure AD. It enables the application to read and modify various directory objects, such as users, groups, and devices.

**What It Does:** `Directory.ReadWrite.All` allows the application to:
- Retrieve information about directory objects, including users, groups, and devices.
- Update properties of directory objects.
- Create and delete directory objects.
- Manage directory-related configurations and settings.

**Other Uses:**
- Building custom directory synchronization tools.
- Automating the creation and management of directory objects.
- Integrating with external identity providers or directories for unified management.

</details>


## Usage

You can test the API endpoints using tools like Postman or your browser. For example, to get a list of all groups, navigate to: https://localhost:5001/api/graph/groups


## References

- [Microsoft Graph API for Intune: Overview](https://learn.microsoft.com/en-us/graph/api/resources/intune-graph-overview?view=graph-rest-1.0)
- [Win32LobAppAssignmentSettings](https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-win32lobappassignmentsettings?view=graph-rest-1.0)
- [MobileAppAssignment](https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-mobileappassignment?view=graph-rest-1.0)
- [Assignment Target](https://learn.microsoft.com/en-us/graph/api/resources/intune-shared-assignmenttarget?view=graph-rest-1.0)
  - [GroupAssignmentTarget](https://learn.microsoft.com/en-us/graph/api/resources/intune-shared-groupassignmenttarget?view=graph-rest-1.0)
  - [AllDevicesAssignmentTarget](https://learn.microsoft.com/en-us/graph/api/resources/intune-shared-alldevicesassignmenttarget?view=graph-rest-1.0)
