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
        private float fadedOutAlpha;

        [SerializeField]
        private string colorPropertyName = "_Color";

        [SerializeField]
        private UnityEvent onFadedIn;

        [SerializeField]
        private UnityEvent onFadedOut;

        #endregion

        #region Fields

        private Material material;
        private float initialAlpha;
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
        }

        #endregion

        #region Unity Lifecycle

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
            yield return Fade(fadedOutAlpha, initialAlpha, fadeInDuration);
            onFadedIn.Invoke();
        }

        private IEnumerator FadeOut()
        {
            yield return Fade(initialAlpha, fadedOutAlpha, fadeOutDuration);
            ghost.gameObject.SetActive(false);
            onFadedOut.Invoke();
        }

        private IEnumerator Fade(float from, float to, float duration)
        {
            SetAlpha(from);

            var progress = 0f;
            while (progress < 1f)
            {
                var alpha = Mathf.Lerp(from, to, progress);
                SetAlpha(alpha);
                progress += Time.unscaledDeltaTime / duration;
                yield return null;
            }

            SetAlpha(to);

            yield return null;
        }

        private void SetAlpha(float alpha)
        {
            var color = material.GetColor(colorPropertyId);
            color.a = alpha;
            material.SetColor(colorPropertyId, color);
        }

        #endregion
    }
}
