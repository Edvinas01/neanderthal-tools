using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NeanderthalTools.Knapping
{
    [RequireComponent(typeof(Camera))]
    public class KnappingProjectileShooter : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private PrimitiveType type = PrimitiveType.Sphere;

        [Min(0f)]
        [SerializeField]
        private float velocity = 2f;

        [SerializeField]
        private bool useGravity;

        [Min(0f)]
        [SerializeField]
        private float trailScale = 0.5f;

        [Min(0f)]
        [SerializeField]
        private float scale = 0.05f;

        [Min(0f)]
        [SerializeField]
        private float mass = 1f;

        [Min(0f)]
        [SerializeField]
        private float lifetimeSeconds = 5f;

        private new Camera camera;
        private Mouse mouse;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            camera = GetComponent<Camera>();
            mouse = Mouse.current;
        }

        private void Update()
        {
            if (Application.isFocused && IsFireProjectile())
            {
                FireProjectile();
            }
        }

        #endregion

        #region Methods

        private bool IsFireProjectile()
        {
            return mouse.leftButton.wasPressedThisFrame;
        }

        private void FireProjectile()
        {
            var projectile = GameObject.CreatePrimitive(type);

            SetupTransform(projectile);
            SetupRigidbody(projectile);
            SetupTrail(projectile);
            SetupKnappingProjectile(projectile);

            StartCoroutine(DestroyAfterLifetime(projectile));
        }

        private void SetupTransform(GameObject projectile)
        {
            var projectileTransform = projectile.transform;
            projectileTransform.localScale = Vector3.one * scale;
            projectileTransform.position = transform.position;
        }

        private void SetupRigidbody(GameObject projectile)
        {
            var projectileRigidbody = projectile.AddComponent<Rigidbody>();
            projectileRigidbody.useGravity = useGravity;
            projectileRigidbody.velocity = GetDirection() * velocity;
            projectileRigidbody.mass = mass;
        }

        private Vector3 GetDirection()
        {
            var mousePosition = mouse.position.ReadValue();
            var mouseRay = camera.ScreenPointToRay(mousePosition);

            return mouseRay.direction;
        }

        private void SetupTrail(GameObject projectile)
        {
            var projectileTrailScale = scale * trailScale;
            var projectileTrail = projectile.AddComponent<TrailRenderer>();

            projectileTrail.startWidth = projectileTrailScale;
            projectileTrail.endWidth = projectileTrailScale;
        }

        private static void SetupKnappingProjectile(GameObject projectile)
        {
            projectile.AddComponent<Knapper>();
        }

        private IEnumerator DestroyAfterLifetime(Object projectile)
        {
            yield return new WaitForSeconds(lifetimeSeconds);
            Destroy(projectile);
        }

        #endregion
    }
}
