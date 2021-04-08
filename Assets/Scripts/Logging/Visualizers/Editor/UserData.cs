using System.Collections.Generic;
using NeanderthalTools.Logging.Loggers.Session;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public class UserData
    {
        public SessionData Session { get; set; }

        public List<PoseData> Poses { get; set; }

        public bool IsDraw { get; set; } = true;

        public Color Color { get; set; }
    }
}
