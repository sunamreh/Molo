using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Molo.Application.Common.Exceptions;
using Molo.Infrastructure.Common.Interfaces;

namespace Molo.Infrastructure.Common.Http
{
    public class RestClient : IRestClient
    {
        private readonly ILogger<RestClient> _logger;

        public HttpClient Client { get; set; }
        public RestClient(HttpClient httpClient, ILogger<RestClient> logger)
        {
            Client = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> MakeHttpRequestAsync(string url, HttpMethod httpMethod, HttpContent content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(url)
            };

            string jsonContent = string.Empty;

            if (content != null)
            {
                jsonContent = await content.ReadAsStringAsync();
                request.Content = content;
            }

            _logger.LogInformation($"MakeHttpRequestAsync: Sending request to [{httpMethod.Method}] {url}. Content: \n {jsonContent}");

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                //if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                //{
                //    throw new NotFoundException();
                //}

                //throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");

                _logger.LogError($"MakeHttpRequestAsync: HTTP request to [{httpMethod.Method}] {url} failed with status code {response.StatusCode}");

                return response;
            }
            
            _logger.LogInformation($"MakeHttpRequestAsync: HTTP request to [{httpMethod.Method}] {url} successful. ResponseL \n {response.Content}");
            return response;
        }
    }
}
