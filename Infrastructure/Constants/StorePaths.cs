using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Constants
{
    public class StorePaths
    {
        public static string SourceStorePath => System.IO.Path.GetFullPath("allsources.bin");
        public static string ArchivedFeedsStorePath => System.IO.Path.GetFullPath("archivedFeeds.bin");
    }
}
