namespace NotificationService.Repository;

public static class StoredProcedureNames
{
    public const string GetNotifications = "dbo.GetNotifications";
    public const string GetNotificationsByStatus = "dbo.GetNotificationsByStatus";
    public const string GetNotificationById = "dbo.GetNotificationById";
    public const string CreateNotification = "dbo.CreateNotification";
    public const string UpdateNotification = "dbo.UpdateNotification";
}
