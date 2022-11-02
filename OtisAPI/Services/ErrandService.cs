using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;
using OtisAPI.Infrastructure;
using OtisAPI.Model.DataEntities.Errands;
using OtisAPI.Model.DataEntities.Users;
using OtisAPI.Model.InputModels.Errands;
using OtisAPI.Model.ViewModels.Errands;

namespace OtisAPI.Services;

public interface IErrandService
{
    public enum StatusCodes
    {
        Success,
        Failed,
        Conflict,
        NoElevatorAttached,
        NeedsFirstUpdate,
        ErrandDoesNotExist,
    }

    public Task<IErrandService.StatusCodes> CreateErrandAsync(ErrandInputModel input);
    public Task<ErrandViewModel> GetErrandAsync(string errandNummer);
    public Task<List<ErrandViewModel>> GetAllErrandsAsync(int take = 0);
}
public class ErrandService : IErrandService
{
    private readonly SqlContext _context;
    private readonly IMapper _mapper;

    public ErrandService(SqlContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IErrandService.StatusCodes> CreateErrandAsync(ErrandInputModel input)
    {
        bool validErrandNumber = false;

        //If the errand number generator creates a duplicate number, it will re-generate a new errand-number until it's unique
        while (!validErrandNumber)
        {
            if (await _context.Errands.FirstOrDefaultAsync(x => x.ErrandNumber == input.ErrandNumber) == null)
            {
                validErrandNumber = true;
                break;
            }
            else
                input.ErrandNumber = ErrandNumberGenerator.GenerateErrandNumber();
        }

        try
        {
            if (input.Elevator == null)
                return IErrandService.StatusCodes.NoElevatorAttached;
            if (input.ErrandUpdates == null || input.ErrandUpdates.Count == 0)
                return IErrandService.StatusCodes.NeedsFirstUpdate;

            _context.Entry(_mapper.Map<ErrandEntity>(input)).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return IErrandService.StatusCodes.Success;
        }
        catch { }
        return IErrandService.StatusCodes.Failed;
    }

    public async Task<ErrandViewModel> GetErrandAsync(string errandNummer)
    {
        return _mapper.Map<ErrandViewModel>(await _context.Errands
            .Include(x => x.AssignedTechnicians)
            .Include(x => x.ErrandUpdates)
            .Include(x => x.Elevator)
            .FirstOrDefaultAsync(x => x.ErrandNumber == errandNummer)) ?? null!;
    }

    public async Task<List<ErrandViewModel>> GetAllErrandsAsync(int take = 0)
    {
        if (take <= 0)
            return _mapper.Map<List<ErrandViewModel>>(await _context.Errands
                .Include(x => x.AssignedTechnicians)
                .Include(x => x.ErrandUpdates)
                .Include(x => x.Elevator)
                .ToListAsync()) ?? null!;

        return _mapper.Map<List<ErrandViewModel>>(await _context.Errands
            .Include(x => x.AssignedTechnicians)
            .Include(x => x.ErrandUpdates)
            .Include(x => x.Elevator)
            .Take(take)
            .ToListAsync()) ?? null!;
    }

    public async Task<IErrandService.StatusCodes> AddErrandUpdateAsync(ErrandUpdateInputModel input)
    {
        try
        {
            var errand = await _context.Errands
                .Include(x => x.ErrandUpdates)
                .FirstOrDefaultAsync(x => x.Id == input.ErrandId);
            if (errand == null)
                return IErrandService.StatusCodes.ErrandDoesNotExist;

            errand.ErrandUpdates.Add(new ErrandUpdateEntity
            {
                Id = Guid.NewGuid(),
                Status = input.Status,
                Message = input.Message,
                DateOfUpdate = DateTime.UtcNow
            });

            if (input.Employees != null)
            {
                foreach (var employee in input.Employees)
                {
                    errand.AssignedTechnicians.Add(_mapper.Map<EmployeeEntity>(employee));
                }
            }

            _context.Entry(errand).State = EntityState.Modified;


            await _context.SaveChangesAsync();
            return IErrandService.StatusCodes.Success;
        }
        catch { }
        return IErrandService.StatusCodes.Failed;
    }
}