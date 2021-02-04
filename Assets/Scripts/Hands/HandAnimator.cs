using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.XR;

namespace Hands
{
    public class HandAnimator : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        [Tooltip("Which devices input to query")]
        private XRNode node = XRNode.LeftHand;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Animation speed")]
        private float speed = 10f;

        #endregion

        #region Fields

        private const string AnimatorPinchValueName = "Pinch";
        private const string AnimatorFlexValueName = "Flex";
        private const string AnimatorPointLayerName = "Point Layer";
        private const string AnimatorThumbLayerName = "Thumb Layer";

        private InputDevice device;
        private Animator animator;

        private List<Finger> fingers;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();

            fingers = new List<Finger>
            {
                new FlexFinger(
                    CommonUsages.trigger,
                    Animator.StringToHash(AnimatorPinchValueName)
                ),
                new FlexFinger(
                    CommonUsages.grip,
                    Animator.StringToHash(AnimatorFlexValueName)
                ),
                new TouchFinger(
                    OculusUsages.indexTouch,
                    animator.GetLayerIndex(AnimatorPointLayerName)
                ),
                new TouchFinger(
                    OculusUsages.thumbTouch,
                    animator.GetLayerIndex(AnimatorThumbLayerName)
                )
            };
        }

        private void Update()
        {
            if (!device.isValid)
            {
                device = InputDevices.GetDeviceAtXRNode(node);
            }

            var time = speed * Time.unscaledDeltaTime;
            foreach (var finger in fingers)
            {
                finger.Animate(animator, device, time);
            }
        }

        #endregion
    }
}
