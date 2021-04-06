using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class FlakeEventArgs
    {
        public XRBaseInteractor ObjectiveInteractor { get; }

        public XRBaseInteractor KnapperInteractor { get; }

        public Objective Objective { get; }

        public Flake Flake { get; }

        public FlakeEventArgs(
            XRBaseInteractor objectiveInteractor,
            XRBaseInteractor knapperInteractor,
            Objective objective,
            Flake flake
        )
        {
            ObjectiveInteractor = objectiveInteractor;
            KnapperInteractor = knapperInteractor;
            Objective = objective;
            Flake = flake;
        }
    }
}
