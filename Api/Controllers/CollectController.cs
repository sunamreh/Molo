using Microsoft.AspNetCore.Mvc;
using Molo.Application.Molo.Collection.Command;
using Molo.Application.Molo.Collection.Queries;
using Molo.Application.Molo.Transact.Commands;

namespace Molo.Api.Controllers
{
    [Route("collect")]
    public class CollectController : ApiControllerBase
    {
        [HttpGet]
        [Route("activeclients")]
        public async Task<List<UnsettledClientsDto>> ActiveClients([FromQuery] ActiveClientsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        [Route("collectbalance")]
        public async Task Transact(CollectCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpPost]
        [Route("settle")]
        public async Task Settle(SettleCommand command)
        {
            await Mediator.Send(command);
        }
    }
}
