﻿using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(XRGrabInteractable))]
    public class GrabInteractableWelder : MonoBehaviour
    {
        #region Fields

        private XRGrabInteractable interactable;
        private FixedJoint joint;

        [SerializeField]
        [Tooltip("Should this interactable snap to interactor anchor position")]
        private bool snapPosition;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            interactable = GetComponent<XRGrabInteractable>();

            // PhysicsHand ignores tracking if a welder exists, so this is added here just in case.
            // Other settings produce weird and glitched results when welding, as movement is
            // handled by the joint.
            SetupInteractableTracking(false);

            if (snapPosition)
            {
                SetupAttach();
            }
        }

        private void OnDisable()
        {
            DestroyJoint();
        }

        #endregion

        #region Methods

        private void SetupInteractableTracking(bool track)
        {
            interactable.trackPosition = track;
            interactable.trackRotation = track;
            interactable.movementType = track
                ? XRBaseInteractable.MovementType.Instantaneous
                : XRBaseInteractable.MovementType.VelocityTracking;
        }

        private void SetupAttach()
        {
            interactable.attachTransform = transform;
        }

        public void Weld(SelectEnterEventArgs args)
        {
            DestroyJoint();

            var interactor = args.interactor;
            var interactorRigidbody = interactor.GetComponent<Rigidbody>();
            if (interactorRigidbody == null)
            {
                return;
            }

            if (snapPosition)
            {
                SnapPosition(interactor);
            }

            CreateJoint(interactorRigidbody);
        }

        public void UnWeld(SelectExitEventArgs args)
        {
            DestroyJoint();
        }

        private void SnapPosition(XRBaseInteractor interactor)
        {
            var interactorAttachTransform = interactor.attachTransform;
            var interactableTransform = transform;

            interactableTransform.position = interactorAttachTransform.position;
            interactableTransform.rotation = interactorAttachTransform.rotation;
        }

        private void CreateJoint(Rigidbody interactorRigidbody)
        {
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = interactorRigidbody;
        }

        private void DestroyJoint()
        {
            Destroy(joint);
        }

        #endregion
    }
}
