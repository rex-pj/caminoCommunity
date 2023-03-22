using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Application.Contracts.AppServices.Media;
using Camino.Application.Contracts;
using Module.Media.Api.Models;
using Camino.Application.Validators;
using Camino.Core.Validators;
using Microsoft.AspNetCore.Http;
using Camino.Shared.Configuration.Options;
using Camino.Shared.File;
using Microsoft.Extensions.Options;

namespace Module.Media.Api.Controllers
{
    [Route("api/pictures")]
    public class PictureController : BaseTokenAuthController
    {

        private readonly IPictureAppService _pictureAppService;
        private readonly BaseValidatorContext _validatorContext;
        private readonly IOptions<ApplicationSettings> _appSettings;

        public PictureController(IHttpContextAccessor httpContextAccessor,
            IPictureAppService pictureAppService,
            BaseValidatorContext validatorContext,
            IOptions<ApplicationSettings> appSettings)
            : base(httpContextAccessor)
        {
            _pictureAppService = pictureAppService;
            _validatorContext = validatorContext;
            _appSettings = appSettings;
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

            bool isValid;
            if (!string.IsNullOrEmpty(criterias.Url))
            {
                _validatorContext.SetValidator(new ImageUrlValidator());
                isValid = _validatorContext.Validate<string, bool>(criterias.Url);
                if (!isValid)
                {
                    _validatorContext.SetValidator(new Base64ImageValidator());
                    isValid = _validatorContext.Validate<string, bool>(criterias.Url);
                }

                if (!isValid)
                {
                    return ValidationProblem(nameof(criterias.Url));
                }

                return Ok();
            }

            if (criterias.File != null)
            {
                _validatorContext.SetValidator(new FormFileValidator(_appSettings));
                isValid = _validatorContext.Validate<IFormFile, bool>(criterias.File);
                if (!isValid)
                {
                    return ValidationProblem(nameof(criterias.File));
                }

                var fileData = await FileUtils.GetBytesAsync(criterias.File);
                _validatorContext.SetValidator(new ImageBufferValidator());
                isValid = _validatorContext.Validate<byte[], bool>(fileData);
                if (!isValid)
                {
                    return ValidationProblem(nameof(criterias.File));
                }

                return Ok();
            }

            return BadRequest();
        }
    }
}
