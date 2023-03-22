using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Farms;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Shared.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Module.Farm.Api.Models;
using Camino.Application.Contracts.AppServices.Farms.Dtos;
using System.Collections.Generic;
using System.Linq;
using Camino.Infrastructure.AspNetCore.Models;
using System.ComponentModel.DataAnnotations;
using Camino.Core.Validators;
using Camino.Shared.Configuration.Options;
using Microsoft.Extensions.Options;
using Camino.Application.Validators;

namespace Module.Farm.Api.Controllers
{
    [Route("api/farms")]
    public class FarmController : BaseTokenAuthController
    {
        private readonly IFarmAppService _farmAppService;
        private readonly BaseValidatorContext _validatorContext;
        private readonly IOptions<ApplicationSettings> _appSettings;

        public FarmController(IHttpContextAccessor httpContextAccessor,
            IFarmAppService farmAppService,
            BaseValidatorContext validatorContext,
            IOptions<ApplicationSettings> appSettings) : base(httpContextAccessor)
        {
            _farmAppService = farmAppService;
            _validatorContext = validatorContext;
            _appSettings = appSettings;
        }

        [HttpPost]
        [TokenAuthentication]
        public async Task<IActionResult> CreateAsync([FromForm] CreateFarmModel request, IList<PictureRequestModel> pictures)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var farm = new FarmModifyRequest
                {
                    CreatedById = LoggedUserId,
                    UpdatedById = LoggedUserId,
                    Description = request.Description,
                    Name = request.Name,
                    FarmTypeId = request.FarmTypeId,
                    Address = request.Address,
                };

                if (pictures?.Any() ?? false)
                {
                    var files = new List<PictureRequest>();
                    foreach (var file in pictures)
                    {
                        _validatorContext.SetValidator(new FormFileValidator(_appSettings));
                        bool canUpdate = _validatorContext.Validate<IFormFile, bool>(file.File);
                        if (!canUpdate)
                        {
                            return BadRequest(nameof(file.File));
                        }

                        var fileData = await FileUtils.GetBytesAsync(file.File);
                        _validatorContext.SetValidator(new ImageBufferValidator());
                        canUpdate = _validatorContext.Validate<byte[], bool>(fileData);
                        if (!canUpdate)
                        {
                            return BadRequest(nameof(file.File));
                        }

                        files.Add(new PictureRequest
                        {
                            BinaryData = fileData,
                            FileName = file.File.FileName,
                            ContentType = file.File.ContentType
                        });
                    }

                    farm.Pictures = files;
                }

                var id = await _farmAppService.CreateAsync(farm);
                return Ok(id);
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPut("{id}")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateAsync([Required] long id, UpdateFarmModel request, IList<PictureRequestModel> pictures)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var exist = await _farmAppService.FindAsync(new IdRequestFilter<long>
                {
                    Id = id,
                    CanGetInactived = true
                });

                if (exist == null)
                {
                    return NotFound();
                }

                if (LoggedUserId != exist.CreatedById)
                {
                    return Unauthorized();
                }

                var farm = new FarmModifyRequest
                {
                    Id = id,
                    CreatedById = LoggedUserId,
                    UpdatedById = LoggedUserId,
                    Description = request.Description,
                    Name = request.Name,
                    FarmTypeId = request.FarmTypeId,
                    Address = request.Address,
                };

                if (pictures?.Any() ?? false)
                {
                    var files = new List<PictureRequest>();
                    foreach (var file in pictures)
                    {
                        if (file.PictureId > 0)
                        {
                            files.Add(new PictureRequest
                            {
                                Id = file.PictureId.GetValueOrDefault()
                            });

                            continue;
                        }

                        _validatorContext.SetValidator(new FormFileValidator(_appSettings));
                        bool canUpdate = _validatorContext.Validate<IFormFile, bool>(file.File);
                        if (!canUpdate)
                        {
                            return BadRequest(nameof(file.File));
                        }

                        var fileData = await FileUtils.GetBytesAsync(file.File);
                        _validatorContext.SetValidator(new ImageBufferValidator());
                        canUpdate = _validatorContext.Validate<byte[], bool>(fileData);
                        if (!canUpdate)
                        {
                            return BadRequest(nameof(file.File));
                        }

                        files.Add(new PictureRequest
                        {
                            BinaryData = fileData,
                            FileName = file.File.FileName,
                            ContentType = file.File.ContentType
                        });
                    }

                    farm.Pictures = files;
                }

                await _farmAppService.UpdateAsync(farm);
                return Ok();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpDelete("{id}")]
        [TokenAuthentication]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(id), "The id is required");
                    return BadRequest(ModelState);
                }

                var exist = await _farmAppService.FindAsync(new IdRequestFilter<long>
                {
                    Id = id,
                    CanGetInactived = true
                });

                if (exist == null)
                {
                    return NotFound();
                }

                if (LoggedUserId != exist.CreatedById)
                {
                    return Unauthorized();
                }

                await _farmAppService.SoftDeleteAsync(new FarmModifyRequest
                {
                    Id = id,
                    UpdatedById = LoggedUserId,
                });

                return Ok();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
