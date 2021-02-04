using System.Linq;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace Hands
{
    public class Hand : MonoBehaviour
    {
        [SerializeField]
        private XRNode controller;

        private InputDevice device;
        private InputDevice Device
        {
            get
            {
                if (!device.isValid)
                {
                    device = InputDevices.GetDeviceAtXRNode(controller);
                }

                return device;
            }
        }

        [FormerlySerializedAs("m_animator")]
        public Animator mAnimator = null;

        public const string AnimLayerNamePoint = "Point Layer";
        public const string AnimLayerNameThumb = "Thumb Layer";
        public const string AnimParamNameFlex = "Flex";
        public const string AnimParamNamePose = "Pose";

        private int mAnimLayerIndexThumb = -1;
        private int mAnimLayerIndexPoint = -1;
        private int mAnimParamIndexFlex = -1;
        private int mAnimParamIndexPose = -1;
        private Collider[] mColliders = null;

        [FormerlySerializedAs("anim_frames")]
        public float animFrames = 4f;
        private float gripState = 0f;
        private float triggerState = 0f;
        private float triggerCapState = 0f;
        private float thumbCapState;

        private void Awake()
        {
            // animator =
            // controller.GetComponent<XRController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            mColliders = this.GetComponentsInChildren<Collider>()
                .Where(childCollider => !childCollider.isTrigger).ToArray();
            for (int i = 0; i < mColliders.Length; ++i)
            {
                Collider collider = mColliders[i];
                // collider.transform.localScale = new Vector3(COLLIDER_SCALE_MIN, COLLIDER_SCALE_MIN, COLLIDER_SCALE_MIN);
                collider.enabled = true;
            }

            mAnimLayerIndexPoint = mAnimator.GetLayerIndex(AnimLayerNamePoint);
            mAnimLayerIndexThumb = mAnimator.GetLayerIndex(AnimLayerNameThumb);
            mAnimParamIndexFlex = Animator.StringToHash(AnimParamNameFlex);
            mAnimParamIndexPose = Animator.StringToHash(AnimParamNamePose);

            // var inputDevices = new List<UnityEngine.XR.InputDevice>();
            // UnityEngine.XR.InputDevices.GetDevices(inputDevices);
            //
            // foreach (var device in inputDevices)
            // {
            //     Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
            //     solutionText.SetText(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
            //
            // }
        }

        // Update is called once per frame
        void Update()
        {
            if (Device.TryGetFeatureValue(CommonUsages.grip, out float gripTarget))
            {
                // solutionText.SetText(gripValue.ToString());
                var gripStateDelta = gripTarget - gripState;
                if (gripStateDelta > 0f)
                {
                    gripState = Mathf.Clamp(gripState + 1 / animFrames, 0f, gripTarget);
                }
                else if (gripStateDelta < 0f)
                {
                    gripState = Mathf.Clamp(gripState - 1 / animFrames, gripTarget, 1f);
                }
                else
                {
                    gripState = gripTarget;
                }

                mAnimator.SetFloat(mAnimParamIndexFlex, gripState);
            }

            if (Device.TryGetFeatureValue(CommonUsages.trigger, out float triggerTarget)
            )
            {
                var triggerStateDelta = triggerTarget - triggerState;
                if (triggerStateDelta > 0f)
                {
                    triggerState = Mathf.Clamp(triggerState + 1 / animFrames, 0f, triggerTarget);
                }
                else if (triggerStateDelta < 0f)
                {
                    triggerState = Mathf.Clamp(triggerState - 1 / animFrames, triggerTarget, 1f);
                }
                else
                {
                    triggerState = triggerTarget;
                }

                mAnimator.SetFloat("Pinch", triggerState);
            }

            if (Device.TryGetFeatureValue(OculusUsages.indexTouch,
                out bool indexTouched))
            {
                var triggerCapTarget = indexTouched ? 1f : 0f;
                var triggerCapStateDelta = triggerCapTarget - triggerCapState;
                if (triggerCapStateDelta > 0f)
                {
                    triggerCapState =
                        Mathf.Clamp(triggerCapState + 1 / animFrames, 0f, triggerCapTarget);
                }
                else if (triggerCapStateDelta < 0f)
                {
                    triggerCapState =
                        Mathf.Clamp(triggerCapState - 1 / animFrames, triggerCapTarget, 1f);
                }
                else
                {
                    triggerCapState = triggerCapTarget;
                }

                mAnimator.SetLayerWeight(mAnimLayerIndexPoint, 1f - triggerCapState);
            }

            if (Device.TryGetFeatureValue(OculusUsages.thumbTouch,
                out bool thumbTouched))
            {
                var thumbCapTarget = thumbTouched ? 1f : 0f;
                var thumbCapStateDelta = thumbCapTarget - thumbCapState;
                if (thumbCapStateDelta > 0f)
                {
                    thumbCapState = Mathf.Clamp(thumbCapState + 1 / animFrames, 0f, thumbCapTarget);
                }
                else if (thumbCapStateDelta < 0f)
                {
                    thumbCapState = Mathf.Clamp(thumbCapState - 1 / animFrames, thumbCapTarget, 1f);
                }
                else
                {
                    thumbCapState = thumbCapTarget;
                }

                mAnimator.SetLayerWeight(mAnimLayerIndexThumb, 1f - thumbCapState);
            }
        }
    }
}
