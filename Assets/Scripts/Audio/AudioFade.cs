using System.Collections;
using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Audio
{
    public class AudioFade : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private AudioSettings settings;

        [Min(0f)]
        [SerializeField]
        private float fadeInDuration = 1.0f;

        [Min(0f)]
        [SerializeField]
        private float fadeOutDuration = 1.0f;

        #endregion

        #region Methods

        public IEnumerator FadeOut()
        {
            return Coroutines.Progress(1f, 0f, fadeOutDuration, OnFadeVolume);
        }

        public IEnumerator FadeIn()
        {
            return Coroutines.Progress(0f, 1f, fadeInDuration, OnFadeVolume);
        }

        private void OnFadeVolume(float volume)
        {
            settings.SetOptionalVolume(volume);
        }

        #endregion
    }
}
