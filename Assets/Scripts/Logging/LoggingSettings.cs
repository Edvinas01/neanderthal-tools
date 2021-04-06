using System;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEngine;

namespace NeanderthalTools.Logging
{
    [CreateAssetMenu(fileName = "LoggingSettings", menuName = "Game/Logging/Logging Settings")]
    public class LoggingSettings : ScriptableObject
    {
        #region Fields

        [Header("General logs")]
        [SerializeField]
        [Tooltip("Should logging be enabled")]
        private bool enableLogging;

        [SerializeField]
        [Tooltip("Should logs be uploaded to dropbox")]
        private bool uploadLogsToDropbox;

        [SerializeField]
        [ShowIf("uploadLogsToDropbox")]
        [Tooltip("Authorization token used to upload log files via Dropbox API")]
        private string dropboxAuthorizationToken;

        [SerializeField]
        [Tooltip("Directory where to store all logs")]
        private string logFileDirectory = "logs";

        [Scene]
        [SerializeField]
        [Tooltip("List of scenes which should generate logs")]
        private List<int> loggingSceneIndexes;

        [Header("Log files and types")]
        [SerializeField]
        [Tooltip("Should each log file be compressed using gzip")]
        private bool compressLogs = true;

        [SerializeField]
        [Tooltip("Suffix of log files")]
        private string logFileSuffix = "log";

        [Min(0f)]
        [SerializeField]
        [Tooltip("How often to write (dump) aggregated log samples to a file (in seconds)")]
        private float writeInterval = 0.1f;

        #endregion

        #region Properties

        public bool EnableLogging => enableLogging;

        public bool UploadLogsToDropbox => uploadLogsToDropbox;

        public string DropboxAuthorizationToken => dropboxAuthorizationToken;

        [field: NonSerialized]
        public string CurrentLogFileDirectory { get; private set; }

        public string LogFileDirectory
        {
            get => Path.Combine(logFileDirectory, CurrentLogFileDirectory);
            set => CurrentLogFileDirectory = value;
        }

        public List<int> LoggingSceneIndexes => loggingSceneIndexes;

        public bool CompressLogs => compressLogs;

        public string LogFileSuffix => logFileSuffix;

        public float WriteInterval => writeInterval;

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
