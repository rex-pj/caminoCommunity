using Api.Auth.GraphQLTypes;
using Api.Auth.Models;
using Coco.Api.Framework.Commons.Encode;
using Coco.Api.Framework.Models;
using Coco.Entities.Enums;
using GraphQL.Types;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Api.Auth.GraphQLMutations
{
    public class AccountMutation : ObjectGraphType
    {
        public AccountMutation(UserManager<ApplicationUser> userManager)
        {
            FieldAsync(typeof(RegisterResultType), "adduser",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<RegisterInputType>> { Name = "user" }),
                resolve: async context => 
                {
                    var model = context.GetArgument<RegisterModel>("user");

                    var parameters = new ApplicationUser()
                    {
                        BirthDate = model.BirthDate,
                        CreatedDate = DateTime.Now,
                        DisplayName = $"{model.Lastname} {model.Firstname}",
                        Email = model.Email,
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        GenderId = (byte)model.GenderId,
                        StatusId = (byte)UserStatusEnum.IsPending,
                        UpdatedDate = DateTime.Now,
                        UserName = model.Email,
                        PasswordSalt = SaltGenerator.GetSalt()
                    };

                    var result = await userManager.CreateAsync(parameters, model.Password);

                    return result;
                });
        }
    }
}
