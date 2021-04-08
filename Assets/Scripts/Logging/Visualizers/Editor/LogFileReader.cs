using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public static class LogFileReader
    {
        #region Fields

        private const string CompressedSuffix = ".gz";

        #endregion

        #region Methods

        public static IEnumerable<string> Read(string filePath)
        {
            return filePath.EndsWith(CompressedSuffix)
                ? ReadCompressedFile(filePath)
                : ReadRegularFile(filePath);
        }

        private static IEnumerable<string> ReadCompressedFile(string filePath)
        {
            using var fileStream = File.OpenRead(filePath);
            using var gZipStream = new GZipStream(fileStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gZipStream);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        private static IEnumerable<string> ReadRegularFile(string filePath)
        {
            using var reader = new StreamReader(filePath);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        #endregion
    }
}
