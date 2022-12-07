using ApiLambda.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiLambda.Controllers
{
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] string password, [FromForm] IFormFile[] file)
        {
            var output = await _portfolioService.Upload(password, file);

            return output.Success ? Ok(output) : BadRequest(output);
        }

        [HttpPost("Download")]
        public async Task<IActionResult> Download()
        {
            var output = await _portfolioService.Download();

            return output.Success ? Ok(output) : BadRequest(output);
        }

        //[HttpPost("Download")]
        //public async Task<IActionResult> Download([FromForm] string password)
        //{
        //    var output = await _portfolioService.Download(password);

        //    return output.Success ? Ok(output) : BadRequest(output);
        //}        
    }
}
