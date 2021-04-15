using System.Collections;
using System.Collections.Generic;
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
        [MinMaxSlider(0f, 2f)]
        [ShowIf("randomizePitch")]
        private Vector2 pitchRange = new Vector2(0.9f, 1.0f);

        [SerializeField]
        private bool randomizeClips;

        [SerializeField]
        [ShowIf("randomizeClips")]
        private List<AudioClip> randomClips;

        [FormerlySerializedAs("destroyOnFinish")]
        [SerializeField]
        private bool destroyOnStop;

        [SerializeField]
        private bool playOnStart;

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

        private void Start()
        {
            if (playOnStart)
            {
                Play();
            }
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
                audioSource.pitch = GetRandomPitch();
            }

            if (randomizeClips)
            {
                audioSource.clip = GetRandomClip();
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

            if (destroyOnStop)
            {
                StartCoroutine(DestroyDelayed());
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

        private IEnumerator DestroyDelayed()
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            Destroy(gameObject);
        }

        private AudioClip GetRandomClip()
        {
            var clip = randomClips.GetRandom();
            if (clip == null)
            {
                return audioSource.clip;
            }

            return clip;
        }

        private float GetRandomPitch()
        {
            return Random.Range(pitchRange.x, pitchRange.y);
        }

        private void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }

        #endregion
    }
}
