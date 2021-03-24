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

        #region Properties

        public List<ScriptableObjectGroupState> GroupStates { get; set; }

        public List<ScriptableObject> ScriptableObjects => scriptableObjects;

        public string Search { get; set; }

        #endregion

        #region Unity Lifecycle

        private void OnValidate()
        {
            GroupStates = null;
        }

        #endregion
    }
}
