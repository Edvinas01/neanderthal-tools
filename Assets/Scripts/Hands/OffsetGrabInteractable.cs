using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Based on: https://www.youtube.com/watch?v=-a36GpPkW-Q
namespace NeanderthalTools.Hands
{
    public class OffsetGrabInteractable : XRGrabInteractable
    {
        #region Fields

        private Vector3 interactorPosition = Vector3.zero;
        private Quaternion interactorRotation = Quaternion.identity;
        private GrabInteractableWelder interactableWelder;

        #endregion

        #region Overrides

        protected override void Awake()
        {
            base.Awake();
            interactableWelder = GetComponent<GrabInteractableWelder>();
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            // Welding doesn't support or require any kind of movement.
            if (interactableWelder != null)
            {
                return;
            }

            base.ProcessInteractable(updatePhase);
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            base.OnSelectEntering(args);

            var interactor = args.interactor;
            SetInteractorPose(interactor);
            SetAttachmentPose(interactor);
        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            base.OnSelectExiting(args);

            var interactor = args.interactor;
            ResetAttachmentPose(interactor);
            ClearInteractorPose();
        }

        #endregion

        #region Methods

        private void SetInteractorPose(XRBaseInteractor interactor)
        {
            var interactorTransform = interactor.attachTransform;
            interactorPosition = interactorTransform.localPosition;
            interactorRotation = interactorTransform.localRotation;
        }

        private void SetAttachmentPose(XRBaseInteractor interactor)
        {
            if (attachTransform != null)
            {
                return;
            }

            var interactorAttachTransform = interactor.attachTransform;
            var interactableTransform = transform;

            interactorAttachTransform.position = interactableTransform.position;
            interactorAttachTransform.rotation = interactableTransform.rotation;
        }

        private void ResetAttachmentPose(XRBaseInteractor interactor)
        {
            var interactorAttachTransform = interactor.attachTransform;
            interactorAttachTransform.localPosition = interactorPosition;
            interactorAttachTransform.localRotation = interactorRotation;
        }

        private void ClearInteractorPose()
        {
            interactorPosition = Vector3.zero;
            interactorRotation = Quaternion.identity;
        }

        #endregion
    }
}
