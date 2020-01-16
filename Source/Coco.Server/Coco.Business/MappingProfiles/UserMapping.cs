﻿using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.User;
using System;
using System.Linq.Expressions;

namespace Coco.Business.Mapping
{
    public static class UserMapping
    {
        public static Expression<Func<User, UserDto>> UserModelSelector = user => new UserDto
        {
            DisplayName = user.DisplayName,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            UpdatedDate = user.UpdatedDate,
            CreatedDate = user.CreatedDate,
            UpdatedById = user.UpdatedById,
            CreatedById = user.CreatedById,
            IsActived = user.IsActived,
            StatusId = user.StatusId,
            Email = user.Email,
            Password = user.Password,
            Id = user.Id,
            IsEmailConfirmed = user.IsEmailConfirmed,
            GenderId = user.UserInfo.GenderId,
            Address = user.UserInfo.Address,
            BirthDate = user.UserInfo.BirthDate,
            CountryId = user.UserInfo.CountryId,
            PhoneNumber = user.UserInfo.PhoneNumber,
            AvatarUrl = user.UserInfo.AvatarUrl,
            CoverPhotoUrl = user.UserInfo.CoverPhotoUrl,
        };

        public static Expression<Func<User, UserFullDto>> FullUserModelSelector = user => new UserFullDto
        {
            CreatedDate = user.CreatedDate,
            DisplayName = user.DisplayName,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            PhoneNumber = user.UserInfo.PhoneNumber,
            Description = user.UserInfo.Description,
            Address = user.UserInfo.Address,
            AvatarUrl = user.UserInfo.AvatarUrl,
            CoverPhotoUrl = user.UserInfo.CoverPhotoUrl,
            BirthDate = user.UserInfo.BirthDate,
            GenderId = user.UserInfo.GenderId,
            GenderLabel = user.UserInfo.Gender.Name,
            StatusId = user.StatusId,
            IsActived = user.IsActived,
            StatusLabel = user.Status.Name,
            Id = user.Id,
            CountryId = user.UserInfo.CountryId,
            CountryCode = user.UserInfo.Country.Code,
            CountryName = user.UserInfo.Country.Name
        };
    }
}