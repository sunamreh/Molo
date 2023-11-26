using Molo.Application.Molo.Transact.Commands;

namespace Molo.Application.Common.Interfaces
{
    public interface IDisbursementService
    {
        Task<bool> Transfer(TransferDto transferDto);
        Task<bool> GetTransferStatus(string referenceId);
    }
}
