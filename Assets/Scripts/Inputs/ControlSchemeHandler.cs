using UnityEngine;

namespace NeanderthalTools.Inputs
{
    public class ControlSchemeHandler : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private ControlSchemeSettings controlSchemeSettings;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            controlSchemeSettings.ApplyControlScheme();
        }

        #endregion
    }
}
