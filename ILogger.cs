using System.Diagnostics;

namespace LoggingLayer
{
    public interface ILogger
    {
        void Log(string message, TraceEventType type);
        void LogError(string message);
        void LogInfo(string message);
    }
}