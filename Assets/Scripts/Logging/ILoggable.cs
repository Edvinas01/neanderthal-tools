namespace NeanderthalTools.Logging
{
    public interface ILoggable
    {
        int Order { get; }

        void Accept(IDescribable describable);

        void Accept(ILogger logger);
    }
}
