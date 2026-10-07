using System.IO;

namespace Singleton_Configurator.Services
{
    public enum LogLevel { Trace = 0, Debug, Info, Warn, Error, Fatal }

    public sealed class Logger
    {
        private static Logger? instance;
        private static readonly object _lock = new object();
        private readonly string fileName;
        private LogLevel minimumLevel = LogLevel.Trace;

        private Logger()
        {
            fileName = Path.Combine(AppContext.BaseDirectory, "LogFile.txt");
        }

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new Logger();
                    }
                }
            }
            return instance;
        }

        /// <summary>Meldungen unter diesem Level werden nicht geschrieben.</summary>
        public LogLevel MinimumLevel
        {
            get => minimumLevel;
            set
            {
                minimumLevel = value;
                LogInfo($"Minimum log level set to {value}");
            }
        }

        public void LogTrace(string message) => Write(LogLevel.Trace, message);
        public void LogDebug(string message) => Write(LogLevel.Debug, message);
        public void LogInfo(string message) => Write(LogLevel.Info, message);
        public void LogWarn(string message) => Write(LogLevel.Warn, message);

        public void LogError(string message, Exception? ex = null)
        {
            if (ex != null)
                message += $" | {ex.GetType().Name}: {ex.Message}";
            Write(LogLevel.Error, message);
        }

        public void LogFatal(string message) => Write(LogLevel.Fatal, message);

        private void Write(LogLevel level, string message)
        {
            if (level < minimumLevel)
                return;

            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level.ToString().ToUpperInvariant()}] {ToOneLine(message)}";

            lock (_lock)
            {
                try
                {
                    File.AppendAllText(fileName, line + Environment.NewLine);
                }
                catch (IOException)
                {
                   
                }
            }
        }

        private static string ToOneLine(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            return text.Replace("\r\n", " ").Replace('\r', ' ').Replace('\n', ' ');
        }
    }
}
