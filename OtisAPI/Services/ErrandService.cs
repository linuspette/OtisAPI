using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;
using OtisAPI.Infrastructure;
using OtisAPI.Model.InputModels.Errands;

namespace OtisAPI.Services;

public interface IErrandService
{
    public enum StatusCodes
    {
        Created,
        Failed,
        Conflict
    }
}
public class ErrandService
{
    private readonly SqlContext _sqlContext;

    public ErrandService(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }

    public async Task<IElevatorService.StatusCodes> CreateErrandAssync(ErrandInputModel input)
    {
        bool validErrandNumber = false;
        string errandNumber = "";

        //If the errand number generator creates a duplicate number, it will re-generate a new errand-number until it's unique
        while (!validErrandNumber)
        {
            errandNumber = ErrandNumberGenerator.GenerateErrandNumber();
            if (await _sqlContext.Errands.FirstOrDefaultAsync(x => x.ErrandNumber == errandNumber) == null)
                validErrandNumber = true;
        }

        return IElevatorService.StatusCodes.Failed;
    }
}