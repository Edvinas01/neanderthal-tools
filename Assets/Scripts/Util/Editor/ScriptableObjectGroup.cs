using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Util.Editor
{
    [CreateAssetMenu(fileName = "ScriptableObjectGroup", menuName = "Game/Scriptable Object Group")]
    public class ScriptableObjectGroup : ScriptableObject
    {
        #region Editor

        [SerializeField]
        private List<ScriptableObject> scriptableObjects;

        #endregion

        #region Fields

        private List<ScriptableObjectEditor> scriptableObjectEditors;

        #endregion

        #region Properties

        public List<ScriptableObjectEditor> ScriptableObjectEditors
        {
            get
            {
                if (scriptableObjectEditors == null)
                {
                    SetupScriptableObjectEditors();
                }

                return scriptableObjectEditors;
            }
        }

        #endregion

        #region Unity Lifecycle

        private void OnValidate()
        {
            scriptableObjectEditors = null;
        }

        #endregion

        #region Methods

        private void SetupScriptableObjectEditors()
        {
            scriptableObjectEditors = new List<ScriptableObjectEditor>();

            foreach (var scriptableObject in scriptableObjects)
            {
                // Missing or set to null in the inspector.
                if (scriptableObject == null)
                {
                    continue;
                }

                ScriptableObjectEditors.Add(new ScriptableObjectEditor(scriptableObject));
            }
        }

        #endregion
    }
}
