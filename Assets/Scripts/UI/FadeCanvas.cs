using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NeanderthalTools.UI
{
    [RequireComponent(typeof(Canvas))]
    public class FadeCanvas : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private float fadeDuration = 1.0f;

        #endregion

        #region Fields

        private Canvas canvas;
        private Image image;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            image = GetComponentInChildren<Image>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fade out the canvas.
        /// </summary>
        public IEnumerator FadeOut()
        {
            yield return Fade(1, 0, SetAlpha);
            canvas.enabled = false;
        }

        /// <summary>
        /// Fade in the canvas.
        /// </summary>
        public IEnumerator FadeIn()
        {
            canvas.enabled = true;
            yield return Fade(0, 1, SetAlpha);
        }

        private IEnumerator Fade(float from, float to, Action<float> onFade)
        {
            onFade(from);

            var progress = 0f;
            while (progress < 1f)
            {
                onFade(Mathf.Lerp(from, to, progress));
                progress += Time.unscaledDeltaTime / fadeDuration;
                yield return new WaitForEndOfFrame();
            }

            onFade(to);
        }

        private void SetAlpha(float alpha)
        {
            var imageColor = image.color;
            imageColor.a = alpha;
            image.color = imageColor;
        }

        #endregion
    }
}
