using Infrastructure.Models;
using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out the source which the user was editing
    /// </summary>
    public class EditSourceEvent : PubSubEvent<Source>
    {
    }
}
