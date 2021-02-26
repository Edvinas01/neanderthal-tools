using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Knapping
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

            var flake = collision.collider.GetComponent<Flake>();
            if (flake == null)
            {
                return;
            }

            HandleFlakeCollision(flake, collision);
        }

        private void HandleFlakeCollision(Flake flake, Collision collision)
        {
            var direction = (collision.contacts[0].point - transform.position).normalized;
            var force = collision.impulse.magnitude / Time.fixedDeltaTime;

            flake.HandleImpact(direction, force);
        }

        #endregion
    }
}
