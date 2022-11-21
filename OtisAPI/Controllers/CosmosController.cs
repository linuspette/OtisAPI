using Microsoft.AspNetCore.Mvc;
using OtisAPI.Services;

namespace OtisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CosmosController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public CosmosController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet]
        [Route("getalldata")]
        public async Task<IActionResult> GetAllDataAsync()
        {
            try
            {
                var result = await _cosmosDbService.GetAllDataAsync();

                if (result == null)
                    return new NotFoundObjectResult("No objects found");

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet]
        [Route("getelevatordata")]
        public async Task<IActionResult> GetElevatorDataAsync(string deviceId)
        {
            try
            {
                var result = await _cosmosDbService.GetElevatorDataAsync(deviceId);

                if (result == null)
                    return new NotFoundObjectResult("No objects found");

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}
