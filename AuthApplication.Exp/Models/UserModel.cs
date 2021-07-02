using AuthApp.Core.Exp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Application.Exp.Models
{
    /* Model is controller-oriented, using for mapping informaiton coming from controller (which is also the client side)
    entity is design for database*/

    public class CreateUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType Type { get; set; }
        public UserSource Source { get; set; }
        public string Password { get; set; }
        public string Connection { get; set; }
        public Guid TenantPublicId { get; set; }
    }

    // Act like an interface
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Auth0Id { get; set; }
        public UserType Type { get; set; }
        public UserSource Source { get; set; }
        public DateTime DateCreated { get; set; }
    }

    // used for converting the output mode
    public static class UserModelMappings
    {
        public static UserModel ToUserModel(this User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                Type = user.Type,
                Source = user.Source,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateCreated = user.DateCreated
            };
        }

    }

}