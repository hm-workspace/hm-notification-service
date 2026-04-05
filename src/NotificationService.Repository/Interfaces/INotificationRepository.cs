using NotificationService.InternalModels.Entities;

namespace NotificationService.Repository;

public interface INotificationRepository
{
    Task<IReadOnlyCollection<NotificationEntity>> GetAllAsync();
    Task<IReadOnlyCollection<NotificationEntity>> GetByStatusAsync(string status);
    Task<NotificationEntity?> GetByIdAsync(int id);
    Task<NotificationEntity> CreateAsync(NotificationEntity entity);
    Task<bool> UpdateAsync(NotificationEntity entity);
}

