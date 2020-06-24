using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.General;
using Coco.Framework.Providers.Contracts;
using Coco.Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class SetupController : Controller
    {
        private readonly ISeedDataBusiness _seedDataBusiness;
        private readonly ISetupProvider _setupProvider;
        private readonly IMapper _mapper;
        public SetupController(ISeedDataBusiness seedDataBusiness, ISetupProvider setupProvider, IMapper mapper)
        {
            _setupProvider = setupProvider;
            _seedDataBusiness = seedDataBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (_setupProvider.HasSetupDatabase)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Index(SetupViewModel installModel)
        {
            if (_setupProvider.HasSetupDatabase)
            {
                return RedirectToAction("Index", "Home");
            }

            var installDto = _mapper.Map<SetupDto>(installModel);
            _seedDataBusiness.SeedingIdentityDb(installDto);
            return View();
        }
    }
}