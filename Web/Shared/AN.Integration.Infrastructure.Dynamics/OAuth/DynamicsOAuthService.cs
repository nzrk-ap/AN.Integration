using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using AN.Integration.DynamicsCore.DynamicsTooling.OAuth;

namespace AN.Integration.Infrastructure.Dynamics.OAuth
{
    public sealed class DynamicsOAuthService
    {
        private readonly IOptions<ClientOptions> _clientOptions;
        private readonly string _authority;

        public DynamicsOAuthService(IOptions<ClientOptions> clientOptions)
        {
            _clientOptions = clientOptions;
            _authority = "https://login.microsoftonline.com/" +
                         $"{_clientOptions.Value.DirectoryId}";
        }

        public async Task<AuthenticationResult> GetAccessTokenAsync()
        {
            var authContext = new AuthenticationContext(_authority, false);
            var clientCred = new ClientCredential(_clientOptions.Value.ClientId,
                _clientOptions.Value.ClientSecret);
            return await authContext.AcquireTokenAsync(_clientOptions.Value.Resource, clientCred);
        }
    }
}