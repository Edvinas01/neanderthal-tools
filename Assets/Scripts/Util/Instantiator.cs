using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public class Instantiator : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private bool randomize;

        [SerializeField]
        private List<GameObject> prefabs;

        #endregion

        #region Methods

        public void Instantiate()
        {
            if (randomize)
            {
                InstantiateRandom();
            }
            else
            {
                InstantiateAll();
            }
        }

        private void InstantiateAll()
        {
            foreach (var prefab in prefabs)
            {
                Instantiate(prefab);
            }
        }

        private void InstantiateRandom()
        {
            var randomPrefab = prefabs.GetRandom();
            if (randomPrefab == null)
            {
                return;
            }

            Instantiate(randomPrefab);
        }

        private void Instantiate(GameObject prefab)
        {
            var instantiatorTransform = transform;

            Instantiate(
                prefab,
                instantiatorTransform.position,
                instantiatorTransform.rotation
            );
        }

        #endregion
    }
}
