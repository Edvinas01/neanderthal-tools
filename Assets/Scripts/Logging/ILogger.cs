namespace NeanderthalTools.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Log data, such as position, fps, etc.
        /// </summary>
        void Log(object value);
    }
}
