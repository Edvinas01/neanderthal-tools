using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Effects
{
    public class FireflyFader : MonoBehaviour
    {
        #region Fields

        private readonly List<Fireflies> fireflies = new List<Fireflies>();

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            fireflies.AddRange(GetComponentsInChildren<Fireflies>());
        }

        #endregion

        #region Methods

        public void StartFadeIn()
        {
            foreach (var firefly in fireflies)
            {
                firefly.StartFadeIn();
            }
        }

        public void StartFadeOut()
        {
            foreach (var firefly in fireflies)
            {
                firefly.StartFadeOut();
            }
        }

        #endregion
    }
}
