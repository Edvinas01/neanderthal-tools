using System.Linq;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace Hands
{
    public class HandAnimator : MonoBehaviour
    {
        [SerializeField]
        private XRNode node;

        private InputDevice device;
        private InputDevice Device
        {
            get
            {
                if (!device.isValid)
                {
                    device = InputDevices.GetDeviceAtXRNode(node);
                }

                return device;
            }
        }

        public const string AnimLayerNamePoint = "Point Layer";
        public const string AnimLayerNameThumb = "Thumb Layer";
        public const string AnimParamNameFlex = "Flex";
        public const string AnimParamNamePose = "Pose";

        private int mAnimLayerIndexThumb = -1;
        private int mAnimLayerIndexPoint = -1;
        private int mAnimParamIndexFlex = -1;
        private int mAnimParamIndexPose = -1;
        private Collider[] mColliders = null;

        [SerializeField]
        private float frames = 4f;

        private float gripState = 0f;
        private float triggerState = 0f;
        private float triggerCapState = 0f;
        private float thumbCapState;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
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

            mAnimLayerIndexPoint = animator.GetLayerIndex(AnimLayerNamePoint);
            mAnimLayerIndexThumb = animator.GetLayerIndex(AnimLayerNameThumb);
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
                    gripState = Mathf.Clamp(gripState + 1 / frames, 0f, gripTarget);
                }
                else if (gripStateDelta < 0f)
                {
                    gripState = Mathf.Clamp(gripState - 1 / frames, gripTarget, 1f);
                }
                else
                {
                    gripState = gripTarget;
                }

                animator.SetFloat(mAnimParamIndexFlex, gripState);
            }

            if (Device.TryGetFeatureValue(CommonUsages.trigger, out float triggerTarget)
            )
            {
                var triggerStateDelta = triggerTarget - triggerState;
                if (triggerStateDelta > 0f)
                {
                    triggerState = Mathf.Clamp(triggerState + 1 / frames, 0f, triggerTarget);
                }
                else if (triggerStateDelta < 0f)
                {
                    triggerState = Mathf.Clamp(triggerState - 1 / frames, triggerTarget, 1f);
                }
                else
                {
                    triggerState = triggerTarget;
                }

                animator.SetFloat("Pinch", triggerState);
            }

            if (Device.TryGetFeatureValue(OculusUsages.indexTouch,
                out bool indexTouched))
            {
                var triggerCapTarget = indexTouched ? 1f : 0f;
                var triggerCapStateDelta = triggerCapTarget - triggerCapState;
                if (triggerCapStateDelta > 0f)
                {
                    triggerCapState =
                        Mathf.Clamp(triggerCapState + 1 / frames, 0f, triggerCapTarget);
                }
                else if (triggerCapStateDelta < 0f)
                {
                    triggerCapState =
                        Mathf.Clamp(triggerCapState - 1 / frames, triggerCapTarget, 1f);
                }
                else
                {
                    triggerCapState = triggerCapTarget;
                }

                animator.SetLayerWeight(mAnimLayerIndexPoint, 1f - triggerCapState);
            }

            if (Device.TryGetFeatureValue(OculusUsages.thumbTouch,
                out bool thumbTouched))
            {
                var thumbCapTarget = thumbTouched ? 1f : 0f;
                var thumbCapStateDelta = thumbCapTarget - thumbCapState;
                if (thumbCapStateDelta > 0f)
                {
                    thumbCapState = Mathf.Clamp(thumbCapState + 1 / frames, 0f, thumbCapTarget);
                }
                else if (thumbCapStateDelta < 0f)
                {
                    thumbCapState = Mathf.Clamp(thumbCapState - 1 / frames, thumbCapTarget, 1f);
                }
                else
                {
                    thumbCapState = thumbCapTarget;
                }

                animator.SetLayerWeight(mAnimLayerIndexThumb, 1f - thumbCapState);
            }
        }
    }
}
