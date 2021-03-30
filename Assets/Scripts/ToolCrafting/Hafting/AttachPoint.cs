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
        private Transform attachTransform;

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
            if (attachTransform == null)
            {
                return;
            }

            var position = attachTransform.position;
            var direction = attachTransform.rotation * Vector3.forward / 2;

            Gizmos.DrawWireSphere(position, 0.1f);
            Gizmos.DrawRay(position, direction);
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(toolTypeName))
            {
                toolTypeName = FindToolTypeNames().FirstOrDefault();
            }
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

        public void Attach(GameObject part)
        {
            AttachTransform(part);
            SetActiveNextAttachPoints(true);

            attachCollider.enabled = false;
            Destroy(this);
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

        private void AttachTransform(GameObject part)
        {
            var partTransform = part.transform;
            partTransform.position = attachTransform.position;
            partTransform.rotation = attachTransform.rotation;
            partTransform.parent = attachTransform;
        }

        private void SetupAttachTransform()
        {
            if (attachTransform == null)
            {
                attachTransform = GetAttachTransform();
            }
        }

        private Transform GetAttachTransform()
        {
            return GetComponentsInChildren<Transform>().FirstOrDefault(
                target => target.GetComponents<Component>().Length == 1
            ) ?? transform;
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
