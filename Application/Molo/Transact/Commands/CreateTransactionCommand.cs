using MediatR;
using Molo.Application.Common.Exceptions;
using Molo.Application.Common.Interfaces;
using Molo.Domain.Entities;
using System.Text.Json.Serialization;

namespace Molo.Application.Molo.Transact.Commands
{
    public class CreateTransactionCommand : IRequest
    {
        [JsonPropertyName("subscriberId")]
        public Guid SubscriberId { get; set; }

        [JsonPropertyName("clientName")]
        public string ClientName { get; set; }

        [JsonPropertyName("clientMsisdn")]
        public string ClientMsisdn { get; set; }

        [JsonPropertyName("loanAmount")]
        public string LoanAmount { get; set;}

        [JsonPropertyName("collectionDate")]
        public string CollectionDate { get; set; }

        [JsonPropertyName("interestRateId")]
        public byte InterestRateId { get; set; }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand>
    {
        private readonly ITransactService _transactService;
        private readonly ICollectionService _collectionService;

        public CreateTransactionCommandHandler(ITransactService transactService, ICollectionService collectionService)
        {
            _transactService = transactService;
            _collectionService = collectionService;
        }

        public async Task Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var isClientValidAccountHolder = await _collectionService.ValidateAccountHolder(request.ClientMsisdn);

            if (!isClientValidAccountHolder)
                throw new NotFoundException();

            await _transactService.PublishTransaction(request);
        }
    }
}
