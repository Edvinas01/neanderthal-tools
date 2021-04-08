using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public static class PoseDataReader
    {
        #region Fields

        private const int PoseColumnCount = 6;

        private const int PositionXOffset = 0;
        private const int PositionYOffset = 1;
        private const int PositionZOffset = 2;

        private const int RotationXOffset = 0;
        private const int RotationYOffset = 1;
        private const int RotationZOffset = 2;

        #endregion

        #region Methods

        public static List<PoseData> Read(string filePath)
        {
            var lines = LogFileReader.Read(filePath);

            List<PoseData> poses = null;
            var first = true;

            foreach (var line in lines)
            {
                var lineParts = line.Split(',');
                if (first)
                {
                    poses = ParsePoses(lineParts);
                    first = false;
                }
                else
                {
                    var poseIndex = 0;

                    // Starting from 1 as first column is Time.
                    for (var index = 1; index < lineParts.Length; index += PoseColumnCount)
                    {
                        var position = ParsePosition(lineParts, index);
                        var rotation = ParseRotation(lineParts, index + 3);

                        var pose = poses[poseIndex++];
                        pose.Positions.Add(position);
                    }
                }
            }

            return poses;
        }

        private static List<PoseData> ParsePoses(IReadOnlyList<string> lineParts)
        {
            var poses = new List<PoseData>();

            // Starting from 1 as first column is Time.
            for (var index = 1; index < lineParts.Count; index += PoseColumnCount)
            {
                var header = lineParts[index];
                var headerParts = header.Split('_');
                var headerPrefix = headerParts[0];

                var pose = new PoseData
                {
                    Name = headerPrefix,
                    Positions = new List<Vector3>()
                };

                poses.Add(pose);
            }

            return poses;
        }

        private static Vector3 ParsePosition(IReadOnlyList<string> parts, int index)
        {
            return new Vector3(
                float.Parse(parts[index + PositionXOffset]),
                float.Parse(parts[index + PositionYOffset]),
                float.Parse(parts[index + PositionZOffset])
            );
        }

        private static Quaternion ParseRotation(IReadOnlyList<string> parts, int index)
        {
            return Quaternion.Euler(
                float.Parse(parts[index + RotationXOffset]),
                float.Parse(parts[index + RotationYOffset]),
                float.Parse(parts[index + RotationZOffset])
            );
        }

        #endregion
    }
}
