using NeanderthalTools.Logging.Writers;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers
{
    public class PoseLogger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private LogWriterProvider logWriterProvider;

        #endregion

        #region Fields

        private Transform loggableTransform;
        private ILogWriter logWriter;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            loggableTransform = transform;
        }

        private void OnEnable()
        {
            logWriter = logWriterProvider.CreateLogWriter(name);

            logWriter.Start();
            logWriter.Write(
                "PositionX",
                "PositionY",
                "PositionZ",
                "RotationX",
                "RotationY",
                "RotationZ"
            );
        }

        private void OnDisable()
        {
            logWriter.Close();
            logWriter = null;
        }

        private void Update()
        {
            var position = loggableTransform.position;
            var rotation = loggableTransform.rotation.eulerAngles;

            logWriter.Write(
                position.x, position.y, position.z,
                rotation.x, rotation.y, rotation.z
            );
        }

        #endregion
    }
}
