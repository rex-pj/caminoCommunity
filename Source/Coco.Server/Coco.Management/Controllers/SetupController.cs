using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.General;
using Coco.Framework.Providers.Contracts;
using Coco.Management.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Coco.Management.Controllers
{
    public class SetupController : Controller
    {
        private readonly ISeedDataBusiness _seedDataBusiness;
        private readonly ISetupProvider _setupProvider;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;
        public SetupController(ISeedDataBusiness seedDataBusiness, ISetupProvider setupProvider, IMapper mapper, IFileProvider fileProvider)
        {
            _setupProvider = setupProvider;
            _seedDataBusiness = seedDataBusiness;
            _mapper = mapper;
            _fileProvider = fileProvider;
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
            var settings = _setupProvider.LoadSettings();
            var identityDbScript = _fileProvider.ReadText(settings.CreateIdentityPath, Encoding.Default);
            _seedDataBusiness.SeedingIdentityDb(installDto, identityDbScript);

            var contentDbScript = _fileProvider.ReadText(settings.CreateContentDbPath, Encoding.Default);
            _seedDataBusiness.SeedingContentDb(installDto, contentDbScript);
            return View();
        }
    }
}