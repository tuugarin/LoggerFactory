namespace LoggingLayer
{
    public interface ILoggerFactory
    {
        Logger this[string name] { get; }
        Logger DefaultLogger { get; }
    }
}