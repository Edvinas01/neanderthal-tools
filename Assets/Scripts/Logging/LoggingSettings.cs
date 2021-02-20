using NeanderthalTools.Logging.Writers;
using UnityEngine;

namespace NeanderthalTools.Logging
{
    [CreateAssetMenu(fileName = "LoggingSettings", menuName = "Game/Logging/Logging Settings")]
    public class LoggingSettings : ScriptableObject
    {
        #region Fields

        [SerializeField]
        [Tooltip("Log file type for all log writers")]
        private LogWriterType logWriterType = LogWriterType.None;

        [SerializeField]
        [Tooltip("Should each log file be compressed using gzip")]
        private bool compressLogs = true;

        [SerializeField]
        [Tooltip("Directory where to store all logs")]
        private string logFileDirectory = "logs";

        [SerializeField]
        [Tooltip("Suffix of log files")]
        private string logFileSuffix = "log";

        [Min(0f)]
        [SerializeField]
        [Tooltip("How often to write (dump) aggregated log samples to a file (in seconds)")]
        private float writeIntervalSeconds = 0.1f;

        #endregion

        #region Properties

        public LogWriterType LogWriterType => logWriterType;

        public bool CompressLogs => compressLogs;

        public string LogFileDirectory => logFileDirectory;

        public string LogFileSuffix => logFileSuffix;

        public float WriteIntervalSeconds => writeIntervalSeconds;

        #endregion
    }
}
