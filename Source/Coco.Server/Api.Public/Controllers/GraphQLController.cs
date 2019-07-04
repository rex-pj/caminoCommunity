using Coco.Api.Framework.Controllers;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Controllers
{
    [ApiController]
    [Route("api/graphql")]
    public class GraphQLController : GraphQLBaseController
    {
        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter) :
            base(schema, documentExecuter)
        {
        }
    }
}
