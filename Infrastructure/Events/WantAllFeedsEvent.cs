using System.Collections.Generic;
using Infrastructure.Models;
using Prism.Events;

namespace Infrastructure.Events
{
    public class WantAllFeedsEvent : PubSubEvent<ICollection<Source>>
    {
    }
}
