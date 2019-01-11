using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out the user wants to close the browser
    /// </summary>
    public class WantCloseUriEvent : PubSubEvent
    {
    }
}
