using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtisAPI.Controllers;
using OtisAPI.Model.InputModels.Elevator;
using OtisAPI.Model.ViewModels.Elevator;
using OtisAPI.Services;
using OtisAPI.Test.Dependencies;

namespace OtisAPI.Test.Controllers;

public class ElevatorsController_Tests
{
    private readonly ElevatorSeedDataFixture _dataContext;
    private readonly ElevatorsController _sut;
    private readonly AutoMapperDependency _autoMapper;

    public ElevatorsController_Tests()
    {
        _dataContext = new ElevatorSeedDataFixture();
        _autoMapper = new AutoMapperDependency();

        _sut = new ElevatorsController(new ElevatorService(_dataContext.SqlContext, _autoMapper.Mapper));
    }

    //GET
    [Fact]
    public async Task Test_Get_Elevator_Should_Return_Specific_Elevator()
    {
        var elevatorsActionResult = await _sut.GetElevatorIdsAsync() as OkObjectResult;

        if (elevatorsActionResult?.Value != null)
        {
            List<Guid> elevators = (List<Guid>)elevatorsActionResult.Value;
            var elevatorActionResult = await _sut.GetElevatorAsync(elevators[0]) as OkObjectResult;
            if (elevatorActionResult?.Value != null)
            {
                var elevator = (ElevatorViewModel)elevatorActionResult.Value;
                Assert.StrictEqual(elevators[0], elevator.Id);
            }
        }
    }
    [Fact]
    public async Task Test_Get_Elevator_Ids_Should_Return_All_ElevatorIds()
    {
        var elevatorIdsToCompare = _dataContext.SqlContext.Elevators.Select(x => x.Id).ToList();
        List<Guid> elevatorIds = new List<Guid>();

        elevatorIdsToCompare = elevatorIdsToCompare.OrderBy(x => x).ToList();

        var elevatorIdsActionResults = await _sut.GetElevatorIdsAsync() as OkObjectResult;
        if (elevatorIdsActionResults?.Value != null)
        {
            elevatorIds = (List<Guid>)elevatorIdsActionResults.Value;

            elevatorIds = elevatorIds.OrderBy(x => x).ToList();
        }

        Assert.Equal(elevatorIdsToCompare, elevatorIds);
    }
    [Fact]
    public async Task Test_Get_Elevators_Should_Return_All_Elevators()
    {
        var elevatorsActionResult = await _sut.GetElevatorsAsync() as OkObjectResult;
        var elevatorsToCompare = _autoMapper.Mapper.Map<List<ElevatorViewModel>>(await _dataContext.SqlContext.Elevators.ToListAsync());
        bool elevatorsExists = true;
        if (elevatorsActionResult?.Value != null)
        {
            var elevators = (List<ElevatorViewModel>)elevatorsActionResult.Value;

            foreach (var elevator in elevators)
            {
                if (elevatorsToCompare.Find(x => x.Id == elevator.Id) == null)
                {
                    elevatorsExists = false;
                    break;
                }
            }
        }
        else
            elevatorsExists = false;

        Assert.True(elevatorsExists);
    }

    //POST
    [Fact]
    public async Task Test_Add_Elevator_Should_Return_Success()
    {
        var testElevator = new Faker<ElevatorInputModel>()
            .StrictMode(false)
            .RuleFor(e => e.Id, f => Guid.NewGuid())
            .RuleFor(e => e.Location, f => f.Address.StreetAddress());

        var result = await _sut.AddElevatorAsync(testElevator);

        Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task Test_Add_Elevator_Should_Return_Conflict()
    {
        var testElevator = new Faker<ElevatorInputModel>()
            .StrictMode(false)
            .RuleFor(e => e.Id, f => Guid.NewGuid())
            .RuleFor(e => e.Location, f => f.Address.StreetAddress()).Generate();

        await _sut.AddElevatorAsync(testElevator);

        var result = await _sut.AddElevatorAsync(testElevator);


        Assert.IsType<ConflictObjectResult>(result);
    }
    [Fact]
    public async Task Test_Add_Elevator_Should_Return_BadRequest()
    {
        var result = await _sut.AddElevatorAsync(null!);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}