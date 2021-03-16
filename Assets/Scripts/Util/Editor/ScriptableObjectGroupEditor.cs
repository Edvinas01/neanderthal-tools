using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NeanderthalTools.Util.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObjectGroup))]
    public class ScriptableObjectGroupEditor : UnityEditor.Editor
    {
        #region Enums

        private class EditorInfo
        {
            public bool Expanded { get; set; }

            public UnityEditor.Editor Editor { get; set; }

            public string Name { get; set; }
        }

        #endregion

        #region Fields

        private ScriptableObjectGroup scriptableObjectGroup;
        private readonly List<EditorInfo> editorInfos = new List<EditorInfo>();

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            scriptableObjectGroup = (ScriptableObjectGroup) target;
            SetupEditorInfos();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();

            if (EditorGUI.EndChangeCheck())
            {
                SetupEditorInfos();
            }

            DrawEditors();
        }

        #endregion

        #region Methods

        private void SetupEditorInfos()
        {
            editorInfos.Clear();

            foreach (var scriptableObject in scriptableObjectGroup.ScriptableObjects)
            {
                if (scriptableObject == null)
                {
                    continue;
                }

                SetupEditorInfo(scriptableObject);
            }
        }

        private void SetupEditorInfo(Object obj)
        {
            UnityEditor.Editor editor = null;
            CreateCachedEditor(obj, null, ref editor);

            var editorInfo = new EditorInfo
            {
                Expanded = GetExpanded(obj),
                Editor = editor,
                Name = ObjectNames.NicifyVariableName(obj.name)
            };

            editorInfos.Add(editorInfo);
        }

        private void DrawEditors()
        {
            foreach (var editorInfo in editorInfos)
            {
                var expanded = editorInfo.Expanded;

                EditorGUI.BeginChangeCheck();
                expanded = EditorGUILayout.Foldout(
                    expanded,
                    editorInfo.Name,
                    GetFoldoutStyle()
                );

                if (EditorGUI.EndChangeCheck())
                {
                    SetExpanded(editorInfo.Editor.target, expanded);
                }

                if (expanded)
                {
                    editorInfo.Editor.DrawDefaultInspector();
                }

                editorInfo.Expanded = expanded;

                EditorGUILayout.Space();
            }
        }

        private static GUIStyle GetFoldoutStyle()
        {
            var style = EditorStyles.foldout;
            style.fontStyle = FontStyle.Bold;

            return style;
        }

        private bool GetExpanded(Object obj)
        {
            return EditorPrefs.GetBool(GetExpandedKey(obj), false);
        }

        private void SetExpanded(Object obj, bool expanded)
        {
            EditorPrefs.SetBool(GetExpandedKey(obj), expanded);
        }

        private string GetExpandedKey(Object obj)
        {
            return $"{scriptableObjectGroup.name}-{obj.name}";
        }

        #endregion
    }
}
