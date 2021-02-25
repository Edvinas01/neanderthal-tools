using UnityEngine;

namespace NeanderthalTools.Knapping
{
    [RequireComponent(typeof(Rigidbody))]
    public class KnappingProjectile : MonoBehaviour
    {
        #region Fields

        private new Rigidbody rigidbody;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var flake = collision.collider.GetComponent<Flake>();
            if (flake == null)
            {
                return;
            }

            var force = collision.impulse.magnitude / Time.fixedDeltaTime;
            flake.Knapp(rigidbody.velocity, force);
        }

        #endregion
    }
}
