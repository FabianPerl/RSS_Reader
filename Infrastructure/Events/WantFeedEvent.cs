using Infrastructure.Models;
using Prism.Events;

namespace Infrastructure.Events
{
    public class WantFeedEvent : PubSubEvent<Source>
    {
    }
}
