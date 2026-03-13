using NotificationService.InternalModels.Entities;

namespace NotificationService.Services;

public static class NotificationStore
{
    public static int NotificationSeed = 1;
    public static readonly List<NotificationEntity> Notifications = new()
    {
        new NotificationEntity
        {
            Id = 1,
            Recipient = "john.doe@hm.local",
            Channel = "Email",
            Subject = "Appointment Reminder",
            Message = "Your appointment is scheduled for tomorrow at 10:00.",
            Status = "Sent",
            CreatedAt = DateTime.UtcNow.AddHours(-2),
            SentAt = DateTime.UtcNow.AddHours(-2),
            FailureReason = string.Empty
        }
    };
}


