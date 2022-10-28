using Bogus;
using OtisAPI.Model.InputModels.Errands;
using OtisAPI.Model.ViewModels.Elevator;
using OtisAPI.Services;
using OtisAPI.Test.Dependencies;

namespace OtisAPI.Test.Services;

public class ErrandService_Tests
{
    private readonly ErrandService _sut;
    private readonly ErrandSeedDataFixture dataContext;
    private readonly Random random = new Random();

    public ErrandService_Tests()
    {
        dataContext = new ErrandSeedDataFixture();
        var autoMapper = new AutoMapperDependency();

        _sut = new ErrandService(dataContext.SqlContext, autoMapper.Mapper);
    }

    [Fact]
    public async Task Test_Create_Errand_Async()
    {
        var result = await _sut.CreateErrandAsync(GenerateMockInputModel());
        Assert.StrictEqual<IErrandService.StatusCodes>(IErrandService.StatusCodes.Success, result);
    }

    [Fact]
    public async Task Test_Get_Errand_Async()
    {
        var errandNumber = await GenerateMockViewModel();
        var result = await _sut.GetErrandAsync(errandNumber);

        Assert.Equal(errandNumber, result.ErrandNumber);
    }

    private ErrandInputModel GenerateMockInputModel()
    {
        var errand = new Faker<ErrandInputModel>()
            .StrictMode(false)
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .Generate();

        errand.ErrandUpdates.Add(new Faker<ErrandUpdateInputModel>()
            .StrictMode(false)
            .RuleFor(x => x.Message, f => f.Lorem.Sentences(5))
            .Generate());

        var elevators = dataContext.SqlContext.Elevators.ToList();
        var elevator = elevators[random.Next(0, elevators.Count)];
        errand.Elevator = new ElevatorViewModel
        {
            Id = elevator.Id,
            Location = elevator.Location,
        };

        return errand;
    }

    private async Task<string> GenerateMockViewModel()
    {
        var errand = new Faker<ErrandInputModel>()
            .StrictMode(false)
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .Generate();

        errand.ErrandUpdates.Add(new Faker<ErrandUpdateInputModel>()
            .StrictMode(false)
            .RuleFor(x => x.Message, f => f.Lorem.Sentences(5))
            .Generate());

        var elevators = dataContext.SqlContext.Elevators.ToList();
        var elevator = elevators[random.Next(0, elevators.Count)];
        errand.Elevator = new ElevatorViewModel
        {
            Id = elevator.Id,
            Location = elevator.Location,
        };
        await _sut.CreateErrandAsync(errand);

        return errand.ErrandNumber;
    }
}