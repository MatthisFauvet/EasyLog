using EasyLog.entity;

namespace EasyLog.writers
{
    /// <summary>
    /// Defines the contract for writing log messages to a destination (e.g., file, console, database).
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// Writes a log message to the destination.
        /// </summary>
        /// <param name="logType">The type of log (e.g., Error, Warning, Info).</param>
        /// <param name="message">The main content of the log message, such as an exception or information.</param>
        /// <param name="timeStamp">The timestamp indicating when the event occurred.</param>
        void write(LogType logType, string timeStamp, Dictionary<string, string> message);
    }
}