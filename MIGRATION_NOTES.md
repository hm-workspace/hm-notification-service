# Database Migration - In-Memory to SQL Server

## Overview
The Notification Service has been migrated from an in-memory data store to a SQL Server database with a proper layered architecture.

## Architecture Changes

### Previous Architecture
- **Controller** ? **In-Memory DataStore** (static list)

### New Architecture
- **Controller** ? **Service Layer** ? **Repository Layer** ? **Database**

## Components

### 1. Controller Layer (`NotificationService.Api`)
- `NotificationsController` - Handles HTTP requests and responses
- Now depends on `INotificationService` instead of the static `NotificationStore`
- All methods are now async

### 2. Service Layer (`NotificationService.Services`)
- `INotificationService` - Service interface
- `NotificationService` - Business logic implementation
- Handles business rules and coordinates repository calls

### 3. Repository Layer (`NotificationService.Repository`)
- `INotificationRepository` - Repository interface with CRUD operations
- `NotificationRepository` - Data access implementation using Dapper
- Methods:
  - `GetAllAsync()` - Retrieve all notifications
  - `GetByStatusAsync(status)` - Filter notifications by status
  - `GetByIdAsync(id)` - Get a specific notification
  - `CreateAsync(entity)` - Create a new notification
  - `UpdateAsync(entity)` - Update an existing notification

### 4. Data Layer (`NotificationService.Data`)
- `IDbConnectionFactory` - Connection factory interface
- `SqlConnectionFactory` - SQL Server connection factory

## Database Setup

### 1. Connection String Configuration
Update your `appsettings.json` or `appsettings.Development.json` with the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=HospitalManagement;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 2. Create Database Table
Run the SQL script located at `database/setup.sql` to create the `Notifications` table with the following schema:

```sql
Notifications
- Id (INT, PRIMARY KEY, IDENTITY)
- Recipient (NVARCHAR(255))
- Channel (NVARCHAR(50))
- Subject (NVARCHAR(500))
- Message (NVARCHAR(MAX))
- Status (NVARCHAR(50))
- CreatedAt (DATETIME2)
- SentAt (DATETIME2, NULL)
- FailureReason (NVARCHAR(MAX))
```

## Dependency Injection

The following services are registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
```

## API Endpoints

All endpoints remain the same but now use async/await pattern:

- `GET /api/notifications` - Get all notifications (optional ?status query parameter)
- `GET /api/notifications/{id}` - Get notification by ID
- `POST /api/notifications` - Create a new notification
- `POST /api/notifications/{id}/send` - Mark notification as sent
- `POST /api/notifications/{id}/fail` - Mark notification as failed

## Benefits of This Architecture

1. **Separation of Concerns** - Each layer has a single responsibility
2. **Testability** - Easy to mock interfaces for unit testing
3. **Maintainability** - Changes to data access don't affect business logic
4. **Scalability** - Can easily swap out SQL Server for another database
5. **Data Persistence** - Data survives application restarts
6. **Thread Safety** - No more shared static state

## Migration Notes

- Removed `DataStore.cs` (static in-memory store)
- All controller methods are now async
- ID generation is now handled by the database (IDENTITY column)
- Error handling is implemented at the service layer
