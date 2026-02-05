using System.Text.Json;
using EasyLog.entity;

namespace EasyLog.writers
{
    /// <summary>
    /// Provides functionality to write log messages as JSON lines to a file.
    /// Each log entry is stored as a single JSON object in the file.
    /// </summary>
    public class JsonFileWriter : ILogWriter, IDisposable
    {
        private readonly string _context;
        private readonly string _filePath;
        private readonly StreamWriter _streamWriter;

        /// <summary>
        /// Initializes a new instance of <see cref="JsonFileWriter"/> and prepares the JSON log file.
        /// </summary>
        /// <param name="fileDirectory">The directory where the log file will be created.</param>
        /// <param name="context">
        /// The context for the log, used in the file name and as a field in each log entry.
        /// Can be a module name, description, or other identifier.
        /// </param>
        public JsonFileWriter(string fileDirectory, string context)
        {
            _context = context;
            _filePath = ComputeFilePath(fileDirectory, context);

            // Ensure the directory exists before creating the file
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _streamWriter = new StreamWriter(_filePath, append: true)
            {
                AutoFlush = true
            };
        }

        /// <summary>
        /// Writes a log entry to the JSON log file.
        /// </summary>
        /// <param name="logType">The type or level of log (e.g., Error, Warning, Info).</param>
        /// <param name="message">The content of the log message.</param>
        /// <param name="timeStamp">The timestamp when the event occurred.</param>
        public void write(LogType logType, string message, string timeStamp)
        {
            try
            {
                var logEntry = new
                {
                    timestamp = timeStamp,
                    level = logType.ToString(),
                    context = _context,
                    message = message
                };

                string json = JsonSerializer.Serialize(logEntry);
                _streamWriter.WriteLine(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Disposes the underlying <see cref="StreamWriter"/> and releases resources.
        /// </summary>
        public void Dispose()
        {
            _streamWriter?.Dispose();
        }

        /// <summary>
        /// Computes the full file path for the JSON log file based on directory and context.
        /// </summary>
        /// <param name="fileDirectory">The directory where the file will be stored.</param>
        /// <param name="context">The context used to generate the file name.</param>
        /// <returns>The full file path including the file name.</returns>
        private string ComputeFilePath(string fileDirectory, string context)
        {
            return Path.Combine(fileDirectory, ComputeFileName(context));
        }

        /// <summary>
        /// Generates a JSON log file name based on the context and current date.
        /// </summary>
        /// <param name="context">The context for the log file name.</param>
        /// <returns>A string representing the file name, formatted as 'context-dd-MM-yyyy.jsonl'.</returns>
        private string ComputeFileName(string context)
        {
            return $"{context}-{DateTime.Now:dd-MM-yyyy}.jsonl";
        }
    }
}
