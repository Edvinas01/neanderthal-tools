using UnityEngine;

namespace NeanderthalTools.Logging
{
    public class SpatialLoggable : MonoBehaviour, ILoggable
    {
        #region Editor

        [SerializeField]
        private LoggableCollection loggables;

        #endregion

        #region Fields

        private Transform loggableTransform;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            loggableTransform = transform;
        }

        private void OnEnable()
        {
            loggables.Add(this);
        }

        private void OnDisable()
        {
            loggables.Remove(this);
        }

        #endregion

        #region Overrides

        public void Accept(ILogger logger)
        {
            var position = loggableTransform.position;
            var rotation = loggableTransform.rotation;

            logger.Log(position);
            logger.Log(rotation);
        }

        #endregion
    }
}
