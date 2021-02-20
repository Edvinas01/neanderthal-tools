using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Logging.Writers
{
    public abstract class BaseLogWriter : ILogWriter
    {
        #region Fields

        private readonly AsyncFileWriter writer;
        private readonly float sampleIntervalSeconds;

        private float nextSampleTime;

        #endregion

        #region Methods

        protected BaseLogWriter(AsyncFileWriter writer, float sampleIntervalSeconds)
        {
            this.writer = writer;
            this.sampleIntervalSeconds = sampleIntervalSeconds;
        }

        public void Write(params object[] values)
        {
            if (Time.time < nextSampleTime)
            {
                return;
            }

            WriteValues(values);
            nextSampleTime = Time.time + sampleIntervalSeconds;
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
            nextSampleTime = 0f;
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
