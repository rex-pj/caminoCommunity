using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.General;
using Coco.Framework.Providers.Contracts;
using Coco.Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coco.Management.Controllers
{
    public class InstallationController : Controller
    {
        private readonly ISeedDataBusiness _seedDataBusiness;
        private readonly IInstallProvider _installProvider;
        private readonly IMapper _mapper;
        public InstallationController(ISeedDataBusiness seedDataBusiness, IInstallProvider installProvider, IMapper mapper)
        {
            _installProvider = installProvider;
            _seedDataBusiness = seedDataBusiness;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (_installProvider.IsDatabaseInstalled)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Index(InstallationViewModel installModel)
        {
            if (_installProvider.IsDatabaseInstalled)
            {
                return RedirectToAction("Index", "Home");
            }

            var installDto = _mapper.Map<InstallationDto>(installModel);
            _seedDataBusiness.SeedingIdentityDb(installDto);
            return View();
        }
    }
}