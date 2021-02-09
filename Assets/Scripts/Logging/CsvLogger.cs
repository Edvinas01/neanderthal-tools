using System.Globalization;
using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Logging
{
    public class CsvLogger : MonoBehaviour, IDescribable, ILogger
    {
        #region Editor

        [SerializeField]
        private LoggableCollection loggables;

        [SerializeField]
        private LoggingSettings settings;

        #endregion

        #region Fields

        private readonly AsyncFileWriter writer = new AsyncFileWriter();
        private float nextSample;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            gameObject.SetActive(settings.EnableLogging);
            SetupFileWriter();
        }

        private void OnEnable()
        {
            writer.Start();
        }

        private void OnDisable()
        {
            writer.Stop();
        }

        private void Start()
        {
            SetupLoggables();
            WriteDescriptions();
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

        public void Describe(params string[] values)
        {
            foreach (var value in values)
            {
                writer.Write(value);
                writer.Write(",");
            }
        }

        public void Log(float value)
        {
            writer.Write(value.ToString(CultureInfo.InvariantCulture));
            writer.Write(",");
        }

        #endregion

        #region Methods

        private void SetupFileWriter()
        {
            writer.WriteIntervalSeconds = settings.WriteIntervalSeconds;
            writer.FileDirectory = settings.LogFileDirectory;
            writer.FileSuffix = settings.LogFileSuffix;
            writer.CompressFile = settings.CompressLogs;
        }

        private void SetupLoggables()
        {
            loggables.Sort();
        }

        private void WriteDescriptions()
        {
            foreach (var loggable in loggables)
            {
                loggable.AcceptDescribable(this);
            }

            writer.Write("\n");
        }

        private void WriteLogs()
        {
            foreach (var loggable in loggables)
            {
                loggable.AcceptLogger(this);
            }

            writer.Write("\n");
        }

        #endregion
    }
}
