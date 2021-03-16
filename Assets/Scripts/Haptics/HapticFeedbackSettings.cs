using NaughtyAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

        #endregion

        #region Methods

        public void SendHapticImpulse(params XRBaseController[] controllers)
        {
            var calculatedAmplitude = GetAmplitude();
            var calculatedDuration = GetDurationSeconds();

            foreach (var controller in controllers)
            {
                controller.SendHapticImpulse(calculatedAmplitude, calculatedDuration);
            }
        }

        private float GetAmplitude()
        {
            if (randomizeAmplitude)
            {
                return Random.Range(amplitudeRange.x, amplitudeRange.y);
            }

            return amplitude;
        }

        private float GetDurationSeconds()
        {
            if (randomizeDuration)
            {
                return Random.Range(durationSecondsRange.x, durationSecondsRange.y);
            }

            return durationSeconds;
        }

        #endregion
    }
}
