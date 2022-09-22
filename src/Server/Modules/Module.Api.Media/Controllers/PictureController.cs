using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Camino.Framework.Controllers;
using Camino.Application.Contracts.AppServices.Media;
using Camino.Application.Contracts;
using Module.Api.Media.Models;
using Camino.Application.Validators;
using Camino.Core.Validators;

namespace Module.Api.Media.Controllers
{
    [Route("pictures")]
    public class PictureController : BaseController
    {
        
        private readonly IPictureAppService _pictureAppService;
        private readonly IValidationStrategyContext _validationStrategyContext;
        public PictureController(IPictureAppService pictureAppService, IValidationStrategyContext validationStrategyContext)
        {
            _pictureAppService = pictureAppService;
            _validationStrategyContext = validationStrategyContext;
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

        public IActionResult ValidateUrl(ImageValidationModel criterias)
        {
            _validationStrategyContext.SetStrategy(new ImageUrlValidationStrategy());
            if (criterias == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(criterias.Url))
            {
                return BadRequest();
            }

            bool canUpdate = _validationStrategyContext.Validate(criterias.Url);
            if (!canUpdate)
            {
                _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(criterias.Url);
            }

            if (canUpdate)
            {
                return Ok();
            }

            return ValidationProblem();
        }
    }
}
