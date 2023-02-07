using FluentFTP;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace FtpCmdline
{
    /// <summary>
    /// general logger
    /// </summary>
    public class Logger : IFtpLogger, IDisposable
    {
        private readonly string? _outputPath;
        
        private LogLevel _level = LogLevel.Info;
        private LogLevel _screenLevel = LogLevel.Info;
        private LogLevel _ftpClientLevel = LogLevel.Warn;
        
        private readonly StreamWriter? _outputFile = null;
        private readonly object _outputLock = new();

        private LogLevel _oldScreenLevel = LogLevel.Info;
        private LogLevel _oldFtpClientLevel = LogLevel.Warn;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <param name="level"></param>
        /// <param name="screenLevel"></param>
        /// <param name="ftpClientLevel"></param>
        public Logger(InvocationContext context, Option<string>? output, Option<LogLevel>? level, Option<LogLevel>? screenLevel, Option<LogLevel>? ftpClientLevel)
        {
            _outputPath = output != null ? context.ParseResult.GetValueForOption(output) : string.Empty;
            _level = level != null ? context.ParseResult.GetValueForOption(level) : LogLevel.Info;
            _screenLevel = screenLevel != null ? context.ParseResult.GetValueForOption(screenLevel) : LogLevel.Info;
            _ftpClientLevel = ftpClientLevel != null ? context.ParseResult.GetValueForOption(ftpClientLevel) : LogLevel.Warn;

            if (!string.IsNullOrEmpty(_outputPath))
            {
                if (File.Exists(_outputPath))
                {
                    _outputFile = File.AppendText(_outputPath);
                }
                else
                {
                    _outputFile = File.CreateText(_outputPath);
                }
            }
        }

        /// <summary>
        /// log to console
        /// </summary>
        public bool LogToConsole
        {
            get => _ftpClientLevel == LogLevel.Verbose;
        }

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            if (_outputFile != null && _level >= LogLevel.Error)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(message);
                }
            }
            if (_screenLevel >= LogLevel.Error)
            {
                AnsiConsole.WriteLine(message);
            }
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

        /// <summary>
        /// log error object
        /// </summary>
        /// <param name="obj"></param>
        public void LogError(object obj)
        {
            if (_outputFile != null && _level >= LogLevel.Error)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(obj.ToString());
                }
            }
            if (_screenLevel >= LogLevel.Error)
            {
                AnsiConsole.WriteLine(obj.ToString() ?? "");
            }
        }

        /// <summary>
        /// log exception
        /// </summary>
        /// <param name="ex"></param>
        public void LogError(Exception ex)
        {
            if (_outputFile != null && _level >= LogLevel.Error)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(ex.Message);
                    if(!string.IsNullOrEmpty(ex.StackTrace))
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

        /// <summary>
        /// Log Warning
        /// </summary>
        /// <param name="message"></param>
        public void LogWarn(string message)
        {
            if (_outputFile != null && _level >= LogLevel.Warn)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(message);
                }
            }
            if (_screenLevel >= LogLevel.Warn)
            {
                AnsiConsole.WriteLine(message);
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
        
        /// <summary>
        /// log warning object
        /// </summary>
        /// <param name="obj"></param>
        public void LogWarn(object obj)
        {
            if (_outputFile != null && _level >= LogLevel.Warn)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(obj.ToString());
                }
            }
            if (_screenLevel >= LogLevel.Warn)
            {
                AnsiConsole.WriteLine(obj.ToString() ?? "");
            }
        }

        /// <summary>
        /// Log Info
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            if (_outputFile != null && _level >= LogLevel.Info)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(message);
                }
            }
            if (_screenLevel >= LogLevel.Info)
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

        /// <summary>
        /// log info object
        /// </summary>
        /// <param name="obj"></param>
        public void LogInfo(object obj)
        {
            if (_outputFile != null && _level >= LogLevel.Info)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(obj.ToString());
                }
            }
            if (_screenLevel >= LogLevel.Info)
            {
                AnsiConsole.WriteLine(obj.ToString() ?? "");
            }
        }

        /// <summary>
        /// Log Verbose
        /// </summary>
        /// <param name="message"></param>
        public void LogVerbose(string message)
        {
            if (_outputFile != null && _level >= LogLevel.Verbose)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(message);
                }
            }
            if (_screenLevel >= LogLevel.Verbose)
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
        /// log verbose object
        /// </summary>
        /// <param name="obj"></param>
        public void LogVerbose(object obj)
        {
            if (_outputFile != null && _level >= LogLevel.Verbose)
            {
                lock (_outputLock)
                {
                    _outputFile.WriteLine(obj.ToString());
                }
            }
            if (_screenLevel >= LogLevel.Verbose)
            {
                AnsiConsole.WriteLine(obj.ToString() ?? "");
            }
        }
        /// <summary>
        /// interface implementation
        /// </summary>
        /// <inheritdoc/>
        public void Dispose()
        {
            _outputFile?.Close();
            _outputFile?.Dispose();
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
        public void DoInProgress()
        {
            _oldScreenLevel = _screenLevel;
            _screenLevel = LogLevel.Error;

            _oldFtpClientLevel= _ftpClientLevel;
            _ftpClientLevel = LogLevel.Error;
        }

        /// <summary>
        /// set no in progress
        /// </summary>
        public void StopInProgress()
        {
            _screenLevel = _oldScreenLevel;
            _ftpClientLevel = _oldFtpClientLevel;
        }
    }
}
