using FluentFTP;

namespace FtpCmdline
{
    /// <summary>
    /// file logger class for FluentFTP client
    /// </summary>
    internal class FileLogger : IFtpLogger
    {
        private readonly StreamWriter _writer;
        private readonly LogLevel _level;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="level"></param>
        public FileLogger(StreamWriter writer, LogLevel level = LogLevel.None)
        {
            _writer = writer;
            _level = level;
        }

        /// <summary>
        /// write log entry to file
        /// </summary>
        /// <param name="entry"></param>
        public void Log(FtpLogEntry entry)
        {
            switch(entry.Severity)
            {
                case FtpTraceLevel.Verbose:
                    if (_level >= LogLevel.Verbose)
                    {
                        _writer.WriteLine("Verbose: " + entry.Message);
                    }
                    break;
                case FtpTraceLevel.Info:
                    if (_level >= LogLevel.Info)
                    {
                        _writer.WriteLine("Info: " + entry.Message);
                    }
                    break;
                case FtpTraceLevel.Warn:
                    if (_level >= LogLevel.Warn)
                    {
                        _writer.WriteLine("Warn: " + entry.Message);
                    }
                    break;
                case FtpTraceLevel.Error:
                    if (_level >= LogLevel.Error)
                    {
                        _writer.WriteLine("Error: " + entry.Message);
                        if (entry.Exception != null)
                        {
                            _writer.WriteLine("Error: " + entry.Exception.Message);
                        }
                    }
                    break;
            }
        }
    }
}
