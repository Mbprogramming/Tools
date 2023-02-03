using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpCmdline
{
    /// <summary>
    /// Timestamp Helper
    /// </summary>
    public class TimestampHelper : IDisposable
    {
        /// <summary>
        /// output file
        /// </summary>
        public StreamWriter? output { get; set; }

        /// <summary>
        /// Log Level
        /// </summary>
        public LogLevel logLevel = LogLevel.Error;

        private DateTimeOffset _timestamp;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_output"></param>
        /// <param name="_level"></param>
        /// <remarks>
        /// starts the timer
        /// </remarks>
        public TimestampHelper(StreamWriter? _output, LogLevel _level) 
        {
            _timestamp = DateTimeOffset.Now;
            output = _output;
            logLevel = _level;
        }

        /// <summary>
        /// stops the timer and output duration
        /// </summary>
        public void Dispose()
        {
            var ts = $"Duration {DateTimeOffset.Now - _timestamp}";
            if (output != null && logLevel >= LogLevel.Info)
            {
                output.WriteLine(ts);
            }
            AnsiConsole.WriteLine(ts);
        }
    }
}
