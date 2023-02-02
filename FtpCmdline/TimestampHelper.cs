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
        private DateTimeOffset _timestamp;
        /// <summary>
        /// constructor
        /// </summary>
        /// <remarks>
        /// starts the timer
        /// </remarks>
        public TimestampHelper() 
        {
            _timestamp = DateTimeOffset.Now;
        }

        /// <summary>
        /// stops the timer and output duration
        /// </summary>
        public void Dispose()
        {
            AnsiConsole.WriteLine($"Duration {DateTimeOffset.Now - _timestamp}");
        }
    }
}
