using UnityEngine;

namespace NeanderthalTools.Knapping
{
    [RequireComponent(typeof(Rigidbody))]
    public class KnappingStone : MonoBehaviour
    {
        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void Knapp(GameObject knappingStonePart)
        {
            knappingStonePart.transform.parent = null;
            knappingStonePart.AddComponent<Rigidbody>();
        }
    }
}
