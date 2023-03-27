// =============================================================================
// AuthenticationOauth2.cs
// =============================================================================
using DotnetCuritiba.AuthLibs.Models;
using Newtonsoft.Json;
using RestSharp;

namespace DotnetCuritiba.AuthLibs.Authentication
{
    public class AuthenticationOauth2
    {
        private readonly string _baseUrl;
        private readonly string _clientId;
        private readonly string _scope;
        private readonly string _token;
        
        public AuthenticationOauth2(string baseUrl, string clientId, string scope, string token)
        {
            _baseUrl = baseUrl;
            _clientId = clientId;
            _scope = scope;
            _token = token;
        }

        public async Task<string> AuthenticationAsync()
        {
            var options = new RestClientOptions
            {
                MaxTimeout = 10000
            };

            using var client = new RestClient(options);

            var request = new RestRequest(_baseUrl, Method.Post)
                .AddHeader("content-type", "application/x-www-form-urlencoded")
                .AddParameter("client_id", _clientId)
                .AddParameter("grant_type", "client_credentials")
                .AddParameter("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer")
                .AddParameter("client_assertion", _token)
                .AddParameter("scope", _scope);

            request.Timeout = 30000;

            var response = await client.ExecuteAsync(request, default).ConfigureAwait(false);
            
            if (!response.IsSuccessful && response.ErrorMessage != null)
                return response.ErrorMessage;
            
            TokenResponse token = new();

            if (response.Content != null)
                token = JsonConvert.DeserializeObject<TokenResponse>(response.Content) ?? new TokenResponse();

            return token.AccessToken;
        }
    }
}