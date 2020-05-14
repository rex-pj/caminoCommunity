﻿using AutoMapper;
using Coco.Business.Contracts;
using Coco.Framework.Controllers;
using Coco.Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Management.Controllers
{
    public class UserController : BaseAuthController
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper, IUserBusiness userBusiness, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _userBusiness = userBusiness;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _userBusiness.GetFull();
            var userModels = _mapper.Map<List<UserViewModel>>(users);
            var userPage = new PagerViewModel<UserViewModel>(userModels);

            return View(userPage);
        }

        [HttpGet]
        public IActionResult Search(string q)
        {
            var users = _userBusiness.Search(q);
            if (users == null || !users.Any())
            {
                return Json(new
                {
                    Items = new List<Select2Item>()
                });
            }

            var userModels = _mapper.Map<List<UserViewModel>>(users)
                .Select(x => new Select2Item
                {
                    Id = x.Id.ToString(),
                    Text = x.Lastname + " " + x.Firstname
                });

            return Json(userModels);
        }
    }
}