using NeanderthalTools.Logging.Writers;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    public class SessionLogger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private LogWriterProvider logWriterProvider;

        [SerializeField]
        private LoggingSettings loggingSettings;

        #endregion

        #region Fields

        private ILogWriter logWriter;
        private SessionData sessionData;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            SetupSessionData();
            SetupLogWriter();
        }

        private void OnDisable()
        {
            CleanupLogWriter();
        }

        private void Start()
        {
            // todo debug
            sessionData.Pickups.Add(new PickupData
            {
                HandName = "gello"
            });
        }

        #endregion

        #region Methods

        private void SetupSessionData()
        {
            sessionData = new SessionData
            {
                LoggingId = loggingSettings.LoggingId
            };
        }

        private void SetupLogWriter()
        {
            logWriter = logWriterProvider.CreateLogWriter(name);
            logWriter.Start();
        }

        private void CleanupLogWriter()
        {
            logWriter.Write(sessionData);
            logWriter.Close();
            logWriter = null;
        }

        #endregion
    }
}
