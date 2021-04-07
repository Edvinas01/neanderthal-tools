using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class FlakeEventArgs
    {
        /// <summary>
        /// The interactor that is holding the objective, can be null.
        /// </summary>
        public XRBaseInteractor ObjectiveInteractor { get; }

        public XRBaseInteractor KnapperInteractor { get; }

        public Objective Objective { get; }

        public Flake Flake { get; }

        public float ImpactForce { get; }

        public FlakeEventArgs(
            XRBaseInteractor objectiveInteractor,
            XRBaseInteractor knapperInteractor,
            Objective objective,
            Flake flake,
            float impactForce
        )
        {
            ObjectiveInteractor = objectiveInteractor;
            KnapperInteractor = knapperInteractor;
            Objective = objective;
            Flake = flake;
            ImpactForce = impactForce;
        }
    }
}
