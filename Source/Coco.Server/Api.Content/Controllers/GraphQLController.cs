using Coco.Api.Framework.Attributes;
using Coco.Api.Framework.Controllers;
using Coco.Api.Framework.SessionManager.Contracts;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace Api.Content.Controllers
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