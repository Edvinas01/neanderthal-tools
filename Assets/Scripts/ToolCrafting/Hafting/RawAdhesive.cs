using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class RawAdhesive : MonoBehaviour
    {
        #region Editor

        [Range(0f, 1f)]
        [SerializeField]
        [Tooltip("Amount that this raw adhesive material contributes to the production")]
        private float produceAmount = 0.1f;

        [SerializeField]
        private AdhesiveUnityEvent onConsume;

        #endregion

        #region Properties

        public float ProduceAmount => produceAmount;

        #endregion

        #region Methods

        public void HandleConsume()
        {
            onConsume.Invoke(new AdhesiveEventArgs(this));
            Destroy(gameObject);
        }

        #endregion
    }
}
