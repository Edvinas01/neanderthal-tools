using UnityEngine;

namespace NeanderthalTools.Util.Editor
{
    public class ScriptableObjectGroupState
    {
        #region Properties

        public ScriptableObject ScriptableObject { get; }

        public UnityEditor.Editor Editor { get; }

        public string Name { get; }

        public bool IsExpanded { get; set; }

        #endregion

        #region Methods

        public ScriptableObjectGroupState(
            ScriptableObject scriptableObject,
            UnityEditor.Editor editor,
            string name
        )
        {
            ScriptableObject = scriptableObject;
            Editor = editor;
            Name = name;
        }

        #endregion
    }
}
