using CmdlineBase;
using FluentFTP;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace FtpCmdline
{
    /// <summary>
    /// general logger
    /// </summary>
    public class Logger : LoggerBase, IFtpLogger
    {
        /// <summary>
        /// ftp client log level
        /// </summary>
        protected LogLevel _ftpClientLevel = LogLevel.Warn;
        /// <summary>
        /// ftp client old log level for progress functions
        /// </summary>
        protected LogLevel _oldFtpClientLevel = LogLevel.Warn;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <param name="level"></param>
        /// <param name="screenLevel"></param>
        /// <param name="ftpClientLevel"></param>
        public Logger(InvocationContext context, Option<string>? output, Option<LogLevel>? level, Option<LogLevel>? screenLevel, Option<LogLevel>? ftpClientLevel)
            :base(context, output, level, screenLevel)
        {
            _ftpClientLevel = ftpClientLevel != null ? context.ParseResult.GetValueForOption(ftpClientLevel) : LogLevel.Warn;
        }

        /// <summary>
        /// log to console
        /// </summary>
        public bool LogToConsole
        {
            get => _ftpClientLevel == LogLevel.Verbose;
        }

        private void LogErrorFtp(string message)
        {
            if (_outputFile != null && _ftpClientLevel >= LogLevel.Error)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(message);
                }
            }
            if (_ftpClientLevel >= LogLevel.Error)
            {
                AnsiConsole.WriteLine(message);
            }
        }

        private void LogErrorFtp(Exception ex)
        {
            if (_outputFile != null && _ftpClientLevel >= LogLevel.Error)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(ex.Message);
                    if (!string.IsNullOrEmpty(ex.StackTrace))
                        _outputFile.WriteLine(ex.StackTrace);
                }
            }
            if (_screenLevel >= LogLevel.Error)
            {
                AnsiConsole.WriteException(ex);
            }
            if (ex.InnerException != null)
            {
                LogError(ex.InnerException);
            }
        }

        private void LogWarnFtp(string message)
        {
            if (_outputFile != null && _ftpClientLevel >= LogLevel.Warn)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(message);
                }
            }
            if (_ftpClientLevel >= LogLevel.Warn)
            {
                AnsiConsole.WriteLine(message);
            }
        }

        private void LogInfoFtp(string message)
        {
            if (_outputFile != null && _ftpClientLevel >= LogLevel.Info)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(message);
                }
            }
            if (_ftpClientLevel >= LogLevel.Info)
            {
                AnsiConsole.WriteLine(message);
            }
        }

        private void LogVerboseFtp(string message)
        {
            if (_outputFile != null && _ftpClientLevel >= LogLevel.Verbose)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(message);
                }
            }
            if (_ftpClientLevel >= LogLevel.Verbose)
            {
                AnsiConsole.WriteLine(message);
            }
        }

        /// <summary>
        /// interface implementation
        /// </summary>
        /// <param name="entry"></param>
        /// <inheritdoc/>
        public void Log(FtpLogEntry entry)
        {
            switch (entry.Severity)
            {
                case FtpTraceLevel.Verbose:
                    LogVerboseFtp(entry.Message);
                    break;
                case FtpTraceLevel.Info:
                    LogInfoFtp(entry.Message);
                    break;
                case FtpTraceLevel.Warn:
                    LogWarnFtp(entry.Message);
                    break;
                case FtpTraceLevel.Error:
                    LogErrorFtp(entry.Message);
                    if (entry.Exception != null)
                    {
                        LogErrorFtp(entry.Exception);
                    }
                    break;
            }
        }

        /// <summary>
        /// set in progress
        /// </summary>
        public override void DoInProgress()
        {
            base.DoInProgress();

            _oldFtpClientLevel = _ftpClientLevel;
            _ftpClientLevel = LogLevel.Error;
        }

        /// <summary>
        /// set no in progress
        /// </summary>
        public override void StopInProgress()
        {
            _screenLevel = _oldScreenLevel;
            _ftpClientLevel = _oldFtpClientLevel;

            base.StopInProgress();
        }
    }
}
