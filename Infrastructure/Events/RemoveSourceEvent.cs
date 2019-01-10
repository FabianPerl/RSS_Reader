using Infrastructure.Models;
using Prism.Events;

namespace Infrastructure.Events
{
    public class RemoveSourceEvent : PubSubEvent<Source>
    {
    }
}
