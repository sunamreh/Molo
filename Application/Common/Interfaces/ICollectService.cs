using Molo.Application.Molo.Collection.Command;
using Molo.Application.Molo.Collection.Queries;
using Molo.Domain.Entities;

namespace Molo.Application.Common.Interfaces
{
    public interface ICollectService
    {
        Task<List<UnsettledClientsDto>> GetActiveClients(Guid subscriberId);
        Task PublishTransaction(CollectCommand transaction);
        Task CreateTransaction(Guid clientId, Guid transactionId, Guid externalId);
        Task SettleAccount(Guid clientId);
    }
}
