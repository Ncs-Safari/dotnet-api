
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(
        IUserRepository userRepository
    )
    {
        Users = userRepository;
    }
    public IUserRepository Users { get; set; }
}
