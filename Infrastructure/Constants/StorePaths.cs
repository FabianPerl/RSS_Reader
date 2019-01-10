namespace Infrastructure.Constants
{
    public class StorePaths
    {
        public static string SourceStorePath => System.IO.Path.GetFullPath("allsources.json");
        public static string ArchivedFeedsStorePath => System.IO.Path.GetFullPath("archivedFeeds.json");
    }
}
