using System.Collections.Generic;
using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Logging
{
    public abstract class BaseLogger : MonoBehaviour, IMetaLogger, ILogger
    {
        #region Editor

        [SerializeField]
        private LoggableCollection loggables;

        [SerializeField]
        private LoggingSettings settings;

        #endregion

        #region Fields

        // Keeps a list of values that have been written during this "sample".
        private readonly List<object> buffer = new List<object>();

        // Async writing to disk.
        private AsyncFileWriter writer;

        // When should the next "sample" buffer be taken.
        private float nextSample;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            gameObject.SetActive(settings.EnableLogging);
        }

        private void OnEnable()
        {
            SetupWriter();
        }

        private void OnDisable()
        {
            CloseWriter();
        }

        private void Start()
        {
            SetupLoggables();
            WriteMeta();
            Debug.Log($"Writing logs to: {writer.FilePath}");
        }

        private void Update()
        {
            if (Time.time < nextSample)
            {
                return;
            }

            WriteLogs();

            nextSample = Time.time + settings.SampleIntervalSeconds;
        }

        #endregion

        #region Overrides

        public void LogMeta(object value)
        {
            buffer.Add(value.ToString());
        }

        public void Log(object value)
        {
            buffer.Add(value.ToString());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Write provided list of raw object values to file.
        /// </summary>
        protected abstract void Write(IReadOnlyList<object> values);

        /// <summary>
        /// Write provided byte value to file.
        /// </summary>
        protected void Write(byte[] value)
        {
            writer.Write(value);
        }

        private void SetupWriter()
        {
            writer = new AsyncFileWriter(
                settings.LogFileDirectory,
                $"{Files.FileDateName()}.{settings.LogFileSuffix}",
                settings.CompressLogs,
                settings.WriteIntervalSeconds
            );

            writer.Start();
        }

        private void CloseWriter()
        {
            writer.Close();
            writer = null;
        }

        private void SetupLoggables()
        {
            loggables.Sort();
        }

        private void WriteMeta()
        {
            foreach (var loggable in loggables)
            {
                loggable.AcceptMetaLogger(this);
            }

            Write(buffer);
            buffer.Clear();
        }

        private void WriteLogs()
        {
            foreach (var loggable in loggables)
            {
                loggable.AcceptLogger(this);
            }

            Write(buffer);
            buffer.Clear();
        }

        #endregion
    }
}
