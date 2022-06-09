namespace Camino.Core.Domains.Products.DomainServices
{
    public interface IProductPriceDomainService
    {
        Task<bool> UpdateProductPriceAsync(long productId, decimal price, bool needSaveChanges = false);
    }
}
