using Molo.Application.Common.Enums;
using Molo.Application.Common.Interfaces;
using Molo.Application.Molo.Collection.Command;
using Molo.Application.Molo.Transact.Commands;
using Molo.Application.Molo.Transact.Queries;
using Molo.Domain.Entities;
using Molo.Infrastructure.MessageBroker;

namespace Molo.Infrastructure.Services.Molo.Transact
{
    public class TransactService : ITransactService
    {
        private readonly IMoloDbRepository<Subscriber> _subscriberRepository;
        private readonly IMoloDbRepository<InterestRate> _interestRateRepository;
        private readonly IMoloDbRepository<Client> _clientRepository;
        private readonly IMoloDbRepository<Loan> _loanRepository;
        private readonly IMoloDbRepository<Transaction> _transactionRepository;
        private readonly IMessageBrokerProducer<CreateTransactionCommand> _transactionProducer;

        public TransactService(IMessageBrokerProducer<CreateTransactionCommand> transactionProducer,
            IMoloDbRepository<Subscriber> subscriberRepository, 
            IMoloDbRepository<Client> clientRepository,
            IMoloDbRepository<Loan> loanRepository,
            IMoloDbRepository<Transaction> transactionRepository,
            IMoloDbRepository<InterestRate> interestRateRepository)
        {
            _transactionProducer = transactionProducer;
            _subscriberRepository = subscriberRepository;
            _clientRepository = clientRepository;
            _loanRepository = loanRepository;
            _transactionRepository = transactionRepository;
            _interestRateRepository = interestRateRepository;
        }

        public Task Collect(Guid clientId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateTransaction(TransactionEventCommand transactionCommand)
        {
            var subscriber = await _subscriberRepository.GetById(transactionCommand.SubscriberId);

            var client = await _clientRepository.Get(c => c.Msisdn == transactionCommand.ClientMsisdn);

            if (client == null)
            {
                client = new Client
                {
                    Id = Guid.NewGuid(),
                    Name = transactionCommand.ClientName,
                    DateCreated = DateTimeOffset.UtcNow,
                    Msisdn = transactionCommand.ClientMsisdn,
                    SubscriberId = transactionCommand.SubscriberId,
                    IsActive = true
                };
                await _clientRepository.Add(client);
            }

            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                SubscriberId = subscriber.Id,
                Amount = decimal.Parse(transactionCommand.LoanAmount),
                Currency = CurrencyEnum.USD.ToString(),
                CollectionDate = DateTimeOffset.Parse(transactionCommand.CollectionDate),
                IsSettled = false,
                SettlementDate = null,
                InterestRateId = transactionCommand.InterestRateId,
                SubscriberClientId = client.Id
            };

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                SubscriberId = subscriber.Id,
                SubscriberClientId = client.Id,
                SubscriberLoanId = loan.Id,
                TransactionTypeId = (byte)TransactionTypeEnum.Debit,
                TransactionAmount = loan.Amount,
                TransactionId = transactionCommand.TransactionId,
                ExternalId = transactionCommand.ExternalId
            };

            await _loanRepository.Add(loan);
            await _transactionRepository.Add(transaction);
        }

        public async Task<Subscriber> GetSubscriber(string msisdn)
        {
            //TODO: Handle Exceptions
            return await _subscriberRepository.Get(s => s.Msisdn == msisdn && s.IsActive) ?? throw new Exception();
        }

        public async Task PublishTransaction(CreateTransactionCommand transaction)
        {
            _transactionProducer.Publish(transaction);

            await Task.CompletedTask;
        }
    }
}
