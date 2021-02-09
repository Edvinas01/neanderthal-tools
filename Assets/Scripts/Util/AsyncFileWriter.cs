using System;
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

        private readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        private bool runTask;
        private Task task;

        private StreamWriter streamWriter;

        #endregion

        #region Properties

        public float WriteIntervalSeconds { private get; set; }

        public string FileDirectory { private get; set; }

        public string FileSuffix { private get; set; }

        public string ArchiveSuffix { private get; set; } = "gz";

        public bool CompressFile { private get; set; }

        #endregion

        #region Methods

        public void Start()
        {
            streamWriter = CreateWriter();
            runTask = true;
            task = Task.Factory.StartNew(Write);
        }

        public void Stop()
        {
            runTask = false;

            task?.Wait();
            task = null;

            streamWriter?.Close();
            streamWriter = null;
        }

        public void Write(string value)
        {
            queue.Enqueue(value);
        }

        private StreamWriter CreateWriter()
        {
            var path = GetFilePath();
            var dir = Path.GetDirectoryName(path);

            Directory.CreateDirectory(dir ?? string.Empty);

            return CompressFile
                ? CreateCompressedWriter(path)
                : CreateSimpleWriter(path);
        }

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

        private async void Write()
        {
            while (runTask)
            {
                while (queue.TryDequeue(out var value))
                {
                    await streamWriter.WriteAsync(value);
                }

                await Task.Delay((int) (WriteIntervalSeconds * 1000));
            }
        }

        #endregion
    }
}
