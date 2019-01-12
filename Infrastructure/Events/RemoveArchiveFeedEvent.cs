using Infrastructure.ViewModels;
using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out the Feed which should be removed from the archive
    /// </summary>
    public class RemoveArchiveFeedEvent : PubSubEvent<FeedViewModel>
    {
    }
}
