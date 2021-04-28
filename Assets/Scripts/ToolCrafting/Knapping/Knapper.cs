using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class Knapper : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private List<Collider> knappingColliders;

        #endregion

        #region Fields

        private XRBaseInteractable interactable;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
            SetupKnappingColliders();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (interactable != null && !interactable.isSelected)
            {
                return;
            }

            if (!IsKnappingCollider(collision))
            {
                return;
            }

            var flake = GetFlake(collision);
            if (flake == null)
            {
                return;
            }

            HandleFlakeCollision(flake, collision);
        }

        #endregion

        #region Methods

        private void SetupKnappingColliders()
        {
            if (knappingColliders.Count == 0)
            {
                knappingColliders = GetComponentsInChildren<Collider>().ToList();
            }
        }

        private bool IsKnappingCollider(Collision collision)
        {
            foreach (var contact in collision.contacts)
            {
                if (knappingColliders.Contains(contact.thisCollider))
                {
                    return true;
                }
            }

            return false;
        }


        private static Flake GetFlake(Collision collision)
        {
            var collisionCollider = collision.collider;
            var flake = collisionCollider.GetComponentInParent<Flake>();
            if (flake == null)
            {
                // Backwards compatibility for prototype knapping stone.
                return collisionCollider.GetComponent<Flake>();
            }

            return flake;
        }

        private void HandleFlakeCollision(Flake flake, Collision collision)
        {
            var point = collision.contacts[0].point;
            var direction = (point - transform.position).normalized;
            var force = collision.impulse.magnitude / Time.fixedDeltaTime;

            flake.HandleImpact(
                interactable.selectingInteractor,
                direction,
                point,
                force
            );
        }

        #endregion
    }
}
