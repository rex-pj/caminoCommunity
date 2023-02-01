using Camino.Application.Contracts;
using Camino.Application.Contracts.AppServices.Products;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Shared.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Module.Product.Api.Models;
using Camino.Application.Contracts.AppServices.Products.Dtos;
using System.Collections.Generic;
using System.Linq;
using Camino.Infrastructure.AspNetCore.Models;
using System.ComponentModel.DataAnnotations;

namespace Module.Product.Api.Controllers
{
    [Route("api/products")]
    public class ProductController : BaseTokenAuthController
    {
        private readonly IProductAppService _farmAppService;
        public ProductController(IHttpContextAccessor httpContextAccessor, IProductAppService farmAppService) : base(httpContextAccessor)
        {
            _farmAppService = farmAppService;
        }

        [HttpPost]
        [TokenAuthentication]
        public async Task<IActionResult> CreateAsync(CreateProductModel request, IEnumerable<PictureRequestModel> pictures)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var product = new ProductModifyRequest()
                {
                    CreatedById = LoggedUserId,
                    UpdatedById = LoggedUserId,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Categories = request.Categories.Select(x => new ProductCategoryRequest
                    {
                        Id = x.Id
                    }),
                    Farms = request.Farms.Select(x => new ProductFarmRequest
                    {
                        FarmId = x.Id
                    }),
                    ProductAttributes = request.ProductAttributes.Select(x => new ProductAttributeRelationRequest
                    {
                        ControlTypeId = x.ControlTypeId,
                        DisplayOrder = x.DisplayOrder,
                        ProductAttributeId = x.AttributeId,
                        TextPrompt = x.TextPrompt ?? "",
                        AttributeRelationValues = x.AttributeRelationValues.Select(c => new ProductAttributeRelationValueRequest
                        {
                            DisplayOrder = c.DisplayOrder,
                            Name = c.Name,
                            PriceAdjustment = c.PriceAdjustment,
                            PricePercentageAdjustment = c.PricePercentageAdjustment,
                            Quantity = c.Quantity
                        })
                    })
                };

                if (pictures?.Any() ?? false)
                {
                    var files = new List<PictureRequest>();
                    foreach (var file in pictures)
                    {
                        var fileData = file.File != null ? await FileUtils.GetBytesAsync(file.File) : null;
                        files.Add(new PictureRequest
                        {
                            BinaryData = fileData,
                            FileName = file.File?.FileName,
                            ContentType = file.File?.ContentType
                        });
                    }

                    product.Pictures = files;
                }

                var id = await _farmAppService.CreateAsync(product);
                return Ok(id);
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPut("{id}")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateAsync([Required] long id, [FromForm] UpdateProductModel request, IEnumerable<PictureRequestModel> pictures)
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

                var product = new ProductModifyRequest
                {
                    Id = id,
                    CreatedById = LoggedUserId,
                    UpdatedById = LoggedUserId,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Categories = request.Categories.Select(x => new ProductCategoryRequest()
                    {
                        Id = x.Id
                    }),
                    Farms = request.Farms.Select(x => new ProductFarmRequest()
                    {
                        FarmId = x.Id
                    }),
                    ProductAttributes = request.ProductAttributes?.Select(x => new ProductAttributeRelationRequest
                    {
                        Id = x.Id.GetValueOrDefault(),
                        ControlTypeId = x.ControlTypeId,
                        DisplayOrder = x.DisplayOrder,
                        ProductAttributeId = x.AttributeId,
                        TextPrompt = x.TextPrompt,
                        AttributeRelationValues = x.AttributeRelationValues?.Select(c => new ProductAttributeRelationValueRequest
                        {
                            Id = c.Id.GetValueOrDefault(),
                            DisplayOrder = c.DisplayOrder,
                            Name = c.Name,
                            PriceAdjustment = c.PriceAdjustment,
                            PricePercentageAdjustment = c.PricePercentageAdjustment,
                            Quantity = c.Quantity
                        })
                    })
                };

                if (pictures?.Any() ?? false)
                {
                    var files = new List<PictureRequest>();
                    foreach (var file in pictures)
                    {
                        var fileData = file.File != null ? await FileUtils.GetBytesAsync(file.File) : null;
                        files.Add(new PictureRequest
                        {
                            BinaryData = fileData,
                            FileName = file.File?.FileName,
                            ContentType = file.File?.ContentType,
                            Id = file.PictureId.GetValueOrDefault()
                        });
                    }

                    product.Pictures = files;
                }

                await _farmAppService.UpdateAsync(product);
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

                await _farmAppService.SoftDeleteAsync(id, LoggedUserId);

                return Ok();
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
