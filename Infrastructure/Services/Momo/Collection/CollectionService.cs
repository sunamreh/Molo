using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Molo.Infrastructure.Common.Interfaces;
using Molo.Infrastructure.Common.Models;
using System.Text.Json;
using Molo.Application.Molo.Transact.Queries;
using Molo.Infrastructure.Services.Momo.Collection.Models.Response;
using Molo.Infrastructure.Services.Momo.Collection.Models.Request;
using Molo.Application.Common.Interfaces;
using Molo.Application.Molo.Transact.Commands;

namespace Molo.Infrastructure.Services.Momo.Collection
{
    public class CollectionService : MomoCollectionServiceBase, ICollectionService
    {
        private readonly MomoSettings _settings;

        public CollectionService(IRestClient restClient,
            IRestClient tokenClient,
            IOptions<MomoSettings> settings, IMemoryCache memoryCache) : base(restClient, tokenClient, settings, memoryCache)
        {
            _settings = settings.Value;
        }

        public async Task<AccountBalanceDto> GetAccountBalance()
        {
            var url = $"{_settings.HostUrl}{_settings.Collection.GetAccountBalance.Path}";

            var response = await Send(url, HttpMethod.Get);

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<GetAccountBalanceResponseModel>(responseJson);

            //Mock Response: API Unpredictable
            return new AccountBalanceDto
            {
                AvailableBalance = 1000, //decimal.Parse(responseData.AvailableBalance),
                Currency = "EUR" //responseData.Currency
            };
        }

        public async Task<bool> RequestToPay(RequestToPayDto requestToPayDto)
        {
            var request = new RequestToPayRequestModel
            {
                Amount = requestToPayDto.Amount,
                Currency = requestToPayDto.Currency,
                ExternalId = requestToPayDto.ExternalId.ToString(),
                PayeeNote = requestToPayDto.PayeeNote,
                PayerMessage = requestToPayDto.PayerMessage,
                Payer = new PartyRequestModel
                {
                    PartyId = requestToPayDto.PartyId,
                    PartyTypeId = requestToPayDto.PartyTypeId
                }
            };

            var url = $"{_settings.HostUrl}{_settings.Collection.RequestToPay.Path}";

            var content = new StringContent(JsonSerializer.Serialize(request));

            content.Headers.Add("X-Callback-Url", _settings.Collection.RequestToPay.CallbackUrl);
            content.Headers.Add("X-Reference-Id", requestToPayDto.TransactionId.ToString());

            var response = await Send(url, HttpMethod.Post, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return true;
            }

            return false;
        }
        
        public async Task RequesttoPayDeliveryNotification(string referenceId, string message)
        {
            var url = $"{_settings.HostUrl}{_settings.Collection.RequesttoPayDeliveryNotification.Path}/{referenceId}/deliverynotification";

            var model = new RequesttoPayDeliveryNotificationRequestModel
            {
                NotificationMessage = message
            };

            var content = new StringContent(JsonSerializer.Serialize(model));

            content.Headers.Add("notificationMessage", message);

            await Send(url, HttpMethod.Post, content);
        }

        public async Task<bool> RequesttoPayTransactionStatus(string referenceId)
        {
            var url = $"{_settings.HostUrl}{_settings.Collection.RequestToPay.Path}/{referenceId}";

            var response = await Send(url, HttpMethod.Get);
            var responseJson = await response.Content.ReadAsStringAsync();

            var responseData = JsonSerializer.Deserialize<RequesttoPayTransactionStatusResponseModel>(responseJson);

            if (responseData != null && responseData.Status == "SUCCESSFUL") 
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ValidateAccountHolder(string msisdn)
        {
            var url = $"{_settings.HostUrl}{_settings.Collection.ValidateAccountHolderStatus.Path}/msisdn/{msisdn}/active";

            var response = await Send(url, HttpMethod.Get);

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ValidateAccountHolderStatusResponseModel>(responseJson);

            return responseData.Result;
        }
    }
}
