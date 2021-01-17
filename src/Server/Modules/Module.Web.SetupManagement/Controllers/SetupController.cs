using AutoMapper;
using Camino.Service.Projections.Identity;
using Camino.Data.Enums;
using Camino.Framework.Providers.Contracts;
using Module.Web.SetupManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using Camino.IdentityManager.Models;
using Camino.IdentityManager.Contracts;
using Camino.Service.Business.Setup.Contracts;
using Camino.Service.Projections.Request;
using Camino.Core.Constants;

namespace Module.Web.SetupManagement.Controllers
{
    public class SetupController : Controller
    {
        private readonly IIdentityDataSetupBusiness _identityDataSetupBusiness;
        private readonly IContentDataSetupBusiness _contentDataSetupBusiness;
        private readonly ISetupProvider _setupProvider;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;
        private readonly IUserSecurityStampStore<ApplicationUser> _userSecurityStampStore;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IUserManager<ApplicationUser> _userManager;
        
        public SetupController(ISetupProvider setupProvider, IMapper mapper, 
            IFileProvider fileProvider, IUserSecurityStampStore<ApplicationUser> userSecurityStampStore, 
            IPasswordHasher<ApplicationUser> passwordHasher, IUserManager<ApplicationUser> userManager,
            IIdentityDataSetupBusiness identityDataSetupBusiness, IContentDataSetupBusiness contentDataSetupBusiness)
        {
            _setupProvider = setupProvider;
            _identityDataSetupBusiness = identityDataSetupBusiness;
            _mapper = mapper;
            _fileProvider = fileProvider;
            _userSecurityStampStore = userSecurityStampStore;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
            _contentDataSetupBusiness = contentDataSetupBusiness;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (_setupProvider.HasSetupDatabase)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new SetupModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(SetupModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (_setupProvider.HasSetupDatabase)
            {
                return RedirectToAction("Index", "Home");
            }

            var settings = _setupProvider.LoadSettings();

            try
            {
                // Create Identity database
                var identityDbScript = _fileProvider.ReadText(settings.CreateIdentityPath, Encoding.Default);
                _identityDataSetupBusiness.SeedingIdentityDb(identityDbScript);

                // Create Content database
                var contentDbScript = _fileProvider.ReadText(settings.CreateContentDbPath, Encoding.Default);
                _contentDataSetupBusiness.SeedingContentDb(contentDbScript);

                var initialUser = new ApplicationUser()
                {
                    DisplayName = $"{model.Lastname} {model.Firstname}",
                    Email = model.AdminEmail,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    StatusId = (byte)UserStatus.Actived,
                    UserName = model.AdminEmail,
                };

                initialUser.PasswordHash = _passwordHasher.HashPassword(initialUser, model.AdminPassword);
                await _userSecurityStampStore.SetSecurityStampAsync(initialUser, _userManager.NewSecurityStamp(), default);

                // Get Identity json data
                var indentityJson = _fileProvider.ReadText(settings.PrepareIdentityDataPath, Encoding.Default);
                var identitySetup = JsonConvert.DeserializeObject<SetupRequest>(indentityJson);
                identitySetup.InitualUser = _mapper.Map<UserProjection>(initialUser);

                // Initialize identity database
                await _identityDataSetupBusiness.PrepareIdentityDataAsync(identitySetup);

                // Get content json data
                var contentJson = _fileProvider.ReadText(settings.PrepareContentDataPath, Encoding.Default);
                var contentSetup = JsonConvert.DeserializeObject<SetupRequest>(contentJson);

                // Initialize content database
                await _contentDataSetupBusiness.PrepareContentDataAsync(contentSetup);

                _setupProvider.SetDatabaseHasBeenSetup();
                return RedirectToAction("Succeed");
            }
            catch(Exception ex)
            {
                _fileProvider.DeleteFile(SetupSettingsConst.FilePath);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Succeed()
        {
            return View();
        }
    }
}