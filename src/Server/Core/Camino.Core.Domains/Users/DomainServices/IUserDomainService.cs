namespace Camino.Core.Domains.Users.DomainServices
{
    public interface IUserDomainService
    {
        Task<bool> ConfirmAsync(long id, long updatedById);
    }
}
