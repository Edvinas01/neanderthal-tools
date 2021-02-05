using UnityEngine;
using UnityEngine.XR;

namespace NeanderthalTools.Hands
{
    public abstract class Finger
    {
        #region Fields

        private float targetValue;
        private float currentValue;

        #endregion

        #region Methods

        public void Animate(Animator animator, InputDevice device, float time)
        {
            targetValue = GetTargetValue(device);
            currentValue = Mathf.MoveTowards(currentValue, targetValue, time);
            SetAnimatorValue(animator, currentValue);
        }

        protected abstract float GetTargetValue(InputDevice device);

        protected abstract void SetAnimatorValue(Animator animator, float value);

        #endregion
    }
}
