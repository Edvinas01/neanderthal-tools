using System.Collections.Generic;
using System.IO;
using NeanderthalTools.Logging.Loggers.Session;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public static class AggregatedSessionDataReader
    {
        #region Fields

        private const string SessionFileName = "SessionLogger";
        private const string PoseFilename = "PoseLogger";

        #endregion

        #region Methods

        public static List<AggregatedSessionData> Read(string directoryPath)
        {
            var sessions = new List<AggregatedSessionData>();
            Read(sessions, directoryPath);
            return sessions;
        }

        private static void Read(ICollection<AggregatedSessionData> sessions, string path)
        {
            if (Directory.Exists(path))
            {
                var directoryPaths = Directory.GetDirectories(path);
                foreach (var directoryPath in directoryPaths)
                {
                    Read(sessions, directoryPath);
                }
            }

            var filePaths = Directory.GetFiles(path);
            var directoryName = Path.GetFileName(path);

            Read(sessions, filePaths, directoryName);
        }

        private static void Read(
            ICollection<AggregatedSessionData> sessions,
            IEnumerable<string> filePaths,
            string directoryName
        )
        {
            SessionData session = null;
            List<PoseData> poses = null;

            foreach (var filePath in filePaths)
            {
                if (Directory.Exists(filePath))
                {
                    Read(sessions, filePath);
                }
                else
                {
                    var fileName = Path.GetFileName(filePath);
                    if (fileName.StartsWith(SessionFileName))
                    {
                        session = SessionDataReader.Read(filePath);
                    }
                    else if (fileName.StartsWith(PoseFilename))
                    {
                        poses = PoseDataReader.Read(filePath);
                    }
                }
            }

            if (session == null || poses == null)
            {
                return;
            }

            var aggregatedSession = new AggregatedSessionData(directoryName, session, poses)
            {
                Color = GetRandomColor()
            };

            sessions.Add(aggregatedSession);
        }

        private static Color GetRandomColor()
        {
            return new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                1f
            );
        }

        #endregion
    }
}
