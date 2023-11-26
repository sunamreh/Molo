using Microsoft.AspNetCore.Mvc;
using Molo.Application.Molo.Transact.Commands;
using Molo.Application.Molo.Transact.Queries;

namespace Molo.Api.Controllers
{
    [Route("transaction")]
    public class TransactController : ApiControllerBase
    {
        [HttpGet]
        [Route("interestrates")]
        public async Task<List<InterestRateDto>> InterestRates([FromQuery] InterestRateQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet]
        [Route("subscriberprofile")]
        public async Task<SubscriberProfileDto> SubscriberProfile([FromQuery] GetSubscriberProfileQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        [Route("transact")]
        public async Task Transact(CreateTransactionCommand command)
        {
            await Mediator.Send(command);
        }
    }
}
