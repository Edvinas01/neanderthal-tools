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

        public void Spawn(GameObject target)
        {
            var targetTransform = target.transform;
            var spawnTransform = transform;
            
            targetTransform.position = spawnTransform.position;
            targetTransform.rotation = spawnTransform.rotation;
        }

        #endregion
    }
}
