using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Based on: https://www.youtube.com/watch?v=-a36GpPkW-Q
namespace NeanderthalTools.Hands
{
    public class OffsetGrabInteractable : XRGrabInteractable
    {
        #region Fields

        private int originalLayer;
        private Vector3 interactorPosition = Vector3.zero;
        private Quaternion interactorRotation = Quaternion.identity;

        #endregion

        #region Overrides

        protected override void Awake()
        {
            base.Awake();
            originalLayer = gameObject.layer;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            var interactor = args.interactor;
            SetInteractorPose(interactor);
            SetAttachmentPose(interactor);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

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

            ApplyLayer(colliders, interactor.gameObject.layer);
        }

        private void ResetAttachmentPose(XRBaseInteractor interactor)
        {
            var interactorAttachTransform = interactor.attachTransform;
            interactorAttachTransform.localPosition = interactorPosition;
            interactorAttachTransform.localRotation = interactorRotation;

            ApplyLayer(colliders, originalLayer);
        }

        private void ClearInteractorPose()
        {
            interactorPosition = Vector3.zero;
            interactorRotation = Quaternion.identity;
        }

        private static void ApplyLayer(IEnumerable<Collider> colliders, int layer)
        {
            foreach (var collider in colliders)
            {
                collider.gameObject.layer = layer;
            }
        }

        #endregion
    }
}
