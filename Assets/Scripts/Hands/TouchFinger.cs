using UnityEngine;
using UnityEngine.XR;

namespace NeanderthalTools.Hands
{
    public class TouchFinger : Finger
    {
        #region Fields

        private readonly InputFeatureUsage<bool> usage;
        private readonly int layerIndex;

        #endregion

        #region Methods

        public TouchFinger(InputFeatureUsage<bool> usage, int layerIndex)
        {
            this.usage = usage;
            this.layerIndex = layerIndex;
        }

        #endregion

        #region Overrides

        protected override float GetTargetValue(InputDevice device)
        {
            if (device.TryGetFeatureValue(usage, out var value))
            {
                return value ? 0f : 1f;
            }

            return 0f;
        }

        protected override void SetAnimatorValue(Animator animator, float value)
        {
            animator.SetLayerWeight(layerIndex, value);
        }

        #endregion
    }
}
