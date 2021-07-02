using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Application.Controllers;
using AuthApp.Application.Exp.Extensions;
using AuthApp.Application.Exp.Interfaces;
using AuthApp.Application.Exp.Models;
using AuthApp.Application.Exp.Services;
using AuthApp.Core.Exp.Entities;
using AuthApp.Infra.Exp.Data;
using AuthApp.Infra.Exp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthApp.Application.Exp.Controllers.ApiController
{
    public class PostUserBody
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserBody
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [Authorize]
    [ApiController]
    [Route("/users")]
    public class UserController : ApiControllerBase
    {
        private readonly AuthAppContext context;
        private readonly Auth0Service auth0Service;
        private readonly IUserService userService;
        private readonly ILogger logger;

        public UserController(
            AuthAppContext context,
            IUserService userService,
            Auth0Service auth0Service,
            ILogger<UserController> logger)
        {
            this.context = context;
            this.userService = userService;
            this.auth0Service = auth0Service;
            this.logger = logger;
        }

        [HttpPost("")]
        public async Task<IActionResult> PostUser([FromBody] PostUserBody body)
        {
            // verifiy if an authorized user doing the following action
            var customer = await User.GetCustomer(context);
            if (customer == null)
                return Unauthorized();

            var curUser = await User.GetUser(context);
            if (curUser == null)
                return Unauthorized();

            // Only ALMs can create users for now
            if (curUser.Type != UserType.ALM)
                return Unauthorized();

            var customerRepo = new CustomerRepository(context);
            var authConName = await customerRepo.GetAuthConnectionName(customer.Id);
            if (authConName == null)
                return BadRequest();

            var userRepo = new UserRepository(context);
            var user = await userService.CreateUser(userRepo, customer.Id, new CreateUserModel()
            {
                Email = body.Email,
                FirstName = body.FirstName,
                LastName = body.LastName,
                Type = UserType.Standard,
                Source = UserSource.Internal,
                Password = body.Password,
                Connection = authConName,
                TenantPublicId = customer.PublicId
            });

            return Ok(user);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var customer = await User.GetCustomer(context);
            if (customer == null)
            {
                logger.LogInformation("Customer not found!");
                return BadRequest();
            }
            logger.LogInformation("CustomerId: " + customer?.Id);

            var userRepo = new UserRepository(context);
            var users = await userService.GetAllUsers(userRepo, customer.Id);

            // for the purpose of completing a task and return a successful status
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var customer = await User.GetCustomer(context);
            if (customer == null)
                return Unauthorized();

            var userRepo = new UserRepository(context);
            var user = await userService.GetUserById(userRepo, customer.Id, userId);

            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserBody body)
        {
            var customer = await User.GetCustomer(context);
            if (customer == null)
                return Unauthorized();

            // @TODO Permission: Can update user

            var userRepo = new UserRepository(context);

            var user = await userService.GetUserById(userRepo, customer.Id, userId);
            if (user == null)
                return NotFound();

            if (user.Source == UserSource.IdentityProvider)
                return BadRequest();

            logger.LogInformation("UpdateUser - FirstName: " + body.FirstName + ", LastName: " + body.LastName);

            await auth0Service.UpdateUser(user.Auth0Id, new Auth0Service.UpdateUserOptions()
            {
                FirstName = body.FirstName,
                LastName = body.LastName
            });

            await userRepo.UpdateUser(customer.Id, userId, new Core.Exp.Repositories.UpdateUserOptions()
            {
                FirstName = body.FirstName,
                LastName = body.LastName
            });

            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var customer = await User.GetCustomer(context);
            if (customer == null)
                return Unauthorized();

            var curUser = await User.GetUser(context);
            if (curUser == null)
                return Unauthorized();

            // Only ALMs can delete users for now
            if (curUser.Type != UserType.ALM)
                return Unauthorized();

            var userRepo = new UserRepository(context);
            var user = await userService.GetUserById(userRepo, customer.Id, userId);
            if (user == null)
                return NotFound();

            // delete action is created in the user repository
            // @TODO Need to delete results etc?
            await userRepo.DeleteUser(customer.Id, userId);
            await auth0Service.DeleteUser(user.Auth0Id);
            return Ok();
        }
    }
}
