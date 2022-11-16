using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtisAPI.DataAccess;
using OtisAPI.Model.DataEntities.Users;
using OtisAPI.Model.InputModels.Users;
using OtisAPI.Model.ViewModels.Users;

namespace OtisAPI.Services;

public interface IEmployeeService
{
    public enum StatusCodes
    {
        Success,
        Failed,
        Error,
        Conflict,
        NotFound
    }

    public Task<EmployeeViewModel> GetEmployeeAsync(int employeeNumber);
    public Task<List<EmployeeViewModel>> GetEmployeesAsync(int take = 0);
    public Task<IEmployeeService.StatusCodes> AddEmployeeAsync(EmployeeInputModel employeeInput);
    public Task<IEmployeeService.StatusCodes> DeleteEmployeeAsync(int EmployeeNumber);
}
public class EmployeeService : IEmployeeService
{
    private readonly SqlContext _context;
    private readonly IMapper _mapper;

    public EmployeeService(SqlContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EmployeeViewModel> GetEmployeeAsync(int employeeNumber)
    {
        try
        {
            var employee = await _context.Employees
                .Include(x => x.AssignedErrands)
                .FirstOrDefaultAsync(x => x.EmployeeNumber == employeeNumber);

            return _mapper.Map<EmployeeViewModel>(employee) ?? null!;
        }
        catch { }

        return null!;
    }

    public async Task<List<EmployeeViewModel>> GetEmployeesAsync(int take = 0)
    {
        try
        {
            var employees = new List<EmployeeEntity>();
            if (take > 0)
                employees = await _context.Employees.Include(x => x.AssignedErrands).Take(take).ToListAsync();
            else
                employees = await _context.Employees.Include(x => x.AssignedErrands).ToListAsync();

            return _mapper.Map<List<EmployeeViewModel>>(employees);
        }
        catch { }
        return null!;
    }

    public async Task<IEmployeeService.StatusCodes> AddEmployeeAsync(EmployeeInputModel employeeInput)
    {
        try
        {
            if (await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeNumber == employeeInput.EmployeeNumber) != null)
                return IEmployeeService.StatusCodes.Conflict;
            _context.Employees.Attach(_mapper.Map<EmployeeEntity>(employeeInput)).State = EntityState.Added;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return IEmployeeService.StatusCodes.Success;

            return IEmployeeService.StatusCodes.Failed;
        }
        catch { }

        return IEmployeeService.StatusCodes.Error;
    }

    public async Task<IEmployeeService.StatusCodes> DeleteEmployeeAsync(int EmployeeNumber)
    {
        try
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeNumber == EmployeeNumber);
            if (employee == null)
                return IEmployeeService.StatusCodes.NotFound;

            _context.Employees.Remove(employee);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return IEmployeeService.StatusCodes.Success;

            return IEmployeeService.StatusCodes.Failed;
        }
        catch { }
        return IEmployeeService.StatusCodes.Error;
    }
}