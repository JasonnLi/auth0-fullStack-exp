using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Core.Exp.Entities;
using AuthApp.Core.Exp.Repositories;
using AuthApp.Infra.Exp.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Infra.Exp.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AuthAppContext _context;

        public UserRepository(AuthAppContext context)
        {
            this._context = context;
        }

         public async Task<User> CreateUser(int tenantId, CreateUserOptions options)
        {
            var newUser = new User()
            {
                CustomerId = tenantId,
                Auth0Id = options.Auth0Id,
                Email = options.Email,
                FirstName = options.FirstName,
                LastName = options.LastName,
                Type = options.Type,
                Source = options.Source
            };

            var createdUser = await this._context.User.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return createdUser.Entity;
        }

        public async Task<List<User>> GetAllUsers(int tenantId)
        {
            return await _context.User
                .Where(u => u.CustomerId == tenantId).ToListAsync();
        }

        public async Task<User> GetUserById(int tenantId, int userId)
        {
            return await this._context.User
                .FirstOrDefaultAsync(u =>
                u.CustomerId == tenantId &&
                u.Id == userId);
        }

        public async Task<User> GetUserByAuth0Id(int tenantId, string auth0Id)
        {
            return await this._context.User
                .FirstOrDefaultAsync(u =>
                    u.CustomerId == tenantId &&
                    u.Auth0Id == auth0Id);
        }

        public async Task UpdateUser(int tenantId, int userId, UpdateUserOptions options)
        {
            var user = await GetUserById(tenantId, userId);
            user.FirstName = options.FirstName;
            user.LastName = options.LastName;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int tenantId, int userId)
        {
            var user = await GetUserById(tenantId, userId);
            _ = _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Role> AddRole(Role role)
        {
            var newRole = new Role()
            {
                Name = role.Name,
            };

            var createdRole = await this._context.Role.AddAsync(newRole);
            await _context.SaveChangesAsync();
            return createdRole.Entity;
        }

        public async Task<Permission> AddPermission(Permission permission)
        {
            var newPermission = new Permission()
            {
                Action = permission.Action
            };

            var createdPermission = await this._context.Permission.AddAsync(newPermission);
            await _context.SaveChangesAsync();
            return createdPermission.Entity;
        }

    }
}
