using System;
namespace UserManagement.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
    }
}

