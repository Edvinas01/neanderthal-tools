using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Util
{
    [CreateAssetMenu(fileName = "ScriptableObjectGroup", menuName = "Game/Scriptable Object Group")]
    public class ScriptableObjectGroup : ScriptableObject
    {
        #region Editor

        [SerializeField]
        private List<ScriptableObject> scriptableObjects;

        #endregion

        #region Properties

        public List<ScriptableObject> ScriptableObjects => scriptableObjects;

        #endregion
    }
}
