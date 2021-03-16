using UnityEditor;
using UnityEngine;

namespace NeanderthalTools.Util.Editor
{
    public class ScriptableObjectEditor
    {
        #region Fields

        private static readonly GUIStyle FoldoutStyle = CreateFoldoutStyle();

        private readonly ScriptableObject scriptableObject;
        private readonly UnityEditor.Editor editor;
        private readonly string name;
        private bool expanded;

        #endregion

        #region Methods

        public ScriptableObjectEditor(ScriptableObject scriptableObject)
        {
            this.scriptableObject = scriptableObject;

            UnityEditor.Editor.CreateCachedEditor(scriptableObject, null, ref editor);
            name = ObjectNames.NicifyVariableName(scriptableObject.name);
        }

        public void Draw()
        {
            // Might be deleted.
            if (scriptableObject == null)
            {
                return;
            }

            expanded = EditorGUI.Foldout(
                EditorGUILayout.GetControlRect(),
                expanded,
                name,
                true,
                FoldoutStyle
            );

            if (expanded)
            {
                editor.DrawDefaultInspector();
                EditorGUILayout.Space();
            }
        }

        private static GUIStyle CreateFoldoutStyle()
        {
            return new GUIStyle(EditorStyles.foldout)
            {
                font = EditorStyles.boldFont
            };
        }

        #endregion
    }
}
