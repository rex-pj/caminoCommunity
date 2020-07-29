using HotChocolate.Types;

namespace Camino.Core.Modular.Contracts
{
    public abstract class BaseMutationType : ObjectType
    {
        public virtual void Register(IObjectTypeDescriptor descriptor)
        {
            this.Configure(descriptor);
        }
    }
}
