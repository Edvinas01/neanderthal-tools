using System.Collections.Generic;
using System.Linq;
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

        #region Properties

        public FlakeUnityEvent OnDependenciesRemaining => onDependenciesRemaining;

        public FlakeUnityEvent OnInvalidAngle => onInvalidAngle;

        public FlakeUnityEvent OnWeakImpact => onWeakImpact;

        public FlakeUnityEvent OnDetach => onDetach;

        public IReadOnlyList<Flake> Flakes => flakes;

        /// <summary>
        /// Interactor that holds the objective, can be null.
        /// </summary>
        public XRBaseInteractor Interactor => interactable.selectingInteractor;

        /// <summary>
        /// Is detachment available.
        /// </summary>
        public bool IsDetachReady { get; private set; }

        #endregion

        #region Fields

        private XRBaseInteractable interactable;
        private List<Flake> flakes;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
            flakes = GetComponentsInChildren<Flake>().ToList();
        }

        private void OnEnable()
        {
            interactable.selectExited.AddListener(OnSelectExited);
        }

        private void OnDisable()
        {
            interactable.selectExited.RemoveListener(OnSelectExited);
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            var knapper = otherCollider.GetComponentInParent<Knapper>();
            if (knapper != null)
            {
                IsDetachReady = true;
            }
        }

        #endregion

        #region Methods

        public void HandleDependenciesRemaining(FlakeEventArgs args)
        {
            onDependenciesRemaining.Invoke(args);
            IsDetachReady = false;
        }

        public void HandleInvalidAngle(FlakeEventArgs args)
        {
            onInvalidAngle.Invoke(args);
            IsDetachReady = false;
        }

        public void HandleWeakImpact(FlakeEventArgs args)
        {
            onWeakImpact.Invoke(args);
            IsDetachReady = false;
        }

        public void HandleDetach(FlakeEventArgs args)
        {
            var flake = args.Flake;
            flakes.Remove(flake);
            RemoveInteractableColliders(flake);
            AddInteractable(flake);

            onDetach.Invoke(args);
            IsDetachReady = false;
        }

        private void OnSelectExited(SelectExitEventArgs args)
        {
            IsDetachReady = true;
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

            var flakeName = flake.Name = $"{interactable.name}_{flake.name}";
            flake.Name = flakeName;
            interactableFlake.name = flakeName;

            flakeTransform.parent = interactableFlake.transform;

            // Assuming that the prefab is disabled beforehand. Otherwise "flakeTransform.parent"
            // won't have the desired effect, as "interactables" collect child colliders on "Awake"
            // which would normally fire upon calling "Instantiate".
            interactableFlake.SetActive(true);
        }

        #endregion
    }
}
