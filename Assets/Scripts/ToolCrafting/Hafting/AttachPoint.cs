using System.Collections.Generic;
using System.Linq;
using NeanderthalTools.ToolCrafting.Knapping;
using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    [RequireComponent(typeof(Collider))]
    public class AttachPoint : MonoBehaviour
    {
        #region Enums

        private enum Target
        {
            Adhesive,
            Flake
        }

        #endregion

        #region Editor

        [SerializeField]
        [Tooltip("Other parts are blocked by this part")]
        private List<AttachPoint> blockedAttachPoints;

        [SerializeField]
        [Tooltip("Where to attach the object on this attacher")]
        private Transform attachTransform;

        [SerializeField]
        private Target target;

        #endregion

        #region Fields

        private Collider attachPointCollider;
        private Handle handle;

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

        private void Awake()
        {
            attachPointCollider = GetComponent<Collider>();
            handle = GetComponentInParent<Handle>();
            SetupAttachTransform();
            SetBlockedAttachPoints(false);
        }

        #endregion

        #region Methods

        public void HandleAttach(GameObject otherGameObject)
        {
            switch (target)
            {
                case Target.Adhesive:
                    HandleAttachAdhesive(otherGameObject);
                    break;
                case Target.Flake:
                    HandleAttachFlake(otherGameObject);
                    break;
                default:
                    Debug.LogError($"Unsupported target: ${target}");
                    break;
            }

            if (IsAttached())
            {
                Destroy(attachPointCollider);
                Destroy(this);
            }
        }

        private void SetupAttachTransform()
        {
            if (attachTransform == null)
            {
                attachTransform = GetAttachTransform();
            }
        }

        private void SetBlockedAttachPoints(bool active)
        {
            foreach (var blockedAttachPoint in blockedAttachPoints)
            {
                blockedAttachPoint.gameObject.SetActive(active);
            }
        }

        private Transform GetAttachTransform()
        {
            return GetComponentsInChildren<Transform>()
                .Where(IsSingleTransform)
                .FirstOrDefault() ?? transform;
        }

        private static bool IsSingleTransform(Transform target)
        {
            return target.GetComponents<Component>().Length == 1;
        }

        private void HandleAttachAdhesive(GameObject otherGameObject)
        {
            var adhesive = otherGameObject.GetComponentInParent<Adhesive>();
            if (adhesive == null)
            {
                return;
            }

            handle.HandleAttachAdhesive(adhesive);
            Attach(adhesive);
        }

        private void HandleAttachFlake(GameObject otherGameObject)
        {
            var flake = otherGameObject.GetComponentInParent<Flake>();
            if (flake == null || !flake.IsAttachable)
            {
                return;
            }

            handle.HandleAttachFlake(flake);
            Attach(flake);
        }

        private bool IsAttached()
        {
            return attachTransform.childCount > 0;
        }

        private void Attach(Component part)
        {
            part.transform.parent = attachTransform;
            SetBlockedAttachPoints(false);
        }

        #endregion
    }
}
