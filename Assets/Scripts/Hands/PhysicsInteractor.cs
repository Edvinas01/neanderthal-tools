using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    public class PhysicsInteractor : XRDirectInteractor
    {
        #region Fields

        private XRBaseInteractable currentInteractable;

        // Number of overlapping colliders on the "currentInteractable".
        private int colliderCount;

        #endregion

        #region Unity Lifecycle

        private new void OnTriggerEnter(Collider other)
        {
            var interactable = GetInteractable(other);
            if (interactable == null)
            {
                return;
            }

            HandleEnteredInteractable(interactable);
        }

        private new void OnTriggerExit(Collider other)
        {
            Remove(other);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove collider from the interactor. E.g. when the collider is detached or removed.
        /// </summary>
        public void Remove(Collider other)
        {
            var interactable = GetInteractable(other);
            if (interactable == null || currentInteractable != interactable)
            {
                return;
            }

            HandleExitedInteractable();
        }

        private void HandleEnteredInteractable(XRBaseInteractable interactable)
        {
            if (currentInteractable == null)
            {
                SetCurrentInteractable(interactable);
            }
            else if (currentInteractable == interactable)
            {
                IncrementColliders();
            }
        }

        private void HandleExitedInteractable()
        {
            colliderCount--;
            if (colliderCount > 0)
            {
                return;
            }

            ClearCurrentInteractable();
        }

        private XRBaseInteractable GetInteractable(Collider other)
        {
            return interactionManager == null
                ? null
                : interactionManager.TryGetInteractableForCollider(other);
        }

        private void SetCurrentInteractable(XRBaseInteractable interactable)
        {
            currentInteractable = interactable;

            // Can only target one interactable, otherwise might result in "floating" pickups.
            if (validTargets.Count > 0)
            {
                validTargets.Clear();
            }

            validTargets.Add(currentInteractable);
            colliderCount = 1;
        }

        private void IncrementColliders()
        {
            colliderCount++;
        }

        private void ClearCurrentInteractable()
        {
            // Clear all targets just in-case, as only one can be targeted.
            validTargets.Clear();

            currentInteractable = null;
            colliderCount = 0;
        }

        #endregion
    }
}
