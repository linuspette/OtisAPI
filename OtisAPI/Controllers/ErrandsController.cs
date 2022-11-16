using Microsoft.AspNetCore.Mvc;
using OtisAPI.Model.InputModels.Errands;
using OtisAPI.Services;

namespace OtisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrandsController : ControllerBase
    {
        private readonly IErrandService _errandService;

        public ErrandsController(IErrandService errandService)
        {
            _errandService = errandService;
        }

        //Errands
        [HttpGet]
        [Route("geterrands")]
        public async Task<IActionResult> GetAllErrandsAsync([FromHeader] int take = 0)
        {
            try
            {
                var errands = await _errandService.GetAllErrandsAsync(take);

                if (errands == null)
                    return new NotFoundObjectResult("No errands found");

                return new OkObjectResult(errands);
            }
            catch { }

            return new BadRequestObjectResult("Could not process your request");
        }
        [HttpGet]
        [Route("geterrand")]
        public async Task<IActionResult> GetErrandAsync([FromHeader] string errandNumber)
        {
            try
            {
                var errand = await _errandService.GetErrandAsync(errandNumber);
                if (errand == null)
                    return new NotFoundObjectResult("Errand not found");

                return new OkObjectResult(errand);
            }
            catch { }

            return new BadRequestObjectResult("Could not process your request");
        }
        [HttpPost]
        [Route("createerrand")]
        public async Task<IActionResult> CreateErrandAsync([FromBody] ErrandInputModel input)
        {
            try
            {
                var result = await _errandService.CreateErrandAsync(input);

                if (result == IErrandService.StatusCodes.Success)
                    return new OkObjectResult(result);

                return new BadRequestObjectResult(result);
            }
            catch { }

            return new BadRequestObjectResult("Could not process your request");
        }
        [HttpDelete]
        [Route("deleteerrand")]
        public async Task<IActionResult> DeleteErrandAsync([FromHeader] string errandNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(errandNumber))
                    return new BadRequestObjectResult("Errand number must be supplied");

                var result = await _errandService.DeleteErrandAsync(errandNumber);

                if (result == IErrandService.StatusCodes.Success)
                    return new OkObjectResult(result);
                else if (result == IErrandService.StatusCodes.NotFound)
                    return new NotFoundObjectResult(result);

                return new BadRequestObjectResult(result);
            }
            catch { }

            return new BadRequestObjectResult("Error");
        }

        //ErrandUpdates
        [HttpPost]
        [Route("updateerrand")]
        public async Task<IActionResult> AddErrandUpdateAsync([FromBody] ErrandUpdateInputModel input)
        {
            try
            {
                var result = await _errandService.AddErrandUpdateAsync(input);

                if (result == IErrandService.StatusCodes.Success)
                    return new OkObjectResult(result);

                return new BadRequestObjectResult("Could not update errand");
            }
            catch { }
            return new BadRequestObjectResult("Could not process your request");
        }

        [HttpDelete]
        [Route("deleteerrandupdate")]
        public async Task<IActionResult> DeleteErrandUpdateAsync([FromHeader] string errandNumber, Guid updateId)
        {
            try
            {
                var result = await _errandService.DeleteErrandUpdateAsync(errandNumber, updateId);

                if (result == IErrandService.StatusCodes.Success)
                    return new OkObjectResult(result);
                else if (result == IErrandService.StatusCodes.NotFound)
                    return new NotFoundObjectResult(result);

                return new BadRequestObjectResult(result);
            }
            catch { }

            return new BadRequestObjectResult("Error");
        }
    }
}
