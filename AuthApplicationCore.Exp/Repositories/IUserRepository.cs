using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthApp.Core.Exp.Entities;

namespace AuthApp.Core.Exp.Repositories
{
    public class CreateUserOptions
    {
        public UserType Type { get; set; }
        public UserSource Source { get; set; }
        public string Auth0Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UpdateUserOptions
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public interface IUserRepository
    {

        Task<List<User>> GetAllUsers(int tenantId);

        Task<User> GetUserById(int tenantId, int userId);

        Task<User> CreateUser(int tenantId, CreateUserOptions options);

        Task DeleteUser(int tenantId, int userId);

        Task UpdateUser(int tenantId, int userId, UpdateUserOptions options);

        Task<Role> AddRole(Role role);

        Task<Permission> AddPermission(Permission permission);
    }
}
