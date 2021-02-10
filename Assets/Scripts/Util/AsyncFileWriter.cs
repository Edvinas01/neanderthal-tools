using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace NeanderthalTools.Util
{
    public abstract class AsyncFileWriter<TValue>
    {
        #region Fields

        private readonly ConcurrentQueue<TValue> queue = new ConcurrentQueue<TValue>();

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

        public void Start()
        {
            StartWriter();
            runTask = true;
            task = Task.Factory.StartNew(Write);
        }

        public void Stop()
        {
            runTask = false;

            task?.Wait();
            task = null;

            StopWriter();
        }

        public void EnqueueWrite(TValue value)
        {
            queue.Enqueue(value);
        }

        protected abstract Task WriteAsync(TValue value);

        protected abstract void StartWriter();

        protected abstract void StopWriter();

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

        private async void Write()
        {
            while (runTask)
            {
                while (queue.TryDequeue(out var value))
                {
                    await WriteAsync(value);
                }

                await Task.Delay((int) (WriteIntervalSeconds * 1000));
            }
        }

        #endregion
    }
}
