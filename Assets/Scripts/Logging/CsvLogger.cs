using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace NeanderthalTools.Logging
{
    public class CsvLogger : MonoBehaviour, IDescribable, ILogger
    {
        #region Editor

        [SerializeField]
        private LoggableCollection loggables;

        [Min(0f)]
        [SerializeField]
        private float logIntervalSeconds = 0.1f;

        #endregion

        #region Fields

        private StreamWriter streamWriter;
        private float nextLogSeconds;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            SetupWriter();
        }

        private void OnDisable()
        {
            CleanupWriter();
        }

        private void Start()
        {
            SetupLoggables();
            DescribeLoggables();
        }

        private void Update()
        {
            if (Time.time < nextLogSeconds)
            {
                return;
            }

            LogLoggables();

            nextLogSeconds = Time.time + logIntervalSeconds;
        }

        #endregion

        #region Overrides

        public void Describe(params string[] values)
        {
            foreach (var value in values)
            {
                streamWriter.Write(value);
                streamWriter.Write(",");
            }
        }

        public void Log(float value)
        {
            streamWriter.Write(value);
            streamWriter.Write(",");
        }

        #endregion

        #region Methods

        private void SetupWriter()
        {
            var date = DateTime.UtcNow;
            var path = Application.persistentDataPath + $"/{date:yyyy-MM-dd hh-mm-ss}.log";

            var directoryName = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryName ?? string.Empty);

            streamWriter = new StreamWriter(path);
        }

        private void CleanupWriter()
        {
            streamWriter?.Close();
        }

        private void SetupLoggables()
        {
            loggables.Sort();
        }

        private void DescribeLoggables()
        {
            foreach (var loggable in loggables)
            {
                loggable.Accept((IDescribable) this);
            }

            streamWriter.Write("\n");
        }

        private void LogLoggables()
        {
            foreach (var loggable in loggables)
            {
                loggable.Accept((ILogger) this);
            }

            streamWriter.Write("\n");
        }

        #endregion
    }
}
