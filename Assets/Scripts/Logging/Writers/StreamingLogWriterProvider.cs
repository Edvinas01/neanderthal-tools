using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Logging.Writers
{
    [CreateAssetMenu(
        fileName = "StreamingLoggerWriterProvider",
        menuName = "Game/Logging/Streaming Log Writer Provider"
    )]
    public class StreamingLogWriterProvider : ScriptableObject
    {
        #region Editor

        [SerializeField]
        private LoggingSettings loggingSettings;

        #endregion

        #region Methods

        /// <returns>
        /// New log writer instance with given name.
        /// </returns>
        public IStreamingLogWriter CreateLogWriter(string loggerName)
        {
            if (loggingSettings.LogWriterType == LogWriterType.None)
            {
                return NoOpStreamingLogWriter.Instance;
            }

            var fileName = CreateLogFileName(loggerName);
            var writer = CreateFileWriter(fileName);

            return loggingSettings.LogWriterType switch
            {
                LogWriterType.Csv => new CsvStreamingLogWriter(writer),
                LogWriterType.Binary => new BinaryStreamingLogWriter(writer),
                _ => NoOpStreamingLogWriter.Instance
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
                loggingSettings.WriteInterval
            );
        }

        #endregion
    }
}
