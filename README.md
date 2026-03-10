# Task Management API

.NET 8 Web API for managing tasks with different types (Procurement, Development). Each type has its own status workflow and required data fields.

## Prerequisites

- .NET 8 SDK
- SQL Server (local or remote)

## Setup

1. Configure your connection string in `src/TaskManagement.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskManagement;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

2. Run the API:

```bash
dotnet run --project src/TaskManagement.API
```

The database and tables are created automatically on first run. Three test users are seeded.

Swagger UI is available at `https://localhost:{port}/swagger`.

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/users` | List all users |
| `POST` | `/api/tasks` | Create a new task |
| `PATCH` | `/api/tasks/{id}/status` | Advance task status |
| `POST` | `/api/tasks/{id}/close` | Close a task (must be at final status) |
| `GET` | `/api/tasks/assigned-to/{userId}` | Get tasks assigned to a user |

## Task Types

| Type | Value | Statuses |
|------|-------|----------|
| Procurement | `1` | 1 (Created) -> 2 (Offers Received) -> 3 (Purchased) |
| Development | `2` | 1 (Created) -> 2 (Spec Done) -> 3 (Dev Done) -> 4 (Distributed) |

Each status transition requires specific data fields in the `statusData` dictionary (e.g. price quotes for procurement status 2, branch name for dev status 3).

## Adding a New Task Type

The architecture follows the Open/Closed Principle — adding a new task type requires **no changes to existing code**. Type-specific data is stored as a single JSON column (`TypeData`) on the task entity, and handlers are auto-discovered via assembly scanning.

To add a new task type:

1. Add a value to the `TaskType` enum (e.g. `Support = 3`).
2. Create a handler class that extends `TaskTypeHandlerBase` and implements `ITaskTypeHandler` — define the status flow, field validation, and how data is applied.
3. Run `dotnet ef migrations add <Name>` (only if the enum storage needs updating; no schema change is needed for the type-specific data itself since it lives in the JSON column).

That's it. No changes to `TaskEntity`, `AppDbContext`, `TaskRepository`, DI registration, or any existing handler.
