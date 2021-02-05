using UnityEngine;
using UnityEngine.XR;

namespace NeanderthalTools.Hands
{
    public class FlexFinger : Finger
    {
        #region Fields

        private readonly InputFeatureUsage<float> usage;
        private readonly int valueId;

        #endregion

        #region Methods

        public FlexFinger(InputFeatureUsage<float> usage, int valueId)
        {
            this.usage = usage;
            this.valueId = valueId;
        }

        #endregion

        #region Overrides

        protected override float GetTargetValue(InputDevice device)
        {
            return device.TryGetFeatureValue(usage, out var value) ? value : 0f;
        }

        protected override void SetAnimatorValue(Animator animator, float value)
        {
            animator.SetFloat(valueId, value);
        }

        #endregion
    }
}
