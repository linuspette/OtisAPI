using Bogus;
using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;
using OtisAPI.Infrastructure;
using OtisAPI.Model.DataEntities.Elevators;
using OtisAPI.Model.DataEntities.Errands;
using OtisAPI.Model.DataEntities.Users;

namespace OtisAPI.Test.Dependencies;

public class ErrandSeedDataFixture : IDisposable
{
    public SqlContext SqlContext { get; private set; }

    public ErrandSeedDataFixture()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase("ElevatorInmemoryDb")
            .Options;

        SqlContext = new SqlContext(options);

        GenerateMockElevators();
        GenerateMockErrands();
    }

    private void GenerateMockElevators()
    {
        while (SqlContext.Elevators.Count() < 20)
        {
            var elevator = new Faker<ElevatorEntity>()
                .StrictMode(false)
                .RuleFor(x => x.Id, f => Guid.NewGuid())
                .RuleFor(x => x.Location, f => f.Address.StreetAddress())
                .Generate();

            SqlContext.Entry(elevator).State = EntityState.Added;
            SqlContext.SaveChanges();
        }
    }

    //Generates mock-errands
    private void GenerateMockErrands()
    {
        var elevators = SqlContext.Elevators.ToList();
        var random = new Random();
        bool validErrandNumber = false;

        while (SqlContext.Errands.Count() < 20)
        {
            var errand = new Faker<ErrandEntity>()
                .StrictMode(false)
                .RuleFor(x => x.Title, f => f.Commerce.ProductName())
                .Generate();

            errand.AssignedTechnicians = new List<EmployeeEntity>();
            errand.ErrandUpdates = new List<ErrandUpdateEntity>();
            errand.ErrandUpdates.Add(new Faker<ErrandUpdateEntity>()
                .StrictMode(false)
                .RuleFor(x => x.Id, f => Guid.NewGuid())
                .RuleFor(x => x.Message, f => f.Lorem.Sentences(random.Next(0, 10)))
            );

            errand.Elevator = elevators[random.Next(0, elevators.Count)];

            errand.ErrandNumber = ErrandNumberGenerator.GenerateErrandNumber();
            while (!validErrandNumber)
            {
                if (SqlContext.Errands.FirstOrDefault(x => x.ErrandNumber == errand.ErrandNumber) == null)
                    validErrandNumber = true;
                else
                    errand.ErrandNumber = ErrandNumberGenerator.GenerateErrandNumber();
            }

            SqlContext.Entry(errand).State = EntityState.Added;
            SqlContext.SaveChanges();
        }
    }

    public void Dispose()
    {
        SqlContext.Dispose();
    }
}