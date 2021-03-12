using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Knapping
{
    public class FlakeEventArgs
    {
        public Flake Flake { get; }

        public XRBaseInteractor KnapperInteractor { get; }

        public FlakeEventArgs(XRBaseInteractor knapperInteractor, Flake flake)
        {
            KnapperInteractor = knapperInteractor;
            Flake = flake;
        }
    }
}
