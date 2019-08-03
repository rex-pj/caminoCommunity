using Coco.Api.Framework.UserIdentity.Contracts;
using Coco.Api.Framework.Attributes;
using Coco.Api.Framework.Controllers;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace Api.Identity.Controllers
{
    [Route("api/graphql")]
    [ApiController]
    [AuthenticationUser]
    public class GraphQLController : GraphQLBaseController
    {
        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ISessionContext sessionContext) :
            base(schema, documentExecuter, sessionContext)
        {
        }
    }
}
