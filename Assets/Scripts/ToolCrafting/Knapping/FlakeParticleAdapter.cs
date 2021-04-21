using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    [RequireComponent(typeof(ParticleSystem))]
    public class FlakeParticleAdapter : MonoBehaviour
    {
        #region Fields

        private new ParticleSystem particleSystem;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        #endregion

        #region Methods

        public void Play(FlakeEventArgs args)
        {
            var knapperPosition = args.KnapperInteractor.transform.position;
            var impactPosition = args.ImpactPoint;

            var particleTransform = particleSystem.transform;
            var normalRotation = Quaternion.LookRotation(
                (knapperPosition - impactPosition).normalized
            );

            particleTransform.position = impactPosition;
            particleTransform.rotation = normalRotation;
            particleSystem.Play();
        }

        #endregion
    }
}
