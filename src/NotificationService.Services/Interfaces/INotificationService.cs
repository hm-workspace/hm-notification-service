using NotificationService.InternalModels.DTOs;

namespace NotificationService.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetAllAsync(string? status);
    Task<NotificationDto?> GetByIdAsync(int id);
    Task<NotificationDto> CreateAsync(CreateNotificationDto dto);
    Task<NotificationDto?> SendAsync(int id);
    Task<NotificationDto?> MarkFailedAsync(int id, string reason);
}
