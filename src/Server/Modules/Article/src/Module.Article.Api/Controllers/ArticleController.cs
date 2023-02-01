using Camino.Application.Contracts.AppServices.Articles.Dtos;
using Camino.Application.Contracts.AppServices.Media.Dtos;
using Camino.Application.Contracts;
using Camino.Infrastructure.AspNetCore.Controllers;
using Camino.Shared.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module.Article.Api.Models;
using System;
using System.Threading.Tasks;
using Camino.Infrastructure.Identity.Attributes;
using Camino.Application.Contracts.AppServices.Articles;
using System.ComponentModel.DataAnnotations;
using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Article.Api.Controllers
{
    [Route("api/articles")]
    public class ArticleController : BaseTokenAuthController
    {
        private readonly IArticleAppService _articleAppService;
        public ArticleController(IHttpContextAccessor httpContextAccessor, IArticleAppService articleAppService) : base(httpContextAccessor)
        {
            _articleAppService = articleAppService;
        }

        [HttpPost]
        [TokenAuthentication]
        public async Task<IActionResult> CreateAsync([FromForm] CreateArticleModel request, PictureRequestModel picture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var article = new ArticleModifyRequest
                {
                    CreatedById = LoggedUserId,
                    UpdatedById = LoggedUserId,
                    Content = request.Content,
                    Name = request.Name,
                    ArticleCategoryId = request.ArticleCategoryId
                };

                if (picture?.File != null)
                {
                    var fileData = await FileUtils.GetBytesAsync(picture.File);
                    article.Picture = new PictureRequest()
                    {
                        BinaryData = fileData,
                        ContentType = picture.File.ContentType,
                        FileName = picture.File.FileName
                    };
                }

                var id = await _articleAppService.CreateAsync(article);
                return Ok(id);
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [HttpPut("{id}")]
        [TokenAuthentication]
        public async Task<IActionResult> UpdateAsync([Required] long id, [FromForm] UpdateArticleModel request, PictureRequestModel picture)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var exist = await _articleAppService.FindAsync(new IdRequestFilter<long>
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

                var article = new ArticleModifyRequest
                {
                    Id = id,
                    CreatedById = LoggedUserId,
                    UpdatedById = LoggedUserId,
                    Content = request.Content,
                    Name = request.Name,
                    ArticleCategoryId = request.ArticleCategoryId
                };

                if (picture?.File != null)
                {
                    var fileData = await FileUtils.GetBytesAsync(picture.File);
                    article.Picture = new PictureRequest
                    {
                        BinaryData = fileData,
                        ContentType = picture.File.ContentType,
                        FileName = picture.File.FileName,
                        Id = picture.PictureId.GetValueOrDefault()
                    };
                }

                await _articleAppService.UpdateAsync(article);
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

                var exist = await _articleAppService.FindAsync(new IdRequestFilter<long>
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

                await _articleAppService.SoftDeleteAsync(new ArticleModifyRequest
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
