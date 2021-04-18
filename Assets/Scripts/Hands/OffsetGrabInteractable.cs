using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Based on: https://www.youtube.com/watch?v=-a36GpPkW-Q
namespace NeanderthalTools.Hands
{
    [RequireComponent(typeof(Rigidbody))]
    public class OffsetGrabInteractable : XRGrabInteractable
    {
        #region Fields

        private new Rigidbody rigidbody;

        private Vector3 savedLocalAttachPosition;
        private Quaternion savedLocalAttachRotation;

        #endregion

        #region Overrides

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
        }

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            if (attachTransform == null)
            {
                var interactorAttachTransform = args.interactor.attachTransform;
                SaveAttachPose(interactorAttachTransform);
                ApplyOffsetAttachPose(interactorAttachTransform);
            }

            base.OnSelectEntering(args);
        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            if (attachTransform == null)
            {
                var interactorAttachTransform = args.interactor.attachTransform;
                ApplySavedAttachPose(interactorAttachTransform);
                ClearSavedAttachPose();
            }

            base.OnSelectExiting(args);
        }

        #endregion

        #region Methods

        private void SaveAttachPose(Transform interactorAttachTransform)
        {
            savedLocalAttachPosition = interactorAttachTransform.localPosition;
            savedLocalAttachRotation = interactorAttachTransform.localRotation;
        }

        private void ApplyOffsetAttachPose(Transform interactorAttachTransform)
        {
            interactorAttachTransform.SetPositionAndRotation(
                rigidbody.worldCenterOfMass,
                transform.rotation
            );
        }

        private void ApplySavedAttachPose(Transform interactorAttachTransform)
        {
            interactorAttachTransform.localPosition = savedLocalAttachPosition;
            interactorAttachTransform.localRotation = savedLocalAttachRotation;
        }

        private void ClearSavedAttachPose()
        {
            savedLocalAttachPosition = Vector3.zero;
            savedLocalAttachRotation = Quaternion.identity;
        }

        #endregion
    }
}
