using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// Based on: https://www.youtube.com/watch?v=6lK8QXL4bxc
namespace NeanderthalTools.Hands
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsPoser : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private InputActionReference positionAction;

        [SerializeField]
        private InputActionReference rotationAction;

        [Range(0, 1)]
        [SerializeField]
        private float slowDownVelocity = 0.75f;

        [Range(0, 1)]
        [SerializeField]
        private float slowDownAngularVelocity = 0.75f;

        [Range(0, 100)]
        [SerializeField]
        private float maxPositionChange = 75f;

        [Range(0, 100)]
        [SerializeField]
        private float maxRotationChange = 75f;

        #endregion

        #region Fields

        private new Rigidbody rigidbody;

        private Vector3 targetPosition = Vector3.zero;
        private Quaternion targetRotation = Quaternion.identity;

        private int colliderCount;

        #endregion

        #region Properties

        /// <summary>
        /// List of colliders under this poser.
        /// </summary>
        public List<Collider> Colliders { get; private set; }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            Colliders = GetComponentsInChildren<Collider>().ToList();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            positionAction.action.performed += OnPositionChanged;
            rotationAction.action.performed += OnRotationChanged;
        }

        private void OnDisable()
        {
            positionAction.action.performed -= OnPositionChanged;
            rotationAction.action.performed -= OnRotationChanged;
        }

        private void Start()
        {
            MoveImmediate();
            RotateImmediate();
        }

        private void OnTriggerEnter(Collider other)
        {
            colliderCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            colliderCount--;
        }

        private void FixedUpdate()
        {
            if (IsColliderInRange())
            {
                MovePhysics();
                RotatePhysics();
            }
            else
            {
                MoveImmediate();
                RotateImmediate();
            }
        }

        #endregion

        #region Methods

        private void OnPositionChanged(InputAction.CallbackContext ctx)
        {
            targetPosition = ctx.ReadValue<Vector3>();
        }

        private void OnRotationChanged(InputAction.CallbackContext ctx)
        {
            targetRotation = ctx.ReadValue<Quaternion>();
        }

        private void MoveImmediate()
        {
            rigidbody.velocity = Vector3.zero;
            transform.localPosition = targetPosition;
        }

        private void RotateImmediate()
        {
            rigidbody.angularVelocity = Vector3.zero;
            transform.localRotation = targetRotation;
        }

        private bool IsColliderInRange()
        {
            return colliderCount > 0;
        }

        private void MovePhysics()
        {
            rigidbody.velocity *= slowDownVelocity;

            var velocity = FindNewVelocity();
            if (IsValidVelocity(velocity))
            {
                rigidbody.velocity = Vector3.MoveTowards(
                    rigidbody.velocity,
                    velocity,
                    maxPositionChange * Time.deltaTime
                );
            }
        }

        private Vector3 FindNewVelocity()
        {
            var worldPosition = transform.root.TransformPoint(targetPosition);
            var positionDiff = worldPosition - rigidbody.position;

            return positionDiff / Time.deltaTime;
        }

        private void RotatePhysics()
        {
            rigidbody.angularVelocity *= slowDownAngularVelocity;

            var velocity = FindNewAngularVelocity();
            if (IsValidVelocity(velocity))
            {
                rigidbody.angularVelocity = Vector3.MoveTowards(
                    rigidbody.angularVelocity,
                    velocity,
                    maxRotationChange * Time.deltaTime
                );
            }
        }

        private Vector3 FindNewAngularVelocity()
        {
            var worldRotation = transform.root.rotation * targetRotation;
            var rotationDiff = worldRotation * Quaternion.Inverse(rigidbody.rotation);

            rotationDiff.ToAngleAxis(out var angle, out var axis);
            if (angle > 180)
            {
                angle -= 360;
            }

            return angle * Mathf.Deg2Rad * axis / Time.deltaTime;
        }

        private static bool IsValidVelocity(Vector3 velocity)
        {
            return IsValid(velocity.x) && IsValid(velocity.y) && IsValid(velocity.z);
        }

        private static bool IsValid(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }

        #endregion
    }
}
