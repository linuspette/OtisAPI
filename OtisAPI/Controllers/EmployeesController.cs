using Microsoft.AspNetCore.Mvc;
using OtisAPI.Model.InputModels.Users;
using OtisAPI.Services;

namespace OtisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Route("getemployee")]
        public async Task<IActionResult> GetEmployeeAsync(int employeeNumber)
        {
            try
            {
                var result = await _employeeService.GetEmployeeAsync(employeeNumber);

                if (result == null)
                    return new NotFoundObjectResult("Employee not found");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(ex.Message);
            }

        }
        [HttpGet]
        [Route("getemployees")]
        public async Task<IActionResult> GetEmployeesAsync(int take = 0)
        {
            try
            {
                var result = await _employeeService.GetEmployeesAsync(take);

                if (result.Count > 0)
                    return new OkObjectResult(result);

                return new BadRequestObjectResult("Employees not found");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddEmployeeAsync(EmployeeInputModel input)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult("Invalid input");

                var result = await _employeeService.AddEmployeeAsync(input);

                if (result == IEmployeeService.StatusCodes.Conflict)
                    return new ConflictObjectResult("Employee already exists");
                if (result == IEmployeeService.StatusCodes.Success)
                    return new OkObjectResult(result);

                return new BadRequestObjectResult("Failed when trying to create a new employee");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteEmployeeAsync(int employeeNumber)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(employeeNumber);

                if (result == IEmployeeService.StatusCodes.NotFound)
                    return new NotFoundObjectResult("Employee not found");
                if (result == IEmployeeService.StatusCodes.Success)
                    return new OkObjectResult(result);

                return new BadRequestObjectResult("Failed while trying to delete employee");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
