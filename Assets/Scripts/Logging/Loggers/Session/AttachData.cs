using System;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class AttachData
    {
        #region Fields

        [SerializeField]
        private string toolPartHandName;

        [SerializeField]
        private string handleHandName;

        [SerializeField]
        private string toolPartName;

        [SerializeField]
        private string handleName;

        [SerializeField]
        private float time;

        #endregion

        #region Properties

        public string ToolPartHandName
        {
            get => toolPartHandName;
            set => toolPartHandName = value;
        }

        public string HandleHandName
        {
            get => handleHandName;
            set => handleHandName = value;
        }

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
