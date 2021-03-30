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
            RemoveComponents(adhesive);
            onAttachAdhesive.Invoke(CreateEventArgs(adhesive));
        }

        public void HandleAttachFlake(Flake flake)
        {
            RemoveComponents(flake);
            onAttachFlake.Invoke(CreateEventArgs(flake));
        }

        // todo: this is busted
        private static void RemoveComponents(Component part)
        {
            // Can't pickup an attached piece.
            if (part.TryGetComponent<XRBaseInteractable>(out var interactable))
            {
                Destroy(interactable);
            }

            // Can't weld an attached piece as well.
            if (part.TryGetComponent<GrabInteractableWelder>(out var welder))
            {
                Destroy(welder);
            }

            // Attached pieces moves as one with the handle.
            if (part.TryGetComponent<Rigidbody>(out var rb))
            {
                Destroy(rb);
            }

            // Attach point is no longer necessary as the piece cannot be detached.
            var attachPoint = part.GetComponentInParent<AttachPoint>();
            if (attachPoint != null)
            {
                Destroy(attachPoint);
            }
        }

        private HaftEventArgs CreateEventArgs(IToolPart toolPart)
        {
            return new HaftEventArgs(handleInteractable, toolPart);
        }

        #endregion
    }
}
