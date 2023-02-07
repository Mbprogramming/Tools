namespace CmdlineBase
{
    /// <summary>
    /// Timestamp Helper
    /// </summary>
    public class TimestampHelper : IDisposable
    {
        /// <summary>
        /// logger
        /// </summary>
        public LoggerBase Logger { get; set; }

        private readonly DateTimeOffset _timestamp;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_logger"></param>
        /// <remarks>
        /// starts the timer
        /// </remarks>
        public TimestampHelper(LoggerBase _logger)
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
