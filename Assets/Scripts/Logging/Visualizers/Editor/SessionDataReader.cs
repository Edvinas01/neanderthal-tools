using System;
using NeanderthalTools.Logging.Loggers.Session;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public static class SessionDataReader
    {
        public static SessionData Read(string filePath)
        {
            var lines = LogFileReader.Read(filePath);
            var json = string.Join(Environment.NewLine, lines);

            return JsonUtility.FromJson<SessionData>(json);
        }
    }
}
