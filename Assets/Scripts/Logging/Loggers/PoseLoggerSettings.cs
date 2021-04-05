using UnityEngine;

namespace NeanderthalTools.Logging.Loggers
{
    [CreateAssetMenu(
        fileName = "PoseLoggerSettings",
        menuName = "Game/Logging/Pose Logger Settings"
    )]
    public class PoseLoggerSettings : ScriptableObject
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        [Tooltip("Sample interval in seconds")]
        private float sampleInterval = 0.1f;

        #endregion

        #region Properties

        public float SampleInterval => sampleInterval;

        #endregion
    }
}
