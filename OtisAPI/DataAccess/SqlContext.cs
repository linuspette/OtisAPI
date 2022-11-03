using Microsoft.EntityFrameworkCore;
using OtisAPI.Model.DataEntities.Elevators;
using OtisAPI.Model.DataEntities.Errands;
using OtisAPI.Model.DataEntities.Users;

namespace OtisAPI.DataAccess;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
    }

    public DbSet<ErrandEntity> Errands { get; set; } = null!;
    public DbSet<ElevatorEntity> Elevators { get; set; } = null!;
    public DbSet<ErrandUpdateEntity> ErrandUpdates { get; set; } = null!;
    public DbSet<EmployeeEntity> Employees { get; set; } = null!;
}