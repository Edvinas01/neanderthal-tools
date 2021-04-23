using System.Collections.Generic;
using NeanderthalTools.Hands;
using NeanderthalTools.ToolCrafting.Knapping;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class Handle : MonoBehaviour
    {
        #region Editor

        [Range(0f, 1f)]
        [SerializeField]
        [Tooltip("Required adhesive amount for hafting")]
        private float requiredAdhesiveAmount = 0.1f;

        [SerializeField]
        [Tooltip("Adhesive prefab that will be instantiated on the handle")]
        private GameObject adhesivePrefab;

        [SerializeField]
        private HaftUnityEvent onAttachAdhesive;

        [SerializeField]
        private HaftUnityEvent onAttachFlake;

        #endregion

        #region Fields

        private XRBaseInteractable handleInteractable;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            handleInteractable = GetComponent<XRBaseInteractable>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Must be held.
            if (!handleInteractable.isSelected)
            {
                return;
            }

            if (!FindAttachPoint(collision, out var otherGameObject, out var attachPoint))
            {
                return;
            }

            if (IsHeld(otherGameObject))
            {
                HandleAttach(otherGameObject, attachPoint, collision);
            }
        }

        #endregion

        #region Methods

        private static bool IsHeld(GameObject otherGameObject)
        {
            var interactable = otherGameObject.GetComponentInParent<XRBaseInteractable>();

            // Can be held or fixed to the ground (e.g. adhesive is fixed).
            return interactable == null || interactable.isSelected;
        }

        private static bool FindAttachPoint(
            Collision collision,
            out GameObject otherGameObject,
            out AttachPoint attachPoint
        )
        {
            otherGameObject = null;
            attachPoint = null;

            for (var i = 0; i < collision.contactCount; i++)
            {
                var collisionContact = collision.GetContact(i);
                var thisCollider = collisionContact.thisCollider;

                attachPoint = thisCollider.GetComponent<AttachPoint>();
                if (attachPoint != null)
                {
                    otherGameObject = collisionContact.otherCollider.gameObject;
                    return true;
                }
            }

            return false;
        }

        private void HandleAttach(
            GameObject otherGameObject,
            AttachPoint attachPoint,
            Collision collision
        )
        {
            var toolPart = otherGameObject.GetComponentInParent<IToolPart>();
            if (toolPart == null || !attachPoint.IsMatchingPart(toolPart))
            {
                return;
            }

            switch (toolPart)
            {
                case Adhesive adhesive:
                {
                    HandleAttachAdhesive(attachPoint, adhesive, collision);
                    break;
                }
                case Flake flake:
                {
                    HandleAttachFlake(attachPoint, flake, collision);
                    break;
                }
            }
        }

        private void HandleAttachAdhesive(
            AttachPoint attachPoint,
            Adhesive adhesive,
            Collision collision
        )
        {
            if (adhesive.Amount < requiredAdhesiveAmount)
            {
                return;
            }

            var attachPart = Instantiate(adhesivePrefab);
            var force = GetForce(collision);

            if (attachPoint.Attach(adhesive, attachPart, force))
            {
                adhesive.Amount -= requiredAdhesiveAmount;
                onAttachAdhesive.Invoke(CreateEventArgs(null, adhesive));
            }
        }

        private void HandleAttachFlake(AttachPoint attachPoint, Flake flake, Collision collision)
        {
            if (!flake.IsAttachable)
            {
                return;
            }

            var flakeInteractable = flake.GetComponentInParent<XRBaseInteractable>();
            var attachPart = flake.gameObject;
            var force = GetForce(collision);

            if (attachPoint.Attach(flake, attachPart, force))
            {
                onAttachFlake.Invoke(CreateEventArgs(flakeInteractable.selectingInteractor, flake));
                RemoveComponents(flake);
                RegisterColliders(flake);
            }
        }

        private static float GetForce(Collision collision)
        {
            return collision.impulse.magnitude / Time.fixedDeltaTime;
        }

        private static void RemoveComponents(Flake flake)
        {
            RemoveComponent<GrabInteractableWelder>(flake);
            RemoveComponent<XRBaseInteractable>(flake);
            RemoveComponent<Rigidbody>(flake);
            Destroy(flake);
        }

        private void RegisterColliders(Flake flake)
        {
            var interactionManager = handleInteractable.interactionManager;

            var colliderMap = new Dictionary<Collider, XRBaseInteractable>();
            interactionManager.GetColliderToInteractableMap(ref colliderMap);

            foreach (var flakeCollider in flake.Colliders)
            {
                colliderMap[flakeCollider] = handleInteractable;
            }
        }

        private static void RemoveComponent<T>(Component from) where T : Component
        {
            var component = from.GetComponent<T>();
            if (component != null)
            {
                Destroy(component);
            }
        }

        private HaftEventArgs CreateEventArgs(
            XRBaseInteractor toolPartInteractor,
            IToolPart toolPart
        )
        {
            return new HaftEventArgs(
                toolPartInteractor,
                handleInteractable.selectingInteractor,
                toolPart
            );
        }

        #endregion
    }
}
