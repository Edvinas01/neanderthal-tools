namespace NeanderthalTools.Logging.Writers
{
    public interface ILogWriter
    {
        /// <summary>
        /// Initialize the log writer and underlying writing task.
        /// </summary>
        void Start();

        /// <summary>
        /// Cleanup writing task and stop the logger.
        /// </summary>
        void Close();

        /// <summary>
        /// Write given log values
        /// </summary>
        void Write(params object[] values);
    }
}
