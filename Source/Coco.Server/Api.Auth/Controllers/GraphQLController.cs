using Microsoft.AspNetCore.Mvc;
using Coco.Api.Framework.Controllers;
using GraphQL;
using GraphQL.Types;

namespace Api.Auth.Controllers
{
    [ApiController]
    [Route("api/graphql")]
    public class GraphQLController : BaseGraphQLController
    {
        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter) :
            base(schema, documentExecuter)
        {
        }
    }
}
