using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NeanderthalTools.Triggers
{
    [RequireComponent(typeof(ColliderTrigger))]
    public class XRRigTrigger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private XRRigUnityEvent onXrRigEnter;

        [SerializeField]
        private XRRigUnityEvent onXRRigExit;

        #endregion

        #region Fields

        private ColliderTrigger colliderTrigger;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            colliderTrigger = GetComponent<ColliderTrigger>();
        }

        private void OnEnable()
        {
            colliderTrigger.OnColliderEnter.AddListener(OnColliderEnter);
            colliderTrigger.OnColliderExit.AddListener(OnColliderExit);
        }

        private void OnDisable()
        {
            colliderTrigger.OnColliderEnter.RemoveListener(OnColliderEnter);
            colliderTrigger.OnColliderExit.RemoveListener(OnColliderExit);
        }

        #endregion

        #region Methods

        private void OnColliderEnter(Collider other)
        {
            var rig = GetXRRig(other);
            if (rig != null)
            {
                onXrRigEnter.Invoke(rig);
            }
        }

        private void OnColliderExit(Collider other)
        {
            var rig = GetXRRig(other);
            if (rig != null)
            {
                onXRRigExit.Invoke(rig);
            }
        }

        private static XRRig GetXRRig(Component component)
        {
            return component.GetComponentInParent<XRRig>();
        }

        #endregion
    }
}
