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

namespace Module.Farm.Api.Controllers
{
    [Route("api/farms")]
    public class FarmController : BaseTokenAuthController
    {
        private readonly IFarmAppService _farmAppService;
        public FarmController(IHttpContextAccessor httpContextAccessor, IFarmAppService farmAppService) : base(httpContextAccessor)
        {
            _farmAppService = farmAppService;
        }

        [HttpPost]
        [TokenAuthentication]
        public async Task<IActionResult> CreateAsync([FromForm] CreateFarmModel request, IList<PictureRequestModel> files)
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

                if (files?.Any(x => x.File != null) ?? false)
                {
                    var pictures = new List<PictureRequest>();
                    foreach (var file in files)
                    {
                        var fileData = await FileUtils.GetBytesAsync(file.File);
                        pictures.Add(new PictureRequest
                        {
                            BinaryData = fileData,
                            FileName = file.File.FileName,
                            ContentType = file.File.ContentType
                        });
                    }

                    farm.Pictures = pictures;
                }

                var id = await _farmAppService.CreateAsync(farm);
                return Ok(id);
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPut]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateAsync(UpdateFarmModel request, IList<PictureRequestModel> files)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var exist = await _farmAppService.FindAsync(new IdRequestFilter<long>
                {
                    Id = request.Id,
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
                    Id = request.Id,
                    CreatedById = LoggedUserId,
                    UpdatedById = LoggedUserId,
                    Description = request.Description,
                    Name = request.Name,
                    FarmTypeId = request.FarmTypeId,
                    Address = request.Address,
                };

                if (files?.Any(x => x.File != null) ?? false)
                {
                    var pictures = new List<PictureRequest>();
                    foreach (var file in files)
                    {
                        var fileData = await FileUtils.GetBytesAsync(file.File);
                        pictures.Add(new PictureRequest
                        {
                            BinaryData = fileData,
                            FileName = file.File.FileName,
                            ContentType = file.File.ContentType,
                            Id = file.PictureId.GetValueOrDefault()
                        });
                    }

                    farm.Pictures = pictures;
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
