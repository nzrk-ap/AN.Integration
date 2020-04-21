using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AN.Integration.Dynamics.Core.DynamicsTooling.OAuth;
using AN.Integration.Dynamics.Core.DynamicsTypes;

namespace AN.Integration.Dynamics.Core.DynamicsTooling
{
    internal class DynamicsConnector : IDynamicsConnector
    {
        private readonly HttpClient _httpClient;
        private readonly ClientOptions _options;
        private readonly IEntityConverter _converter;

        public DynamicsConnector(ClientOptions options, IEntityConverter converter)
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

        public async Task<Guid> CreateAsync(EntityCore target)
        {
            var requestUri = $"/api/data/{_options.ApiVersion}/" +
                             $"{target.LogicalName}s";
            var content = ToContent(_converter.ToJSon(target));
            var result = await _httpClient.PostAsync(requestUri, content);
            return ParseId(await ReadResponse(result));
        }

        public async Task UpdateAsync(EntityCore target)
        {
            var requestUri = $"/api/data/{_options.ApiVersion}/" +
                             $"{target.LogicalName}s({target.Id})";
            var content = ToContent(_converter.ToJSon(target));
            await ReadResponse(await _httpClient.PutAsync(requestUri, content));
        }

        public async Task DeleteAsync(ReferenceCore target)
        {
            var requestUri = $"/api/data/{_options.ApiVersion}/" +
                             $"{target.LogicalName}s({target.Id})";
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