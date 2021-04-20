using NeanderthalTools.Haptics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    public class InteractionHapticFeedbackAdapter : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private HapticFeedbackSettings settings;

        #endregion

        #region Methods

        public void SendHapticImpulse(BaseInteractionEventArgs args)
        {
            settings.SendHapticImpulse(args.interactor);
        }

        #endregion
    }
}
