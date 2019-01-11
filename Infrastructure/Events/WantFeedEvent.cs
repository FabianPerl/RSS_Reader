using Infrastructure.Models;
using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out that the user wants to see the feeds for a specific Source
    /// </summary>
    public class WantFeedEvent : PubSubEvent<Source>
    {
    }
}
