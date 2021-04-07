using System;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class ConsumeData
    {
        #region Fields

        [SerializeField]
        private string adhesiveName;

        [SerializeField]
        private float time;

        #endregion

        #region Properties

        public string AdhesiveName
        {
            get => adhesiveName;
            set => adhesiveName = value;
        }

        public float Time
        {
            get => time;
            set => time = value;
        }

        #endregion
    }
}
