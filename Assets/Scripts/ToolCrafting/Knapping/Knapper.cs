using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class Knapper : MonoBehaviour
    {
        #region Fields

        private XRBaseInteractable interactable;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (interactable != null && !interactable.isSelected)
            {
                return;
            }

            var collisionCollider = collision.collider;
            var flake = collisionCollider.GetComponentInParent<Flake>();
            if (flake == null)
            {
                // Backwards compatibility for prototype knapping stone.
                flake = collisionCollider.GetComponent<Flake>();
                if (flake == null)
                {
                    return;
                }

                return;
            }

            HandleFlakeCollision(flake, collision);
        }

        private void HandleFlakeCollision(Flake flake, Collision collision)
        {
            var direction = (collision.contacts[0].point - transform.position).normalized;
            var force = collision.impulse.magnitude / Time.fixedDeltaTime;

            flake.HandleImpact(interactable.selectingInteractor, direction, force);
        }

        #endregion
    }
}
