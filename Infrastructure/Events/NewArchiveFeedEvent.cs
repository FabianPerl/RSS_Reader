using Infrastructure.ViewModels;
using Prism.Events;

namespace Infrastructure.Events
{
    public class NewArchiveFeedEvent : PubSubEvent<FeedViewModel>
    {
    }
}
