using Molo.Application.Common.Enums;
using Molo.Application.Common.Interfaces;
using Molo.Application.Molo.Collection.Command;
using Molo.Application.Molo.Collection.Queries;
using Molo.Application.Molo.Transact.Commands;
using Molo.Domain.Entities;
using Molo.Infrastructure.MessageBroker;

namespace Molo.Infrastructure.Services.Molo.Collect
{
    public class CollectService : ICollectService
    {
        private readonly IMoloDbRepository<Loan> _loanDbRepository;
        private readonly IMoloDbRepository<Transaction> _transactionDbRepository;
        private readonly IMessageBrokerProducer<CollectCommand> _collectProducer;
        private readonly IMoloDbRepository<Client> _clientRepository;

        public CollectService(IMoloDbRepository<Loan> loanDbRepository,
            IMoloDbRepository<Transaction> transactionDbRepository,
            IMoloDbRepository<Client> clientRepository,
            IMessageBrokerProducer<CollectCommand> collectProducer)
        {
            _loanDbRepository = loanDbRepository;
            _clientRepository = clientRepository;
            _collectProducer = collectProducer;
            _transactionDbRepository = transactionDbRepository;
        }

        public async Task CreateTransaction(Guid clientId, Guid transactionId, Guid externalId)
        {
            var client = await _clientRepository.GetById(clientId);

            var loans = await _loanDbRepository.GetAll(l => l.SubscriberClientId == clientId);

            foreach (var loan in loans)
            {
                loan.IsSettled = true;
                loan.SettlementDate = DateTimeOffset.UtcNow;
                await _loanDbRepository.Update(loan);

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    SubscriberId = client.SubscriberId,
                    SubscriberClientId = client.Id,
                    SubscriberLoanId = loan.Id,
                    TransactionTypeId = (byte)TransactionTypeEnum.Credit,
                    TransactionAmount = loan.Amount,
                    TransactionId = transactionId,
                    ExternalId = externalId
                };

                await _transactionDbRepository.Add(transaction);
            }
        }

        public async Task<List<UnsettledClientsDto>> GetActiveClients(Guid subscriberId)
        {
            var activeLoans = await _loanDbRepository
                .GetAll(l => l.SubscriberId == subscriberId && l.SubscriberClient.IsActive && !l.IsSettled);

            if (!activeLoans.Any())
            {
                return new List<UnsettledClientsDto>();
            }

            var result = activeLoans.Select(l => new UnsettledClientsDto
            {
                ClientId = l.SubscriberClientId,
                Name = l.SubscriberClient.Name,
                Msisdn = l.SubscriberClient.Msisdn,
                Balance = l.Amount * (1 + (l.InterestRate.Percentage / 100))
            }).ToList();

            return result;
        }

        public async Task PublishTransaction(CollectCommand transaction)
        {
            _collectProducer.Publish(transaction);

            await Task.CompletedTask;
        }

        public async Task SettleAccount(Guid clientId)
        {
            var client = await _clientRepository.GetById(clientId);

            var loans = await _loanDbRepository.GetAll(l => l.SubscriberClientId == clientId);

            foreach (var loan in loans)
            {
                loan.IsSettled = true;
                loan.SettlementDate = DateTimeOffset.UtcNow;
                await _loanDbRepository.Update(loan);
            }
        }
    }
}
