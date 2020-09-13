using AutoMapper;
using Camino.Service.Projections.Request;
using Module.Web.SetupManagement.Models;

namespace Module.Web.SetupManagement.Infrastructure.AutoMap
{
    public class SetupMappingProfile : Profile
    {
        public SetupMappingProfile()
        {
            CreateMap<SetupModel, SetupRequest>();
        }
    }
}
