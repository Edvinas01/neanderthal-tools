using System;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.Triggers
{
    public class CountingTrigger : MonoBehaviour
    {
        #region Editor

        [Min(0)]
        [SerializeField]
        private int desiredCount;

        [SerializeField]
        private UnityEvent onTrigger;

        #endregion

        #region Fields

        [NaughtyAttributes.ShowNonSerializedField]
        private int currentCount;

        #endregion

        #region Unity Lifecycle

        private void OnDisable()
        {
            currentCount = 0;
        }

        #endregion

        #region Methods

        public void Increment()
        {
            if (IsDesiredCount())
            {
                return;
            }

            currentCount++;
            if (IsDesiredCount())
            {
                onTrigger.Invoke();
            }
        }

        public void Decrement()
        {
            if (IsDesiredCount())
            {
                return;
            }

            currentCount = Math.Max(currentCount - 1, 0);
        }

        private bool IsDesiredCount()
        {
            return currentCount == desiredCount;
        }

        #endregion
    }
}
