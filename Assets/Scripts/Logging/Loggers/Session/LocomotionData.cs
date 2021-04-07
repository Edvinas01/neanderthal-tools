using System;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers.Session
{
    [Serializable]
    public class LocomotionData
    {
        #region Fields

        [SerializeField]
        private Vector3 cameraPosition;

        [SerializeField]
        private Vector3 cameraRotation;

        [SerializeField]
        private Vector3 rigPosition;

        [SerializeField]
        private Vector3 rigRotation;

        [SerializeField]
        private float time;

        #endregion

        #region Properties

        public Vector3 CameraPosition
        {
            get => cameraPosition;
            set => cameraPosition = value;
        }

        public Vector3 CameraRotation
        {
            get => cameraRotation;
            set => cameraRotation = value;
        }

        public Vector3 RigPosition
        {
            get => rigPosition;
            set => rigPosition = value;
        }

        public Vector3 RigRotation
        {
            get => rigRotation;
            set => rigRotation = value;
        }

        public float Time
        {
            get => time;
            set => time = value;
        }

        #endregion
    }
}
