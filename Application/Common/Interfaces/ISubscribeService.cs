using Molo.Application.Molo.Subscription.Commands;
using Molo.Domain.Entities;

namespace Molo.Application.Common.Interfaces
{
    public interface ISubscribeService
    {
        Task<bool> CheckSubscriberExists(string msisdn);
        Task Subscribe(Subscriber subscriber);
        Task Publish(SubscribeCommand subscriber);
    }
}
