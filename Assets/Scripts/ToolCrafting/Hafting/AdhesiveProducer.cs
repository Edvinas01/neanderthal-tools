using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class AdhesiveProducer : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        [Tooltip("Target adhesive production point")]
        private Adhesive produce;

        #endregion

        #region Methods

        public void ProduceAdhesive(Component component)
        {
            var rawAdhesive = component.GetComponentInParent<RawAdhesive>();
            if (rawAdhesive == null)
            {
                return;
            }

            ProduceAdhesive(rawAdhesive);
        }

        private void ProduceAdhesive(RawAdhesive rawAdhesive)
        {
            produce.Amount += rawAdhesive.Amount;
            rawAdhesive.HandleConsume();
        }

        #endregion
    }
}
