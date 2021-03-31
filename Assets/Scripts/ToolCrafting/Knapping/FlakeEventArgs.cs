using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class FlakeEventArgs
    {
        public Flake Flake { get; }

        public XRBaseInteractor ObjectiveInteractor { get; }

        public XRBaseInteractor KnapperInteractor { get; }

        public FlakeEventArgs(
            XRBaseInteractor objectiveInteractor,
            XRBaseInteractor knapperInteractor,
            Flake flake
        )
        {
            ObjectiveInteractor = objectiveInteractor;
            KnapperInteractor = knapperInteractor;
            Flake = flake;
        }
    }
}
