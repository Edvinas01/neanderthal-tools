using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace NeanderthalTools.Util
{
    public class AsyncTextFileWriter : AsyncFileWriter<string>
    {
        #region Fields

        private StreamWriter streamWriter;

        #endregion

        #region Overrides

        protected override Task WriteAsync(string value)
        {
            return streamWriter.WriteAsync(value);
        }

        protected override void StartWriter()
        {
            streamWriter = CreateWriter();
        }

        protected override void StopWriter()
        {
            streamWriter?.Close();
            streamWriter = null;
        }

        #endregion

        #region Methods

        private StreamWriter CreateWriter()
        {
            var path = FilePath;
            var dir = Path.GetDirectoryName(path);

            Directory.CreateDirectory(dir ?? string.Empty);

            return CompressFile
                ? CreateCompressedWriter(path)
                : CreateSimpleWriter(path);
        }

        private static StreamWriter CreateCompressedWriter(string path)
        {
            var fileStream = File.Create(path);
            var gzipStream = new GZipStream(fileStream, CompressionMode.Compress);

            return new StreamWriter(gzipStream);
        }

        private static StreamWriter CreateSimpleWriter(string path)
        {
            return new StreamWriter(path);
        }

        #endregion
    }
}
