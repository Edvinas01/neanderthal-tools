using UnityEngine;

namespace NeanderthalTools.Logging
{
    [CreateAssetMenu(fileName = "LoggingSettings", menuName = "Game/Logging Settings")]
    public class LoggingSettings : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private string logFileDirectory = "logs";

        [SerializeField]
        private string logFileSuffix = "log";

        [Min(0f)]
        [SerializeField]
        [Tooltip("How often to sample logs from loggables, match with target FPS")]
        private float sampleIntervalSeconds = 0.01f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("How often to write (dump) all log samples to file - optimization")]
        private float writeIntervalSeconds = 0.1f;

        [SerializeField]
        private bool enableLogging = true;

        [SerializeField]
        private bool compressLogs = true;

        #endregion

        #region Properties

        public string LogFileDirectory => logFileDirectory;

        public string LogFileSuffix => logFileSuffix;

        public float SampleIntervalSeconds => sampleIntervalSeconds;

        public float WriteIntervalSeconds => writeIntervalSeconds;

        public bool EnableLogging => enableLogging;

        public bool CompressLogs => compressLogs;

        #endregion
    }
}
