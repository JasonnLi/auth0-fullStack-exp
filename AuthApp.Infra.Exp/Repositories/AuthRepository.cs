using AuthApp.Core.Exp.Entities;
using AuthApp.Infra.Exp.Data;
using System.Threading.Tasks;

namespace AuthApp.Infra.Exp.Repositories
{
    public class AuthRepository
    {
        private readonly AuthAppContext _context;

        public AuthRepository(AuthAppContext context)
        {
            this._context = context;
        }

        public async Task<AuthConnection> CreateConnection(string applicationId, int customerId, string name, string environmentType)
        {
            /*return context.AuthConnection.FirstOrDefaultAsync(ac => ac.ApplicationId == applicationId
            && ac.CustomerId == customerId
            && ac.ConnectionName == name);*/

            AuthConnection authConnection = new AuthConnection()
            {
                ApplicationId = applicationId,
                CustomerId = customerId,
                EnvironmentType = environmentType,
                ConnectionName = name,
            };
            var authConnectionEntity = await this._context.AuthConnection.AddAsync(authConnection);
            await _context.SaveChangesAsync();
            return authConnectionEntity.Entity;
        }
    }
}