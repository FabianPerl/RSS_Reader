using Prism.Events;

namespace Infrastructure.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Spread out that the user wants to fetch the data new
    /// </summary>
    public class FetchDataEvent : PubSubEvent<bool>
    {
    }
}
