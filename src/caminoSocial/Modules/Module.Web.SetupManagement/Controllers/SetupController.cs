using AutoMapper;
using Camino.Business.Contracts;
using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using Camino.Data.Enums;
using Camino.Framework.Models;
using Camino.Framework.Providers.Contracts;
using Camino.Framework.SessionManager.Contracts;
using Module.Web.SetupManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Module.Web.SetupManagement.Controllers
{
    public class SetupController : Controller
    {
        private readonly ISeedDataBusiness _seedDataBusiness;
        private readonly ISetupProvider _setupProvider;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;
        private readonly IUserSecurityStampStore<ApplicationUser> _userSecurityStampStore;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IUserManager<ApplicationUser> _userManager;
        public SetupController(ISeedDataBusiness seedDataBusiness, ISetupProvider setupProvider, IMapper mapper, 
            IFileProvider fileProvider, IUserSecurityStampStore<ApplicationUser> userSecurityStampStore, 
            IPasswordHasher<ApplicationUser> passwordHasher, IUserManager<ApplicationUser> userManager)
        {
            _setupProvider = setupProvider;
            _seedDataBusiness = seedDataBusiness;
            _mapper = mapper;
            _fileProvider = fileProvider;
            _userSecurityStampStore = userSecurityStampStore;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (_setupProvider.HasSetupDatabase)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new SetupViewModel());
        }

        [HttpPost]
        public IActionResult Index(SetupViewModel model)
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
                _seedDataBusiness.SeedingIdentityDb(identityDbScript);

                // Create Content database
                var contentDbScript = _fileProvider.ReadText(settings.CreateContentDbPath, Encoding.Default);
                _seedDataBusiness.SeedingContentDb(contentDbScript);

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
                _userSecurityStampStore.SetSecurityStampAsync(initialUser, _userManager.NewSecurityStamp(), default);

                // Get Identity json data
                var indentityJson = _fileProvider.ReadText(settings.PrepareIdentityDataPath, Encoding.Default);
                var identitySetup = JsonConvert.DeserializeObject<SetupDto>(indentityJson);
                identitySetup.InitualUser = _mapper.Map<UserDto>(initialUser);

                // Initialize identity database
                _seedDataBusiness.PrepareIdentityData(identitySetup);

                // Get content json data
                var contentJson = _fileProvider.ReadText(settings.PrepareContentDataPath, Encoding.Default);
                var contentSetup = JsonConvert.DeserializeObject<SetupDto>(contentJson);

                // Initialize content database
                _seedDataBusiness.PrepareContentData(contentSetup);

                _setupProvider.SetDatabaseHasBeenSetup();
                return RedirectToAction("Succeed");
            }
            catch(Exception e)
            {
                _fileProvider.DeleteFile(settings.SetupUrl);
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