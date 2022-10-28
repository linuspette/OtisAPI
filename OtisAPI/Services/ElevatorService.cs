using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;
using OtisAPI.Model.DataEntities.Elevators;
using OtisAPI.Model.InputModels.Elevator;
using OtisAPI.Model.ViewModels.Elevator;

namespace OtisAPI.Services;

public interface IElevatorService
{
    public enum StatusCodes
    {
        Success,
        Failed,
        Error
    }

    public Task<List<ElevatorViewModel>> GetElevatorsAsync(int take = 0);
}
public class ElevatorService : IElevatorService
{
    private readonly SqlContext _context;
    private readonly IMapper _mapper;

    public ElevatorService(SqlContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ElevatorViewModel>> GetElevatorsAsync(int take = 0)
    {
        return _mapper.Map<List<ElevatorViewModel>>(await GetElevators(take)) ?? null!;
    }

    public async Task<ElevatorViewModel> GetElevator(Guid id)
    {
        return _mapper.Map<ElevatorViewModel>(await _context.Elevators
                .Include(x => x.Errands).ThenInclude(x => x.ErrandUpdates)
                .Include(x => x.Errands).ThenInclude(x => x.AssignedTechnicians)
                .FirstOrDefaultAsync(x => x.Id == id)) ?? null!;
    }

    public async Task<IElevatorService.StatusCodes> AddElevator(ElevatorInputModel input)
    {

        return IElevatorService.StatusCodes.Error;
    }
    private async Task<List<ElevatorEntity>> GetElevators(int take)
    {
        if (take <= 0)
            return await _context.Elevators
                .Include(x => x.Errands).ThenInclude(x => x.ErrandUpdates)
                .Include(x => x.Errands).ThenInclude(x => x.AssignedTechnicians)
                .ToListAsync();
        return await _context.Elevators
            .Include(x => x.Errands).ThenInclude(x => x.ErrandUpdates)
            .Include(x => x.Errands).ThenInclude(x => x.AssignedTechnicians)
            .Take(take)
            .ToListAsync();
    }
}