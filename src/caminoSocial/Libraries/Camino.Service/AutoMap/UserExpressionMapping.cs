using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.Identity;
using System;
using System.Linq.Expressions;

namespace Camino.Service.AutoMap
{
    public static class UserExpressionMapping
    {
        public static Expression<Func<User, UserProjection>> UserModelSelector { get; } = user => new UserProjection
        {
            DisplayName = user.DisplayName,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            UserName = user.UserName,
            UpdatedDate = user.UpdatedDate,
            CreatedDate = user.CreatedDate,
            UpdatedById = user.UpdatedById,
            CreatedById = user.CreatedById,
            StatusId = user.StatusId,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            SecurityStamp = user.SecurityStamp,
            Id = user.Id,
            IsEmailConfirmed = user.IsEmailConfirmed,
            GenderId = user.UserInfo.GenderId,
            Address = user.UserInfo.Address,
            BirthDate = user.UserInfo.BirthDate,
            CountryId = user.UserInfo.CountryId,
            PhoneNumber = user.UserInfo.PhoneNumber,
        };
    }
}
