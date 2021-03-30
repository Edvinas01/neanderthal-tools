using NeanderthalTools.Hands;
using NeanderthalTools.ToolCrafting.Knapping;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class Handle : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private HaftUnityEvent onAttachAdhesive;

        [SerializeField]
        private HaftUnityEvent onAttachFlake;

        #endregion

        #region Fields

        private XRBaseInteractable handleInteractable;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            handleInteractable = GetComponent<XRBaseInteractable>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            for (var i = 0; i < collision.contactCount; i++)
            {
                var contact = collision.GetContact(i);
                var point = contact.thisCollider.GetComponent<AttachPoint>();
                if (point == null)
                {
                    continue;
                }

                point.HandleAttach(collision.gameObject);
                return;
            }
        }

        #endregion

        #region Methods

        public void HandleAttachAdhesive(Adhesive adhesive)
        {
            onAttachAdhesive.Invoke(CreateEventArgs(adhesive));
            RemoveComponents(adhesive);
        }

        public void HandleAttachFlake(Flake flake)
        {
            onAttachFlake.Invoke(CreateEventArgs(flake));
            RemoveComponents(flake);
        }

        private static void RemoveComponents(Component part)
        {
            RemoveComponent<GrabInteractableWelder>(part);
            RemoveComponent<XRBaseInteractable>(part);
            RemoveComponent<Rigidbody>(part);
        }

        private static void RemoveComponent<T>(Component from) where T : Component
        {
            var component = from.GetComponent<T>();
            if (component != null)
            {
                Destroy(component);
            }
        }

        private HaftEventArgs CreateEventArgs(IToolPart toolPart)
        {
            return new HaftEventArgs(handleInteractable, toolPart);
        }

        #endregion
    }
}
