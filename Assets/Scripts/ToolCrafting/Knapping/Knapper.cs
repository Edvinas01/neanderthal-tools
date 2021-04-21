using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class Knapper : MonoBehaviour
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        private float knappingCooldown = 0.01f;

        #endregion

        #region Fields

        private XRBaseInteractable interactable;
        private float knappingAvailableTime;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            interactable = GetComponent<XRBaseInteractable>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsCooldown())
            {
                return;
            }

            knappingAvailableTime = Time.time + knappingCooldown;

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

        private bool IsCooldown()
        {
            return Time.time < knappingAvailableTime;
        }

        #endregion
    }
}
