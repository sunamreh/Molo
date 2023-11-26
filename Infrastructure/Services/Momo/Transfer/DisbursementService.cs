using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Molo.Application.Common.Interfaces;
using Molo.Application.Molo.Transact.Commands;
using Molo.Infrastructure.Common.Interfaces;
using Molo.Infrastructure.Common.Models;
using Molo.Infrastructure.Services.Momo.Transfer.Models.Request;
using Molo.Infrastructure.Services.Momo.Transfer.Models.Response;
using System.Runtime;
using System.Text.Json;

namespace Molo.Infrastructure.Services.Momo.Transfer
{
    public class DisbursementService : MomoDisbursementServiceBase, IDisbursementService
    {
        private readonly MomoSettings _settings;

        public DisbursementService(IRestClient restClient,
            IRestClient tokenClient,
            IOptions<MomoSettings> settings, IMemoryCache memoryCache) : base(restClient, tokenClient, settings, memoryCache)
        {
            _settings = settings.Value;
        }

        public async Task<bool> GetTransferStatus(string referenceId)
        {
            var url = $"{_settings.HostUrl}{_settings.Disbursement.Transfer.Path}/{referenceId}";

            var response = await Send(url, HttpMethod.Get);
            var responseJson = await response.Content.ReadAsStringAsync();

            var responseData = JsonSerializer.Deserialize<GetTransferStatusResponseModel>(responseJson);

            if (responseData != null && responseData.Status == "SUCCESSFUL")
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Transfer(TransferDto transferDto)
        {
            var request = new TransferRequestModel
            {
                Amount = transferDto.Amount,
                Currency = transferDto.Currency,
                ExternalId = transferDto.ExternalId.ToString(),
                PayeeNote = transferDto.PayeeNote,
                PayerMessage = transferDto.PayerMessage,
                Payee = new PartyRequestModel
                {
                    PartyId = transferDto.PartyId,
                    PartyTypeId = transferDto.PartyTypeId
                }
            };

            var url = $"{_settings.HostUrl}{_settings.Disbursement.Transfer.Path}";

            var content = new StringContent(JsonSerializer.Serialize(request));

            content.Headers.Add("X-Callback-Url", _settings.Collection.RequestToPay.CallbackUrl);
            content.Headers.Add("X-Reference-Id", transferDto.TransactionId.ToString());

            var response = await Send(url, HttpMethod.Post, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return true;
            }

            return false;
        }
    }
}
