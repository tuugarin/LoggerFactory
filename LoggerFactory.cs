using System.Collections.Generic;
using System.Threading;

namespace LoggingLayer
{
    public class LoggerFactory : ILoggerFactory
    {
        static volatile LoggerFactory _loggerFactory = new LoggerFactory();
        static readonly Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();
        static readonly object sync = new object();

        private LoggerFactory()
        {

        }
        public static LoggerFactory Instance
        {
            get
            {
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

                if (!loggers.ContainsKey(name))
                {
                    var logger = new Logger(name);
                    Monitor.Enter(sync);
                    loggers.Add(name, logger);
                    Monitor.Exit(sync);
                    return logger;
                }
                else return loggers[name];
            }

        }
    }
}