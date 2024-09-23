
using Common.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUser(string idOrUsernameOrEmail);
        Task<IEnumerable<ApplTask>> GetMenus(string languageId, string applId, string userId);
    }
}

