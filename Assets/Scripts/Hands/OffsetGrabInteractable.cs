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

        #endregion

        #region Overrides

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            var interactor = args.interactor;
            SetInteractorPose(interactor);
            SetAttachmentPose(interactor);
            SetIgnoreCollision(interactor, true);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            var interactor = args.interactor;
            ResetAttachmentPose(interactor);
            ClearInteractorPose();
            SetIgnoreCollision(interactor, false);
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
            var interactorAttachTransform = interactor.attachTransform;
            if (attachTransform != null)
            {
                interactorAttachTransform.position = attachTransform.position;
                interactorAttachTransform.rotation = attachTransform.rotation;
            }
            else
            {
                var interactableTransform = transform;

                interactorAttachTransform.position = interactableTransform.position;
                interactorAttachTransform.rotation = interactableTransform.rotation;
            }
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

        private void SetIgnoreCollision(Component interactor, bool ignore)
        {
            var physicsPoser = interactor.GetComponent<PhysicsPoser>();
            if (physicsPoser == null)
            {
                return;
            }

            foreach (var physicsPoserCollider in physicsPoser.Colliders)
            {
                foreach (var interactableCollider in colliders)
                {
                    Debug.Log(physicsPoserCollider.name + " ~ " + interactableCollider.name + " ignore=" + ignore);
                    Physics.IgnoreCollision(
                        physicsPoserCollider,
                        interactableCollider,
                        ignore
                    );
                }
            }
        }

        #endregion
    }
}
