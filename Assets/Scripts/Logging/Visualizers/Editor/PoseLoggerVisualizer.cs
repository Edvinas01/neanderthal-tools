using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    // TODO:
    // 1. trimming
    // 2. multi-user support
    // 3. gzip support

    public class PoseLoggerVisualizer : EditorWindow
    {
        #region Structs

        private readonly struct User
        {
            public Vector3[][] Poses { get; }

            public Color Color { get; }

            public User(Vector3[][] poses, Color color)
            {
                Poses = poses;
                Color = color;
            }
        }

        #endregion

        #region Fields

        // Use same seed to ensure consistency when randomising colors, etc.
        private static readonly System.Random Random = new System.Random(0);
        private readonly List<User> users = new List<User>();

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
            foreach (var user in users)
            {
                Handles.color = user.Color;
                foreach (var positions in user.Poses)
                {
                    Handles.DrawPolyLine(positions);
                }
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
            // todo path may contain gzip data
            // path

            users.Clear();

            // Reading file.
            var lines = File.ReadAllLines(path);
            if (lines.Length == 0)
            {
                return;
            }

            var poseCount = GetPoseCount(lines);
            var positionCount = GetPositionCount(lines);
            var rawPositionCount = GetRawPositionCount(lines);

            var poses = CreatePoses(poseCount, positionCount);
            ParsePoses(poses, lines);
            PadPoses(poses, rawPositionCount);

            var color = GetRandomColor();
            var user = new User(poses, color);

            users.Add(user);
        }

        private static int GetPoseCount(IEnumerable<string> lines)
        {
            var line = lines.FirstOrDefault();
            if (line == null)
            {
                return 0;
            }

            // -1 to ignore the Time column, / 6 as each pose consists of Vector3 x 2.
            return (line.Split(',').Length - 1) / 6;
        }

        private static int GetPositionCount(IReadOnlyCollection<string> lines)
        {
            var positionCount = GetRawPositionCount(lines);
            if (positionCount % 2 == 0)
            {
                return positionCount;
            }

            // Ensure that each position has a pari (necessary for drawing).
            return positionCount + 1;
        }

        private static int GetRawPositionCount(IReadOnlyCollection<string> lines)
        {
            // -1 to ignore the header row.
            return Mathf.Max(lines.Count - 1, 0);
        }

        private static Vector3[][] CreatePoses(int poseCount, int positionCount)
        {
            var poses = new Vector3[poseCount][];
            for (var index = 0; index < poseCount; index++)
            {
                poses[index] = new Vector3[positionCount];
            }

            return poses;
        }

        private static void ParsePoses(IReadOnlyList<Vector3[]> poses, IReadOnlyList<string> lines)
        {
            // Starting from 1 to skip the header.
            for (var lineIndex = 1; lineIndex < lines.Count; lineIndex++)
            {
                var line = lines[lineIndex];
                var parts = line.Split(',');

                var positionIndex = 0;

                // Starting from 1 as first column is Time.
                for (var index = 1; index < parts.Length; index += 6)
                {
                    var position = ParsePosition(parts, index);
                    // ReSharper disable once UnusedVariable
                    var rotation = ParseRotation(parts, index + 3);

                    var positions = poses[positionIndex++];

                    // -1 since line index starts from 1.
                    positions[lineIndex - 1] = position;
                }
            }
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

        private static Color GetRandomColor()
        {
            return new Color(
                (float) Random.NextDouble(),
                (float) Random.NextDouble(),
                (float) Random.NextDouble()
            );
        }

        private static void PadPoses(IEnumerable<Vector3[]> poses, int fromIndex)
        {
            foreach (var positions in poses)
            {
                for (var index = fromIndex; index < positions.Length; index++)
                {
                    var previousPosition = positions[index - 1];
                    positions[index] = previousPosition;
                }
            }
        }

        #endregion
    }
}
