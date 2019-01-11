using System.Collections.Generic;
using Infrastructure.Models;
using Infrastructure.ViewModels;

namespace Infrastructure.Services
{
    /// <summary>
    /// Store used to save and load Sources and Archived Feeds from the user
    /// </summary>
    public interface IRssStore
    {
        #region sources
        /// <summary>
        /// Gets Saved Sources
        /// </summary>
        /// <returns>Returns all saved Sources from the User</returns>
        ICollection<Source> GetAllSources();

        /// <summary>
        /// Delete all Sources, saved from the User
        /// </summary>
        void DeleteAllSources();

        /// <summary>
        /// Safes all Sources from the User
        /// </summary>
        /// <param name="allSources">The Sources which should be saved to memory</param>
        void SafeAllSources(ICollection<Source> allSources);
        #endregion

        #region archived Feeds
        /// <summary>
        /// Safes all archived Feeds from the User
        /// </summary>
        /// <param name="allFeedViewModels">The archived Feeds which should be saved to memory</param>
        void SafeAllArchiveFeeds(ICollection<FeedViewModel> allFeedViewModels);

        /// <summary>
        /// Gets all saved Archived Feeds from the User
        /// </summary>
        /// <returns>Returns all saved Archived Feeds from the User</returns>
        ICollection<FeedViewModel> LoadAllArchiveFeeds();
        #endregion
    }
}
