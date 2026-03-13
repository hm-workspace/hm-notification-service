using NotificationService.InternalModels.Entities;

namespace NotificationService.Repository;

public interface INotificationRepository
{
    Task<IReadOnlyCollection<NotificationEntity>> GetAllAsync();
}

