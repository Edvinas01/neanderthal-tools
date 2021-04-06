using System.IO;
using UnityEngine;

namespace NeanderthalTools.Logging.Writers
{
    public class JsonFileLogWriter : ILogWriter
    {
        #region Fields

        private readonly string filePath;

        #endregion

        #region Methods

        public JsonFileLogWriter(string filePath)
        {
            this.filePath = filePath;
        }

        public void Start()
        {
        }

        public void Close()
        {
        }

        public void Write(params object[] values)
        {
            Debug.Log($"Writing json log to: {filePath}");

            using var writer = new StreamWriter(filePath);
            foreach (var value in values)
            {
                var json = JsonUtility.ToJson(value, false);
                writer.WriteLine(json);
            }
        }

        #endregion
    }
}
