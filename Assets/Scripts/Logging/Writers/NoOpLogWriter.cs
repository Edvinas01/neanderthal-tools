namespace NeanderthalTools.Logging.Writers
{
    /// <summary>
    /// Log writer which does nothing.
    /// </summary>
    public class NoOpLogWriter : ILogWriter
    {
        #region Fields

        public static readonly ILogWriter Instance = new NoOpLogWriter();

        #endregion

        #region Methods

        private NoOpLogWriter()
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
