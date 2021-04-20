using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    public static class PoseExtensions
    {
        public static void MatchAttachPose(
            this XRBaseInteractable interactable,
            XRBaseInteractor interactor
        )
        {
            if (interactable is XRGrabInteractable grabInteractable
                && grabInteractable.attachTransform != null)
            {
                MatchGrabAttachPose(grabInteractable, interactor);
            }
            else
            {
                MatchRegularAttachPose(interactable, interactor);
            }
        }

        private static void MatchGrabAttachPose(
            XRGrabInteractable interactable,
            XRBaseInteractor interactor
        )
        {
            var interactableTransform = interactable.transform;
            var interactableAttachTransform = interactable.attachTransform;
            if (interactableAttachTransform == null)
            {
                interactableAttachTransform = interactableTransform;
            }

            var interactorAttachTransform = interactor.attachTransform;

            var worldAttachPosition = GetWorldAttachPosition(
                interactableTransform,
                interactableAttachTransform,
                interactorAttachTransform
            );

            var worldAttachRotation = GetWorldAttachRotation(
                interactableAttachTransform,
                interactorAttachTransform
            );

            interactableTransform.position = worldAttachPosition;
            interactableTransform.rotation = worldAttachRotation;
        }

        private static void MatchRegularAttachPose(
            XRBaseInteractable interactable,
            XRBaseInteractor interactor
        )
        {
            var interactableTransform = interactable.transform;
            var attachTransform = interactor.attachTransform;

            interactableTransform.position = attachTransform.position;
            interactableTransform.rotation = attachTransform.rotation;
        }

        private static Vector3 GetWorldAttachPosition(
            Transform interactableTransform,
            Transform interactableAttachTransform,
            Transform interactorAttachTransform
        )
        {
            var attachOffset = interactableTransform.position -
                               interactableAttachTransform.position;

            var localAttachOffset = interactableAttachTransform
                .InverseTransformDirection(attachOffset);

            return interactorAttachTransform.position +
                   interactorAttachTransform.rotation *
                   localAttachOffset;
        }

        private static Quaternion GetWorldAttachRotation(
            Transform interactableAttachTransform,
            Transform interactorAttachTransform
        )
        {
            var inverseRotation = Quaternion.Inverse(interactableAttachTransform.localRotation);
            return interactorAttachTransform.rotation * inverseRotation;
        }
    }
}
