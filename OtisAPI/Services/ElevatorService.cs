using OtisAPI.DataAccess;
using OtisAPI.Model.ViewModels.Elevator;

namespace OtisAPI.Services;

public interface IElevatorService
{

}
public class ElevatorService : IElevatorService
{
    private readonly SqlContext _context;

    public ElevatorService(SqlContext context)
    {
        _context = context;
    }

    public async Task<List<ElevatorViewModel>> GetElevatorsAsync(int take = 0)
    {
        return null!;
    }
}