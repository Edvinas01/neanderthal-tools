using System.Collections;
using NaughtyAttributes;
using NeanderthalTools.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace NeanderthalTools.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceWrangler : MonoBehaviour
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        [Tooltip("At which time to start playing the clip")]
        private float startTime;

        [SerializeField]
        private Transform follow;

        [SerializeField]
        private bool fadeInOnPlay;

        [Min(0f)]
        [SerializeField]
        [ShowIf("fadeInOnPlay")]
        private float fadeInDuration = 0.5f;

        [SerializeField]
        private bool fadeOutOnStop;

        [Min(0f)]
        [SerializeField]
        [ShowIf("fadeOutOnStop")]
        private float fadeOutDuration = 0.5f;

        [SerializeField]
        private bool randomizePitch;

        [SerializeField]
        [MinMaxSlider(0f, 10f)]
        [ShowIf("randomizePitch")]
        private Vector2 pitchRange = new Vector2(0.9f, 1.0f);

        #endregion

        #region Fields

        private float initialVolume;
        private AudioSource audioSource;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            initialVolume = audioSource.volume;
        }

        private void Update()
        {
            if (follow == null)
            {
                return;
            }

            transform.position = follow.position;
        }

        #endregion

        #region Methods

        public void Play()
        {
            audioSource.time = startTime;
            if (randomizePitch)
            {
                audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            }

            if (fadeInOnPlay)
            {
                StopAllCoroutines();
                StartCoroutine(FadeIn());
            }
            else
            {
                audioSource.Play();
            }
        }

        public void Stop()
        {
            if (fadeOutOnStop)
            {
                StopAllCoroutines();
                StartCoroutine(FadeOut());
            }
            else
            {
                audioSource.Stop();
            }
        }

        private IEnumerator FadeIn()
        {
            audioSource.volume = 0f;
            audioSource.Play();
            yield return Coroutines.Progress(0f, initialVolume, fadeInDuration, SetVolume);
        }

        private IEnumerator FadeOut()
        {
            yield return Coroutines.Progress(initialVolume, 0f, fadeOutDuration, SetVolume);
            audioSource.Stop();
        }

        private void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }

        #endregion
    }
}
