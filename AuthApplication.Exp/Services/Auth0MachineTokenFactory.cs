using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AuthApp.Application.Exp.Services
{
    // interface for _externalAuthOptions (the management API)
    public class Auth0MachineTokenFactoryOptions
    {
        public string Domain { get; set; }
        public string Audience { get; set; }
        public string ManagementClientId { get; set; }
        public string ManagementClientSecret { get; set; }
        public int JwtExpirationInSeconds { get; set; }
    }

    public class Auth0MachineTokenFactory
    {
        private readonly Auth0MachineTokenFactoryOptions _externalAuthOptions;
        // Using http client to get M2M token via Auth0 management API
        private readonly HttpClient _client;

        // Time that the access token is retrieved
        private DateTimeOffset _tokenLastRetrieved;
        private TimeSpan _tokenExpiration;
        private string _accessToken;

        public string Domain => _externalAuthOptions.Domain;

        public Auth0MachineTokenFactory(Auth0MachineTokenFactoryOptions externalAuthOptions)
        {
            _externalAuthOptions = externalAuthOptions;
            _tokenExpiration = TimeSpan.FromSeconds(_externalAuthOptions.JwtExpirationInSeconds);
            _client = new HttpClient();
        }

        public async Task<string> GetTokenAsync(ILogger logger)
        {
            if (_externalAuthOptions == null)
            {
                return null;
            }

            // validate if there is existing and valid token
            if (!string.IsNullOrEmpty(_accessToken) && _tokenLastRetrieved + _tokenExpiration > DateTimeOffset.UtcNow)
            {
                return _accessToken;
            }

            var request = new Auth0TokenRequest
            {
                grant_type = "client_credentials",
                client_id = _externalAuthOptions.ManagementClientId,
                client_secret = _externalAuthOptions.ManagementClientSecret,
                audience = _externalAuthOptions.Audience
            };
            // convert to JsonObject payload
            var payload = JsonConvert.SerializeObject(request);
            logger.LogInformation(payload);

            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            // the client executes the post request with above request content and domain, to get token response
            var response = await _client.PostAsync($"https://{_externalAuthOptions.Domain}/oauth/token", content);
            response.EnsureSuccessStatusCode();
            var accessTokenJson = await response.Content.ReadAsStringAsync();
            var accessTokenResponse = JsonConvert.DeserializeObject<Auth0TokenResponse>(accessTokenJson);
            _accessToken = accessTokenResponse.access_token;
            _tokenExpiration = TimeSpan.FromSeconds(accessTokenResponse.expires_in);
            _tokenLastRetrieved = DateTimeOffset.UtcNow;
            return _accessToken;
        }

        private class Auth0TokenRequest
        {
            public string grant_type { get; set; }
            public string client_id { get; set; }
            public string client_secret { get; set; }
            public string audience { get; set; }
            public string scope { get; set; }
        }

        // M2M token response
        private class Auth0TokenResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; } // in seconds
            public string scope { get; set; }
            public string token_type { get; set; }
        }
    }
}