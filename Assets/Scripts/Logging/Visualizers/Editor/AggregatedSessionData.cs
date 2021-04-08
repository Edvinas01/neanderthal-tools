using System.Collections.Generic;
using NeanderthalTools.Logging.Loggers.Session;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    public class AggregatedSessionData
    {
        #region Properties

        public string SessionName { get; }

        public bool IsDraw { get; set; } = true;

        public string LoggingId => Session.LoggingId;

        public Color Color { get; set; } = Color.cyan;

        public SessionData Session { get; }

        public List<PoseData> Poses { get; }

        #endregion

        #region Methods

        public AggregatedSessionData(string sessionName, SessionData session, List<PoseData> poses)
        {
            SessionName = sessionName;
            Session = session;
            Poses = poses;
        }

        #endregion
    }
}
