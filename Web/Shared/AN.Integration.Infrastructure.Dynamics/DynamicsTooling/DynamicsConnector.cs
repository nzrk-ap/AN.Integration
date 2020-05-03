using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using AN.Integration.DynamicsCore.DynamicsTooling.OAuth;
using AN.Integration.Infrastructure.Dynamics.DynamicsTooling.Api;
using AN.Integration.Infrastructure.Dynamics.OAuth;
using Microsoft.Extensions.Logging;

namespace AN.Integration.Infrastructure.Dynamics.DynamicsTooling
{
    public sealed class DynamicsConnector : IDynamicsConnector
    {
        private readonly IOptions<ClientOptions> _options;
        private readonly ILogger<DynamicsConnector> _logger;
        private readonly IRequestConverter _converter;
        private readonly HttpClient _httpClient;

        public DynamicsConnector(IOptions<ClientOptions> options, ILogger<DynamicsConnector> logger,
            IRequestConverter converter, DynamicsOAuthService oAuthService)
        {
            _options = options;
            _converter = converter;
            _logger = logger;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Resource)
            };
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                oAuthService.GetAccessTokenAsync().Result.AccessToken);
        }

        public async Task UpsertAsync(ApiRequest request)
        {
            var requestUri = $"/api/data/v{_options.Value.ApiVersion}/" +
                             $"{request.EntityName}s({request.RecordId ?? Guid.NewGuid()})";
            var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = ToContent(_converter.ToJSon(request))
            };
            var result = await _httpClient.SendAsync(patchRequest);
            var response = await ReadResponse(result);
            _logger.LogInformation($"{result.StatusCode}\n{response}");
        }

        public async Task DeleteAsync(ApiRequest request)
        {
            var recordId = request.RecordId ??
                           throw new ArgumentNullException(nameof(request.RecordId));
            var requestUri = $"/api/data/v{_options.Value.ApiVersion}/" +
                             $"{request.EntityName}s({recordId})";
            var result = await _httpClient.DeleteAsync(requestUri);
            _logger.LogInformation($"{result.StatusCode}\n{ReadResponse(result)}");
        }

        private static StringContent ToContent(string json) =>
            new StringContent(json, Encoding.UTF8, "application/json");

        private static async Task<string> ReadResponse(HttpResponseMessage response) =>
            await response.Content.ReadAsStringAsync();
    }
}