using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.Triggers
{
    public class DelayedTrigger : MonoBehaviour
    {
        #region Editor

        [Min(0f)]
        [SerializeField]
        [Tooltip("Delay in seconds")]
        private float delay = 1f;

        [SerializeField]
        private UnityEvent onTrigger;

        #endregion

        #region Unity Lifecycle

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            onTrigger.Invoke();
            Destroy(this);
        }

        #endregion
    }
}
