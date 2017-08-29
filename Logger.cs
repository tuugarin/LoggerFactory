using System;
using System.Diagnostics;
using System.IO;

namespace LoggingLayer
{
    public class Logger : ILogger, IDisposable
    {

        private readonly TraceSource traceSource;
        DateTime currentDate = DateTime.Now;
        string pathLogs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        string name;

        internal Logger(string name)
        {
            traceSource = new TraceSource(name, SourceLevels.All);
            this.name = name;
            traceSource.Listeners.Add(CreateNewXmlWriterTraceListner());
        }

        public void Log(string message, TraceEventType type)
        {
            if (currentDate.Date != DateTime.Now.Date)
            {
                currentDate = DateTime.Now;
                traceSource.Listeners[name].Flush();
                traceSource.Listeners[name].Close();
                traceSource.Listeners.Remove(name);

                traceSource.Listeners.Add(CreateNewXmlWriterTraceListner());
            }

            traceSource.TraceEvent(type, 1,
              message);
        }
        public void LogInfo(string message)
        {
            var userName = System.Threading.Thread.CurrentPrincipal?.Identity?.Name;
            Log($"{message} " + (string.IsNullOrEmpty(userName) ? string.Empty : $"UserName : { userName}"), TraceEventType.Information);
        }
        public void LogError(string message)
        {
            var userName = System.Threading.Thread.CurrentPrincipal?.Identity?.Name;
            Log($"{message} " + (string.IsNullOrEmpty(userName) ? string.Empty : $"UserName : { userName}"), TraceEventType.Error);
        }
        XmlWriterTraceListener CreateNewXmlWriterTraceListner()
        {
            var path = Path.Combine(pathLogs, currentDate.ToString("ddMMyyyy"));
            Directory.CreateDirectory(path);

            return new XmlWriterTraceListener(Path.Combine(path, string.Format("{0}.xml", name)), name);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (TraceListener listner in traceSource.Listeners)
                    {
                        listner.Flush();
                        listner.Close();
                        listner.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}
