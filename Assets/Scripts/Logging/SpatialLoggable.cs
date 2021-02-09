using UnityEngine;

namespace NeanderthalTools.Logging
{
    public class SpatialLoggable : MonoBehaviour, ILoggable
    {
        #region Editor

        [SerializeField]
        private LoggableCollection loggables;

        [Min(0)]
        [SerializeField]
        private int order;

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

        public int Order => order;

        public void Accept(IDescribable describable)
        {
            describable.Describe(
                $"{name}PositionX",
                $"{name}PositionY",
                $"{name}PositionZ",
                $"{name}RotationX",
                $"{name}RotationY",
                $"{name}RotationZ"
            );
        }

        public void Accept(ILogger logger)
        {
            var position = loggableTransform.position;
            var rotation = loggableTransform.rotation.eulerAngles;

            logger.Log(position.x);
            logger.Log(position.y);
            logger.Log(position.z);

            logger.Log(rotation.x);
            logger.Log(rotation.y);
            logger.Log(rotation.z);
        }

        #endregion
    }
}
