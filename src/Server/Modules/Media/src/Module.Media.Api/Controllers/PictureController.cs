using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Application.Contracts.AppServices.Media;
using Camino.Application.Contracts;
using Module.Media.Api.Models;
using Camino.Application.Validators;
using Camino.Core.Validators;
using Microsoft.AspNetCore.Http;

namespace Module.Media.Api.Controllers
{
    [Route("api/pictures")]
    public class PictureController : BaseTokenAuthController
    {

        private readonly IPictureAppService _pictureAppService;
        private readonly BaseValidatorContext _validatorContext;
        public PictureController(IHttpContextAccessor httpContextAccessor,
            IPictureAppService pictureAppService,
            BaseValidatorContext validatorContext)
            : base(httpContextAccessor)
        {
            _pictureAppService = pictureAppService;
            _validatorContext = validatorContext;
        }

        [HttpGet("{id}")]
        [IgnoreAntiforgeryToken]
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

        [HttpPut("validations")]
        public async Task<IActionResult> Validate([FromForm] ImageValidationModel criterias)
        {
            if (criterias == null)
            {
                return BadRequest();
            }

            bool isValid = false;
            if (!string.IsNullOrEmpty(criterias.Url))
            {
                _validatorContext.SetValidator(new ImageUrlValidator());
                isValid = _validatorContext.Validate<string, bool>(criterias.Url);
            }
            else if (criterias.File != null)
            {
                _validatorContext.SetValidator(new ImageFormFileValidator());
                isValid = await _validatorContext.ValidateAsync<IFormFile, bool>(criterias.File);
            }

            if (!string.IsNullOrEmpty(criterias.Url) && !isValid)
            {
                _validatorContext.SetValidator(new Base64ImageValidator());
                isValid = _validatorContext.Validate<string, bool>(criterias.Url);
            }

            if (isValid)
            {
                return Ok();
            }

            return ValidationProblem();
        }
    }
}
