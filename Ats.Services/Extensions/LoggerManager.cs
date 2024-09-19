using log4net;
using System.Diagnostics;
using System.Reflection;

namespace Ats.Services.Extensions
{
    public interface ILoggerManager
    {
        void LogInfo(string message);

        void LogWarn(string message);

        void LogDebug(string message);

        void LogError(string message);
    }

    public class LoggerManager : ILoggerManager
    {
        // private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //private readonly ILog logger = LogManager.GetLogger(typeof(LoggerManager));

        private ILog GetLogger()
        {
            var stack = new StackTrace();
            var frame = stack.GetFrame(2);
            return LogManager.GetLogger(frame.GetMethod().DeclaringType);
        }

        public void LogDebug(string message)
        {
            var logger = GetLogger();
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            var logger = GetLogger();
            logger.Error(message);
        }

        public void LogInfo(string message)
        {
            var logger = GetLogger();
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            var logger = GetLogger();
            logger.Warn(message);
        }
    }
}