﻿using Camino.Framework.GraphQL.Attributes;
using Camino.Framework.GraphQL.Mutations;
using Camino.Core.Domain.Identities;
using HotChocolate;
using HotChocolate.Types;
using Module.Api.Product.GraphQL.Resolvers.Contracts;
using Module.Api.Product.Models;
using System.Threading.Tasks;

namespace Module.Api.Product.GraphQL.Mutations
{
    [ExtendObjectType("Mutation")]
    public class ProductMutations : BaseMutations
    {
        [GraphQlAuthentication]
        public async Task<ProductModel> CreateProductAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IProductResolver productResolver,
            ProductModel criterias)
        {
            return await productResolver.CreateProductAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<ProductModel> UpdateProductAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IProductResolver productResolver,
            ProductModel criterias)
        {
            return await productResolver.UpdateProductAsync(currentUser, criterias);
        }

        [GraphQlAuthentication]
        public async Task<bool> DeleteProductAsync([ApplicationUserState] ApplicationUser currentUser, [Service] IProductResolver productResolver,
            ProductFilterModel criterias)
        {
            return await productResolver.DeleteProductAsync(currentUser, criterias);
        }
    }
}
