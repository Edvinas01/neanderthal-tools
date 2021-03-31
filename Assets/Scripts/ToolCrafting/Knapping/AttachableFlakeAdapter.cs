using UnityEngine;
using UnityEngine.Events;

namespace NeanderthalTools.ToolCrafting.Knapping
{
    public class AttachableFlakeAdapter : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private UnityEvent onDetach;

        #endregion

        #region Methods

        public void OnDetach(FlakeEventArgs args)
        {
            if (args.Flake.IsAttachable)
            {
                onDetach.Invoke();
            }
        }

        #endregion
    }
}
