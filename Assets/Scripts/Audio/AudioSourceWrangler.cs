using UnityEngine;

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

        public void Play()
        {
            audioSource.time = startTime;
            audioSource.Play();
        }

        #endregion
    }
}
