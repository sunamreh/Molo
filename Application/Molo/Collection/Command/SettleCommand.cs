using MediatR;
using Molo.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace Molo.Application.Molo.Collection.Command
{
    public class SettleCommand : IRequest
    {
        [JsonPropertyName("clientId")]
        public Guid ClientId { get; set; }
    }

    public class SettleCommandHandler : IRequestHandler<SettleCommand>
    {
        private readonly ICollectService _collectService;

        public SettleCommandHandler(ICollectService collectService)
        {
            _collectService = collectService;
        }

        public async Task Handle(SettleCommand request, CancellationToken cancellationToken)
        {
            await _collectService.SettleAccount(request.ClientId);
        }
    }
}
