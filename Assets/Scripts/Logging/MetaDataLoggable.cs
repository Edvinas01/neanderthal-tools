using UnityEngine;

namespace NeanderthalTools.Logging
{
    public class MetaDataLoggable : MonoBehaviour, ILoggable
    {
        #region Editor

        [SerializeField]
        private LoggableCollection loggables;

        #endregion

        #region Fields

        private float fps;

        #endregion

        #region Unity Lifecycle

        private void OnDisable()
        {
            loggables.Remove(this);
        }

        private void OnEnable()
        {
            loggables.Add(this);
        }

        private void Update()
        {
            fps = 1f / Time.deltaTime;
        }

        #endregion

        #region Overrides

        public int Order => -1;

        public void AcceptMetaLogger(IMetaLogger metaLogger)
        {
            metaLogger.LogMeta("Time");
            metaLogger.LogMeta("FPS");
        }

        public void AcceptLogger(ILogger logger)
        {
            logger.Log(Time.time);
            logger.Log(fps);
        }

        #endregion
    }
}
