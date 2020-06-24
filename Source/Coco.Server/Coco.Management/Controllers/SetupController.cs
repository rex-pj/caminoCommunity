using AutoMapper;
using Coco.Business.Contracts;
using Coco.Entities.Dtos.General;
using Coco.Entities.Dtos.User;
using Coco.Entities.Enums;
using Coco.Framework.Models;
using Coco.Framework.Providers.Contracts;
using Coco.Framework.SessionManager.Contracts;
using Coco.Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Coco.Management.Controllers
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

            return View();
        }

        [HttpPost]
        public IActionResult Index(SetupViewModel setupModel)
        {
            if (_setupProvider.HasSetupDatabase)
            {
                return RedirectToAction("Index", "Home");
            }

            var settings = _setupProvider.LoadSettings();

            // Create Identity database
            var identityDbScript = _fileProvider.ReadText(settings.CreateIdentityPath, Encoding.Default);
            _seedDataBusiness.SeedingIdentityDb(identityDbScript);

            // Create Content database
            var contentDbScript = _fileProvider.ReadText(settings.CreateContentDbPath, Encoding.Default);
            _seedDataBusiness.SeedingContentDb(contentDbScript);

            var initialUser = new ApplicationUser()
            {
                BirthDate = DateTime.UtcNow.AddYears(-10),
                DisplayName = $"Trung Le",
                Email = setupModel.AdminEmail,
                Firstname = "Le",
                Lastname = "Trung",
                StatusId = (byte)UserStatusEnum.New,
                UserName = setupModel.AdminEmail,
            };

            initialUser.PasswordHash = _passwordHasher.HashPassword(initialUser, setupModel.AdminPassword);
            _userSecurityStampStore.SetSecurityStampAsync(initialUser, _userManager.NewSecurityStamp(), default);

            // Get Identity json data
            var indentityJson = _fileProvider.ReadText(settings.PrepareIdentityDataPath, Encoding.Default);
            var setupDto = JsonConvert.DeserializeObject<SetupDto>(indentityJson);
            setupDto.InitualUser = _mapper.Map<UserDto>(initialUser);

            _seedDataBusiness.PrepareIdentityData(setupDto);
            _setupProvider.SetDatabaseHasBeenSetup();
            return RedirectToAction("Login", "Authentication");
        }
    }
}