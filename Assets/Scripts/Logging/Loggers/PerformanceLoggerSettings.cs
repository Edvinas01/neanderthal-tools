using UnityEngine;

namespace NeanderthalTools.Logging.Loggers
{
    [CreateAssetMenu(
        fileName = "PerformanceLoggerSettings",
        menuName = "Game/Logging/Performance Logger Settings"
    )]
    public class PerformanceLoggerSettings : ScriptableObject
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        [Tooltip("Sample interval in seconds")]
        private float sampleInterval = 0.01f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Threshold below which to start sampling performance")]
        private float fpsThreshold = 60f;

        #endregion

        #region Properties

        public float SampleInterval => sampleInterval;

        public float FpsThreshold => fpsThreshold;

        #endregion
    }
}
