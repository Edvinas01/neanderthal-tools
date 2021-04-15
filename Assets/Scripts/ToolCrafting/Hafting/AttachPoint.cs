using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    [RequireComponent(typeof(Collider))]
    public class AttachPoint : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        [Tooltip("Attach points that will unlock after this attach point is satisfied")]
        private List<AttachPoint> nextAttachPoints;

        [SerializeField]
        [Tooltip("Where to attach the object")]
        private List<Transform> attachTransforms;

        [SerializeField]
        [Range(0f, 180f)]
        [Tooltip("Max impact angle that still registers as an attachment")]
        private float maxAngle = 180f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Min impact force required to attach the tool part")]
        private float minForce;

        [SerializeField]
        [NaughtyAttributes.Dropdown("FindToolTypeNames")]
        private string toolTypeName;

        #endregion

        #region Fields

        private Collider attachCollider;
        private Type toolType;

        #endregion

        #region Unity Lifecycle

        private void OnDrawGizmos()
        {
            if (attachTransforms == null)
            {
                return;
            }

            foreach (var attachTransform in attachTransforms)
            {
                var position = attachTransform.position;
                var direction = attachTransform.rotation * Vector3.forward / 4;

                Gizmos.DrawWireSphere(position, 0.025f);
                Gizmos.DrawRay(position, -direction);
                Gizmos.DrawRay(position, direction);
            }
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(toolTypeName))
            {
                toolTypeName = FindToolTypeNames().FirstOrDefault();
            }

            attachTransforms = GetAttachTransforms();
        }

        private void Awake()
        {
            attachCollider = GetComponent<Collider>();
            toolType = Type.GetType(toolTypeName);

            SetupAttachTransform();
            SetActiveNextAttachPoints(false);
        }

        #endregion

        #region Methods

        public bool IsMatchingPart(IToolPart toolPart)
        {
            return toolType.IsInstanceOfType(toolPart);
        }

        public bool Attach(IToolPart toolPart, GameObject attachPart, float impactForce)
        {
            if (IsWeakImpact(impactForce))
            {
                return false;
            }

            if (!IsValidAngle(toolPart, out var attachTransform))
            {
                return false;
            }

            AttachTransform(attachPart, attachTransform);
            SetActiveNextAttachPoints(true);

            attachCollider.enabled = false;
            Destroy(this);

            return true;
        }

        private bool IsWeakImpact(float force)
        {
            return force < minForce;
        }

        private bool IsValidAngle(IToolPart toolPart, out Transform attachTransform)
        {
            attachTransform = null;

            foreach (var currentTransform in attachTransforms)
            {
                var direction = toolPart.AttachDirection;
                var angle = Vector3.Angle(direction, currentTransform.rotation * Vector3.forward);

                if (angle <= maxAngle)
                {
                    attachTransform = currentTransform;
                    return true;
                }
            }

            return false;
        }

        // ReSharper disable once UnusedMember.Local
        private static List<string> FindToolTypeNames()
        {
            var baseType = typeof(IToolPart);

            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => type != baseType && baseType.IsAssignableFrom(type))
                .Select(type => type.FullName)
                .ToList();
        }

        private void AttachTransform(GameObject part, Transform attachTransform)
        {
            var partTransform = part.transform;
            partTransform.position = attachTransform.position;
            partTransform.rotation = attachTransform.rotation;
            partTransform.parent = attachTransform;
        }

        private void SetupAttachTransform()
        {
            if (attachTransforms.Count == 0)
            {
                attachTransforms = GetAttachTransforms();
            }
        }

        private List<Transform> GetAttachTransforms()
        {
            return GetComponentsInChildren<Transform>()
                .Where(childTransform => childTransform.GetComponents<Component>().Length == 1)
                .ToList();
        }

        private void SetActiveNextAttachPoints(bool active)
        {
            foreach (var nextAttachPoint in nextAttachPoints)
            {
                nextAttachPoint.gameObject.SetActive(active);
            }
        }

        #endregion
    }
}
