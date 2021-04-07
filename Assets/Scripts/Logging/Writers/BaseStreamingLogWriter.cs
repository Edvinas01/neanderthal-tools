using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Logging.Writers
{
    public abstract class BaseStreamingLogWriter : ILogWriter
    {
        #region Fields

        private readonly AsyncFileWriter writer;

        #endregion

        #region Methods

        protected BaseStreamingLogWriter(AsyncFileWriter writer)
        {
            this.writer = writer;
        }

        public void Write(params object[] values)
        {
            WriteValues(values);
        }

        /// <summary>
        /// Write provided list of values to disk.
        /// </summary>
        protected abstract void WriteValues(params object[] values);

        public void Start()
        {
            writer.Start();
            Debug.Log($"Writing logs to: {writer.FilePath}");
        }

        public void Close()
        {
            writer.Close();
            Debug.Log($"Stopped writing logs to: {writer.FilePath}");
        }

        /// <summary>
        /// Write bytes to disk.
        /// </summary>
        protected void Write(byte[] value)
        {
            writer.Write(value);
        }

        #endregion
    }
}
