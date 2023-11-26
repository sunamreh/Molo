using MediatR;
using Molo.Application.Common.Interfaces;
using Molo.Application.Molo.Transact.Commands;
using Molo.Domain.Entities;
using System.Text.Json.Serialization;

namespace Molo.Application.Molo.Collection.Command
{
    public class CollectEventCommand : IRequest
    {
        [JsonPropertyName("clientId")]
        public Guid ClientId { get; set; }
    }

    public class CollectEventCommandHandler : IRequestHandler<CollectEventCommand>
    {
        private readonly ICollectionService _collectionService;
        private readonly IDisbursementService _disbursementService;
        private readonly IMoloDbRepository<Client> _clientRepository;
        private readonly ICollectService _collectService;

        public CollectEventCommandHandler(ICollectionService collectionService, 
            IDisbursementService disbursementService, IMoloDbRepository<Client> clientRepository,
            ICollectService collectService)
        {
            _collectionService = collectionService;
            _disbursementService = disbursementService;
            _clientRepository = clientRepository;
            _collectService = collectService;
        }

        public async Task Handle(CollectEventCommand request, CancellationToken cancellationToken)
        {
            var transactionId = Guid.NewGuid();
            var externalId = Guid.NewGuid();

            var client = await _clientRepository.Get(c => c.Id == request.ClientId);

            string amount = client.Loans.Where(c => !c.IsSettled).Sum(c => c.Amount * ((c.InterestRate.Percentage / 100) + 1)).ToString();

            var requestToPayDto = new RequestToPayDto
            {
                Amount = amount,
                Currency = "EUR",
                ExternalId = externalId,
                PartyId = client.Msisdn,
                PartyTypeId = "msisdn",
                PayeeNote = string.Empty,
                PayerMessage = $"A payment of {amount} EUR is request from {client.Subscriber.Name}, {client.Subscriber.Msisdn}, for outstanding loan",
                TransactionId = transactionId
            };

            var isRequestToPaySuccess = await _collectionService.RequestToPay(requestToPayDto);

            if (!isRequestToPaySuccess)
            {
                var message = $"The payment of {amount} EUR to {client.Subscriber.Name}, {client.Subscriber.Msisdn}, was unsuccessful";
                await _collectionService.RequesttoPayDeliveryNotification(requestToPayDto.TransactionId.ToString(), message);
                return;
            }

            var isTransactionStatusSuccess = await _collectionService.RequesttoPayTransactionStatus(requestToPayDto.TransactionId.ToString());

            if (!isTransactionStatusSuccess)
            {
                var message = $"The payment of {amount} EUR to {client.Subscriber.Name}, {client.Subscriber.Msisdn}, was unsuccessful";
                await _collectionService.RequesttoPayDeliveryNotification(requestToPayDto.TransactionId.ToString(), message);
                return;
            }

            var transferDto = new TransferDto
            {
                Amount = amount,
                Currency = "EUR",
                ExternalId = Guid.NewGuid(),
                PartyId = client.Subscriber.Msisdn,
                PartyTypeId = "MSISDN",
                PayeeNote = string.Empty,
                PayerMessage = string.Empty,
                TransactionId = Guid.NewGuid()
            };

            var isTransferStatusSuccess = await _disbursementService.Transfer(transferDto);

            if (!isTransferStatusSuccess)
            {
                var message = $"The payment of {amount} EUR to {client.Subscriber.Name}, {client.Subscriber.Msisdn}, was unsuccessful";
                await _collectionService.RequesttoPayDeliveryNotification(requestToPayDto.TransactionId.ToString(), message);
                return;
            }

            var isSuccessfulTransfer = await _disbursementService.GetTransferStatus(transferDto.TransactionId.ToString());

            //TODO: Implement a remittance to re-debit subscriber
            if (!isSuccessfulTransfer)
            {
                var message = $"The payment of {amount} EUR to {client.Subscriber.Name}, {client.Subscriber.Msisdn}, was unsuccessful";
                await _collectionService.RequesttoPayDeliveryNotification(requestToPayDto.TransactionId.ToString(), message);
                return;
            }

            await _collectService.CreateTransaction(client.Id, transactionId, externalId);
        }
    }
}
