using Camino.Core.Constants;
using HotChocolate;

namespace Camino.Framework.GraphQL.Attributes
{
    public class ApplicationUserStateAttribute : GlobalStateAttribute
    {
        public ApplicationUserStateAttribute() : base(SessionContextConst.CURRENT_USER)
        {

        }
    }
}
