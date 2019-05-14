using Api.Auth.GraphQLTypes.InputTypes;
using Api.Auth.GraphQLTypes.ResultTypes;
using Api.Auth.Models;
using Coco.Api.Framework.Commons.Encode;
using Coco.Api.Framework.Models;
using Coco.Common.Const;
using Coco.Entities.Enums;
using GraphQL;
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
                    try
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

                        if (result.Errors != null && result.Errors.Any())
                        {
                            foreach (var error in result.Errors)
                            {
                                if(!context.Errors.Any() || !context.Errors.Any(x => error.Code.Equals(x.Code)))
                                {
                                    context.Errors.Add(new ExecutionError(error.Description) {
                                        Code = error.Code
                                    });
                                }
                            }
                        }

                        return result;
                    }
                    catch(Exception ex)
                    {
                        throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
                    }
                });
        }
    }
}
