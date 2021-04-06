using NeanderthalTools.Logging.Writers;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers
{
    public class PerformanceLogger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private LogWriterProvider logWriterProvider;

        [SerializeField]
        private PerformanceLoggerSettings performanceLoggerSettings;

        #endregion

        #region Fields

        private ILogWriter logWriter;
        private float nextSampleTime;
        private float fps;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            SetupLogWriter();
            LogMeta();
        }

        private void OnDisable()
        {
            CleanupLogWriter();
        }

        private void Update()
        {
            UpdateFps();
            if (!IsFpsThreshold() || !IsSample())
            {
                return;
            }

            UpdateNextSampleTime();
            LogPerformance();
        }

        #endregion

        #region Methods

        private void SetupLogWriter()
        {
            logWriter = logWriterProvider.CreateLogWriter(name);
            logWriter.Start();
        }

        private void CleanupLogWriter()
        {
            logWriter.Close();
            logWriter = null;
        }

        private void LogMeta()
        {
            logWriter.Write(
                "Time",
                "FPS"
            );
        }

        private void UpdateFps()
        {
            fps = 1f / Time.deltaTime;
        }

        private bool IsFpsThreshold()
        {
            return fps <= performanceLoggerSettings.FpsThreshold;
        }

        private bool IsSample()
        {
            return Time.time >= nextSampleTime;
        }

        private void UpdateNextSampleTime()
        {
            nextSampleTime = Time.time + performanceLoggerSettings.SampleInterval;
        }

        private void LogPerformance()
        {
            logWriter.Write(Time.time, fps);
        }

        #endregion
    }
}
