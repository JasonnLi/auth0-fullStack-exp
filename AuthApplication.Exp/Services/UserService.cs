using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Application.Exp.Interfaces;
using AuthApp.Application.Exp.Models;
using AuthApp.Core.Exp.Entities;
using AuthApp.Core.Exp.Repositories;

namespace AuthApp.Application.Exp.Services
{
    public class UserService : IUserService
    {
        
        private Auth0Service auth0Service;

        public UserService(Auth0Service auth0Service)
        {
            this.auth0Service = auth0Service;
        }

        public async Task<UserModel> CreateUser(
            IUserRepository userRepository,
            int tenantId,
            CreateUserModel createUserModel)
        {
            // create user in Auth0 user store
            var auth0User = await auth0Service.CreateUser(new Auth0Service.CreateUserOptions()
            {
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                Connection = createUserModel.Connection,
                Password = createUserModel.Password,
                TenantId = createUserModel.TenantPublicId.ToString()
            });

            // createUser() return the whole entity to user
            var user = await userRepository.CreateUser(tenantId, new CreateUserOptions()
            {
                Type = createUserModel.Type,
                Source = createUserModel.Source,
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                Auth0Id = auth0User.UserId
            });

            return user.ToUserModel();
        }

        public async Task<UserModel> GetUserById(IUserRepository userRepository, int tenantId, int userId)
        {
            var user = await userRepository.GetUserById(tenantId, userId);
            return user.ToUserModel();
        }

        public async Task<List<UserModel>> GetAllUsers(IUserRepository userRepository, int tenantId)
        {
            var users = await userRepository.GetAllUsers(tenantId);
            return users.ConvertAll(user => user.ToUserModel());
        }
    }
}