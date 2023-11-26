using Molo.Application.Molo.Collection.Command;
using Molo.Application.Molo.Transact.Commands;
using Molo.Application.Molo.Transact.Queries;
using Molo.Domain.Entities;

namespace Molo.Application.Common.Interfaces
{
    public interface ITransactService
    {
        Task<Subscriber> GetSubscriber(string msisdn);
        Task PublishTransaction(CreateTransactionCommand transaction);
        Task CreateTransaction(TransactionEventCommand transaction);
        Task Collect(Guid clientId);
    }
}
