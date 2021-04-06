using System;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class AttachData
    {
        #region Fields

        [SerializeField]
        private string toolPartName;

        [SerializeField]
        private string handleName;

        [SerializeField]
        private float time;

        #endregion

        #region Properties

        public string ToolPartName
        {
            get => toolPartName;
            set => toolPartName = value;
        }

        public string HandleName
        {
            get => handleName;
            set => handleName = value;
        }

        public float Time
        {
            get => time;
            set => time = value;
        }

        #endregion
    }
}
