using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace CmdlineBase
{
    /// <summary>
    /// general logger
    /// </summary>
    public class LoggerBase : IDisposable
    {
        /// <summary>
        /// output file path
        /// </summary>
        protected readonly string? _outputPath;
        
        /// <summary>
        /// log level for output file
        /// </summary>
        protected LogLevel _level = LogLevel.Info;
        /// <summary>
        /// log level for screeb
        /// </summary>
        protected LogLevel _screenLevel = LogLevel.Info;

        /// <summary>
        /// output file
        /// </summary>        
        protected readonly StreamWriter? _outputFile = null;
        /// <summary>
        /// lock to write file from different threads
        /// </summary>
        protected readonly object _outputLock = new();

        /// <summary>
        /// old screen level to disable output during progress
        /// </summary>
        protected LogLevel _oldScreenLevel = LogLevel.Info;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <param name="level"></param>
        /// <param name="screenLevel"></param>
        public LoggerBase(InvocationContext context, Option<string>? output, Option<LogLevel>? level, Option<LogLevel>? screenLevel)
        {
            _outputPath = output != null ? context.ParseResult.GetValueForOption(output) : string.Empty;
            _level = level != null ? context.ParseResult.GetValueForOption(level) : LogLevel.Info;
            _screenLevel = screenLevel != null ? context.ParseResult.GetValueForOption(screenLevel) : LogLevel.Info;

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
        /// set in progress
        /// </summary>
        public virtual void DoInProgress()
        {
            _oldScreenLevel = _screenLevel;
            _screenLevel = LogLevel.Error;
        }

        /// <summary>
        /// set no in progress
        /// </summary>
        public virtual void StopInProgress()
        {
            _screenLevel = _oldScreenLevel;
        }
    }
}
