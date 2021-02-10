using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public abstract class AsyncFileWriter<T>
    {
        #region Fields

        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        private readonly int bufferSize;

        private Stream stream;
        private bool runTask;
        private Task task;

        #endregion

        #region Properties

        public float WriteIntervalSeconds { protected get; set; }

        public string FileDirectory { protected get; set; }

        public string FileSuffix { protected get; set; }

        public string FilePath => GetFilePath();

        public string ArchiveSuffix { protected get; set; } = "gz";

        public bool CompressFile { protected get; set; }

        #endregion

        #region Methods

        protected AsyncFileWriter(int bufferSize = 4096)
        {
            this.bufferSize = bufferSize;
        }

        public void Start()
        {
            stream = CreateStream();

            runTask = true;
            task = Task.Factory.StartNew(Write);
        }

        public void Stop()
        {
            runTask = false;

            task?.Wait();
            task = null;

            stream?.Close();
            stream = null;
        }

        public void EnqueueWrite(T value)
        {
            queue.Enqueue(value);
        }

        protected abstract byte[] GetBytes(T value);

        private string GetFilePath()
        {
            return Path.Combine(
                Application.persistentDataPath,
                FileDirectory,
                $"{DateTime.UtcNow:yyyy-MM-dd'_'hh-mm-ss}.{GetFileSuffix()}"
            );
        }

        private string GetFileSuffix()
        {
            return CompressFile ? $"{FileSuffix}.{ArchiveSuffix}" : FileSuffix;
        }

        private Stream CreateStream()
        {
            var path = FilePath;
            var dir = Path.GetDirectoryName(path);

            Directory.CreateDirectory(dir ?? string.Empty);

            return CompressFile
                ? CreateCompressedStream(path)
                : CreateSimpleStream(path);
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
                    var bytes = GetBytes(value);
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }

                await Task.Delay((int) (WriteIntervalSeconds * 1000));
            }
        }

        #endregion
    }
}
