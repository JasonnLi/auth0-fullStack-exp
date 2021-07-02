using Microsoft.EntityFrameworkCore;
using AuthApp.Core.Exp.Entities;
using AuthApp.Core.Exp.Repositories;
using AuthApp.Infra.Exp.Data;
using System;
using System.Threading.Tasks;

namespace AuthApp.Infra.Exp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AuthAppContext _context;

        public CustomerRepository(AuthAppContext context)
        {
            this._context = context;
        }

        public async Task<Customer> CreateCustomer(
            string applicationId,
            string environmentType,
            string orgId,
            string name)
        {
            Customer customer = new Customer()
            {
                ApplicationId = applicationId,
                EnvironmentType = environmentType,
                OrgId = orgId,
                Name = name
            };
            var customerEntity = await this._context.Customer.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customerEntity.Entity;
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await this._context.Customer.FindAsync(id);
        }

        public async Task<Customer> GetCustomerByPublicId(Guid id)
        {
            return await this._context.Customer.FirstOrDefaultAsync(c => c.PublicId == id);
        }

        public async Task<Customer> GetCustomerFromAuth0Connection(string connectionName)
        {
            var con = await this._context.AuthConnection.FirstOrDefaultAsync(a => a.ConnectionName == connectionName);
            if (con == null)
            {
                return null;
            }
            // mutiple and nested DB request
            var customer = await GetCustomerById(con.CustomerId);
            return customer;
        }

        public async Task<string> GetAuthConnectionName(int customerId)
        {
            var customer = await GetCustomerById(customerId);
            if (customer == null)
                return null;
            var authCon = await _context.AuthConnection.FirstOrDefaultAsync(a => a.CustomerId == customerId);
            if (authCon == null)
                return null;
            return authCon.ConnectionName;
        }
    }
}