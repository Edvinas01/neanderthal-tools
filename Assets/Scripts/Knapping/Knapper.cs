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

            var direction = (collision.contacts[0].point - transform.position).normalized;
            var force = collision.impulse.magnitude / Time.fixedDeltaTime;

            flake.HandleImpact(direction, force);
        }

        #endregion
    }
}
