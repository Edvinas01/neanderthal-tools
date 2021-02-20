using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Logging.Writers
{
    [CreateAssetMenu(
        fileName = "LoggerWriterProvider",
        menuName = "Game/Logging/Log Writer Provider"
    )]
    public class LogWriterProvider : ScriptableObject
    {
        #region Editor

        [SerializeField]
        private LoggingSettings loggingSettings;

        [Min(0f)]
        [SerializeField]
        [Tooltip("How often to sample logs from loggables (in seconds)")]
        protected float sampleIntervalSeconds = 0.01f;

        #endregion

        #region Methods

        /// <returns>
        /// New log writer instance with given name.
        /// </returns>
        public ILogWriter CreateLogWriter(string loggerName)
        {
            if (loggingSettings.LogWriterType == LogWriterType.None)
            {
                return NoOpLogWriter.Instance;
            }

            var fileName = CreateLogFileName(loggerName);
            var writer = CreateFileWriter(fileName);

            return loggingSettings.LogWriterType switch
            {
                LogWriterType.Csv => new CsvLogWriter(writer, sampleIntervalSeconds),
                LogWriterType.Binary => new BinaryLogWriter(writer, sampleIntervalSeconds),
                _ => NoOpLogWriter.Instance
            };
        }

        private string CreateLogFileName(string loggerName)
        {
            var logFileName = string.IsNullOrWhiteSpace(loggerName) ? name : loggerName;

            return $"{logFileName}.{loggingSettings.LogFileSuffix}";
        }

        private AsyncFileWriter CreateFileWriter(string fileName)
        {
            return new AsyncFileWriter(
                loggingSettings.LogFileDirectory,
                fileName,
                loggingSettings.CompressLogs,
                loggingSettings.WriteIntervalSeconds
            );
        }

        #endregion
    }
}
