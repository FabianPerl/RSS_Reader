using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ViewModels;
using Prism.Events;

namespace Infrastructure.Events
{
    public class NewArchiveFeedEvent : PubSubEvent<FeedViewModel>
    {
    }
}
