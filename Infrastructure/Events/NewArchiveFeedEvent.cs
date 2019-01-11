using Infrastructure.ViewModels;
using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out the new feed what should be saved to the archive
    /// </summary>
    public class NewArchiveFeedEvent : PubSubEvent<FeedViewModel>
    {
    }
}
