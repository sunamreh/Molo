using MediatR;
using Molo.Application.Common.Interfaces;

namespace Molo.Application.Molo.Collection.Queries
{
    public class ActiveClientsQuery : IRequest<List<UnsettledClientsDto>>
    {
        public Guid SubscriberId { get; set; }
    }

    public class ActiveClientsQueryHandler : IRequestHandler<ActiveClientsQuery, List<UnsettledClientsDto>>
    {
        private readonly ICollectService _collectService;

        public ActiveClientsQueryHandler(ICollectService collectService)
        {
            _collectService = collectService;
        }

        public async Task<List<UnsettledClientsDto>> Handle(ActiveClientsQuery request, CancellationToken cancellationToken)
        {
            return await _collectService.GetActiveClients(request.SubscriberId);
        }
    }
}
