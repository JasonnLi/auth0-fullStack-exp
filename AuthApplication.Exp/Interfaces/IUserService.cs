using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Application.Exp.Models;
using AuthApp.Core.Exp.Repositories;

namespace AuthApp.Application.Exp.Interfaces
{
    public interface IUserService
    {
        // Task<UserModel> GetUserById(int userId);

        Task<UserModel> CreateUser(IUserRepository userRepo, int tenantId, CreateUserModel user);

        Task<List<UserModel>> GetAllUsers(IUserRepository userRepo, int tenantId);

        Task<UserModel> GetUserById(IUserRepository userRepo, int tenantId, int userId);

        /*Task<List<UserModel>> GetAllUsersForCustomer(int customerId);

        Task AddRole(int userId, int roleId);

        Task DeleteUser(int userId);*/
    }
}