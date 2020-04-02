namespace Coco.Api.Framework.Controllers
{
    //[ApiController]
    //public abstract class GraphQLBaseController : BaseController
    //{
    //    protected readonly ISchema _schema;
    //    protected readonly IDocumentExecuter _documentExecuter;
    //    protected readonly ISessionContext _sessionContext;

    //    public GraphQLBaseController(ISchema schema, IDocumentExecuter documentExecuter, ISessionContext sessionContext) :
    //        base()
    //    {
    //        _schema = schema;
    //        _documentExecuter = documentExecuter;
    //        _sessionContext = sessionContext;
    //    }

    //    [HttpPost]
    //    [EnableCors("AllowOrigin")]
    //    public virtual async Task<IActionResult> Post([FromBody]GraphQLRequest query)
    //    {
    //        if (query == null)
    //        {
    //            throw new ArgumentNullException(nameof(query));
    //        }

    //        var inputs = query.Variables?.ToInputs();
    //        var executionOptions = new ExecutionOptions()
    //        {
    //            Schema = _schema,
    //            Query = query.Query,
    //            Inputs = inputs,
    //            UserContext = new Dictionary<string, object>
    //            {
    //                { "SessionContext", _sessionContext }
    //            }
    //        };

    //        var result = await _documentExecuter.ExecuteAsync(executionOptions);

    //        if (result.Errors != null && result.Errors.Any())
    //        {
    //            return BadRequest(result);
    //        }

    //        return Ok(result);
    //    }
    //}
}
