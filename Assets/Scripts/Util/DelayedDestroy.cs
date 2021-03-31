using System.Collections;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public class DelayedDestroy : MonoBehaviour
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        [Tooltip("Effect this object is destroyed")]
        private float duration;

        #endregion

        #region Unity Lifecycle

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }

        #endregion
    }
}
