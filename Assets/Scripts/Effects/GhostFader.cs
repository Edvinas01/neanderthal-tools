using System.Collections;
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
            colorPropertyId = Shader.PropertyToID(colorPropertyName);

            material = transform.GetComponentInChildren<Renderer>(true).material;
            initialAlpha = material.GetColor(colorPropertyId).a;

            light = ghost.GetComponentInChildren<Light>();
            initialLightIntensity = light.intensity;

            ghost.gameObject.SetActive(false);
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
            yield return Fade(0f, 1f, fadeInDuration);
            onFadedIn.Invoke();
        }

        private IEnumerator FadeOut()
        {
            yield return Fade(1f, 0f, fadeOutDuration);
            ghost.gameObject.SetActive(false);
            onFadedOut.Invoke();
        }

        private IEnumerator Fade(float from, float to, float duration)
        {
            SetAlphaMultiplier(from);
            SetLightIntensityMultiplier(from);

            var progress = 0f;
            while (progress < 1f)
            {
                var multiplier = Mathf.Lerp(from, to, progress);

                SetAlphaMultiplier(multiplier);
                SetLightIntensityMultiplier(multiplier);

                progress += Time.unscaledDeltaTime / duration;

                yield return null;
            }

            SetAlphaMultiplier(to);
            SetLightIntensityMultiplier(to);

            yield return null;
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
