using MediatR;
using Molo.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Molo.Application.Molo.Transact.Commands
{
    public class TransactionEventCommand : IRequest
    {
        [JsonPropertyName("subscriberId")]
        public Guid SubscriberId { get; set; }

        [JsonPropertyName("clientName")]
        public string ClientName { get; set; }

        [JsonPropertyName("clientMsisdn")]
        public string ClientMsisdn { get; set; }

        [JsonPropertyName("loanAmount")]
        public string LoanAmount { get; set; }

        [JsonPropertyName("collectionDate")]
        public string CollectionDate { get; set; }

        [JsonPropertyName("interestRateId")]
        public byte InterestRateId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ExternalId { get; set; }
    }

    public class TransactionEventCommandHandler : IRequestHandler<TransactionEventCommand>
    {
        private readonly ITransactService _transactService;
        private readonly ICollectionService _collectionService;
        private readonly IDisbursementService _disbursementService;

        public TransactionEventCommandHandler(ITransactService transactService, ICollectionService collectionService, IDisbursementService disbursementService)
        {
            _transactService = transactService;
            _collectionService = collectionService;
            _disbursementService = disbursementService;
        }

        public async Task Handle(TransactionEventCommand request, CancellationToken cancellationToken)
        {
            request.TransactionId = Guid.NewGuid();
            request.ExternalId = Guid.NewGuid();

            var requestToPayDto = new RequestToPayDto
            {
                Amount = request.LoanAmount,
                Currency = "EUR",
                ExternalId = request.ExternalId,
                PartyId = request.ClientMsisdn,
                PartyTypeId = "msisdn",
                PayeeNote = string.Empty,
                PayerMessage = $"You are about to make a payment of {request.LoanAmount} EUR to {request.ClientName}, {request.ClientMsisdn}, for collection on {request.CollectionDate}",
                TransactionId = request.TransactionId
            };

            var isRequestToPaySuccess = await _collectionService.RequestToPay(requestToPayDto);

            if (!isRequestToPaySuccess) 
            {
                var message = $"The payment of {request.LoanAmount} EUR to {request.ClientName}, {request.ClientMsisdn}, was unsuccessful";
                await _collectionService.RequesttoPayDeliveryNotification(requestToPayDto.TransactionId.ToString(), message);
                return;
            }

            var isTransactionStatusSuccess = await _collectionService.RequesttoPayTransactionStatus(requestToPayDto.TransactionId.ToString());
            
            if (!isTransactionStatusSuccess)
            {
                var message = $"The payment of {request.LoanAmount} EUR to {request.ClientName}, {request.ClientMsisdn}, was unsuccessful";
                await _collectionService.RequesttoPayDeliveryNotification(requestToPayDto.TransactionId.ToString(), message);
                return;
            }

            var transferDto = new TransferDto
            {
                Amount = request.LoanAmount,
                Currency = "EUR",
                ExternalId = Guid.NewGuid(),
                PartyId = request.ClientMsisdn,
                PartyTypeId = "MSISDN",
                PayeeNote = string.Empty,
                PayerMessage = $"A payment of {request.LoanAmount} EUR to has been made to you for collection on {request.CollectionDate}",
                TransactionId = Guid.NewGuid()
            };

            var isTransferStatusSuccess = await _disbursementService.Transfer(transferDto);

            if (!isTransferStatusSuccess)
            {
                var message = $"The payment of {request.LoanAmount} EUR to {request.ClientName}, {request.ClientMsisdn}, was unsuccessful";
                await _collectionService.RequesttoPayDeliveryNotification(requestToPayDto.TransactionId.ToString(), message);
                return;
            }

            var isSuccessfulTransfer = await _disbursementService.GetTransferStatus(transferDto.TransactionId.ToString());

            //TODO: Implement a remittance to re-debit subscriber
            if (!isSuccessfulTransfer)
            {
                var message = $"The payment of {request.LoanAmount} EUR to {request.ClientName}, {request.ClientMsisdn}, was unsuccessful";
                await _collectionService.RequesttoPayDeliveryNotification(request.TransactionId.ToString(), message);
                return;
            }

            await _transactService.CreateTransaction(request);
        }
    }
}
