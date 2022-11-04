using Bogus;
using OtisAPI.Model.InputModels.Elevator;
using OtisAPI.Model.ViewModels.Elevator;
using OtisAPI.Model.ViewModels.Errands;
using OtisAPI.Services;
using OtisAPI.Test.Dependencies;

namespace OtisAPI.Test.Services;

public class ElevatorService_Tests
{
    private readonly ElevatorSeedDataFixture _dataContext;
    private readonly ElevatorService _sut;

    public ElevatorService_Tests()
    {
        _dataContext = new ElevatorSeedDataFixture();
        var autoMapper = new AutoMapperDependency();

        _sut = new ElevatorService(_dataContext.SqlContext, autoMapper.Mapper);
    }


    [Fact]
    public async Task Test_Add_Elevator()
    {
        var testElevator = new Faker<ElevatorInputModel>()
            .StrictMode(false)
            .RuleFor(e => e.Id, f => Guid.NewGuid())
            .RuleFor(e => e.Location, f => f.Address.StreetAddress());

        var result = await _sut.AddElevatorAsync(testElevator);

        Assert.StrictEqual(IElevatorService.StatusCodes.Success, result);
    }

    [Fact]
    public async Task Test_Get_Elevator()
    {
        var testElevator = new Faker<ElevatorViewModel>()
            .StrictMode(false)
            .RuleFor(e => e.Id, f => Guid.NewGuid())
            .RuleFor(e => e.Location, f => f.Address.StreetAddress()).Generate();

        testElevator.Errands = new List<ErrandViewModel>();

        await _sut.AddElevatorAsync(new ElevatorInputModel
        {
            Id = testElevator.Id,
            Location = testElevator.Location
        });

        var result = await _sut.GetElevatorAsync(testElevator.Id);

        Assert.StrictEqual(testElevator.Id, result.Id);
    }

    [Fact]
    public async Task Test_Get_Elevators()
    {
        var result = await _sut.GetElevatorsAsync();

        Assert.StrictEqual(result.Count, _dataContext.SqlContext.Elevators.Count());
    }

    [Fact]
    public async Task Test_Delete_Elevator()
    {
        var elevatorToDelete = _dataContext.SqlContext.Elevators.First();

        await _sut.DeleteElevatorAsync(elevatorToDelete.Id);

        Assert.Null(_dataContext.SqlContext.Elevators.FirstOrDefault(x => x.Id == elevatorToDelete.Id));
    }

    [Fact]
    public async Task Test_Update_Elevator()
    {
        var elevatorToUpdate = _dataContext.SqlContext.Elevators.First();

        var result = await _sut.UpdateElevatorAsync(new UpdateElevatorInputModel
        {
            Id = elevatorToUpdate.Id,
            Location = "Kungsgatan 8"
        });

        Assert.StrictEqual(IElevatorService.StatusCodes.Success, result);
    }

}