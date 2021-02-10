namespace NeanderthalTools.Logging
{
    public interface ILoggable
    {
        int Order { get; }

        void AcceptMetaLogger(IMetaLogger metaLogger);

        void AcceptLogger(ILogger logger);
    }
}
