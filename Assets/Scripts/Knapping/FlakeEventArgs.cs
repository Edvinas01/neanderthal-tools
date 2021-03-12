using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Knapping
{
    public class FlakeEventArgs
    {
        public Flake Flake { get; }

        public XRBaseInteractor Interactor { get; }

        public FlakeEventArgs(XRBaseInteractor interactor, Flake flake)
        {
            Interactor = interactor;
            Flake = flake;
        }
    }
}
