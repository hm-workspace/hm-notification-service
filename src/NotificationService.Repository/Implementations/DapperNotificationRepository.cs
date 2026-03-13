using Dapper;
using NotificationService.Data;
using NotificationService.InternalModels.Entities;

namespace NotificationService.Repository;

public class DapperNotificationRepository : INotificationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DapperNotificationRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<NotificationEntity>> GetAllAsync()
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM Notifications";
            var rows = await connection.QueryAsync<NotificationEntity>(sql);
            return rows.ToList();
        }
        catch
        {
            return new List<NotificationEntity>();
        }
    }
}


