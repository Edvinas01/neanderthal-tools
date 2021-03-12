using UnityEngine;
using UnityEngine.Serialization;
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

        [Range(0f, 1f)]
        [SerializeField]
        private float amplitude = 0.5f;

        [Min(0f)]
        [SerializeField]
        private float durationSeconds = 0.5f;

        #endregion

        #region Methods

        public void SendHapticImpulse(XRBaseController controller)
        {
            controller.SendHapticImpulse(amplitude, durationSeconds);
        }

        #endregion
    }
}
