using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NeanderthalTools.Util.Editor;
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
        private UserSessions userSessions;

        private Vector2 usersScroll;
        private bool drawUserPositions;
        private int maxPositions;
        private int seekStart;
        private int seekSize;

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
            if (userSessions == null)
            {
                LoadUserSessions();
            }

            EditorGUILayout.LabelField("Users", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            usersScroll = EditorGUILayout.BeginScrollView(
                usersScroll,
                GUIStyle.none,
                GUI.skin.verticalScrollbar
            );

            DrawUsersGUI();
            EditorGUILayout.EndScrollView();

            DrawVisualizerGUI();
            EditorGUILayout.Space();

            if (GUILayout.Button("Save user sessions"))
            {
                SaveUserSessions();
            }

            if (GUILayout.Button("Add user sessions"))
            {
                AddSessions();
            }
        }

        private void DuringSceneGUI(SceneView sceneView)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            if (drawUserPositions)
            {
                DrawUserPositions();
            }
        }

        #endregion

        #region GUI drawing methods

        private void DrawUsersGUI()
        {
            for (var userIndex = users.Count - 1; userIndex >= 0; userIndex--)
            {
                var user = users[userIndex];
                user.Foldout = EditorGUILayout.BeginFoldoutHeaderGroup(
                    user.Foldout,
                    user.LoggingId
                );

                if (user.Foldout)
                {
                    DrawUserGUI(user);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

        private void DrawUserGUI(UserData user)
        {
            EditorGUI.indentLevel++;

            var sessions = user.Sessions;
            for (var sessionIndex = sessions.Count - 1; sessionIndex >= 0; sessionIndex--)
            {
                var session = sessions[sessionIndex];

                DrawSessionGUI(sessions, session);
                EditorGUILayout.Space();
            }

            EditorGUI.indentLevel--;
            if (GUILayout.Button("Remove user"))
            {
                users.Remove(user);
            }

            EditorGUILayout.Space();
        }

        private static void DrawSessionGUI(
            ICollection<AggregatedSessionData> sessions,
            AggregatedSessionData session
        )
        {
            DrawSessionHeaderGUI(sessions, session);
            DrawSessionPoseGUI(session);

            session.IsDraw = EditorGUILayout.Toggle("Draw all", session.IsDraw);
            session.Color = EditorGUILayout.ColorField("Color", session.Color);
        }

        private static void DrawSessionHeaderGUI(
            ICollection<AggregatedSessionData> sessions,
            AggregatedSessionData session
        )
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(session.SessionName);
            if (GUILayout.Button("Remove session"))
            {
                sessions.Remove(session);
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSessionPoseGUI(AggregatedSessionData session)
        {
            foreach (var pose in session.Poses)
            {
                pose.IsDraw = EditorGUILayout.Toggle($"Draw {pose.Name}", pose.IsDraw);
            }
        }

        private void DrawVisualizerGUI()
        {
            EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);
            drawUserPositions = EditorGUILayout.Toggle("Draw user positions", drawUserPositions);
            seekStart = EditorGUILayout.IntSlider("Seek start", seekStart, 0, maxPositions);
            seekSize = EditorGUILayout.IntSlider("Seek size", seekSize, 0, maxPositions);
        }

        #endregion

        #region Utility methods

        private void LoadUserSessions()
        {
            userSessions = ScriptableObjectExtensions.FindOrCreateAsset<UserSessions>(
                "Assets/Settings/Logging/UserSessions.asset"
            );

            users.AddRange(userSessions.Users);
            maxPositions = FindMaxPositions();
            seekSize = maxPositions;
        }

        private void SaveUserSessions()
        {
            userSessions.Users.Clear();
            userSessions.Users.AddRange(users);
            AssetDatabase.SaveAssets();
        }

        private void AddSessions()
        {
            var directoryPath = EditorUtility.OpenFolderPanel("Open sessions directory", "", "");
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                return;
            }

            var sessions = AggregatedSessionDataReader.Read(directoryPath);
            foreach (var session in sessions)
            {
                AddOrReplace(session);
            }

            maxPositions = FindMaxPositions();
        }

        private int FindMaxPositions()
        {
            var max = 0;
            foreach (var user in users)
            {
                foreach (var session in user.Sessions)
                {
                    foreach (var pose in session.Poses)
                    {
                        var positionCount = pose.Positions.Count;
                        if (max < positionCount)
                        {
                            max = positionCount;
                        }
                    }
                }
            }

            return max;
        }

        private void AddOrReplace(AggregatedSessionData session)
        {
            var existingUser = users.FirstOrDefault(user => user.LoggingId == session.LoggingId);
            if (existingUser != null)
            {
                var sessions = existingUser.Sessions;
                var index = sessions.FindIndex(
                    existingSession => existingSession.SessionName == session.SessionName
                );

                if (index != -1)
                {
                    sessions[index] = session;
                }
                else
                {
                    sessions.Add(session);
                }

                sessions.Sort((sessionA, sessionB) =>
                    string.CompareOrdinal(sessionA.SessionName, sessionB.SessionName)
                );
            }
            else
            {
                var newUser = new UserData(session);
                users.Add(newUser);
            }
        }

        #endregion

        #region Scene drawing methods

        private void DrawUserPositions()
        {
            foreach (var user in users)
            {
                foreach (var session in user.Sessions)
                {
                    if (session.IsDraw)
                    {
                        DrawPositions(session);
                    }
                }
            }
        }

        private void DrawPositions(AggregatedSessionData session)
        {
            ApplyWireMaterial();

            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);

            var poses = session.Poses;
            foreach (var pose in poses)
            {
                if (pose.IsDraw)
                {
                    DrawPositions(pose, session.Color);
                }
            }

            GL.PopMatrix();
        }

        private void DrawPositions(PoseData pose, Color color)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);

            var positions = pose.Positions;

            var startIndex = Math.Min(seekStart, positions.Count);
            var endIndex = Math.Min(seekStart + seekSize, positions.Count) - 1;

            for (var index = startIndex; index < endIndex; index++)
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
