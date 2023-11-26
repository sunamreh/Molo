using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Molo.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"ChazTest: {typeof(TRequest).Name} {request}");
        }
    }
}
