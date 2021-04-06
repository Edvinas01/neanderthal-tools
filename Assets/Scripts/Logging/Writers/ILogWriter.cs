namespace NeanderthalTools.Logging.Writers
{
    public interface ILogWriter
    {
        /// <summary>
        /// Initialize the log writer.
        /// </summary>
        void Start();

        /// <summary>
        /// Cleanup writing log and stop the log writer.
        /// </summary>
        void Close();

        /// <summary>
        /// Write given values.
        /// </summary>
        void Write(params object[] values);
    }
}
