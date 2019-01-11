using System;
using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out that the user wants to see the Uri for a specific Feed
    /// </summary>
    public class WantUriEvent : PubSubEvent<Uri>
    {
    }
}
