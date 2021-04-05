using System.Collections.Generic;
using NeanderthalTools.Logging.Writers;
using UnityEngine;

namespace NeanderthalTools.Logging.Loggers
{
    public class PoseLogger : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private StreamingLogWriterProvider logWriterProvider;

        [SerializeField]
        private PoseLoggerSettings poseLoggerSettings;

        [SerializeField]
        private List<Transform> poses;

        #endregion

        #region Fields

        private IStreamingLogWriter logWriter;
        private float nextSampleTime;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            logWriter = logWriterProvider.CreateLogWriter(name);

            logWriter.Start();
            logWriter.Write("Time");

            foreach (var pose in poses)
            {
                var poseName = pose.name;
                logWriter.Write(
                    $"{poseName}_PositionX",
                    $"{poseName}_PositionY",
                    $"{poseName}_PositionZ",
                    $"{poseName}_RotationX",
                    $"{poseName}_RotationY",
                    $"{poseName}_RotationZ"
                );
            }
        }

        private void OnDisable()
        {
            logWriter.Close();
            logWriter = null;
        }

        private void Update()
        {
            if (!IsSample())
            {
                return;
            }

            UpdateNextSampleTime();
            LogPoses();
        }

        #endregion

        #region Methods

        private bool IsSample()
        {
            return Time.time >= nextSampleTime;
        }

        private void UpdateNextSampleTime()
        {
            nextSampleTime = Time.time + poseLoggerSettings.SampleInterval;
        }

        private void LogPoses()
        {

            foreach (var pose in poses)
            {
                LogPose(pose);
            }
        }

        private void LogPose(Transform pose)
        {
            var position = pose.position;
            var rotation = pose.rotation.eulerAngles;

            logWriter.Write(
                position.x, position.y, position.z,
                rotation.x, rotation.y, rotation.z
            );
        }

        #endregion
    }
}
