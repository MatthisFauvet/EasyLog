using EasyLog.entity;

namespace EasyLog.writers;

/// <summary>
/// Provides functionality to write log messages to a text file.
/// </summary>
public class FileLogWriter : ILogWriter
{
    private string _fileName;
    private string _filePath;
    private string _context;
    private StreamWriter _streamWriter; 

    /// <summary>
    /// Initializes a new instance of <see cref="FileLogWriter"/> and prepares the log file.
    /// </summary>
    /// <param name="fileDirectory">The destination directory where the log file will be created.</param>
    /// <param name="context">
    /// The context for the log, used in the file name to describe or identify the log file. 
    /// Can be a description, module name, or similar.
    /// </param>
    public FileLogWriter(string fileDirectory, string context)
    {
        _context = context;
        _filePath = ComputeFilePath(fileDirectory, context);
        InitFileStream();
    }
    
    /// <summary>
    /// Writes a log message to the file.
    /// </summary>
    /// <param name="logType">The type of log (e.g., Error, Warning, Info).</param>
    /// <param name="message">The main content of the log message, such as an exception or information.</param>
    /// <param name="timeStamp">The timestamp indicating when the event occurred.</param>
    public void write(LogType logType, string message, string timeStamp)
    {
        try
        {
            _streamWriter.WriteLine($"[{logType}] - {timeStamp} - {message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    /// <summary>
    /// Initializes the file stream for writing. Creates the directory and file if they do not exist.
    /// </summary>
    private void InitFileStream()
    {
        try
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            FileStream fs = File.Create(_filePath);
            _streamWriter = new StreamWriter(fs)
            {
                AutoFlush = true
            };
        } 
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    /// <summary>
    /// Computes the full file path for the log file based on the directory and context.
    /// </summary>
    /// <param name="fileDirectory">The directory where the file will be stored.</param>
    /// <param name="context">The context used to generate the file name.</param>
    /// <returns>The complete file path including the file name.</returns>
    private string ComputeFilePath(string fileDirectory, string context)
    {
        return Path.Combine(fileDirectory, ComputeFileName(context));
    }

    /// <summary>
    /// Generates a log file name based on the context and the current timestamp.
    /// </summary>
    /// <param name="context">The context for the log file name.</param>
    /// <returns>A string representing the file name, formatted as 'context-dd-MM-yyyy-HH-mm-ss.txt'.</returns>
    private string ComputeFileName(string context)
    {
        return context + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".txt";
    }
}
