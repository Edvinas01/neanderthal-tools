using UnityEngine;
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

        public Vector3 ImpactPoint { get; }

        public float ImpactForce { get; }

        public float ImpactAngle { get; }

        public FlakeEventArgs(
            XRBaseInteractor objectiveInteractor,
            XRBaseInteractor knapperInteractor,
            Objective objective,
            Flake flake,
            Vector3 impactPoint,
            float impactForce,
            float impactAngle
        )
        {
            ObjectiveInteractor = objectiveInteractor;
            KnapperInteractor = knapperInteractor;
            Objective = objective;
            Flake = flake;
            ImpactPoint = impactPoint;
            ImpactForce = impactForce;
            ImpactAngle = impactAngle;
        }
    }
}
