using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    public class PhysicsInteractor : XRDirectInteractor
    {
        #region Fields

        private XRBaseInteractable currentInteractable;

        // List of colliders from "currentInteractable" that are currently hovered.
        private readonly List<Collider> colliders = new List<Collider>();

        #endregion

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();
            if (interactionManager == null)
            {
                interactionManager = FindObjectOfType<XRInteractionManager>();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            interactionManager.interactableUnregistered += OnInteractableUnregistered;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            interactionManager.interactableUnregistered -= OnInteractableUnregistered;
            ClearCurrentInteractable();
        }

        private new void OnTriggerEnter(Collider other)
        {
            var interactable = interactionManager.TryGetInteractableForCollider(other);
            if (interactable == null)
            {
                return;
            }

            if (currentInteractable == null)
            {
                currentInteractable = interactable;
                validTargets.Add(currentInteractable);
            }

            if (currentInteractable == interactable)
            {
                colliders.Add(other);
            }
        }

        private new void OnTriggerExit(Collider other)
        {
            RemoveInteractableCollider(other);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove collider from the interactor. E.g. when the collider is detached or removed.
        /// </summary>
        public void RemoveInteractableCollider(Collider other)
        {
            colliders.Remove(other);
            if (colliders.Count != 0)
            {
                return;
            }

            ClearCurrentInteractable();
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            ClearCurrentInteractable();
        }

        private void ClearCurrentInteractable()
        {
            currentInteractable = null;
            validTargets.Clear();
            colliders.Clear();
        }

        private void OnInteractableUnregistered(InteractableUnregisteredEventArgs obj)
        {
            foreach (var interactableCollider in obj.interactable.colliders)
            {
                RemoveInteractableCollider(interactableCollider);
            }
        }

        #endregion
    }
}
