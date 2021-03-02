using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Based on: https://gist.github.com/sinbad/4a9ded6b00cf6063c36a4837b15df969
namespace NeanderthalTools.Effects
{
    [RequireComponent(typeof(Light))]
    public class FlickerLight : MonoBehaviour
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        [Tooltip("Minimum random light intensity")]
        private float minIntensity;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Maximum random light intensity")]
        private float maxIntensity = 1f;

        [Range(1, 50)]
        [SerializeField]
        [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
        private int smoothing = 5;

        #endregion

        #region Fields

        private Queue<float> smoothQueue;
        private float lastSum;
        private new Light light;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            smoothQueue = new Queue<float>(smoothing);
            light = GetComponent<Light>();
        }

        private void OnDisable()
        {
            smoothQueue.Clear();
            lastSum = 0;
        }

        private void Update()
        {
            while (smoothQueue.Count >= smoothing)
            {
                lastSum -= smoothQueue.Dequeue();
            }

            // Generate random new item, calculate new average.
            var newVal = Random.Range(minIntensity, maxIntensity);
            smoothQueue.Enqueue(newVal);
            lastSum += newVal;

            // Calculate new smoothed average.
            light.intensity = lastSum / smoothQueue.Count;
        }

        #endregion
    }
}
