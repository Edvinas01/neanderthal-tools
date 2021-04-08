using System.IO;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public static class UserDataReader
    {
        #region Fields

        private const string SessionFileName = "SessionLogger";
        private const string PoseFilename = "PoseLogger";

        #endregion

        #region Methods

        public static UserData Read(string directoryPath)
        {
            var filePaths = Directory.GetFiles(directoryPath);
            var user = new UserData
            {
                Color = GetRandomColor()
            };

            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileName(filePath);
                if (fileName.StartsWith(SessionFileName))
                {
                    user.Session = SessionDataReader.Read(filePath);
                }
                else if (fileName.StartsWith(PoseFilename))
                {
                    user.Poses = PoseDataReader.Read(filePath);
                }
            }

            return user;
        }

        private static Color GetRandomColor()
        {
            return new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
        }

        #endregion
    }
}
