using System;
using Microsoft.AspNetCore.Mvc;
using Coco.Api.Framework.Controllers;
using System.Threading.Tasks;
using Api.Auth.GraphQLQueries;
using GraphQL;
using GraphQL.Types;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace Api.Auth.Controllers
{
    [ApiController]
    [Route("api/graphql")]
    public class GraphQLController : BaseController
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _documentExecuter;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter) :
            base()
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> Post([FromBody]GraphQLQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

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
