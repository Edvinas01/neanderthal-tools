using System;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class StateData
    {
        #region Fields

        [SerializeField]
        private string stateName;

        [SerializeField]
        private float startTime;

        [SerializeField]
        private float endTime;

        #endregion

        #region Properties

        public string StateName
        {
            get => stateName;
            set => stateName = value;
        }

        public float StartTime
        {
            get => startTime;
            set => startTime = value;
        }

        public float EndTime
        {
            get => endTime;
            set => endTime = value;
        }

        #endregion
    }
}
