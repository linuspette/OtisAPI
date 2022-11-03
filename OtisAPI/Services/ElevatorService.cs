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
        Error,
        Conflict
    }

    public Task<List<ElevatorViewModel>> GetElevatorsAsync(int take = 0);
    public Task<ElevatorViewModel> GetElevatorAsync(Guid id);
    public Task<IElevatorService.StatusCodes> AddElevatorAsync(ElevatorInputModel input);
    public Task<List<Guid>> GetElevatorIdsAsync(int take = 0);
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

    public async Task<List<Guid>> GetElevatorIdsAsync(int take = 0)
    {
        try
        {
            var elevators = new List<ElevatorEntity>();
            if (take > 0)
                elevators = await _context.Elevators.Take(take).ToListAsync();
            else
                elevators = await _context.Elevators.ToListAsync();

            return elevators.Select(e => e.Id).ToList();
        }
        catch { }
        return null!;
    }
    public async Task<List<ElevatorViewModel>> GetElevatorsAsync(int take = 0)
    {
        return _mapper.Map<List<ElevatorViewModel>>(await GetElevators(take));
    }
    public async Task<ElevatorViewModel> GetElevatorAsync(Guid id)
    {
        try
        {
            return _mapper.Map<ElevatorViewModel>(await _context.Elevators
                .Include(x => x.Errands).ThenInclude(x => x.ErrandUpdates)
                .Include(x => x.Errands).ThenInclude(x => x.AssignedTechnicians)
                .FirstOrDefaultAsync(x => x.Id == id));
        }
        catch { }

        return null!;
    }
    public async Task<IElevatorService.StatusCodes> AddElevatorAsync(ElevatorInputModel input)
    {
        try
        {
            if (await _context.Elevators.FirstOrDefaultAsync(x => x.Id == input.Id) == null)
            {
                var elevator = _mapper.Map<ElevatorEntity>(input);
                _context.Entry(elevator).State = EntityState.Added;

                await _context.SaveChangesAsync();
                return IElevatorService.StatusCodes.Success;
            }
            else
                return IElevatorService.StatusCodes.Conflict;
        }
        catch { }
        return IElevatorService.StatusCodes.Failed;
    }

    //Private methods
    private async Task<List<ElevatorEntity>> GetElevators(int take)
    {
        try
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
        catch { }

        return null!;
    }
}