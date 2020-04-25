using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AN.Integration.Infrastructure.Utilities;

namespace AN.Integration.API.Services
{
    public class HttpQueueClient
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _queueEndpointUrl;

        public HttpQueueClient(string serviceBusExportQueueuSasKey, Uri queueEndpointUrl)
        {
            _queueEndpointUrl = queueEndpointUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add
                ("Authorization", serviceBusExportQueueuSasKey);
        }

        public async Task<(int statusCode, string content)> SendMessageAsync<T>(T value)
        {
            var result = await _httpClient.PostAsync(_queueEndpointUrl, ToContent(value));
            return ((int) result.StatusCode, await ReadContent(result));
        }

        public static StringContent ToContent<T>(T value) =>
            new StringContent(JsonSerializer.ToJson(value), Encoding.UTF8, "application/json");

        public static async Task<string> ReadContent(HttpResponseMessage response) =>
            await response.Content.ReadAsStringAsync();
    }
}