using System.Collections.Generic;
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

        private readonly AsyncTextFileWriter writer = new AsyncTextFileWriter();

        // Current sample log names and the actual logs.
        private readonly List<string> sampleDescriptions = new List<string>();
        private readonly List<string> sampleLogs = new List<string>();

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

        public void Describe(params string[] descriptions)
        {
            sampleDescriptions.AddRange(descriptions);
        }

        public void Log(object value)
        {
            sampleLogs.Add(value.ToString());
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

            WriteLine(sampleDescriptions);
        }

        private void WriteLogs()
        {
            // New logs will be added the next sample.
            sampleLogs.Clear();

            foreach (var loggable in loggables)
            {
                loggable.AcceptLogger(this);
            }

            WriteLine(sampleLogs);
        }

        private void WriteLine(IReadOnlyList<string> values)
        {
            for (var i = 0; i < values.Count; i++)
            {
                writer.EnqueueWrite(values[i]);
                if (i + 1 < values.Count)
                {
                    writer.EnqueueWrite(",");
                }
            }

            writer.EnqueueWrite("\n");
        }

        #endregion
    }
}
