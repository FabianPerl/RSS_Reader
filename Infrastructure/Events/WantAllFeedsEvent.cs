using System.Collections.Generic;
using Infrastructure.Models;
using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out the collection of all Sources which the user wants to see
    /// </summary>
    public class WantAllFeedsEvent : PubSubEvent<ICollection<Source>>
    {
    }
}
