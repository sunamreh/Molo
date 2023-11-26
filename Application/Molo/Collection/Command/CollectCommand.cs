using MediatR;
using Molo.Application.Common.Interfaces;
using Molo.Domain.Entities;
using System.Text.Json.Serialization;

namespace Molo.Application.Molo.Collection.Command
{
    public class CollectCommand : IRequest
    {
        [JsonPropertyName("clientId")]
        public Guid ClientId { get; set; }
    }

    public class CollectCommandHandler : IRequestHandler<CollectCommand>
    {
        private readonly ICollectService _collectService;

        public CollectCommandHandler(ICollectService collectService)
        {
            _collectService = collectService;
        }

        public async Task Handle(CollectCommand request, CancellationToken cancellationToken)
        {
            await _collectService.PublishTransaction(request);
        }
    }
}
