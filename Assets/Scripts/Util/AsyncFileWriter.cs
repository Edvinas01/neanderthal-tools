using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public class AsyncFileWriter
    {
        #region Fields

        private readonly ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();

        private readonly string directoryName;
        private readonly string fileName;
        private readonly bool compress;
        private readonly float writeIntervalSeconds;

        private readonly string compressedSuffix;
        private readonly int bufferSize;

        private Stream stream;
        private bool runTask;
        private Task task;

        #endregion

        #region Properties

        public string FilePath => CreateFilePath();

        #endregion

        #region Methods

        public AsyncFileWriter(
            string directoryName,
            string fileName,
            bool compress,
            float writeIntervalSeconds,
            string compressedSuffix = "gz",
            int bufferSize = 4096
        )
        {
            this.directoryName = directoryName;
            this.fileName = fileName;
            this.compress = compress;
            this.writeIntervalSeconds = writeIntervalSeconds;
            this.compressedSuffix = compressedSuffix;
            this.bufferSize = bufferSize;
        }

        /// <summary>
        /// Start the async streaming task.
        /// </summary>
        public void Start()
        {
            stream = CreateStream();
            runTask = true;
            task = Task.Factory.StartNew(Write);
        }

        /// <summary>
        /// Close the async streaming task and cleanup.
        /// </summary>
        public void Close()
        {
            runTask = false;

            task.Wait();
            task = null;

            stream.Close();
            stream = null;
        }

        public void Write(byte[] value)
        {
            queue.Enqueue(value);
        }

        private Stream CreateStream()
        {
            var path = CreateFilePath();
            CreateDirectory(path);

            return compress
                ? CreateCompressedStream(path)
                : CreateSimpleStream(path);
        }

        private string CreateFilePath()
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

        private static void CreateDirectory(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryName ?? string.Empty);
        }

        private Stream CreateCompressedStream(string path)
        {
            var fileStream = CreateSimpleStream(path);
            return new GZipStream(fileStream, CompressionMode.Compress);
        }

        private Stream CreateSimpleStream(string path)
        {
            return File.Create(path, bufferSize, FileOptions.Asynchronous);
        }

        private async void Write()
        {
            while (runTask)
            {
                while (queue.TryDequeue(out var value))
                {
                    await stream.WriteAsync(value, 0, value.Length);
                }

                await Task.Delay((int) (writeIntervalSeconds * 1000));
            }
        }

        #endregion
    }
}
