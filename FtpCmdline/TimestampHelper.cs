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

        private DateTimeOffset _timestamp;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_output"></param>
        /// <remarks>
        /// starts the timer
        /// </remarks>
        public TimestampHelper(StreamWriter? _output) 
        {
            _timestamp = DateTimeOffset.Now;
            output = _output;
        }

        /// <summary>
        /// stops the timer and output duration
        /// </summary>
        public void Dispose()
        {
            var ts = $"Duration {DateTimeOffset.Now - _timestamp}";
            if (output != null)
            {
                output.WriteLine(ts);
            }
            AnsiConsole.WriteLine(ts);
        }
    }
}
