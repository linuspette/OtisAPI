using Bogus;
using OtisAPI.Infrastructure;
using OtisAPI.Model.InputModels.Errands;
using OtisAPI.Services;
using OtisAPI.Test.Dependencies;

namespace OtisAPI.Test.Services;

public class ErrandService_Tests
{
    private readonly ErrandService _sut;
    private readonly ErrandSeedDataFixture _dataContext;
    private readonly Random random = new Random();

    public ErrandService_Tests()
    {
        _dataContext = new ErrandSeedDataFixture();
        var autoMapper = new AutoMapperDependency();

        _sut = new ErrandService(_dataContext.SqlContext, autoMapper.Mapper);
    }

    //Errands
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
    [Fact]
    public async Task Test_Delete_Errand()
    {
        var mock = GenerateMockInputModel();
        await _sut.CreateErrandAsync(mock);


        var result = await _sut.DeleteErrandAsync(mock.ErrandNumber);
        Assert.StrictEqual(IErrandService.StatusCodes.Success, result);
    }

    //ErrandUpdates
    [Fact]
    public async Task Test_Add_Update_To_Errand()
    {
        var elevator = _dataContext.SqlContext.Elevators.First();
        var errand = new ErrandInputModel
        {
            ErrandNumber = ErrandNumberGenerator.GenerateErrandNumber(),
            Title = "Test",
            ElevatorId = elevator.Id,
            ErrandUpdates = new ErrandUpdateCreationModel
            {
                Status = "Not fixed",
                Message = "Testing"
            },
            IsResolved = false
        };

        var errandResult = await _sut.CreateErrandAsync(errand);
        if (errandResult == IErrandService.StatusCodes.Success)
        {
            var errandUpdate = new ErrandUpdateInputModel
            {
                ErrandNumber = errand.ErrandNumber,
                Status = "Fixed",
                Message = "Ok",
                IsResolved = true,
                Employees = null
            };

            var updateResult = await _sut.AddErrandUpdateAsync(errandUpdate);

            Assert.StrictEqual(IErrandService.StatusCodes.Success, updateResult);
        }
    }

    //Helpers

    private ErrandInputModel GenerateMockInputModel()
    {
        var errand = new Faker<ErrandInputModel>()
            .StrictMode(false)
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .Generate();

        errand.ErrandUpdates = new Faker<ErrandUpdateCreationModel>()
            .StrictMode(false)
            .RuleFor(x => x.Status, f => f.Lorem.Word())
            .RuleFor(x => x.Message, f => f.Lorem.Sentences(5))
            .Generate();

        var elevators = _dataContext.SqlContext.Elevators.ToList();
        var elevator = elevators[random.Next(0, elevators.Count)];
        errand.ElevatorId = elevator.Id;

        return errand;
    }

    private async Task<string> GenerateMockViewModel()
    {
        var errand = new Faker<ErrandInputModel>()
            .StrictMode(false)
            .RuleFor(x => x.Title, f => f.Commerce.ProductName())
            .Generate();

        errand.ErrandUpdates = new Faker<ErrandUpdateCreationModel>()
            .StrictMode(false)
            .RuleFor(x => x.Status, f => f.Lorem.Word())
            .RuleFor(x => x.Message, f => f.Lorem.Sentences(5))
            .Generate();

        var elevators = _dataContext.SqlContext.Elevators.ToList();
        var elevator = elevators[random.Next(0, elevators.Count)];
        errand.ElevatorId = elevator.Id;
        await _sut.CreateErrandAsync(errand);

        return errand.ErrandNumber;
    }
}