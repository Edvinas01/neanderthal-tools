using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NeanderthalTools.Inputs
{
    [CreateAssetMenu(fileName = "ControlSchemeSettings", menuName = "Game/Control Scheme Settings")]
    public class ControlSchemeSettings : ScriptableObject
    {
        #region Editor

        [SerializeField]
        [Dropdown("controlSchemes")]
        private string baseControlSchemeName = "Generic XR Controller";

        [SerializeField]
        [Dropdown("controlSchemes")]
        private string controlSchemeName = "Continuous Move";

        [SerializeField]
        private InputActionAsset inputActionAsset;

        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List<string> controlSchemes = new List<string>();

        #endregion

        #region Unity Lifecycle

        private void OnValidate()
        {
            if (inputActionAsset == null)
            {
                return;
            }

            PopulateControlSchemes();
            baseControlSchemeName = controlSchemes.FirstOrDefault();
        }

        #endregion

        #region Methods

        public void ApplyControlScheme()
        {
            var baseControlScheme = inputActionAsset.FindControlScheme(baseControlSchemeName);
            var controlScheme = inputActionAsset.FindControlScheme(controlSchemeName);

            if (!baseControlScheme.HasValue || !controlScheme.HasValue)
            {
                Debug.LogError(
                    $"Invalid control schemes: {baseControlSchemeName}, {controlSchemeName}",
                    this
                );

                return;
            }

            foreach (var inputActionMap in inputActionAsset.actionMaps)
            {
                inputActionMap.bindingMask = InputBinding.MaskByGroups(
                    baseControlScheme.Value.bindingGroup,
                    controlScheme.Value.bindingGroup
                );
            }
        }

        private void PopulateControlSchemes()
        {
            controlSchemes.Clear();
            foreach (var inputControlScheme in inputActionAsset.controlSchemes)
            {
                controlSchemes.Add(inputControlScheme.name);
            }
        }

        #endregion
    }
}
