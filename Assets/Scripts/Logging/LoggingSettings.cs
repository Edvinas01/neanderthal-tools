using System;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using NeanderthalTools.Logging.Writers;
using UnityEngine;

namespace NeanderthalTools.Logging
{
    [CreateAssetMenu(fileName = "LoggingSettings", menuName = "Game/Logging/Logging Settings")]
    public class LoggingSettings : ScriptableObject
    {
        #region Fields

        [SerializeField]
        [Tooltip("Should logging be enabled")]
        private bool enableLogging;

        [SerializeField]
        [Tooltip("Should each log file be compressed using gzip")]
        private bool compressLogs = true;

        [SerializeField]
        [Tooltip("Should logs be uploaded to dropbox")]
        private bool uploadLogsToDropbox;

        [SerializeField]
        [ShowIf("uploadLogsToDropbox")]
        [Tooltip("Authorization token used to upload log files via Dropbox API")]
        private string dropboxAuthorizationToken;

        [SerializeField]
        [Tooltip("Log file type for all log writers")]
        private LogWriterType logWriterType = LogWriterType.None;

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

        [Scene]
        [SerializeField]
        [Tooltip("List of scenes which should generate logs")]
        private List<int> loggingSceneIndexes;

        #endregion

        #region Properties

        public bool EnableLogging => enableLogging;

        public bool CompressLogs => compressLogs;

        public bool UploadLogsToDropbox => uploadLogsToDropbox;

        public string DropboxAuthorizationToken => dropboxAuthorizationToken;

        public LogWriterType LogWriterType => logWriterType;

        [field: NonSerialized]
        public string CurrentLogFileDirectory { get; private set; }

        public string LogFileDirectory
        {
            get => Path.Combine(logFileDirectory, CurrentLogFileDirectory);
            set => CurrentLogFileDirectory = value;
        }

        public string LogFileSuffix => logFileSuffix;

        public float WriteIntervalSeconds => writeIntervalSeconds;

        public List<int> LoggingSceneIndexes => loggingSceneIndexes;

        public string LoggingId
        {
            get
            {
                var loggingId = PlayerPrefs.GetString("LoggingId");
                if (string.IsNullOrWhiteSpace(loggingId))
                {
                    loggingId = Guid.NewGuid().ToString();
                    PlayerPrefs.SetString("LoggingId", loggingId);
                }

                return loggingId;
            }
        }

        #endregion
    }
}
