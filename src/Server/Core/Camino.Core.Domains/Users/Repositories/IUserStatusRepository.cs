namespace Camino.Core.Domains.Users.Repositories
{
    public interface IUserStatusRepository
    {
        Task<int> CreateAsync(Status status);
        Status Find(int id);
        Status FindByName(string name);
        Task<bool> UpdateAsync(Status status);
    }
}
