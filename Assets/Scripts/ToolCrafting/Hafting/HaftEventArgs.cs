using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class HaftEventArgs
    {
        public XRBaseInteractable HandleInteractable { get; }

        public IToolPart ToolPart { get; }

        public HaftEventArgs(XRBaseInteractable handleInteractable, IToolPart toolPart)
        {
            HandleInteractable = handleInteractable;
            ToolPart = toolPart;
        }
    }
}
