using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtisAPI.Controllers;
using OtisAPI.Model.ViewModels.Errands;
using OtisAPI.Services;
using OtisAPI.Test.Dependencies;

namespace OtisAPI.Test.Controllers;

public class ErrandsContoller_Tests
{
    private readonly ErrandSeedDataFixture _dataContext;
    private readonly AutoMapperDependency _autoMapper;
    private readonly ErrandsController _sut;


    public ErrandsContoller_Tests()
    {
        _dataContext = new ErrandSeedDataFixture();
        _autoMapper = new AutoMapperDependency();

        _sut = new ErrandsController(new ErrandService(_dataContext.SqlContext, _autoMapper.Mapper));
    }

    //GET
    [Fact]
    public async Task Test_Get_Errand_Should_Return_Specific_Errand()
    {
        List<ErrandViewModel> errands = new List<ErrandViewModel>();
        ErrandViewModel errand = new ErrandViewModel();
        var errandsActionResult = await _sut.GetAllErrandsAsync() as OkObjectResult;

        if (errandsActionResult?.Value != null)
        {
            errands = (List<ErrandViewModel>)errandsActionResult.Value;
        }

        var errandActionResult = await _sut.GetErrandAsync(errands[0].ErrandNumber) as OkObjectResult;
        if (errandActionResult?.Value != null)
        {
            errand = (ErrandViewModel)errandActionResult.Value;
        }

        Assert.Equal(errand.ErrandNumber, errands[0].ErrandNumber);

    }
    [Fact]
    public async Task Test_Get_All_Errands_Should_Return_All_Errands()
    {
        var errandsActionResult = await _sut.GetAllErrandsAsync() as OkObjectResult;
        var errandsToCompare =
            _autoMapper.Mapper.Map<List<ErrandViewModel>>(await _dataContext.SqlContext.Errands.ToListAsync());
        bool errandsExists = true;
        if (errandsActionResult?.Value != null)
        {
            var errands = (List<ErrandViewModel>)errandsActionResult.Value;

            foreach (var errand in errands)
            {
                if (errandsToCompare.Find(x => x.ErrandNumber == errand.ErrandNumber) == null)
                {
                    errandsExists = false;
                    break;
                }
            }
        }
        else
            errandsExists = false;

        Assert.True(errandsExists);
    }
    //POST
}