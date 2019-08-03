using Coco.Api.Framework.UserIdentity.Contracts;
using Coco.Api.Framework.Controllers;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace Api.Public.Controllers
{
    [Route("api/graphql")]
    [ApiController]
    public class GraphQLController : GraphQLBaseController
    {
        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ISessionContext sessionContext) :
            base(schema, documentExecuter, sessionContext)
        {
        }
    }
}
