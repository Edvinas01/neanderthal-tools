using NaughtyAttributes;
using NeanderthalTools.Util;
using UnityEngine;

namespace NeanderthalTools.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class CollisionAudioSource : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        [MinMaxSlider(0f, 1f)]
        private Vector2 volumeRange = new Vector2(0.9f, 1f);

        [SerializeField]
        private bool randomVolumeOffset;

        [SerializeField]
        [MinMaxSlider(-1f, 1f)]
        [ShowIf("randomVolumeOffset")]
        private Vector2 randomVolumeOffsetRange = new Vector2(0.05f, 0.1f);

        [SerializeField]
        [MinMaxSlider(0f, 2f)]
        private Vector2 pitchRange = new Vector2(0.9f, 1f);

        [SerializeField]
        private bool randomPitchOffset;

        [SerializeField]
        [MinMaxSlider(-1f, 1f)]
        [ShowIf("randomPitchOffset")]
        private Vector2 randomPitchOffsetRange = new Vector2(0.05f, 0.1f);

        [Min(0f)]
        [SerializeField]
        private float minForce = 5f;

        [Min(0f)]
        [SerializeField]
        private float maxForce = 100f;

        #endregion

        #region Fields

        private AudioSource audioSource;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        #region Methods

        public void Play(Collision collision)
        {
            var force = collision.impulse.magnitude / Time.fixedDeltaTime;
            if (force < minForce)
            {
                return;
            }

            var normalizedForce = NormalizeForce(force);
            var volume = GetVolume(normalizedForce);
            var pitch = GetPitch(normalizedForce);

            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();
        }

        private float NormalizeForce(float force)
        {
            return Mathf.Max(Mathf.Min(force - minForce, maxForce), 0f);
        }

        private float GetVolume(float force)
        {
            var volume = RemapForce(force, volumeRange);
            if (randomVolumeOffset)
            {
                var volumeOffset = GetRandomOffset(randomVolumeOffsetRange);
                return volume + volumeOffset;
            }

            return volume;
        }

        private float GetPitch(float force)
        {
            var pitch = RemapForce(force, pitchRange);
            if (randomPitchOffset)
            {
                var pitchOffset = GetRandomOffset(randomPitchOffsetRange);
                return pitch + pitchOffset;
            }

            return pitch;
        }

        private float RemapForce(float force, Vector2 range)
        {
            return force.Remap(minForce, maxForce, range.x, range.y);
        }

        private static float GetRandomOffset(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }

        #endregion
    }
}
