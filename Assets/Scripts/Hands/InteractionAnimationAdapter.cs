using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    public class InteractionAnimationAdapter : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private int poseIndex;

        [SerializeField]
        private XRBaseInteractable interactable;

        #endregion

        #region Fields

        private static readonly int AnimatorPoseParameter = Animator.StringToHash("Pose");
        private const int FlexPoseIndex = 0;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            interactable.selectEntered.AddListener(TransitionToPose);
            interactable.selectExited.AddListener(TransitionToFlex);
        }

        private void OnDisable()
        {
            interactable.selectEntered.RemoveListener(TransitionToPose);
            interactable.selectExited.RemoveListener(TransitionToFlex);
        }

        #endregion

        #region Methods

        public void TransitionToPose(BaseInteractionEventArgs args)
        {
            SetPoseIndex(args, poseIndex);
        }

        public void TransitionToFlex(BaseInteractionEventArgs args)
        {
            SetPoseIndex(args, FlexPoseIndex);
        }

        private static void SetPoseIndex(BaseInteractionEventArgs args, int index)
        {
            var animator = args.interactor.GetComponentInChildren<Animator>();
            animator.SetInteger(AnimatorPoseParameter, index);
        }

        #endregion
    }
}
