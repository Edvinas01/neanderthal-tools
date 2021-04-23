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

        [Header("Joint")]
        [Min(0f)]
        [SerializeField]
        [Tooltip("Enable pre-processing for the joint")]
        private bool enablePreprocessing = true;

        [SerializeField]
        [Tooltip("Should the connected body anchor be configured automatically")]
        private bool autoconfigureConnectedAnchor;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Mass scale applied to the connected body via joint")]
        private float connectedMassScale = 1.0f;

        #endregion

        #region Fields

        private XRGrabInteractable interactable;
        private Joint joint;

        #endregion

        #region Unity Lifecycle

        private void OnDrawGizmos()
        {
            if (joint == null)
            {
                return;
            }

            var connectedRigidbody = joint.connectedBody;
            var connectedPosition = connectedRigidbody.position
                                    + connectedRigidbody.rotation
                                    * joint.connectedAnchor;

            var anchorTransform = transform;
            var anchorPosition = anchorTransform.position
                                 + anchorTransform.rotation
                                 * joint.anchor;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(connectedPosition, 0.05f);
            Gizmos.DrawWireSphere(anchorPosition, 0.05f);
        }

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

            CreateJoint(interactorRigidbody, interactor);
        }

        public void UnWeld(SelectExitEventArgs args)
        {
            DestroyJoint();
        }

        private void SnapPosition(XRBaseInteractor interactor)
        {
            interactable.MatchAttachPose(interactor);
        }

        private void CreateJoint(Rigidbody interactorRigidbody, XRBaseInteractor interactor)
        {
            var configurableJoint = gameObject.AddComponent<ConfigurableJoint>();

            SetupJointConstraints(configurableJoint);
            SetupJointMass(configurableJoint);
            SetupJointConnectedBody(configurableJoint, interactorRigidbody, interactor);

            joint = configurableJoint;
        }

        private void DestroyJoint()
        {
            Destroy(joint);
        }

        private static void SetupJointConstraints(ConfigurableJoint configurableJoint)
        {
            configurableJoint.xMotion = ConfigurableJointMotion.Limited;
            configurableJoint.yMotion = ConfigurableJointMotion.Limited;
            configurableJoint.zMotion = ConfigurableJointMotion.Limited;
            configurableJoint.angularXMotion = ConfigurableJointMotion.Limited;
            configurableJoint.angularYMotion = ConfigurableJointMotion.Limited;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Limited;
        }

        private void SetupJointMass(ConfigurableJoint configurableJoint)
        {
            configurableJoint.enablePreprocessing = enablePreprocessing;
            configurableJoint.connectedMassScale = connectedMassScale;
        }

        private void SetupJointConnectedBody(
            ConfigurableJoint configurableJoint,
            Rigidbody interactorRigidbody,
            XRBaseInteractor interactor
        )
        {
            configurableJoint.autoConfigureConnectedAnchor = autoconfigureConnectedAnchor;
            configurableJoint.connectedBody = interactorRigidbody;

            if (autoconfigureConnectedAnchor)
            {
                return;
            }

            // Configure anchor manually.
            var worldAnchor = interactor.transform.position;
            var localAnchor = transform.InverseTransformPoint(worldAnchor);

            configurableJoint.connectedAnchor = Vector3.zero;
            configurableJoint.anchor = localAnchor;
        }

        #endregion
    }
}
