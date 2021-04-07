using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Logging.Writers
{
    [CreateAssetMenu(
        fileName = "LogWriterProvider",
        menuName = "Game/Logging/Log Writer Provider"
    )]
    public class LogWriterProvider : ScriptableObject
    {
        #region Editor

        [SerializeField]
        private LoggingSettings loggingSettings;

        [SerializeField]
        [Tooltip("Log file type for streaming log writers")]
        private LogWriterType logWriterType = LogWriterType.None;

        [SerializeField]
        private string logFileSuffix = "log";

        [SerializeField]
        [Tooltip("Should each log file be compressed using gzip")]
        private bool compressLogs = true;

        [Min(0f)]
        [SerializeField]
        [Tooltip("How often to write (dump) aggregated log samples to a file (in seconds)")]
        private float writeInterval = 0.1f;

        #endregion

        #region Methods

        /// <returns>
        /// New log writer instance with given name.
        /// </returns>
        public ILogWriter CreateLogWriter(string loggerName)
        {
            if (logWriterType == LogWriterType.None)
            {
                return NoOpLogWriter.Instance;
            }

            var fileName = CreateLogFileName(loggerName);

            return logWriterType switch
            {
                LogWriterType.Csv => CreateCsvStreamingLogWriter(fileName),
                LogWriterType.Binary => CreateBinaryStreamingLogWriter(fileName),
                LogWriterType.Json => CreateJsonFileLogWriter(fileName),
                _ => NoOpLogWriter.Instance
            };
        }

        private string CreateLogFileName(string loggerName)
        {
            return Files.CreateFileName(this, loggerName, logFileSuffix);
        }

        private CsvStreamingLogWriter CreateCsvStreamingLogWriter(string fileName)
        {
            var writer = CreateFileWriter(fileName);
            return new CsvStreamingLogWriter(writer);
        }

        private BinaryStreamingLogWriter CreateBinaryStreamingLogWriter(string fileName)
        {
            var writer = CreateFileWriter(fileName);
            return new BinaryStreamingLogWriter(writer);
        }

        private JsonLogWriter CreateJsonFileLogWriter(string fileName)
        {
            var filePath = Files.CreateFilePath(
                loggingSettings.LogFileDirectory,
                fileName,
                compressLogs
            );

            Files.CreateDirectory(filePath);

            return new JsonLogWriter(filePath, compressLogs);
        }

        private AsyncFileWriter CreateFileWriter(string fileName)
        {
            return new AsyncFileWriter(
                loggingSettings.LogFileDirectory,
                fileName,
                compressLogs,
                writeInterval
            );
        }

        #endregion
    }
}
