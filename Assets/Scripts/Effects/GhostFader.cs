using System.Collections;
using NeanderthalTools.Util;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.Effects
{
    public class GhostFader : MonoBehaviour
    {
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
        private UnityEvent onFadedIn;

        [SerializeField]
        private UnityEvent onFadedOut;

        #endregion

        #region Fields

        private Material material;
        private new Light light;

        private float initialAlpha;
        private float initialLightIntensity;

        private int colorPropertyId;

        #endregion

        #region Unity Lifecycle

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }

        private void Awake()
        {
            material = transform.GetComponentInChildren<Renderer>().material;
            light = ghost.GetComponentInChildren<Light>();

            colorPropertyId = Shader.PropertyToID(colorPropertyName);

            initialAlpha = material.GetColor(colorPropertyId).a;
            initialLightIntensity = light.intensity;

            ghost.gameObject.SetActive(false);
            SetMultipliers(0f);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        #endregion

        #region Methods

        public void StartFadeIn()
        {
            StartCoroutine(FadeIn());
        }

        public void StartFadeOut()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            ghost.gameObject.SetActive(true);
            yield return Coroutines.Progress(0f, 1f, fadeInDuration, SetMultipliers);
            onFadedIn.Invoke();
        }

        private IEnumerator FadeOut()
        {
            yield return Coroutines.Progress(1f, 0f, fadeOutDuration, SetMultipliers);
            ghost.gameObject.SetActive(false);
            onFadedOut.Invoke();
        }

        private void SetMultipliers(float value)
        {
            SetAlphaMultiplier(value);
            SetLightIntensityMultiplier(value);
        }

        private void SetAlphaMultiplier(float multiplier)
        {
            var color = material.GetColor(colorPropertyId);
            color.a = initialAlpha * multiplier;
            material.SetColor(colorPropertyId, color);
        }

        private void SetLightIntensityMultiplier(float multiplier)
        {
            light.intensity = initialLightIntensity * multiplier;
        }

        #endregion
    }
}
