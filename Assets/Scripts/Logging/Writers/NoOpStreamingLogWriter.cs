namespace NeanderthalTools.Logging.Writers
{
    /// <summary>
    /// Log writer which does nothing.
    /// </summary>
    public class NoOpStreamingLogWriter : IStreamingLogWriter
    {
        #region Fields

        public static readonly IStreamingLogWriter Instance = new NoOpStreamingLogWriter();

        #endregion

        #region Methods

        private NoOpStreamingLogWriter()
        {
        }

        #endregion

        #region Overrides

        public void Start()
        {
        }

        public void Close()
        {
        }

        public void Write(params object[] values)
        {
        }

        #endregion
    }
}
