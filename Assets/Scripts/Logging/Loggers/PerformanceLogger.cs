using NeanderthalTools.Logging.Writers;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers
{
    public class PerformanceLogger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private LogWriterProvider logWriterProvider;

        #endregion

        #region Fields

        private ILogWriter logWriter;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            logWriter = logWriterProvider.CreateLogWriter(name);

            logWriter.Start();
            logWriter.Write(
                "Time",
                "FPS"
            );
        }

        private void OnDisable()
        {
            logWriter.Close();
            logWriter = null;
        }

        private void Update()
        {
            var fps = 1f / Time.deltaTime;
            logWriter.Write(Time.time, fps);
        }

        #endregion
    }
}
