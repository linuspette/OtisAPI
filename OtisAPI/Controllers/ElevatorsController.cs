using Microsoft.AspNetCore.Mvc;
using OtisAPI.Model.InputModels.Elevator;
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

        [HttpGet]
        [Route("getelevator")]
        public async Task<IActionResult> GetElevatorAsync([FromHeader] Guid id)
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

        [HttpGet]
        [Route("getelevators")]
        public async Task<IActionResult> GetElevatorsAsync([FromHeader] int take = 0)
        {
            try
            {
                var elevators = await _elevatorService.GetElevatorsAsync(take);
                if (elevators == null)
                    return new NotFoundObjectResult("Didn't find any elevators");

                return new OkObjectResult(elevators);
            }
            catch { }

            return new BadRequestObjectResult("Error while gettings elevators");
        }

        [HttpGet]
        [Route("getelevatorids")]
        public async Task<IActionResult> GetElevatorIdsAsync([FromHeader] int take = 0)
        {
            try
            {
                var elevatorIds = await _elevatorService.GetElevatorIdsAsync(take);
                if (elevatorIds == null)
                    return new NotFoundObjectResult("Didn't find any Ids");

                return new OkObjectResult(elevatorIds);
            }
            catch { }

            return new BadRequestObjectResult("Error while getting Ids");
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddElevatorAsync([FromBody] ElevatorInputModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _elevatorService.AddElevatorAsync(input);

                    if (result == IElevatorService.StatusCodes.Success)
                        return new OkObjectResult("Elevator successfully added");
                    else if (result == IElevatorService.StatusCodes.Conflict)
                        return new ConflictObjectResult("Elevator already exitst");
                }

                return new BadRequestObjectResult("Elevator could not be created");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
