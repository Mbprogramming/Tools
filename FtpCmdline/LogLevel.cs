namespace FtpCmdline
{
    /// <summary>
    /// Log Level for FluentFTP Logging in output file
    /// </summary>
    internal enum LogLevel
    {
        /// <summary>
        /// No logging
        /// </summary>
        None = 0,
        /// <summary>
        /// log all
        /// </summary>
        Verbose = 4,
        /// <summary>
        /// log infos, warnings and errors
        /// </summary>
        Info = 3,
        /// <summary>
        /// log warnings and errors
        /// </summary>
        Warn = 2,
        /// <summary>
        /// log only errors
        /// </summary>
        Error = 1
    }
}
