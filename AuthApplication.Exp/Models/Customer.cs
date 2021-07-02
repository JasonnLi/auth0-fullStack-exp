using AuthApp.Core.Exp.Entities;
using System;

namespace AuthApp.Application.Exp.Models
{
    public class CreateCustomerModelALM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Connection { get; set; }
    }

    public class CreateCustomerModel
    {
        public string ApplicationId { get; set; }
        public string EnvironmentType { get; set; }
        public string OrgId { get; set; }
        public string Name { get; set; }
        public CreateCustomerModelALM ALM { get; set; }
    }

    public class CustomerModel
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public string OrgId { get; set; }
        public string Name { get; set; }
    }

    public static class CustomerModelMappings
    {
        public static CustomerModel ToCustomerModel(this Customer customer)
        {
            return new CustomerModel()
            {
                Id = customer.Id,
                ApplicationId = customer.ApplicationId,
                OrgId = customer.OrgId,
                Name = customer.Name
            };
        }
    }
}