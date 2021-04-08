using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public class LogVisualizer : EditorWindow
    {
        #region Fields

        private static readonly MethodInfo ApplyWireMaterialMethod = typeof(HandleUtility)
            .GetMethod(
                "ApplyWireMaterial",
                BindingFlags.Static | BindingFlags.NonPublic,
                null,
                new[] {typeof(CompareFunction)},
                null
            );

        private readonly List<UserData> users = new List<UserData>();
        private int maxPositions;

        #endregion

        #region Unity Lifecycle

        [MenuItem("Tools/Log visualizer")]
        public static void Init()
        {
            var window = GetWindow<LogVisualizer>("Log visualizer");
            window.Show();
        }

        private void OnFocus()
        {
            SceneView.duringSceneGui -= DuringSceneGUI;
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= DuringSceneGUI;
        }

        private void OnGUI()
        {
            foreach (var user in users)
            {
                EditorGUILayout.LabelField(user.Session.LoggingId);
                user.IsDraw = EditorGUILayout.Toggle("Draw", user.IsDraw);
                user.Color = EditorGUILayout.ColorField("Color", user.Color);
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add user"))
            {
                users.Clear();

                var directoryPath = EditorUtility.OpenFolderPanel("Open user directory", "", "");
                if (string.IsNullOrWhiteSpace(directoryPath))
                {
                    return;
                }

                var user = UserDataReader.Read(directoryPath);
                if (user.Session == null)
                {
                    Debug.LogError($"No session data found for {directoryPath}", this);
                    return;
                }

                ReplaceOrAdd(user);
                maxPositions = FindMaxPositions();
            }
        }

        private void DuringSceneGUI(SceneView sceneView)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            foreach (var user in users)
            {
                if (user.IsDraw)
                {
                    Draw(user);
                }
            }
        }

        #endregion

        #region Methods

        private int FindMaxPositions()
        {
            var max = 0;
            foreach (var user in users)
            {
                foreach (var pose in user.Poses)
                {
                    var positionCount = pose.Positions.Count;
                    if (max < positionCount)
                    {
                        max = positionCount;
                    }
                }
            }

            return max;
        }

        private void ReplaceOrAdd(UserData newUser)
        {
            var index = users.FindIndex(existingUser =>
                existingUser.Session.LoggingId == newUser.Session.LoggingId
            );

            if (index == -1)
            {
                users.Add(newUser);
            }
            else
            {
                users[index] = newUser;
            }
        }

        private static void Draw(UserData user)
        {
            ApplyWireMaterial();

            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);

            var poses = user.Poses;
            foreach (var pose in poses)
            {
                Draw(pose, user.Color);
            }

            GL.PopMatrix();
        }

        private static void Draw(PoseData pose, Color color)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);

            var positions = pose.Positions;
            for (var index = 0; index < positions.Count - 1; index++)
            {
                var currentPosition = positions[index];
                var nextPosition = positions[index + 1];

                GL.Vertex(currentPosition);
                GL.Vertex(nextPosition);
            }

            GL.End();
        }

        private static void ApplyWireMaterial()
        {
            ApplyWireMaterialMethod.Invoke(null, new object[] {Handles.zTest});
        }

        #endregion
    }
}
