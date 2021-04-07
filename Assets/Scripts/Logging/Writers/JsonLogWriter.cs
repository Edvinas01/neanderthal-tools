using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace NeanderthalTools.Logging.Writers
{
    public class JsonLogWriter : ILogWriter
    {
        #region Fields

        private readonly string filePath;
        private readonly bool compress;

        private Stream stream;

        #endregion

        #region Methods

        public JsonLogWriter(string filePath, bool compress)
        {
            this.filePath = filePath;
            this.compress = compress;
        }

        public void Start()
        {
            stream = CreateStream();
        }

        public void Close()
        {
            stream.Close();
            stream = null;
        }

        public void Write(params object[] values)
        {
            Debug.Log($"Writing json log to: {filePath}");

            foreach (var value in values)
            {
                var json = JsonUtility.ToJson(value, false);
                var bytes = Encoding.UTF8.GetBytes(json);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private Stream CreateStream()
        {
            var fileStream = File.Create(filePath);
            if (compress)
            {
                return new GZipStream(fileStream, CompressionMode.Compress);
            }

            return fileStream;
        }

        #endregion
    }
}
