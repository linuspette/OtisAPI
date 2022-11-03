using Microsoft.EntityFrameworkCore;

namespace OtisAPI.DataAccess;

public class NoSqlContext : DbContext
{
    public NoSqlContext(DbContextOptions<NoSqlContext> options) : base(options)
    {
    }
}