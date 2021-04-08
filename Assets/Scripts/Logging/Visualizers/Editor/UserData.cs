using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Logging.Visualizers.Editor
{
    [Serializable]
    public class UserData
    {
        #region Fields

        [SerializeField]
        private string loggingId;

        [SerializeField]
        private List<AggregatedSessionData> sessions;

        #endregion

        #region Properties

        public string LoggingId => loggingId;

        public bool Foldout { get; set; }

        public List<AggregatedSessionData> Sessions => sessions;

        #endregion

        #region Methods

        public UserData(AggregatedSessionData session)
        {
            loggingId = session.LoggingId;
            sessions = new List<AggregatedSessionData> {session};
        }

        #endregion
    }
}
