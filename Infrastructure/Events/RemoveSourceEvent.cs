using Infrastructure.Models;
using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out the Source which should be removed
    /// </summary>
    public class RemoveSourceEvent : PubSubEvent<Source>
    {
    }
}
