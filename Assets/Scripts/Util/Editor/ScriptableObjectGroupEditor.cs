using UnityEditor;

namespace NeanderthalTools.Util.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObjectGroup))]
    public class ScriptableObjectGroupEditor : UnityEditor.Editor
    {
        #region Fields

        private ScriptableObjectGroup scriptableObjectGroup;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            scriptableObjectGroup = (ScriptableObjectGroup) target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            foreach (var editor in scriptableObjectGroup.ScriptableObjectEditors)
            {
                editor.Draw();
            }
        }

        #endregion
    }
}
