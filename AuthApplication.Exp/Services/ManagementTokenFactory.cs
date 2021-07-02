using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AuthApp.Application.Exp.Services
{
    public class ManagementTokenFactory : Auth0MachineTokenFactory
    {
        public ManagementTokenFactory(Auth0MachineTokenFactoryOptions externalAuthOptions) : base(externalAuthOptions)
        {
        }
    }
}