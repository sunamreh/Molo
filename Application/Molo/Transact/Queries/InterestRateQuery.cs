using MediatR;
using Microsoft.Extensions.Logging;
using Molo.Application.Common.Interfaces;
using Molo.Domain.Entities;

namespace Molo.Application.Molo.Transact.Queries
{
    public class InterestRateQuery : IRequest<List<InterestRateDto>>
    {
    }

    public class InterestRateQueryHandler : IRequestHandler<InterestRateQuery, List<InterestRateDto>>
    {
        private readonly IMoloDbRepository<InterestRate> _interestRate;
        public InterestRateQueryHandler(IMoloDbRepository<InterestRate> interestRate, ILogger<InterestRateQueryHandler> logger)
        {
            _interestRate = interestRate;
        }

        public async Task<List<InterestRateDto>> Handle(InterestRateQuery request, CancellationToken cancellationToken)
        {
            var interestRates = await _interestRate.GetAll();

            var result = interestRates.Select(i => new InterestRateDto
            {
                Id = i.Id,
                Description = i.Description
            }).ToList();

            return await Task.FromResult(result);
        }

    }
}
