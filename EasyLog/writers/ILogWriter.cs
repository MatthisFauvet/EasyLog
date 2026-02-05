using EasyLog.entity;

namespace EasyLog.writers;

public interface ILogWriter
{
    public void write(LogType logType, string message, string timeStamp);
}