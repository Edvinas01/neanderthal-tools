using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public class PoseLoggerVisualizer : EditorWindow
    {
        #region Fields

        private readonly List<List<Vector3>> positionGroups = new List<List<Vector3>>();

        #endregion

        #region Unity Lifecycle

        private void OnGUI()
        {
            if (GUILayout.Button("Load file"))
            {
                var types = new[]
                {
                    NativeFilePicker.ConvertExtensionToFileType("log"),
                    NativeFilePicker.ConvertExtensionToFileType("log.gz")
                };

                NativeFilePicker.PickFile(PickFile, types);
            }
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

        private void DuringSceneGUI(SceneView sceneView)
        {
            foreach (var positions in positionGroups)
            {
                Vector3[] positionArray;
                if (positions.Count % 2 == 0)
                {
                    positionArray = positions.ToArray();
                }
                else
                {
                    positionArray = positions.Skip(1).ToArray();
                }

                Handles.color = GetRandomColor();
                Handles.DrawPolyLine(positionArray);
            }
        }

        #endregion

        #region Methods

        [MenuItem("Tools/Pose Logger Visualizer")]
        public static void Init()
        {
            var window = GetWindow<PoseLoggerVisualizer>("Pose Logger Visualizer");
            window.Show();
        }

        private void PickFile(string path)
        {
            positionGroups.Clear();

            var header = true;
            foreach (var line in File.ReadLines(path))
            {
                var parts = line.Split(',');
                if (header)
                {
                    var positionGroupCount = (parts.Length - 1) / 6;
                    for (var index = 0; index < positionGroupCount; index++)
                    {
                        positionGroups.Add(new List<Vector3>());
                    }

                    header = false;
                    continue;
                }

                var positionGroupIndex = 0;
                for (var index = 1; index < parts.Length; index += 6)
                {
                    var position = ParsePosition(parts, index);
                    var rotation = ParseRotation(parts, index + 3);

                    var positions = positionGroups[positionGroupIndex++];
                    positions.Add(position);
                }
            }
        }

        private static Color GetRandomColor()
        {
            return new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
        }

        private static Vector3 ParsePosition(IReadOnlyList<string> parts, int index)
        {
            return new Vector3(
                float.Parse(parts[index]),
                float.Parse(parts[index + 1]),
                float.Parse(parts[index + 2])
            );
        }

        private static Quaternion ParseRotation(IReadOnlyList<string> parts, int index)
        {
            return Quaternion.Euler(
                float.Parse(parts[index]),
                float.Parse(parts[index + 1]),
                float.Parse(parts[index + 2])
            );
        }

        #endregion
    }
}
