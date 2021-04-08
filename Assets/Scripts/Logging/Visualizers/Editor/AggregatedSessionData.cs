using System;
using System.Collections.Generic;
using NeanderthalTools.Logging.Loggers.Session;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    [Serializable]
    public class AggregatedSessionData
    {
        #region Fields

        [SerializeField]
        private string sessionName;

        [SerializeField]
        private bool isDraw = true;

        [SerializeField]
        private SessionData session;

        [SerializeField]
        private List<PoseData> poses;

        [SerializeField]
        private Color color = Color.cyan;

        #endregion

        #region Properties

        public string SessionName => sessionName;

        public bool IsDraw
        {
            get => isDraw;
            set => isDraw = value;
        }

        public string LoggingId => Session.LoggingId;

        public Color Color
        {
            get => color;
            set => color = value;
        }

        public SessionData Session => session;

        public List<PoseData> Poses => poses;

        #endregion

        #region Methods

        public AggregatedSessionData(string sessionName, SessionData session, List<PoseData> poses)
        {
            this.sessionName = sessionName;
            this.session = session;
            this.poses = poses;
        }

        #endregion
    }
}
