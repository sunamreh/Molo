using Microsoft.AspNetCore.Mvc;
using Molo.Application.Molo.Subscription.Commands;
using Molo.Application.Molo.Subscription.Queries;
using Molo.Application.Molo.Transact.Queries;

namespace Molo.Api.Controllers
{
    [Route("subscription")]
    public class SubscriptionController : ApiControllerBase
    {
        [HttpPost]
        [Route("subscribe")]
        public async Task Subscribe(SubscribeCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpGet]
        [Route("subscriberexists")]
        public async Task<bool> SubscriberExists([FromQuery] SubscriberExistsQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
