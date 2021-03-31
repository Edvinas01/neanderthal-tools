using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.ToolCrafting.Hafting
{
    public class AdhesiveProducer : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        [Tooltip("Target adhesive production points")]
        private List<Adhesive> adhesives;

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
            foreach (var adhesive in adhesives)
            {
                adhesive.Amount += rawAdhesive.ProduceAmount;
            }
            
            rawAdhesive.HandleConsume();
        }

        #endregion
    }
}
