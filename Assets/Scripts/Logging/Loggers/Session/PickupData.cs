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

        #endregion
    }
}
