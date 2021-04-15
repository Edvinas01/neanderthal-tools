using System.Collections.Generic;
using NeanderthalTools.Util;
using UnityEngine;
using UnityEngine.Audio;

namespace NeanderthalTools.Audio
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "Game/Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        #region Editor

        [SerializeField]
        private AudioMixer mixer;

        [SerializeField]
        [Tooltip("Optional audio volume params which can be faded out, ducked, etc")]
        private List<string> optionalVolumeParameters;

        #endregion

        #region Fields

        private const float MinVolume = 0.0001f;

        private const float MinMixerVolume = -80;
        private const float MaxMixerVolume = 0;

        private readonly Dictionary<string, float> initialVolumes
            = new Dictionary<string, float>();

        #endregion

        #region Unity Lifecycle

        private void OnDisable()
        {
            initialVolumes.Clear();
        }

        #endregion

        #region Methods

        public void SetOptionalVolume(float volumePercentage)
        {
            var mixerVolume = GetMixerVolume(volumePercentage);
            foreach (var property in optionalVolumeParameters)
            {
                var remappedMixerVolume = GetRemappedMixerVolume(property, mixerVolume);
                mixer.SetFloat(property, remappedMixerVolume);
            }
        }

        private static float GetMixerVolume(float volumePercentage)
        {
            return volumePercentage <= MinVolume
                ? MinMixerVolume
                : Mathf.Log(volumePercentage) * 20;
        }

        private float GetRemappedMixerVolume(string volumeProperty, float mixerVolume)
        {
            var originalVolume = GetInitialMixerVolume(volumeProperty);

            return mixerVolume.Remap(
                MinMixerVolume,
                MaxMixerVolume,
                MinMixerVolume,
                originalVolume
            );
        }

        private float GetInitialMixerVolume(string volumeProperty)
        {
            if (initialVolumes.TryGetValue(volumeProperty, out var initialVolume))
            {
                return initialVolume;
            }

            mixer.GetFloat(volumeProperty, out initialVolume);
            initialVolumes[volumeProperty] = initialVolume;

            return initialVolume;
        }

        #endregion
    }
}
