using Dapper;
using NotificationService.Data;
using NotificationService.InternalModels.Entities;
using System.Data;

namespace NotificationService.Repository;

public class NotificationRepository : BaseRepository, INotificationRepository
{
    public NotificationRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<IReadOnlyCollection<NotificationEntity>> GetAllAsync()
    {
        var rows = await QueryAsync<NotificationEntity>(
            StoredProcedureNames.GetNotifications,
            commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    public async Task<IReadOnlyCollection<NotificationEntity>> GetByStatusAsync(string status)
    {
        var rows = await QueryAsync<NotificationEntity>(
            StoredProcedureNames.GetNotificationsByStatus,
            new { Status = status },
            commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    public Task<NotificationEntity?> GetByIdAsync(int id)
    {
        return QuerySingleOrDefaultAsync<NotificationEntity>(
            StoredProcedureNames.GetNotificationById,
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<NotificationEntity> CreateAsync(NotificationEntity entity)
    {
        var id = await ExecuteScalarAsync<int>(
            StoredProcedureNames.CreateNotification,
            entity,
            commandType: CommandType.StoredProcedure);
        entity.Id = id;
        return entity;
    }

    public async Task<bool> UpdateAsync(NotificationEntity entity)
    {
        var rowsAffected = await ExecuteAsync(
            StoredProcedureNames.UpdateNotification,
            entity,
            commandType: CommandType.StoredProcedure);
        return rowsAffected > 0;
    }
}


