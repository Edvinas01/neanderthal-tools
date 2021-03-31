using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class Flake : MonoBehaviour, IToolPart
    {
        #region Editor

        [SerializeField]
        [Tooltip("Transforms which outline the angles from which the flake must be hit")]
        private List<Transform> angleOffsetTransforms;

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
        [Tooltip("Is this flake be used as an attachable tool part")]
        private bool isAttachable;

        [SerializeField]
        [Tooltip("Other flakes that need to be detached before this one can be detached")]
        private List<Flake> dependencies;

        #endregion

        #region Fields

        private const float DebugRayDuration = 10f;
        private const float DebugRayLength = 0.2f;

        private readonly List<Flake> dependants = new List<Flake>();

        private int initialDependencyCount;
        private Objective objective;

        #endregion

        #region Properties

        /// <summary>
        /// List of colliders attached to this flake.
        /// </summary>
        public List<Collider> Colliders { get; private set; }

        public bool IsAttachable => IsDetached() && isAttachable;

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

            var position = transform.position;
            foreach (var offsetDirection in GetOffsetDirections())
            {
                Gizmos.DrawRay(position, offsetDirection * DebugRayLength);
            }
        }

        private void OnValidate()
        {
            angleOffsetTransforms = GetAngleOffsetTransforms();
        }

        private void Awake()
        {
            objective = GetComponentInParent<Objective>();
            Colliders = GetComponentsInChildren<Collider>().ToList();

            initialDependencyCount = dependencies.Count;
            foreach (var dependency in dependencies)
            {
                dependency.dependants.Add(this);
            }

            if (angleOffsetTransforms.Count == 0)
            {
                angleOffsetTransforms = GetAngleOffsetTransforms();
            }
        }

        private void OnDisable()
        {
            ClearDependencies();
        }

        #endregion

        #region Methods

        public void HandleImpact(
            XRBaseInteractor knapperInteractor,
            Vector3 impactDirection,
            float impactForce
        )
        {
            if (IsDetached())
            {
                return;
            }

            if (IsWeakImpact(impactForce))
            {
                objective.HandleWeakImpact(knapperInteractor, this);
                return;
            }

            if (IsDependenciesRemaining())
            {
                objective.HandleDependenciesRemaining(knapperInteractor, this);
                return;
            }

            var flakeTransform = transform;
            var flakePosition = flakeTransform.position;

            var oppositeImpactDirection = -impactDirection;
            var validAngleFound = false;

            foreach (var offsetDirection in GetOffsetDirections())
            {
                if (IsValidAngle(oppositeImpactDirection, offsetDirection))
                {
                    validAngleFound = true;
                    break;
                }

                DrawDebugDay(flakePosition, offsetDirection, Color.green);
            }

            var impactColor = Color.red;
            if (validAngleFound)
            {
                impactColor = Color.green;

                ClearDependencies();
                Detach(knapperInteractor);
            }
            else
            {
                objective.HandleInvalidAngle(knapperInteractor, this);
            }

            DrawDebugDay(flakePosition, oppositeImpactDirection, impactColor);
        }

        private List<Transform> GetAngleOffsetTransforms()
        {
            return GetComponentsInChildren<Transform>()
                .Where(childTransform => childTransform.GetComponents<Component>().Length == 1)
                .ToList();
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

        private List<Vector3> GetOffsetDirections()
        {
            return angleOffsetTransforms
                .Select(offsetTransform => offsetTransform.rotation * Vector3.forward)
                .ToList();
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

        private void Detach(XRBaseInteractor knapperInteractor)
        {
            var oldObjective = objective;
            objective = null;
            oldObjective.HandleDetach(knapperInteractor, this);
        }

        private static void DrawDebugDay(Vector3 position, Vector3 direction, Color color)
        {
            Debug.DrawRay(position, direction * DebugRayLength, color, DebugRayDuration);
        }

        #endregion
    }
}
