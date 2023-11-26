using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Molo.Infrastructure.Common.Interfaces;
using Molo.Infrastructure.Common.Models;
using Molo.Infrastructure.Services.Momo.Collection.Models.Response;
using Molo.Infrastructure.Services.Momo.Transfer.Models.Response;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Molo.Infrastructure.Services.Momo.Transfer
{
    public abstract class MomoDisbursementServiceBase
    {
        private readonly MomoSettings _settings;
        private readonly IRestClient _restClient;
        private readonly IRestClient _tokenClient;
        private readonly IMemoryCache _memoryCache;
        private const string TokenCacheKey = "Momo.Disbursement.Token";

        public MomoDisbursementServiceBase(IRestClient restClient, IRestClient tokenClient,
            IOptions<MomoSettings> settings, IMemoryCache memoryCache)
        {
            _settings = settings.Value;
            _restClient = restClient;
            _tokenClient = tokenClient;
            _memoryCache = memoryCache;

            string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.Disbursement.OAuth2.ClientId}:{_settings.Disbursement.OAuth2.ClientSecret}"));

            _tokenClient.Client.DefaultRequestHeaders.Clear();
            _tokenClient.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);

            _restClient.Client.DefaultRequestHeaders.Clear();

            _restClient.Client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            _restClient.Client.DefaultRequestHeaders.Add("X-Target-Environment", _settings.Disbursement.OAuth2.XTargetEnvironment);
            _restClient.Client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.Disbursement.OAuth2.OcpApimSubscriptionKey);
        }

        public virtual async Task<HttpResponseMessage> Send(string url, HttpMethod httpMethod, HttpContent content = null)
        {
            _restClient.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

            return await _restClient.MakeHttpRequestAsync(url, httpMethod, content);
        }

        private async Task<string> GetToken()
        {
            string url;

            FormUrlEncodedContent content;

            BuildTokenRequest(out url, out content);

            var token = await _memoryCache.GetOrCreateAsync(
                TokenCacheKey,
                async cacheEntry =>
                {
                    var response = await _tokenClient.MakeHttpRequestAsync(url, HttpMethod.Post, content);
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var responseData = JsonSerializer.Deserialize<TransferOAuth2TokenResponseModel>(responseJson);
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(responseData.ExpiresIn);
                    return responseData;
                });

            if (token == null)
                throw new HttpRequestException("Unable to retrieve Access Token from Server");

            return token.AccessToken;
        }

        private void BuildTokenRequest(out string url, out FormUrlEncodedContent content)
        {
            url = $"{_settings.HostUrl}{_settings.Collection.OAuth2.Path}";

            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", _settings.Collection.OAuth2.GrantType },
                { "auth_req_id", Guid.NewGuid().ToString() }
            };

            content = new FormUrlEncodedContent(requestBody);

            content.Headers.Add("X-Target-Environment", _settings.Collection.OAuth2.XTargetEnvironment);
            content.Headers.Add("Ocp-Apim-Subscription-Key", _settings.Collection.OAuth2.OcpApimSubscriptionKey);
        }
    }
}
