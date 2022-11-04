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
        Error,
        NotFound
    }

    //Errands
    public Task<IErrandService.StatusCodes> CreateErrandAsync(ErrandInputModel input);
    public Task<ErrandViewModel> GetErrandAsync(string errandNummer);
    public Task<List<ErrandViewModel>> GetAllErrandsAsync(int take = 0);
    public Task<IErrandService.StatusCodes> DeleteErrandAsync(string errandNumber);

    //ErrandUpdates
    public Task<IErrandService.StatusCodes> AddErrandUpdateAsync(ErrandUpdateInputModel input);
    public Task<IErrandService.StatusCodes> DeleteErrandUpdateAsync(string errandNumber, Guid id);
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

    //Errands
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
            if (input.ElevatorId == Guid.Empty)
                return IErrandService.StatusCodes.NoElevatorAttached;
            if (input.ErrandUpdates == null)
                return IErrandService.StatusCodes.NeedsFirstUpdate;

            var errand = new ErrandEntity
            {
                ErrandNumber = input.ErrandNumber,
                Title = input.Title,
                IsResolved = false,
                ErrandUpdates = new List<ErrandUpdateEntity>(),
                Elevator = await _context.Elevators.FirstOrDefaultAsync(x => x.Id == input.ElevatorId) ?? null!
            };

            var errandUpdate = new ErrandUpdateEntity
            {
                Status = input.ErrandUpdates.Status,
                Message = input.ErrandUpdates.Message,
                DateOfUpdate = DateTime.UtcNow
            };
            errand.ErrandUpdates.Add(errandUpdate);

            _context.ErrandUpdates.Attach(errandUpdate).State = EntityState.Added;
            _context.Errands.Attach(errand).State = EntityState.Added;
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
    public async Task<IErrandService.StatusCodes> DeleteErrandAsync(string errandNumber)
    {
        try
        {
            var errandToDelete = await _context.Errands
                .Include(x => x.ErrandUpdates)
                .FirstOrDefaultAsync(x => x.ErrandNumber == errandNumber);

            if (errandToDelete == null)
                return IErrandService.StatusCodes.NotFound;

            _context.Errands.Attach(errandToDelete).State = EntityState.Deleted;
            if (errandToDelete.ErrandUpdates.Count > 0)
            {
                foreach (var errandUpdate in errandToDelete.ErrandUpdates)
                {
                    _context.ErrandUpdates.Attach(errandUpdate).State = EntityState.Deleted;
                }
            }

            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return IErrandService.StatusCodes.Success;

            return IErrandService.StatusCodes.Failed;
        }
        catch { }

        return IErrandService.StatusCodes.Error;
    }

    //Errand Updates
    public async Task<IErrandService.StatusCodes> AddErrandUpdateAsync(ErrandUpdateInputModel input)
    {
        try
        {
            var errand = await _context.Errands
                .Include(x => x.AssignedTechnicians)
                .Include(x => x.ErrandUpdates)
                .FirstOrDefaultAsync(x => x.ErrandNumber == input.ErrandNumber);
            if (errand == null)
                return IErrandService.StatusCodes.ErrandDoesNotExist;

            //Adds new messagef
            var errandUpdate = new ErrandUpdateEntity
            {
                Id = Guid.NewGuid(),
                Status = input.Status,
                Message = input.Message,
                DateOfUpdate = DateTime.UtcNow
            };

            errand.IsResolved = input.IsResolved;

            errand.ErrandUpdates.Add(errandUpdate);
            //Adds employee
            if (input.Employees != null)
            {
                errand.AssignedTechnicians = _mapper.Map<List<EmployeeEntity>>(input.Employees);
            }

            _context.ErrandUpdates.Attach(errandUpdate).State = EntityState.Added;
            _context.Errands.Attach(errand).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return IErrandService.StatusCodes.Success;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return IErrandService.StatusCodes.Failed;
    }
    public async Task<IErrandService.StatusCodes> DeleteErrandUpdateAsync(string errandNumber, Guid id)
    {
        try
        {
            var errand = await _context.Errands.Include(x => x.ErrandUpdates)
                .FirstOrDefaultAsync(x => x.ErrandNumber == errandNumber);

            if (errand == null)
                return IErrandService.StatusCodes.NotFound;

            var errandUpdate = errand.ErrandUpdates.FirstOrDefault(x => x.Id == id);
            if (errandUpdate == null)
                return IErrandService.StatusCodes.NotFound;

            errand.ErrandUpdates.Remove(errandUpdate);

            _context.ErrandUpdates.Attach(errandUpdate).State = EntityState.Deleted;
            _context.Errands.Attach(errand).State = EntityState.Modified;

            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return IErrandService.StatusCodes.Success;

            return IErrandService.StatusCodes.Failed;
        }
        catch { }

        return IErrandService.StatusCodes.Error;
    }
}