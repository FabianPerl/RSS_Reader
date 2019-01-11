namespace Infrastructure.Constants
{
    /// <summary>
    /// The paths used for saving the program's content
    /// </summary>
    public static class StorePaths
    {
        /// <summary>
        /// Gets the path for saving the sources
        /// </summary>
        public static string SourceStorePath => System.IO.Path.GetFullPath("allsources.json");

        /// <summary>
        /// Gets the path for saving the archived feeds
        /// </summary>
        public static string ArchivedFeedsStorePath => System.IO.Path.GetFullPath("archivedFeeds.json");
    }
}
