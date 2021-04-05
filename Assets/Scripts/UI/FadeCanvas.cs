﻿using System.Collections;
using NeanderthalTools.Util;
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
            yield return Coroutines.Progress(1f, 0f, fadeDuration, SetAlpha);
            canvas.enabled = false;
        }

        /// <summary>
        /// Fade in the canvas.
        /// </summary>
        public IEnumerator FadeIn()
        {
            canvas.enabled = true;
            yield return Coroutines.Progress(0f, 1f, fadeDuration, SetAlpha);
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
