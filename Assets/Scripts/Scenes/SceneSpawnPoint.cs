using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Scenes
{
    public class SceneSpawnPoint : MonoBehaviour
    {
        #region Unity Lifecycle

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var spawnTransform = transform;
            var position = spawnTransform.position;

            Gizmos.DrawWireSphere(position, 0.5f);
            Gizmos.DrawRay(position, spawnTransform.forward);
        }

        #endregion

        #region Methods

        public void Spawn(GameObject target)
        {
            var targetRig = target.GetComponent<XRRig>();
            if (targetRig != null)
            {
#if UNITY_EDITOR
                // Necessary to delay as spawning fires a bit too fast in edit mode which can
                // result in rotation not adjusting properly.
                StartCoroutine(SpawnXRRigDelayed(targetRig));
#else
                SpawnXRRig(targetRig);
#endif
            }
            else
            {
                SpawnGameObject(target);
            }
        }

        private IEnumerator SpawnXRRigDelayed(XRRig rig)
        {
            yield return null;
            SpawnXRRig(rig);
        }

        private void SpawnXRRig(XRRig rig)
        {
            var spawnRotation = transform.rotation;

            rig.MatchRigUpCameraForward(
                spawnRotation * Vector3.up,
                spawnRotation * Vector3.forward
            );

            var spawnPosition = GetSpawnPosition(rig);
            rig.MoveCameraToWorldLocation(spawnPosition);
        }

        private Vector3 GetSpawnPosition(XRRig targetRig)
        {
            var heightAdjustment = targetRig.rig.transform.up * targetRig.cameraInRigSpaceHeight;
            var position = transform.position;

            return position + heightAdjustment;
        }

        private void SpawnGameObject(GameObject obj)
        {
            var spawnTransform = transform;

            var targetTransform = obj.transform;
            targetTransform.position = spawnTransform.position;
            targetTransform.rotation = spawnTransform.rotation;
        }

        #endregion
    }
}
