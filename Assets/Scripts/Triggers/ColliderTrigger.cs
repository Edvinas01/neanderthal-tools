using UnityEngine;

namespace NeanderthalTools.Triggers
{
    [RequireComponent(typeof(Collider))]
    public class ColliderTrigger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private ColliderUnityEvent onColliderEnter;

        [SerializeField]
        private ColliderUnityEvent onColliderExit;

        #endregion

        #region Properties

        public ColliderUnityEvent OnColliderEnter => onColliderEnter;

        public ColliderUnityEvent OnColliderExit => onColliderExit;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            var colliderComponent = GetComponent<Collider>();
            colliderComponent.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            onColliderEnter.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            onColliderExit.Invoke(other);
        }

        #endregion
    }
}
