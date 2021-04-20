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

        [SerializeField]
        private XRBaseInteractable interactable;

        [SerializeField]
        [Tooltip("Adapt grab events")]
        private bool isAdaptSelectEntered;

        [SerializeField]
        [Tooltip("Adapt hover events")]
        private bool isAdaptHoverEntered;

        #endregion

        #region Fields

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            if (isAdaptSelectEntered)
            {
                interactable.selectEntered.AddListener(SendHapticImpulse);
            }

            if (isAdaptHoverEntered)
            {
                interactable.hoverEntered.AddListener(SendHapticImpulse);
            }
        }

        private void OnDisable()
        {
            if (isAdaptSelectEntered)
            {
                interactable.selectEntered.RemoveListener(SendHapticImpulse);
            }

            if (isAdaptHoverEntered)
            {
                interactable.hoverEntered.RemoveListener(SendHapticImpulse);
            }
        }

        #endregion

        #region Methods

        public void SendHapticImpulse(BaseInteractionEventArgs args)
        {
            settings.SendHapticImpulse(args.interactor);
        }

        #endregion
    }
}
