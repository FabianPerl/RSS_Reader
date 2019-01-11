using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out that the feeds are all loaded
    /// </summary>
    public class FeedsLoadedEvent : PubSubEvent<bool>
    {
    }
}
