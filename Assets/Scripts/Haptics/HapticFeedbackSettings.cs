using NaughtyAttributes;
using UnityEngine;

namespace NeanderthalTools.Haptics
{
    [CreateAssetMenu(
        fileName = "HapticFeedbackSettings",
        menuName = "Game/Haptics/Haptic Feedback Settings"
    )]
    public class HapticFeedbackSettings : ScriptableObject
    {
        #region Editor

        [SerializeField]
        private bool randomizeAmplitude;

        [Range(0f, 1f)]
        [SerializeField]
        [HideIf("randomizeAmplitude")]
        private float amplitude = 0.5f;

        [SerializeField]
        [MinMaxSlider(0f, 1f)]
        [ShowIf("randomizeAmplitude")]
        private Vector2 amplitudeRange = new Vector2(0f, 0.5f);

        [SerializeField]
        private bool randomizeDuration;

        [Range(0f, 10f)]
        [SerializeField]
        [HideIf("randomizeDuration")]
        private float durationSeconds = 0.5f;

        [SerializeField]
        [MinMaxSlider(0f, 10f)]
        [ShowIf("randomizeDuration")]
        private Vector2 durationSecondsRange = new Vector2(0f, 0.5f);

        [SerializeField]
        [Tooltip("Should currently playing haptics be ignored and played over")]
        private bool overridePlaying;

        #endregion

        #region Properties

        public float Amplitude => randomizeAmplitude
            ? Random.Range(amplitudeRange.x, amplitudeRange.y)
            : amplitude;

        public float Duration => randomizeDuration
            ? Random.Range(durationSecondsRange.x, durationSecondsRange.y)
            : durationSeconds;

        public bool OverridePlaying => overridePlaying;

        #endregion
    }
}
