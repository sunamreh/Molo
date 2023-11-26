using Molo.Application.Molo.Transact.Commands;
using Molo.Application.Molo.Transact.Queries;

namespace Molo.Application.Common.Interfaces
{
    public interface ICollectionService
    {
        Task<AccountBalanceDto> GetAccountBalance();
        Task<bool> RequestToPay(RequestToPayDto requestToPayDto);
        Task RequesttoPayDeliveryNotification(string referenceId, string message);
        Task<bool> RequesttoPayTransactionStatus(string referenceId);
        Task<bool> ValidateAccountHolder(string msisdn);
    }
}
