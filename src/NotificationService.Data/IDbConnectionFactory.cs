using System.Data;

namespace NotificationService.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection(string connectionName = "DefaultConnection");
}


