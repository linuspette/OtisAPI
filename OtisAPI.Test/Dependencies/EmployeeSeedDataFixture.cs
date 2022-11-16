using Bogus;
using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;
using OtisAPI.Model.DataEntities.Errands;
using OtisAPI.Model.DataEntities.Users;

namespace OtisAPI.Test.Dependencies;

public class EmployeeSeedDataFixture
{
    public SqlContext SqlContext { get; set; }

    public EmployeeSeedDataFixture()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase("ElevatorInmemoryDb")
            .Options;

        SqlContext = new SqlContext(options);

        GenerateMockEmployees();
    }

    public void Dispose()
    {
        SqlContext.Dispose();
    }

    private void GenerateMockEmployees()
    {

        while (SqlContext.Employees.Count() < 20)
        {
            var employee = GenerateEmployee();


            SqlContext.Employees.Add(employee);
            SqlContext.SaveChanges();
        }
    }
    private EmployeeEntity GenerateEmployee()
    {
        Random random = new Random();
        return new Faker<EmployeeEntity>()
            .StrictMode(false)
            .RuleFor(x => x.Id, f => Guid.NewGuid())
            .RuleFor(x => x.EmployeeNumber, f => random.Next(11111, 99999))
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.AssignedErrands, f => new List<ErrandEntity>()).Generate();
    }
}