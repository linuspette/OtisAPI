using Microsoft.EntityFrameworkCore;

namespace OtisAPI.DataAccess;

public class NoSqlContext : DbContext
{
    public NoSqlContext(DbContextOptions options) : base(options)
    {
    }
}