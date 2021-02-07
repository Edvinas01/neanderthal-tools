using UnityEngine;

namespace NeanderthalTools.Scenes
{
    public class SceneSpawnPoint : MonoBehaviour
    {
        #region Unity Lifecycle

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

        #endregion

        #region Methods

        public void Place(Transform target)
        {
            var spawnTransform = transform;
            target.position = spawnTransform.position;
            target.rotation = spawnTransform.rotation;
        }

        #endregion
    }
}
