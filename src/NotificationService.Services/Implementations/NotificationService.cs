using NotificationService.InternalModels.DTOs;
using NotificationService.InternalModels.Entities;
using NotificationService.Repository;

namespace NotificationService.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NotificationDto>> GetAllAsync(string? status)
    {
        IReadOnlyCollection<NotificationEntity> notifications;

        if (!string.IsNullOrWhiteSpace(status))
        {
            notifications = await _repository.GetByStatusAsync(status);
        }
        else
        {
            notifications = await _repository.GetAllAsync();
        }

        return notifications.Select(NotificationDto.FromEntity);
    }

    public async Task<NotificationDto?> GetByIdAsync(int id)
    {
        var notification = await _repository.GetByIdAsync(id);
        return notification is not null ? NotificationDto.FromEntity(notification) : null;
    }

    public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
    {
        var notification = new NotificationEntity
        {
            Recipient = dto.Recipient,
            Channel = dto.Channel,
            Subject = dto.Subject,
            Message = dto.Message,
            Status = "Queued",
            CreatedAt = DateTime.UtcNow,
            SentAt = null,
            FailureReason = string.Empty
        };

        var created = await _repository.CreateAsync(notification);
        return NotificationDto.FromEntity(created);
    }

    public async Task<NotificationDto?> SendAsync(int id)
    {
        var notification = await _repository.GetByIdAsync(id);
        if (notification is null)
        {
            return null;
        }

        notification.Status = "Sent";
        notification.SentAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(notification);
        return updated ? NotificationDto.FromEntity(notification) : null;
    }

    public async Task<NotificationDto?> MarkFailedAsync(int id, string reason)
    {
        var notification = await _repository.GetByIdAsync(id);
        if (notification is null)
        {
            return null;
        }

        notification.Status = "Failed";
        notification.FailureReason = reason;

        var updated = await _repository.UpdateAsync(notification);
        return updated ? NotificationDto.FromEntity(notification) : null;
    }
}
