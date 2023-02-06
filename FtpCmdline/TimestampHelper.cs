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
        /// logger
        /// </summary>
        public Logger Logger { get; set; }

        private readonly DateTimeOffset _timestamp;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_logger"></param>
        /// <remarks>
        /// starts the timer
        /// </remarks>
        public TimestampHelper(Logger _logger) 
        {
            _timestamp = DateTimeOffset.Now;
            Logger = _logger;
        }

        /// <summary>
        /// stops the timer and output duration
        /// </summary>
        public void Dispose()
        {
            var ts = $"Duration {DateTimeOffset.Now - _timestamp}";
            Logger.LogInfo(ts);
        }
    }
}
