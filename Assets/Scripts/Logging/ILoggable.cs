namespace NeanderthalTools.Logging
{
    public interface ILoggable
    {
        void Accept(ILogger logger);
    }
}
