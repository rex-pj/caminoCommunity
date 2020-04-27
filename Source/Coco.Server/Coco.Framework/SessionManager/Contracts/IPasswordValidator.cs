﻿using Coco.Framework.Models;
using System.Threading.Tasks;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface IPasswordValidator<TUser> where TUser : class
    {
        Task<ICommonResult> ValidateAsync(IUserManager<ApplicationUser> manager, ApplicationUser user, string password);
    }
}
