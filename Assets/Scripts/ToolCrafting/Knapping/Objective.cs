using System.Collections.Generic;
using NeanderthalTools.Hands;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class Objective : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private GameObject interactablePrefab;

        [SerializeField]
        [Tooltip("Called when there are some dependencies remaining")]
        private FlakeUnityEvent onDependenciesRemaining;

        [SerializeField]
        [Tooltip("Called when the impact angle is invalid")]
        private FlakeUnityEvent onInvalidAngle;

        [SerializeField]
        [Tooltip("Called when the impact force is too weak")]
        private FlakeUnityEvent onWeakImpact;

        [SerializeField]
        [Tooltip("Called when the flake detaches")]
        private FlakeUnityEvent onDetach;

        #endregion

        #region Fields

        private XRBaseInteractable interactable;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
        }

        #endregion

        #region Methods

        public void HandleDependenciesRemaining(XRBaseInteractor knapperInteractor, Flake flake)
        {
            onDependenciesRemaining.Invoke(CreateEventArgs(knapperInteractor, flake));
        }

        public void HandleInvalidAngle(XRBaseInteractor knapperInteractor, Flake flake)
        {
            onInvalidAngle.Invoke(CreateEventArgs(knapperInteractor, flake));
        }

        public void HandleWeakImpact(XRBaseInteractor knapperInteractor, Flake flake)
        {
            onWeakImpact.Invoke(CreateEventArgs(knapperInteractor, flake));
        }

        public void HandleDetach(XRBaseInteractor knapperInteractor, Flake flake)
        {
            RemoveInteractableColliders(flake);
            AddInteractable(flake);

            onDetach.Invoke(CreateEventArgs(knapperInteractor, flake));
        }

        private FlakeEventArgs CreateEventArgs(XRBaseInteractor knapperInteractor, Flake flake)
        {
            return new FlakeEventArgs(interactable.selectingInteractor, knapperInteractor, flake);
        }

        private void RemoveInteractableColliders(Flake flake)
        {
            var interactableMap = new Dictionary<Collider, XRBaseInteractable>();
            var interactor = interactable.selectingInteractor as PhysicsInteractor;

            interactable.interactionManager.GetColliderToInteractableMap(ref interactableMap);
            foreach (var interactableCollider in flake.Colliders)
            {
                if (interactor != null)
                {
                    interactor.RemoveInteractableCollider(interactableCollider);
                }

                interactable.colliders.Remove(interactableCollider);
                interactableMap.Remove(interactableCollider);
            }
        }

        private void AddInteractable(Flake flake)
        {
            var flakeTransform = flake.transform;
            var interactableFlake = Instantiate(
                interactablePrefab,
                flakeTransform.position,
                flakeTransform.rotation,
                null
            );

            interactable.name = flake.name;
            flakeTransform.parent = interactableFlake.transform;

            // Assuming that the prefab is disabled beforehand. Otherwise "flakeTransform.parent"
            // won't have the desired effect, as "interactables" collect child colliders on "Awake"
            // which would normally fire upon calling "Instantiate".
            interactableFlake.SetActive(true);
        }

        #endregion
    }
}
