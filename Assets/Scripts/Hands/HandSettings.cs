using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

namespace NeanderthalTools.Hands
{
    [CreateAssetMenu(fileName = "HandSettings", menuName = "Game/Hand Settings")]
    public class HandSettings : ScriptableObject
    {
        #region Editor

        [Header("Animation")]
        [SerializeField]
        [Tooltip("Which devices input to query when animating the hand")]
        private XRNode xrNode = XRNode.LeftHand;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Finger animation playback speed")]
        private float animationSpeed = 10f;

        [Header("Input")]
        [SerializeField]
        private InputActionReference positionAction;

        [SerializeField]
        private InputActionReference rotationAction;

        [SerializeField]
        private InputActionReference selectAction;

        [Header("Physics")]
        [Range(0, 1)]
        [SerializeField]
        [Tooltip("Velocity change multiplier, drag")]
        private float velocityMultiplier = 0.99f;

        [Range(0, 1)]
        [SerializeField]
        [Tooltip("Angular velocity change multiplier, drag")]
        private float angularVelocityMultiplier = 0.99f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Speed in which the physical position is applied")]
        private float positionChangeSpeed = 400f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Speed in which the physical rotation is applied")]
        private float rotationChangeSpeed = 400f;

        [Min(0f)]
        [SerializeField]
        [Tooltip("Radius used to query the colliders around the hand")]
        private float physicsRadius = 0.1f;

        [SerializeField]
        [Tooltip(
            "Mask which is used to query colliders around the hand which enable physical movement"
        )]
        private LayerMask physicsMask;

        [SerializeField]
        [Tooltip("Layer applied to then hand when its selected")]
        private string incorporealColliderLayer = "IncorporealHands";

        [SerializeField]
        [Tooltip("Layer applied to then hand when its not selected")]
        private string regularColliderLayer = "Hands";

        #endregion

        #region Properties

        public InputDevice InputDevice => InputDevices.GetDeviceAtXRNode(xrNode);

        public float AnimationSpeed => animationSpeed;

        public InputAction PositionAction => positionAction;

        public InputAction RotationAction => rotationAction;

        public InputAction SelectAction => selectAction;

        public float VelocityMultiplier => velocityMultiplier;

        public float AngularVelocityMultiplier => angularVelocityMultiplier;

        public float PositionChangeSpeed => positionChangeSpeed;

        public float RotationChangeSpeed => rotationChangeSpeed;

        public float PhysicsRadius => physicsRadius;

        public LayerMask PhysicsMask => physicsMask;

        public LayerMask IncorporealColliderMask => LayerMask.NameToLayer(incorporealColliderLayer);

        public LayerMask RegularColliderMask => LayerMask.NameToLayer(regularColliderLayer);

        #endregion
    }
}
