using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Based on: https://www.youtube.com/watch?v=-a36GpPkW-Q
namespace NeanderthalTools.Hands
{
    [RequireComponent(typeof(Rigidbody))]
    public class OffsetGrabInteractable : XRGrabInteractable
    {
        #region Fields

        private Vector3 savedLocalAttachPosition;
        private Quaternion savedLocalAttachRotation;

        private new Rigidbody rigidbody;
        private bool throwVelocityResetPending;

        #endregion

        #region Unity Lifectcle

        protected override void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody>();
        }

        #endregion

        #region Overrides

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

        protected override void Detach()
        {
            base.Detach();
            if (throwVelocityResetPending)
            {
                ResetThrowVelocity();
            }
        }

        #endregion

        #region Methods

        public void QueueThrowVelocityReset()
        {
            if (!isSelected || !throwOnDetach)
            {
                return;
            }

            throwVelocityResetPending = true;

            // The throw velocity reset needs to happen late, as XRGrabInteractable estimates the
            // throwing velocity within a time-frame that is defined in throwSmoothingDuration.
            StopAllCoroutines();
            StartCoroutine(ClearVelocityReset());
        }

        private void ResetThrowVelocity()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            throwVelocityResetPending = false;
        }

        private IEnumerator ClearVelocityReset()
        {
            yield return new WaitForSeconds(throwSmoothingDuration);
            yield return null;

            throwVelocityResetPending = false;
        }

        private void SaveAttachPose(Transform interactorAttachTransform)
        {
            savedLocalAttachPosition = interactorAttachTransform.localPosition;
            savedLocalAttachRotation = interactorAttachTransform.localRotation;
        }

        private void ApplyOffsetAttachPose(Transform interactorAttachTransform)
        {
            var interactableTransform = transform;
            interactorAttachTransform.SetPositionAndRotation(
                interactableTransform.position, // rigidbody.worldCenterOfMass,
                interactableTransform.rotation
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
