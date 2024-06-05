# IntuneGraphApi

IntuneGraphApi is a .NET Core Web API project designed to interact with Microsoft Graph API for Intune. This API allows you to manage Intune resources such as groups, apps, and app assignments using various endpoints.

## Features

- List all Azure AD groups.
- List all Intune mobile apps.
- List all Windows Line-of-Business (LOB) apps.
- Retrieve app assignments for a specific app.
- Assign an app to a group.
- Remove an app assignment from a group.

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

## Setup

1. **Clone the repository:**

    ```bash
    git clone https://github.com/Hariokas/IntuneGraphApi.git
    ```

2. **Navigate to the project directory:**

    ```bash
    cd IntuneGraphApi
    ```

3. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

4. **Update `appsettings.json` with your Azure AD and Intune details:**

    ```json
    {
        "GraphApi": {
            "TenantId": "YOUR_TENANT_ID",
            "ClientId": "YOUR_CLIENT_ID",
            "ClientSecret": "YOUR_CLIENT_SECRET"
        }
    }
    ```

5. **Run the application:**

    ```bash
    dotnet run --project IntuneGraphApi.Api
    ```

## Usage

You can test the API endpoints using tools like Postman or your browser. For example, to get a list of all groups, navigate to: https://localhost:5001/api/graph/groups


## References

- [Microsoft Graph API for Intune: Overview](https://learn.microsoft.com/en-us/graph/api/resources/intune-graph-overview?view=graph-rest-1.0)
- [Win32LobAppAssignmentSettings](https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-win32lobappassignmentsettings?view=graph-rest-1.0)
- [MobileAppAssignment](https://learn.microsoft.com/en-us/graph/api/resources/intune-apps-mobileappassignment?view=graph-rest-1.0)
- [Assignment Target](https://learn.microsoft.com/en-us/graph/api/resources/intune-shared-assignmenttarget?view=graph-rest-1.0)
  - [GroupAssignmentTarget](https://learn.microsoft.com/en-us/graph/api/resources/intune-shared-groupassignmenttarget?view=graph-rest-1.0)
  - [AllDevicesAssignmentTarget](https://learn.microsoft.com/en-us/graph/api/resources/intune-shared-alldevicesassignmenttarget?view=graph-rest-1.0)
