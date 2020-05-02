using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AN.Integration.DynamicsCore.Api;
using AN.Integration.DynamicsCore.DynamicsTooling.OAuth;

namespace AN.Integration.DynamicsCore.DynamicsTooling
{
    public sealed class DynamicsConnector : IDynamicsConnector
    {
        private readonly HttpClient _httpClient;
        private readonly ClientOptions _options;
        private readonly IRequestConverter _converter;

        public DynamicsConnector(ClientOptions options, IRequestConverter converter)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(options.Resource)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                "");
            _options = options;
            _converter = converter;
        }

        //public async Task<Guid> CreateAsync(EntityCore target)
        //{
        //    var requestUri = $"/api/data/{_options.ApiVersion}/" +
        //                     $"{target.LogicalName}s";
        //    var content = ToContent(_converter.ToJSon(target));
        //    var result = await _httpClient.PostAsync(requestUri, content);
        //    return ParseId(await ReadResponse(result));
        //}

        public async Task<Guid> UpsertAsync(ApiRequest request)
        {
            var requestUri = $"/api/data/{_options.ApiVersion}/" +
                             $"{request.EntityName}s({request.RecordId})";
            var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = ToContent(_converter.ToJSon(request))
            };
            var result = await _httpClient.SendAsync(patchRequest);
            return ParseId(await ReadResponse(result));
        }

        public async Task DeleteAsync(ApiRequest request)
        {
            var requestUri = $"/api/data/{_options.ApiVersion}/" +
                             $"{request.EntityName}s({request.RecordId})";
            await ReadResponse(await _httpClient.DeleteAsync(requestUri));
        }

        private static StringContent ToContent(string json) =>
            new StringContent(json, Encoding.UTF8, "application/json");

        private static async Task<string> ReadResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private static Guid ParseId(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new Exception("Input content contains no data");
            const string matchPattern =
                @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}";
            var match = Regex.Match(content, matchPattern);
            if (!match.Success)
                throw new Exception("Unable to parse Guid from input content");
            return Guid.Parse(match.Value);
        }
    }
}