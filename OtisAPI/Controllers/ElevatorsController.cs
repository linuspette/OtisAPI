using Microsoft.AspNetCore.Mvc;
using OtisAPI.Model.InputModels.Elevator;
using OtisAPI.Model.ViewModels.Elevator;
using OtisAPI.Services;

namespace OtisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorsController : ControllerBase
    {
        private readonly IElevatorService _elevatorService;

        public ElevatorsController(IElevatorService elevatorService)
        {
            _elevatorService = elevatorService;
        }

        [HttpPost]
        [Route("getelevator")]
        public async Task<IActionResult> GetElevatorAsync(Guid id)
        {
            try
            {
                var elevator = await _elevatorService.GetElevatorAsync(id);
                if (elevator == null)
                    return new NotFoundObjectResult("Elevator not found");

                return new OkObjectResult(elevator);
            }
            catch { }

            return new BadRequestObjectResult("Error while searching for elevator");
        }

        [HttpPost]
        [Route("getelevators")]
        public async Task<IActionResult> GetElevators(int take = 0)
        {
            try
            {
                List<ElevatorViewModel> elevators = await _elevatorService.GetElevatorsAsync(take);
                if (elevators == null)
                    return new NotFoundObjectResult("Didn't find any elevators");

                return new OkObjectResult(elevators);
            }
            catch { }

            return new BadRequestObjectResult("Error while gettings elevators");
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddElevator(ElevatorInputModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _elevatorService.AddElevatorAsync(input);

                    if (result == IElevatorService.StatusCodes.Success)
                        return new OkObjectResult("Elevator successfully added");

                    return new BadRequestObjectResult("Elevator could not be created");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

            return new BadRequestObjectResult("Critical error. Add method could not execute");
        }
    }
}
