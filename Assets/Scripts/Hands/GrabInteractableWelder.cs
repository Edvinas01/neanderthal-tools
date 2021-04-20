using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Hands
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(XRGrabInteractable))]
    public class GrabInteractableWelder : MonoBehaviour
    {
        #region Editor

        [Header("Interactable")]
        [SerializeField]
        [Tooltip("Should this interactable snap to interactor anchor position")]
        private bool snapPosition;

        [Header("Fixed joint")]
        [Min(0f)]
        [SerializeField]
        [Tooltip("Enable pre-processing for the joint")]
        private bool enablePreprocessing = true;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Mass scale applied to the connected body via joint")]
        private float connectedMassScale = 1.0f;

        #endregion

        #region Fields

        private XRGrabInteractable interactable;
        private FixedJoint joint;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            interactable = GetComponent<XRGrabInteractable>();

            SetupInteractableTracking(false);

            if (snapPosition)
            {
                SetupAttach();
            }
        }

        private void OnEnable()
        {
            interactable.selectEntered.AddListener(Weld);
            interactable.selectExited.AddListener(UnWeld);
        }

        private void OnDisable()
        {
            interactable.selectEntered.RemoveListener(Weld);
            interactable.selectExited.RemoveListener(UnWeld);
            DestroyJoint();
        }

        #endregion

        #region Methods

        private void SetupInteractableTracking(bool track)
        {
            interactable.trackPosition = track;
            interactable.trackRotation = track;
            interactable.movementType = track
                ? XRBaseInteractable.MovementType.Instantaneous
                : XRBaseInteractable.MovementType.VelocityTracking;
        }

        private void SetupAttach()
        {
            if (interactable.attachTransform != null)
            {
                return;
            }

            interactable.attachTransform = transform;
        }

        public void Weld(SelectEnterEventArgs args)
        {
            DestroyJoint();

            var interactor = args.interactor;
            var interactorRigidbody = interactor.GetComponent<Rigidbody>();
            if (interactorRigidbody == null)
            {
                return;
            }

            if (snapPosition)
            {
                SnapPosition(interactor);
            }

            CreateJoint(interactorRigidbody);
        }

        public void UnWeld(SelectExitEventArgs args)
        {
            DestroyJoint();
        }

        private void SnapPosition(XRBaseInteractor interactor)
        {
            interactable.MatchAttachPose(interactor);
        }

        private void CreateJoint(Rigidbody interactorRigidbody)
        {
            joint = gameObject.AddComponent<FixedJoint>();
            joint.enablePreprocessing = enablePreprocessing;
            joint.connectedMassScale = connectedMassScale;
            joint.connectedBody = interactorRigidbody;
        }

        private void DestroyJoint()
        {
            Destroy(joint);
        }

        #endregion
    }
}
