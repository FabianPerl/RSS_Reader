using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging;

namespace Infrastructure.Constants
{
    public class ProjectLogger
    {
        public static ILoggerFacade GetLogger { get; } = new DebugLogger();
    }
}
