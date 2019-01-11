using Prism.Logging;

namespace Infrastructure.Constants
{
    /// <summary>
    /// The project's logger for reading out all debug information
    /// </summary>
    public static class ProjectLogger
    {
        /// <summary>
        /// Gets the logger for the project
        /// </summary>
        public static ILoggerFacade GetLogger { get; } = new DebugLogger();
    }
}
