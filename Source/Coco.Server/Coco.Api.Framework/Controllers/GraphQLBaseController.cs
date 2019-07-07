using Coco.Api.Framework.AccountIdentity;
using Coco.Api.Framework.Commons.Helpers;
using Coco.Api.Framework.GraphQLQueries;
using Coco.Api.Framework.GraphQLTypes;
using Coco.Business.Contracts;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Coco.Api.Framework.Controllers
{
    [ApiController]
    public abstract class GraphQLBaseController : BaseController
    {
        protected readonly ISchema _schema;
        protected readonly IDocumentExecuter _documentExecuter;

        public GraphQLBaseController(ISchema schema, IDocumentExecuter documentExecuter) :
            base()
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        [EnableCors("AllowOrigin")]
        public virtual async Task<IActionResult> Post([FromBody]GraphQLQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            var headers = HttpHelper.GetAuthorizationHeaders(this.HttpContext);

            await _schema.ExecuteAsync(_=> {
                _.Query = "...";
                _.UserContext = new GraphQLUserContext();
            });

            var inputs = query.Variables?.ToInputs();
            var executionOptions = new ExecutionOptions()
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs,
                UserContext = this.HttpContext
            };

            var result = await _documentExecuter.ExecuteAsync(executionOptions);

            if (result.Errors != null && result.Errors.Any())
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
