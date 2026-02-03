using EasyLog.entity;

namespace EasyLog.writers;

public class FileLogWriter : ILogWriter
{

    private string _fileName;

    private string _filePath;

    public FileLogWriter(string filePath, string context)
    {
        _filePath = filePath;
        //_fileName = computeFileName(context);
    }
    
    public void write(LogType logType, string message, DateTime timeStamp)
    {
        throw new NotImplementedException();
    }

    private void computeFileName(string context)
    {
        //TODO In the futur this method have to return a String. Return as a string a name for the file.  
    }
}