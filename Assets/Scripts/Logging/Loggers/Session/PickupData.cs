using System;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class PickupData
    {
        #region Fields

        [SerializeField]
        private string targetName;

        [SerializeField]
        private string handName;

        [SerializeField]
        private float time;

        #endregion

        #region Properties

        public string TargetName
        {
            get => targetName;
            set => targetName = value;
        }

        public string HandName
        {
            get => handName;
            set => handName = value;
        }

        public float Time
        {
            get => time;
            set => time = value;
        }

        #endregion
    }
}
