using System.Xml;
using EasyLog.entity;

namespace EasyLog.writers
{
    /// <summary>
    /// Provides functionality to write log messages to an XML file.
    /// </summary>
    public class XmlFileWriter : ILogWriter, IDisposable
    {
        private readonly string _context;
        private readonly string _filePath;
        private readonly XmlWriter _xmlWriter;

        private readonly object _writeLock = new object();
        private bool _disposed = false;

        public XmlFileWriter(string fileDirectory, string context)
        {
            _context = context;
            _filePath = ComputeFilePath(fileDirectory, context);

            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = false,
                CloseOutput = true
            };

            bool fileExists = File.Exists(_filePath);

            _xmlWriter = XmlWriter.Create(_filePath, settings);

            if (!fileExists)
            {
                _xmlWriter.WriteStartDocument();
                _xmlWriter.WriteStartElement("logs");
                _xmlWriter.Flush();
            }
        }

        public void write(LogType logType, string timeStamp, Dictionary<string, string> message)
        {
            lock (_writeLock)
            {
                // Guard against writing after Dispose()
                if (_disposed) return;

                try
                {
                    _xmlWriter.WriteStartElement("log");
                    _xmlWriter.WriteAttributeString("timestamp", timeStamp);
                    _xmlWriter.WriteAttributeString("level", logType.ToString());
                    _xmlWriter.WriteAttributeString("context", _context);

                    foreach (var kvp in message)
                    {
                        _xmlWriter.WriteStartElement("message");
                        _xmlWriter.WriteAttributeString("key", kvp.Key);
                        _xmlWriter.WriteString(kvp.Value);
                        _xmlWriter.WriteEndElement();
                    }

                    _xmlWriter.WriteEndElement(); // log
                    _xmlWriter.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void Dispose()
        {
            lock (_writeLock)
            {
                if (_disposed) return;
                try
                {
                    _xmlWriter.WriteEndElement(); // logs
                    _xmlWriter.WriteEndDocument();
                    _xmlWriter.Flush();
                    _xmlWriter.Dispose();
                }
                catch
                {
                    // ignore dispose errors
                }
                finally
                {
                    // Always mark as disposed, even if the closing tags failed
                    _disposed = true;
                }
            }
            }

        private string ComputeFilePath(string fileDirectory, string context)
        {
            return Path.Combine(fileDirectory, ComputeFileName(context));
        }

        private string ComputeFileName(string context)
        {
            return $"{context}-{DateTime.Now:dd-MM-yyyy}.xml";
        }
    }
}
