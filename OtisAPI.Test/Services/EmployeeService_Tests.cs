using Bogus;
using OtisAPI.Model.InputModels.Users;
using OtisAPI.Model.ViewModels.Errands;
using OtisAPI.Model.ViewModels.Users;
using OtisAPI.Services;
using OtisAPI.Test.Dependencies;

namespace OtisAPI.Test.Services;

public class EmployeeService_Tests
{
    private readonly EmployeeSeedDataFixture _dataContext;
    private readonly EmployeeService _sut;

    public EmployeeService_Tests()
    {
        _dataContext = new EmployeeSeedDataFixture();
        var automapper = new AutoMapperDependency();

        _sut = new EmployeeService(_dataContext.SqlContext, automapper.Mapper);
    }

    [Fact]
    public async Task Test_Add_Employee()
    {
        var result = await _sut.AddEmployeeAsync(new EmployeeInputModel
        {
            EmployeeNumber = 61657,
            FullName = "Test"
        });

        Assert.StrictEqual(IEmployeeService.StatusCodes.Success, result);
    }

    [Fact]
    public async Task Test_Get_Employee()
    {
        var employee = new Faker<EmployeeViewModel>()
            .StrictMode(false)
            .RuleFor(x => x.EmployeeNumber, f => 12345)
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.ErrandViewModels, f => new List<ErrandViewModel>()).Generate();

        await _sut.AddEmployeeAsync(new EmployeeInputModel
        {
            EmployeeNumber = employee.EmployeeNumber,
            FullName = employee.FullName
        });

        var result = await _sut.GetEmployeeAsync(employee.EmployeeNumber);

        Assert.StrictEqual(employee.EmployeeNumber, result.EmployeeNumber);
    }

    [Fact]
    public async Task Test_Get_Employees()
    {
        var amountOfEmployees = _dataContext.SqlContext.Employees.Count();

        var employees = await _sut.GetEmployeesAsync();

        Assert.StrictEqual(amountOfEmployees, employees.Count);
    }

    [Fact]
    public async Task Test_Delete_Employee()
    {
        var employeeToDelete = _dataContext.SqlContext.Employees.First();

        var result = await _sut.DeleteEmployeeAsync(employeeToDelete.EmployeeNumber);
        Assert.StrictEqual(IEmployeeService.StatusCodes.Success, result);
    }
}