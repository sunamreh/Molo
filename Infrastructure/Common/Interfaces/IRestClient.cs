namespace Molo.Infrastructure.Common.Interfaces
{
    public interface IRestClient
    {
        public HttpClient Client { get; set; }
        Task<HttpResponseMessage> MakeHttpRequestAsync(string url, HttpMethod httpMethod, HttpContent content);
    }
}
