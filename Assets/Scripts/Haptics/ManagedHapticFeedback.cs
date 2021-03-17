using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Haptics
{
    [RequireComponent(typeof(XRBaseController))]
    public class ManagedHapticFeedback : MonoBehaviour
    {
        #region Fields

        private XRBaseController controller;
        private float availableAt;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            controller = GetComponent<XRBaseController>();
        }

        #endregion

        #region Methods

        public void SendHapticImpulse(HapticFeedbackSettings settings)
        {
            var currentTime = Time.time;
            if (settings.OverridePlaying || availableAt < currentTime)
            {
                controller.SendHapticImpulse(settings.Amplitude, settings.Duration);
                availableAt = currentTime + settings.Duration;
            }
        }

        #endregion
    }
}
