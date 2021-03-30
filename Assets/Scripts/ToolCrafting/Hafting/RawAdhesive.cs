using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class RawAdhesive : MonoBehaviour
    {
        #region Editor

        [Range(0f, 1f)]
        [SerializeField]
        [Tooltip("Amount that this raw adhesive material contributes to the production")]
        private float amount = 0.1f;

        [SerializeField]
        private UnityEvent onConsume;

        #endregion

        #region Properties

        public float Amount => amount;

        #endregion

        #region Methods

        public void HandleConsume()
        {
            onConsume.Invoke();
            Destroy(gameObject);
        }

        #endregion
    }
}
