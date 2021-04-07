using System;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class HitData
    {
        #region Fields

        [SerializeField]
        private string objectiveHandName;

        [SerializeField]
        private string knapperHandName;

        [SerializeField]
        private string objectiveName;

        [SerializeField]
        private string knapperName;

        [SerializeField]
        private string flakeName;

        [SerializeField]
        private float impactForce;

        [SerializeField]
        private float time;

        #endregion

        #region Properties

        public string ObjectiveHandName
        {
            get => objectiveHandName;
            set => objectiveHandName = value;
        }

        public string KnapperHandName
        {
            get => knapperHandName;
            set => knapperHandName = value;
        }

        public string ObjectiveName
        {
            get => objectiveName;
            set => objectiveName = value;
        }

        public string KnapperName
        {
            get => knapperName;
            set => knapperName = value;
        }

        public string FlakeName
        {
            get => flakeName;
            set => flakeName = value;
        }

        public float ImpactForce
        {
            get => impactForce;
            set => impactForce = value;
        }

        public float Time
        {
            get => time;
            set => time = value;
        }

        #endregion
    }
}
