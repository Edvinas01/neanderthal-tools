using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace NeanderthalTools.Knapping
{
    public class Flake : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        [Tooltip("Transform which outlines the angle from which the flake must be hit")]
        private Transform angleOffsetTransform;

        [SerializeField]
        [Range(0f, 180f)]
        [Tooltip("Max impact angle that would still detach the flake")]
        private float maxAngle = 20f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Min impact force required to detach the flake")]
        private float minForce = 50f;

        [Range(0f, 1f)]
        [SerializeField]
        [Tooltip("Ratio of other flakes that have to be removed in order for this piece to detach")]
        private float removalRatio = 1f;

        [SerializeField]
        [Tooltip("Other flakes that need to be detached before this one can be detached")]
        private List<Flake> dependencies;

        #endregion

        #region Fields

        private const float DebugRayDuration = 10f;
        private const float DebugRayLength = 0.1f;

        private readonly List<Flake> dependants = new List<Flake>();
        private int initialDependencyCount;
        private Objective objective;

        #endregion

        #region Unity Lifecycle

        private void OnDrawGizmosSelected()
        {
            if (dependencies == null)
            {
                return;
            }

            var position = transform.position;

            Gizmos.color = Color.white;
            foreach (var dependency in dependencies)
            {
                if (dependency == null)
                {
                    continue;
                }

                Gizmos.DrawLine(position, dependency.transform.position);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, GetOffsetDirection() * DebugRayLength);
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

        private void Awake()
        {
            objective = GetComponentInParent<Objective>();

            initialDependencyCount = dependencies.Count;
            foreach (var dependency in dependencies)
            {
                dependency.dependants.Add(this);
            }
        }

        private void OnDisable()
        {
            ClearDependencies();
        }

        #endregion

        #region Methods

        public void HandleImpact(Vector3 impactVelocity, float impactForce)
        {
            if (IsDetached())
            {
                return;
            }

            if (IsWeakImpact(impactForce))
            {
                objective.OnWeakImpact.Invoke(this);
                return;
            }

            if (IsDependenciesRemaining())
            {
                objective.OnDependenciesRemaining.Invoke(this);
                return;
            }

            var flakeTransform = transform;
            var flakePosition = flakeTransform.position;

            var impactDirection = impactVelocity.normalized;
            var flakeDirection = GetOffsetDirection();

            var impactColor = Color.red;
            if (IsValidAngle(impactDirection, flakeDirection))
            {
                impactColor = Color.green;

                ClearDependencies();
                objective.OnDetach.Invoke(this);
                Detach();
            }
            else
            {
                objective.OnInvalidAngle.Invoke(this);
            }

            DrawDebugDay(flakePosition, impactDirection, impactColor);
            DrawDebugDay(flakePosition, flakeDirection, Color.green);
        }

        private bool IsDetached()
        {
            return objective == null;
        }

        private bool IsWeakImpact(float force)
        {
            return force <= minForce;
        }

        private bool IsDependenciesRemaining()
        {
            if (initialDependencyCount == 0)
            {
                return false;
            }

            var removedRatio = 1 - (float) dependencies.Count / initialDependencyCount;

            return removedRatio < removalRatio;
        }

        private bool IsValidAngle(Vector3 impactDirection, Vector3 flakeDirection)
        {
            var impactAngle = Vector3.Angle(impactDirection, flakeDirection);
            return impactAngle <= maxAngle;
        }

        private Vector3 GetOffsetDirection()
        {
            return angleOffsetTransform.rotation * Vector3.forward;
        }

        private void ClearDependencies()
        {
            foreach (var dependant in dependants)
            {
                dependant.dependencies.Remove(this);
            }

            dependencies.Clear();
            dependants.Clear();
        }

        private void Detach()
        {
            gameObject.AddComponent<Rigidbody>();
            transform.parent = null;
            objective = null;
        }

        private static void DrawDebugDay(Vector3 position, Vector3 direction, Color color)
        {
            Debug.DrawRay(position, direction * DebugRayLength, color, DebugRayDuration);
        }

        #endregion
    }
}
