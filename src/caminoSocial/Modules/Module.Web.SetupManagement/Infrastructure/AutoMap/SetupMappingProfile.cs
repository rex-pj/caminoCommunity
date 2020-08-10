using AutoMapper;
using Camino.Business.Dtos.General;
using Module.Web.SetupManagement.Models;

namespace Module.Web.SetupManagement.Infrastructure.AutoMap
{
    public class SetupMappingProfile : Profile
    {
        public SetupMappingProfile()
        {
            CreateMap<SetupModel, SetupDto>();
        }
    }
}
