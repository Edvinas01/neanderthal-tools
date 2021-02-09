namespace NeanderthalTools.Logging
{
    public interface ILoggable
    {
        int Order { get; }

        void AcceptDescribable(IDescribable describable);

        void AcceptLogger(ILogger logger);
    }
}
