using Prism.Logging;

namespace Infrastructure.Constants
{
    public class ProjectLogger
    {
        public static ILoggerFacade GetLogger { get; } = new DebugLogger();
    }
}
