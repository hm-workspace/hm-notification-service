using Dapper;
using NotificationService.Data;
using NotificationService.InternalModels.Entities;

namespace NotificationService.Repository;

public class NotificationRepository : INotificationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public NotificationRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<NotificationEntity>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Notifications";
        var rows = await connection.QueryAsync<NotificationEntity>(sql);
        return rows.ToList();
    }

    public async Task<IReadOnlyCollection<NotificationEntity>> GetByStatusAsync(string status)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Notifications WHERE Status = @Status";
        var rows = await connection.QueryAsync<NotificationEntity>(sql, new { Status = status });
        return rows.ToList();
    }

    public async Task<NotificationEntity?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Notifications WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<NotificationEntity>(sql, new { Id = id });
    }

    public async Task<NotificationEntity> CreateAsync(NotificationEntity entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Notifications (Recipient, Channel, Subject, Message, Status, CreatedAt, SentAt, FailureReason)
            VALUES (@Recipient, @Channel, @Subject, @Message, @Status, @CreatedAt, @SentAt, @FailureReason);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        var id = await connection.ExecuteScalarAsync<int>(sql, entity);
        entity.Id = id;
        return entity;
    }

    public async Task<bool> UpdateAsync(NotificationEntity entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sql = @"
            UPDATE Notifications 
            SET Recipient = @Recipient,
                Channel = @Channel,
                Subject = @Subject,
                Message = @Message,
                Status = @Status,
                CreatedAt = @CreatedAt,
                SentAt = @SentAt,
                FailureReason = @FailureReason
            WHERE Id = @Id";

        var rowsAffected = await connection.ExecuteAsync(sql, entity);
        return rowsAffected > 0;
    }
}


