using Bogus;
using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;
using OtisAPI.Model.DataEntities.Elevators;

namespace OtisAPI.Test.Dependencies;

public class ElevatorSeedDataFixture : IDisposable
{
    public SqlContext SqlContext { get; private set; }

    public ElevatorSeedDataFixture()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase("ElevatorInmemoryDb")
            .Options;

        SqlContext = new SqlContext(options);

        GenerateMockElevators();
    }

    private void GenerateMockElevators()
    {
        while (SqlContext.Elevators.Count() < 20)
        {
            var elevator = new Faker<ElevatorEntity>()
                .StrictMode(false)
                .RuleFor(x => x.Id, f => Guid.NewGuid())
                .RuleFor(x => x.Location, f => f.Address.StreetAddress());

            SqlContext.Elevators.Add(elevator);
            SqlContext.SaveChanges();
        }
    }

    public void Dispose()
    {
        SqlContext.Dispose();
    }
}