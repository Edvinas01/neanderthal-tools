using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class HaftEventArgs
    {
        /// <summary>
        /// The interactor that the tool part is grabbed by, can be null.
        /// </summary>
        public XRBaseInteractor ToolPartInteractor { get; }

        public XRBaseInteractor HandleInteractor { get; }

        public IToolPart ToolPart { get; }

        public HaftEventArgs(
            XRBaseInteractor toolPartInteractor,
            XRBaseInteractor handleInteractor,
            IToolPart toolPart
        )
        {
            ToolPartInteractor = toolPartInteractor;
            HandleInteractor = handleInteractor;
            ToolPart = toolPart;
        }
    }
}
