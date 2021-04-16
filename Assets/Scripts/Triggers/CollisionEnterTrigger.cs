using UnityEngine;

namespace NeanderthalTools.Triggers
{
    public class CollisionEnterTrigger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private CollisionUnityEvent onCollision;

        #endregion

        #region Unity Lifecycle

        private void OnCollisionEnter(Collision collision)
        {
            onCollision.Invoke(collision);
        }

        #endregion
    }
}
