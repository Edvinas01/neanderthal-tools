using System;
using System.IO;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public static class Files
    {
        /// <returns>
        /// File and folder friendly date-name.
        /// </returns>
        public static string DateName()
        {
            return $"{DateTime.UtcNow:yyyy-MM-dd'_'hh-mm-ss}";
        }

        public static string CreateFilePath(
            string directoryName,
            string fileName,
            bool compress = false,
            string compressedSuffix = "gz"
        )
        {
            var completeFileName = compress
                ? $"{fileName}.{compressedSuffix}"
                : fileName;

            return Path.Combine(
                Application.persistentDataPath,
                directoryName,
                completeFileName
            );
        }

        public static void CreateDirectory(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryName ?? string.Empty);
        }

        public static string CreateFileName(UnityEngine.Object obj, string name, string suffix)
        {
            var logFileName = string.IsNullOrWhiteSpace(name)
                ? obj.name
                : name;

            return $"{logFileName}.{suffix}";
        }
    }
}
