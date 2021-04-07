using System;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class LocomotionData
    {
        #region Fields

        [SerializeField]
        private Vector3 position;

        [SerializeField]
        private Vector3 rotation;

        [SerializeField]
        private float time;

        #endregion

        #region Properties

        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        public Vector3 Rotation
        {
            get => rotation;
            set => rotation = value;
        }

        public float Time
        {
            get => time;
            set => time = value;
        }

        #endregion
    }
}
