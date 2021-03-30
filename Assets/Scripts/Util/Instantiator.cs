using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public class Instantiator : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private List<GameObject> prefabs;

        #endregion

        #region Methods

        public void Instantiate()
        {
            var randomPrefab = prefabs.GetRandom();
            if (randomPrefab == null)
            {
                return;
            }

            var instantiatorTransform = transform;
            Instantiate(
                randomPrefab,
                instantiatorTransform.position,
                instantiatorTransform.rotation
            );
        }

        #endregion
    }
}
