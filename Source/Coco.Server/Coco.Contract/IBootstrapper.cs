using Microsoft.Extensions.DependencyInjection;

namespace Coco.Contract
{
    public interface IBootstrapper
    {
        void RegiserTypes(IServiceCollection services);
    }
}
