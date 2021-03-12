using NeanderthalTools.Haptics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Knapping
{
    public class FlakeHapticFeedbackAdapter : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private HapticFeedbackSettings settings;

        #endregion

        #region Methods

        public void SendHapticImpulse(FlakeEventArgs args)
        {
            var interactor = args.Interactor;
            if (interactor == null)
            {
                return;
            }

            var controller = interactor.GetComponent<XRBaseController>();
            if (controller == null)
            {
                return;
            }

            settings.SendHapticImpulse(controller);
        }

        #endregion
    }
}
