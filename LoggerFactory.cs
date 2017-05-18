using System.Collections.Generic;

namespace LoggingLayer
{
    public class LoggerFactory : ILoggerFactory
    {
        static volatile LoggerFactory _loggerFactory;
        static readonly Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();
        static readonly object sync = new object();

        private LoggerFactory()
        {

        }
        public static LoggerFactory Instance
        {
            get
            {
                if (_loggerFactory == null)
                {
                    lock (sync)
                    {
                        if (_loggerFactory == null)
                        {
                            _loggerFactory = new LoggerFactory();
                        }
                    }
                }
                return _loggerFactory;
            }
        }

        public Logger DefaultLogger
        {
            get
            {
                return this["DefaultLog"];
            }
        }

        public Logger this[string name]
        {
            get
            {
                lock (sync)
                {
                    if (!loggers.ContainsKey(name))
                    {
                        var logger = new Logger(name);
                        loggers.Add(name, logger);
                        return logger;
                    }
                    else return loggers[name];
                }
            }
        }
    }
}