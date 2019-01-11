using Infrastructure.Models;
using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out the new created Source
    /// </summary>
    public class NewSourceEvent : PubSubEvent<Source>
    {
    }
}
