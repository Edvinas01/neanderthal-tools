using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NeanderthalTools.Util;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.Effects
{
    public class GhostFader : MonoBehaviour
    {
        #region Helper Classes

        private class MaterialWrapper
        {
            public float InitialAlpha { get; }

            public Material Material { get; }

            public MaterialWrapper(float initialAlpha, Material material)
            {
                InitialAlpha = initialAlpha;
                Material = material;
            }
        }

        private class LightWrapper
        {
            public float InitialIntensity { get; }

            public Light Light { get; }

            public LightWrapper(Light light, float initialIntensity)
            {
                Light = light;
                InitialIntensity = initialIntensity;
            }
        }

        #endregion

        #region Editor

        [SerializeField]
        private Transform ghost;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Ghost fade duration in seconds")]
        private float fadeInDuration = 2f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Ghost fade duration in seconds")]
        private float fadeOutDuration = 2f;

        [SerializeField]
        private string colorPropertyName = "_Color";

        [SerializeField]
        [Tooltip("Should the the ghost be deactivated on awake")]
        private bool deactivateOnAwake = true;

        [SerializeField]
        private UnityEvent onFadedIn;

        [SerializeField]
        private UnityEvent onFadedOut;

        #endregion

        #region Fields

        private List<MaterialWrapper> materialWrappers;
        private List<LightWrapper> lightWrappers;

        private new ParticleSystem particleSystem;

        private float initialLightIntensity;

        private int colorPropertyId;

        private Coroutine fadeInCoroutine;
        private Coroutine fadeOutCoroutine;
        private bool fadedIn;

        #endregion

        #region Unity Lifecycle

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }

        private void Awake()
        {
            colorPropertyId = Shader.PropertyToID(colorPropertyName);
            materialWrappers = GetMaterialWrappers();
            lightWrappers = GetLightWrappers();
            particleSystem = ghost.GetComponentInChildren<ParticleSystem>();

            if (deactivateOnAwake)
            {
                ghost.gameObject.SetActive(false);
                SetMultipliers(0f);
            }

            fadedIn = ghost.gameObject.activeInHierarchy;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            fadeInCoroutine = null;
            fadeOutCoroutine = null;
        }

        #endregion

        #region Methods

        public void StartFadeIn()
        {
            if (IsFadingIn())
            {
                return;
            }

            fadeInCoroutine = StartCoroutine(FadeIn());
        }

        public void StartFadeOut()
        {
            if (IsFadingOut())
            {
                return;
            }

            fadeOutCoroutine = StartCoroutine(FadeOut());
        }

        private List<MaterialWrapper> GetMaterialWrappers()
        {
            return GetComponentsInChildren<Renderer>()
                .SelectMany(rend => rend.materials)
                .Select(CreateMaterialWrapper)
                .ToList();
        }

        private MaterialWrapper CreateMaterialWrapper(Material material)
        {
            var initialAlpha = material.GetColor(colorPropertyId).a;
            return new MaterialWrapper(initialAlpha, material);
        }

        private List<LightWrapper> GetLightWrappers()
        {
            return GetComponentsInChildren<Light>()
                .Select(CreateLightWrapper)
                .ToList();
        }

        private static LightWrapper CreateLightWrapper(Light light)
        {
            var initialIntensity = light.intensity;
            return new LightWrapper(light, initialIntensity);
        }

        private IEnumerator FadeIn()
        {
            yield return WaitForFadeOut();

            ghost.gameObject.SetActive(true);
            SetActiveParticleEmission(true);
            yield return Coroutines.Progress(0f, 1f, fadeInDuration, SetMultipliers);
            onFadedIn.Invoke();

            fadeInCoroutine = null;
            fadedIn = true;
        }

        private IEnumerator FadeOut()
        {
            yield return WaitForFadeIn();

            SetActiveParticleEmission(false);
            yield return Coroutines.Progress(1f, 0f, fadeOutDuration, SetMultipliers);
            ghost.gameObject.SetActive(false);
            onFadedOut.Invoke();

            fadeOutCoroutine = null;
            fadedIn = false;
        }

        private void SetActiveParticleEmission(bool active)
        {
            var emission = particleSystem.emission;
            emission.enabled = active;
        }

        private void SetMultipliers(float value)
        {
            SetAlphaMultiplier(value);
            SetLightIntensityMultiplier(value);
        }

        private void SetAlphaMultiplier(float multiplier)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var index = 0; index < materialWrappers.Count; index++)
            {
                var materialWrapper = materialWrappers[index];
                var initialAlpha = materialWrapper.InitialAlpha;
                var material = materialWrapper.Material;

                var color = material.GetColor(colorPropertyId);
                color.a = initialAlpha * multiplier;
                material.SetColor(colorPropertyId, color);
            }
        }

        private void SetLightIntensityMultiplier(float multiplier)
        {
            foreach (var lightWrapper in lightWrappers)
            {
                lightWrapper.Light.intensity = lightWrapper.InitialIntensity * multiplier;
            }
        }

        private bool IsFadingIn()
        {
            return fadeInCoroutine != null;
        }

        private bool IsFadingOut()
        {
            return fadeOutCoroutine != null;
        }

        private IEnumerator WaitForFadeOut()
        {
            yield return new WaitUntil(() => !fadedIn);
        }

        private IEnumerator WaitForFadeIn()
        {
            yield return new WaitUntil(() => fadedIn);
        }

        #endregion
    }
}
