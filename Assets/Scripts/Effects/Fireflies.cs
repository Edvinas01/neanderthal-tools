using System.Collections;
using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Effects
{
    public class Fireflies : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private float fadeInDuration = 1f;

        [SerializeField]
        private float fadeOutDuration = 1f;

        #endregion

        #region Fields

        private new Light light;
        private float initialLightIntensity;

        private new ParticleSystem particleSystem;

        #endregion

        #region Unity Lifecylce

        private void Awake()
        {
            light = GetComponentInChildren<Light>();
            initialLightIntensity = light.intensity;

            particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            SetLightActive(false);
            SetEmissionActive(false);
        }

        #endregion

        #region Methods

        public void StartFadeIn()
        {
            SetEmissionActive(true);
            StartCoroutine(FadeInLight());
        }

        public void StartFadeOut()
        {
            SetEmissionActive(false);
            StartCoroutine(FadeOutLight());
        }

        private void SetLightActive(bool value)
        {
            light.gameObject.SetActive(value);
        }

        private void SetEmissionActive(bool value)
        {
            var emissionModule = particleSystem.emission;
            emissionModule.enabled = value;
        }

        private IEnumerator FadeInLight()
        {
            SetLightActive(true);
            yield return Coroutines.Progress(
                0f,
                initialLightIntensity,
                fadeInDuration,
                SetLightIntensity
            );
        }

        private IEnumerator FadeOutLight()
        {
            yield return Coroutines.Progress(
                initialLightIntensity,
                0f,
                fadeOutDuration,
                SetLightIntensity
            );

            SetLightActive(false);
        }

        private void SetLightIntensity(float value)
        {
            light.intensity = value;
        }

        #endregion
    }
}
