using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
            DrawSearch();

            if (IsSetupGroupStates())
            {
                SetupGroupStates();
            }

            if (IsDrawGroupStates())
            {
                EditorGUILayout.Space();
                DrawGroupStates();
            }
        }

        #endregion

        #region Methods

        private void DrawSearch()
        {
            scriptableObjectGroup.Search =
                EditorGUILayout.TextField("Search", scriptableObjectGroup.Search);
        }

        private bool IsSetupGroupStates()
        {
            return scriptableObjectGroup.GroupStates == null;
        }

        private void SetupGroupStates()
        {
            var groupStates = new List<ScriptableObjectGroupState>();

            foreach (var scriptableObject in scriptableObjectGroup.ScriptableObjects)
            {
                // Missing or set to null in the inspector.
                if (scriptableObject == null)
                {
                    continue;
                }

                groupStates.Add(CreateGroupState(scriptableObject));
            }

            scriptableObjectGroup.GroupStates = groupStates;
        }

        private static ScriptableObjectGroupState CreateGroupState(
            ScriptableObject scriptableObject
        )
        {
            var name = ObjectNames.NicifyVariableName(scriptableObject.name);

            UnityEditor.Editor editor = null;
            CreateCachedEditor(scriptableObject, null, ref editor);

            return new ScriptableObjectGroupState(scriptableObject, editor, name);
        }

        private bool IsDrawGroupStates()
        {
            return scriptableObjectGroup.GroupStates.Count > 0;
        }

        private void DrawGroupStates()
        {
            foreach (var groupState in scriptableObjectGroup.GroupStates)
            {
                if (IsDrawGroupState(groupState))
                {
                    DrawGroupState(groupState);
                }
            }
        }

        private bool IsDrawGroupState(ScriptableObjectGroupState groupState)
        {
            var search = scriptableObjectGroup.Search;
            if (string.IsNullOrWhiteSpace(search))
            {
                return true;
            }

            return groupState.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void DrawGroupState(ScriptableObjectGroupState groupState)
        {
            // Might be deleted.
            if (groupState.ScriptableObject == null)
            {
                return;
            }

            var expanded = groupState.IsExpanded;

            // Foldout header group is used instead of regular foldouts because it gives a nicer
            // header :)
            expanded = EditorGUILayout.BeginFoldoutHeaderGroup(expanded, groupState.Name);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (expanded)
            {
                groupState.Editor.OnInspectorGUI();
                EditorGUILayout.Space();
            }

            groupState.IsExpanded = expanded;
        }

        #endregion
    }
}
