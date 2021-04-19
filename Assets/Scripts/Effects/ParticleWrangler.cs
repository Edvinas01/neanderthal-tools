using UnityEngine;

namespace NeanderthalTools.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleWrangler : MonoBehaviour
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

        public void StartEmission()
        {
            particleSystem.Play();
            SetEmission(true);
        }

        public void StopEmission()
        {
            SetEmission(false);
        }

        private void SetEmission(bool value)
        {
            var emissionModule = particleSystem.emission;
            emissionModule.enabled = value;
        }

        #endregion
    }
}
