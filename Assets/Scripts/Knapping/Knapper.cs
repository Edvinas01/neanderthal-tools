using UnityEngine;

namespace NeanderthalTools.Knapping
{
    public class Knapper : MonoBehaviour
    {
        #region Unity Lifecycle

        private void OnCollisionEnter(Collision collision)
        {
            var flake = collision.collider.GetComponent<Flake>();
            if (flake == null)
            {
                return;
            }

            var force = collision.impulse.magnitude / Time.fixedDeltaTime;
            flake.HandleImpact(collision.relativeVelocity, force);
        }

        #endregion
    }
}
