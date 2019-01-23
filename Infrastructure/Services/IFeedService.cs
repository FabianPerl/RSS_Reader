using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ViewModels;

namespace Infrastructure.Services
{
    /// <summary>
    /// Service to get all the feeds from a uri
    /// </summary>
    public interface IFeedService
    {
        /// <summary>
        /// Gets all Feeds for a Uri as a Task
        /// </summary>
        /// <param name="feedUri">The Uri used to get the feeds</param>
        /// <returns>Returns a Collection of all feeds as a task which handles the fetching</returns>
        Task<ICollection<FeedViewModel>> GetTaskAllFeedsFromUrlAsync(Uri feedUri);
    }
}
