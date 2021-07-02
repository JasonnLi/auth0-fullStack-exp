using AuthApp.Core.Exp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.Core.Exp.Repositories
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomerById(int id);
        public Task<Customer> GetCustomerByPublicId(Guid id);

        public Task<Customer> CreateCustomer(
            string applicationId,
            string environmentType,
            string orgId,
            string name);
    }
}