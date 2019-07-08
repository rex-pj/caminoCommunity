using System.Threading.Tasks;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.Controllers;
using Coco.Api.Framework.GraphQLQueries;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Controllers
{
    [Route("api/graphql")]
    [ApiController]
    public class GraphQLController : GraphQLBaseController
    {
        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, IWorkContext workContext) :
            base(schema, documentExecuter, workContext)
        {
        }

        [Route("")]
        [EnableCors("AllowOrigin")]
        public override Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            return base.Post(query);
        }
    }
}
