using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Camino.Framework.Controllers;
using Camino.Application.Contracts.AppServices.Media;
using Camino.Application.Contracts;

namespace Module.Api.Media.Controllers
{
    [Route("pictures")]
    public class PictureController : BaseController
    {
        
        private readonly IPictureAppService _pictureAppService;

        public PictureController(IPictureAppService pictureAppService)
        {
            _pictureAppService = pictureAppService;
        }

        [HttpGet]
        [IgnoreAntiforgeryToken]
        [Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var exist = await _pictureAppService.FindAsync(new IdRequestFilter<long>
            {
                Id = id,
                CanGetInactived = true
            });

            if (exist == null)
            {
                return NotFound();
            }

            return File(exist.BinaryData, exist.ContentType);
        }
    }
}
