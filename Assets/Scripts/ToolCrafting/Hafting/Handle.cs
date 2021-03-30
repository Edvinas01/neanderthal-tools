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

        #endregion

        #region Methods

        public void HandleAttachAdhesive(Adhesive adhesive)
        {
            onAttachAdhesive.Invoke(CreateEventArgs(adhesive));
        }

        public void HandleAttachFlake(Flake flake)
        {
            onAttachFlake.Invoke(CreateEventArgs(flake));
        }

        private HaftEventArgs CreateEventArgs(IToolPart toolPart)
        {
            return new HaftEventArgs(handleInteractable, toolPart);
        }

        #endregion
    }
}
