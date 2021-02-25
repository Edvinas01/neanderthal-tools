using System.Linq;
using UnityEngine;

namespace NeanderthalTools.Knapping
{
    public class Flake : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private Transform angleOffsetTransform;

        [SerializeField]
        [Range(0f, 180f)]
        [Tooltip("Maximum angle that would still detach the flake")]
        private float maxAngle = 10f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Minimum force required to detach the flake")]
        private float minForce = 100f;

        #endregion

        #region Unity Lifecycle

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, GetOffsetDirection());
        }

        private void OnValidate()
        {
            if (angleOffsetTransform == null || angleOffsetTransform == transform)
            {
                angleOffsetTransform = GetComponentsInChildren<Transform>()
                    .FirstOrDefault(childTransform => childTransform != transform);
            }

            if (angleOffsetTransform == null)
            {
                angleOffsetTransform = transform;
            }
        }

        #endregion

        #region Methods

        public void Knapp(Vector3 impactVelocity, float impactForce)
        {
            if (impactForce <= minForce)
            {
                return;
            }

            // Cache target flake info.
            var flakeTransform = transform;
            var flakePosition = flakeTransform.position;

            // Directions.
            var impactDirection = -impactVelocity.normalized;
            var flakeDirection = GetOffsetDirection();

            // Debug.
            Debug.DrawRay(flakePosition, impactDirection, Color.red, 10f);
            Debug.DrawRay(flakePosition, flakeDirection, Color.green, 10f);

            // Handle angle.
            var impactAngle = Vector3.Angle(impactDirection, flakeDirection);
            if (impactAngle <= maxAngle)
            {
                Detach();
            }
        }

        private Vector3 GetOffsetDirection()
        {
            return angleOffsetTransform.rotation * Vector3.forward;
        }

        private void Detach()
        {
            gameObject.AddComponent<Rigidbody>();
            transform.parent = null;
            Destroy(this);
        }

        #endregion
    }
}
