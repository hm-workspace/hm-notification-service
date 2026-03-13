using NotificationService.InternalModels.Entities;

namespace NotificationService.InternalModels.DTOs;

public class CreateNotificationDto
{
    public string Recipient { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class FailureReasonDto
{
    public string Reason { get; set; } = string.Empty;
}

public class NotificationDto : CreateNotificationDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public string FailureReason { get; set; } = string.Empty;

    public static NotificationDto FromEntity(NotificationEntity entity) => new()
    {
        Id = entity.Id,
        Recipient = entity.Recipient,
        Channel = entity.Channel,
        Subject = entity.Subject,
        Message = entity.Message,
        Status = entity.Status,
        CreatedAt = entity.CreatedAt,
        SentAt = entity.SentAt,
        FailureReason = entity.FailureReason
    };
}


