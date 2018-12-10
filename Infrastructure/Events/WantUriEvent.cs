using System;
using Prism.Events;

namespace Infrastructure.Events
{
    public class WantUriEvent : PubSubEvent<Uri>
    {
    }
}
