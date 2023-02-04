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
        private readonly LogLevel _level;
        private readonly bool? _verboseConsole;
        private readonly StreamWriter? _outputFile = null;
        private readonly object _outputLock = new();
        private LogLevel _screenLevel = LogLevel.Info;
        private LogLevel _oldScreenLevel = LogLevel.Info;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <param name="level"></param>
        /// <param name="log"></param>
        public Logger(InvocationContext context, Option<string>? output, Option<LogLevel>? level, Option<bool>? log)
        {
            _outputPath = output != null ? context.ParseResult.GetValueForOption(output) : string.Empty;
            _level = level != null ? context.ParseResult.GetValueForOption(level) : LogLevel.Warn;
            _verboseConsole = log != null && context.ParseResult.GetValueForOption(log);

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
            if(_verboseConsole.HasValue && _verboseConsole.Value)
            {
                _screenLevel = LogLevel.Verbose;
            }
        }

        /// <summary>
        /// log to console
        /// </summary>
        public bool LogToConsole
        {
            get => _verboseConsole.HasValue && _verboseConsole.Value;
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
                    LogVerbose(entry.Message);
                    break;
                case FtpTraceLevel.Info:
                    LogInfo(entry.Message);
                    break;
                case FtpTraceLevel.Warn:
                    LogWarn(entry.Message);
                    break;
                case FtpTraceLevel.Error:
                    LogError(entry.Message);
                    if (entry.Exception != null)
                    {
                        LogError(entry.Exception.Message);
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
        }

        /// <summary>
        /// set no in progress
        /// </summary>
        public void StopInProgress()
        {
            _screenLevel = _oldScreenLevel;
        }
    }
}
